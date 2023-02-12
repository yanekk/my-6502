
from subprocess import CompletedProcess
from unittest.mock import MagicMock, call
import pytest
from pathlib import Path
from .assembler import Assembler, Subprocess, AssemblyError

@pytest.fixture
def subprocess_mock():
    subprocess_mock = MagicMock(spec=Subprocess)
    def return_valid_completed_process(*args, **kwargs):
        return CompletedProcess(args=[], returncode=0, stdout='', stderr='')
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
        call('ca65', ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path)]),
        call('cl65', ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))])
    ])

def test_assembly_error_builds_message():
    # arrange
    assembly_error = AssemblyError(
        command='ca65',
        stderr='error message',
        return_code=123,
        command_args=['a', 'b', 'c']
    )

    # act
    error_message = str(assembly_error)

    # assert
    assert error_message.splitlines() == [
        'Error while running ca65:',
        '===',
        'error message',
        '===',
        'Return code: 123',
        'Arguments used: a b c'
    ]

def test_assembler_invalid_file_returns_ca65_error_on_stderr(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    expected_error = 'bios.s(3): Error: Unexpected trailing garbage characters'
    def error_on_assembling(*args, **_):
        if args[0] == 'ca65':
            return CompletedProcess(args=[], returncode=123, stdout='', stderr=expected_error)

    subprocess_mock.run.side_effect = error_on_assembling
    
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'invalid_code.s'

    # act
    with pytest.raises(AssemblyError) as assembly_error:
        assembler.assemble(source_code_path=source_code_path)

    # assert
    assert assembly_error.value.command == 'ca65'
    assert assembly_error.value.command_args == ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path)]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == 'bios.s(3): Error: Unexpected trailing garbage characters'


def test_assembler_invalid_file_returns_ca65_error_on_nonzero_returncode(testdata_path: Path, subprocess_mock: Subprocess):
    # arrange
    def error_on_assembling(*args, **_):
        if args[0] == 'ca65':
            return CompletedProcess(args=[], returncode=123, stdout='', stderr='')

    subprocess_mock.run.side_effect = error_on_assembling
    
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'invalid_code.s'

    # act
    with pytest.raises(AssemblyError) as assembly_error:
        assembler.assemble(source_code_path=source_code_path)

    # assert
    assert assembly_error.value.command == 'ca65'
    assert assembly_error.value.command_args == ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path)]
    assert assembly_error.value.return_code == 123    
    assert assembly_error.value.stderr == ''
