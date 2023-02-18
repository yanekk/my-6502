
from subprocess import CompletedProcess
from unittest.mock import MagicMock, call
from pathlib import Path

import pytest

from runner.assembler import Assembler
from runner.subprocess import Subprocess, ExecutionError

@pytest.fixture
def subprocess_mock():
    subprocess_mock = MagicMock(spec=Subprocess)
    def return_valid_completed_process(command: str, args: list[str], *_):
        return CompletedProcess(args=args, returncode=0, stdout='', stderr='')
    subprocess_mock.run.side_effect = return_valid_completed_process
    return subprocess_mock


@pytest.fixture
def testdata_path():
    return Path(__file__).parent / 'testdata'


def test_assembler_valid_file_ca65_returns_no_error(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'valid_code.s'

    # act
    assembler.assemble(source_code_path=source_code_path)

    # assert
    assert subprocess_mock.run.call_count == 2
    subprocess_mock.run.assert_has_calls([
        call('ca65', ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), source_code_path.name], str(source_code_path.parent)),
        call('cl65', ['-t', 'none',  '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))])
    ])

def test_assembler_add_include_relative_path_adds_i_argument_with_resolved_path(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    assembler = Assembler(subprocess_mock)
    assembler.add_include_path('lib')
    expected_path = str(Path.cwd() / Path('lib')).replace('\\', '/')
    # act
    assembler.assemble(source_code_path=testdata_path / 'valid_code.s')

    # assert
    ca65_args: list[str] = subprocess_mock.run.call_args_list[0].args[1]
    assert f'-I {expected_path}' in " ".join(ca65_args)

def test_assembler_add_absolute_include_path_adds_i_argument_wihout_resolving(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    assembler = Assembler(subprocess_mock)
    assembler.add_include_path('H:\\src\\my-6502\\lib')

    # act
    assembler.assemble(source_code_path=testdata_path / 'valid_code.s')

    # assert
    ca65_args: list[str] = subprocess_mock.run.call_args_list[0].args[1]
    assert '-I H:/src/my-6502/lib' in " ".join(ca65_args)

def test_assembler_set_linker_config_file_adds_C_argument(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    assembler = Assembler(subprocess_mock)
    assembler.set_config_file('program.map.cfg')

    expected_path = str(Path.cwd() / Path('program.map.cfg')).replace('\\', '/')

    # act
    assembler.assemble(source_code_path=testdata_path / 'valid_code.s')

    # assert
    cl65_args: list[str] = subprocess_mock.run.call_args_list[1].args[1]
    assert f'-C {expected_path}' in " ".join(cl65_args)
    
def test_assembler_invalid_file_returns_ca65_error_on_stderr(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    expected_error = b'bios.s(3): Error: Unexpected trailing garbage characters'
    def error_on_assembling(command: str, args: list[str], *_):
        if command == 'ca65':
            return CompletedProcess(args=args, returncode=123, stdout='', stderr=expected_error)

    subprocess_mock.run.side_effect = error_on_assembling
    
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'invalid_code.s'

    # act
    with pytest.raises(ExecutionError) as assembly_error:
        assembler.assemble(source_code_path=source_code_path)

    # assert
    assert assembly_error.value.command == 'ca65'
    assert assembly_error.value.command_args == ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path.name)]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == expected_error.decode()


def test_assembler_invalid_file_returns_ca65_error_on_nonzero_returncode(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    def error_on_assembling(command: str, args: list[str], *_):
        if command == 'ca65':
            return CompletedProcess(args=args, returncode=123, stdout='', stderr=b'')

    subprocess_mock.run.side_effect = error_on_assembling
    
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'invalid_code.s'

    # act
    with pytest.raises(ExecutionError) as assembly_error:
        assembler.assemble(source_code_path=source_code_path)

    # assert
    assert assembly_error.value.command == 'ca65'
    assert assembly_error.value.command_args == ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path.name)]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == ''


def test_assembler_invalid_file_returns_cl65_error_on_nonzero_returncode(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    def error_on_assembling(command: str, args: list[str], *_):
        if command == 'cl65':
            return CompletedProcess(args=args, returncode=123, stdout='', stderr=b'')
        return CompletedProcess(args=args, returncode=0, stdout='', stderr=b'')

    subprocess_mock.run.side_effect = error_on_assembling
    
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'invalid_code.s'

    # act
    with pytest.raises(ExecutionError) as assembly_error:
        assembler.assemble(source_code_path=source_code_path)

    # assert
    assert assembly_error.value.command == 'cl65'
    assert assembly_error.value.command_args == ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == ''


def test_assembler_invalid_file_returns_cl65_error_on_stderr(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    expected_error = b'bios.s(3): Error: Unexpected trailing garbage characters'
    def error_on_assembling(command: str, args: list[str], *_):
        if command == 'cl65':
            return CompletedProcess(args=args, returncode=123, stdout='', stderr=expected_error)
        return CompletedProcess(args=args, returncode=0, stdout='', stderr=b'')

    subprocess_mock.run.side_effect = error_on_assembling
    
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'invalid_code.s'

    # act
    with pytest.raises(ExecutionError) as assembly_error:
        assembler.assemble(source_code_path=source_code_path)

    # assert
    assert assembly_error.value.command == 'cl65'
    assert assembly_error.value.command_args == ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == expected_error.decode()