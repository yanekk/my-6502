
from subprocess import CompletedProcess
from unittest.mock import MagicMock
from pathlib import Path

import pytest

from runner.assembler import Assembler, Linker
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
    subprocess_mock.run.assert_called_once_with('ca65', 
        ['-g', '--cpu', '65C02',  '-o', str(source_code_path.with_suffix('.o')), source_code_path.name], str(source_code_path.parent))

def test_assembler_valid_file_ca65_returns_object_file(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'valid_code.s'

    # act
    output_file = assembler.assemble(source_code_path=source_code_path)

    # assert
    assert output_file == source_code_path.with_suffix('.o')

def test_linker_valid_file_cl65_returns_no_error(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    linker = Linker(subprocess_mock)

    object_file_path = testdata_path / 'valid_code.o'

    # act
    linker.link(object_file_path=object_file_path)

    # assert
    subprocess_mock.run.assert_called_once_with('cl65', ['-t', 'none', '-Ln', str(object_file_path.with_suffix('.bin.lmap')), '-o', str(object_file_path.with_suffix('.bin')), str(object_file_path)])

def test_linker_valid_file_cl65_returns_result(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    assembler = Linker(subprocess_mock)

    object_file_path = testdata_path / 'valid_code.o'

    # act
    result = assembler.link(object_file_path=object_file_path)

    # assert
    assert result.binary_file_path == object_file_path.with_suffix('.bin')
    assert result.label_file_path == object_file_path.with_suffix('.bin.lmap')

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

def test_linker_config_file_adds_C_argument(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    assembler = Linker(subprocess_mock)
    assembler.set_config_file('program.map.cfg')

    expected_path = str(Path.cwd() / Path('program.map.cfg')).replace('\\', '/')

    # act
    assembler.link(object_file_path=testdata_path / 'valid_code.o')

    # assert
    cl65_args: list[str] = subprocess_mock.run.call_args_list[0].args[1]
    assert f'-C {expected_path}' in " ".join(cl65_args)
    
def test_assembler_invalid_file_returns_ca65_error_on_stderr(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    expected_error = b'bios.s(3): Error: Unexpected trailing garbage characters'
    def error_on_assembling(command: str, args: list[str], *_):
        return CompletedProcess(args=args, returncode=123, stdout='', stderr=expected_error)

    subprocess_mock.run.side_effect = error_on_assembling
    
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'invalid_code.s'

    # act
    with pytest.raises(ExecutionError) as assembly_error:
        assembler.assemble(source_code_path=source_code_path)

    # assert
    assert assembly_error.value.command == 'ca65'
    assert assembly_error.value.command_args == ['-g', '--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path.name)]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == expected_error.decode()


def test_assembler_invalid_file_returns_ca65_error_on_nonzero_returncode(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    def error_on_assembling(command: str, args: list[str], *_):
        return CompletedProcess(args=args, returncode=123, stdout='', stderr=b'')

    subprocess_mock.run.side_effect = error_on_assembling
    
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'invalid_code.s'

    # act
    with pytest.raises(ExecutionError) as assembly_error:
        assembler.assemble(source_code_path=source_code_path)

    # assert
    assert assembly_error.value.command == 'ca65'
    assert assembly_error.value.command_args == ['-g', '--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path.name)]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == ''


def test_assembler_invalid_file_returns_cl65_error_on_nonzero_returncode(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    def error_on_assembling(command: str, args: list[str], *_):
            return CompletedProcess(args=args, returncode=123, stdout='', stderr=b'')

    subprocess_mock.run.side_effect = error_on_assembling
    
    linker = Linker(subprocess_mock)

    object_file_path = testdata_path / 'invalid_code.o'

    # act
    with pytest.raises(ExecutionError) as assembly_error:
        linker.link(object_file_path=object_file_path)

    # assert
    assert assembly_error.value.command == 'cl65'
    assert assembly_error.value.command_args == ['-t', 'none', '-Ln', str(object_file_path.with_suffix('.bin.lmap')), '-o', str(object_file_path.with_suffix('.bin')), str(object_file_path)]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == ''


def test_linker_invalid_file_returns_cl65_error_on_stderr(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    expected_error = b'bios.s(3): Error: Unexpected trailing garbage characters'
    def error_on_assembling(command: str, args: list[str], *_):
            return CompletedProcess(args=args, returncode=123, stdout='', stderr=expected_error)

    subprocess_mock.run.side_effect = error_on_assembling
    
    linker = Linker(subprocess_mock)

    object_file_path = testdata_path / 'invalid_code.o'

    # act
    with pytest.raises(ExecutionError) as assembly_error:
        linker.link(object_file_path=object_file_path)

    # assert
    assert assembly_error.value.command == 'cl65'
    assert assembly_error.value.command_args == ['-t', 'none', '-Ln', str(object_file_path.with_suffix('.bin.lmap')), '-o', str(object_file_path.with_suffix('.bin')), str(object_file_path)]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == expected_error.decode()