from unittest.mock import MagicMock
from subprocess import CompletedProcess
from pathlib import Path
import pytest

from .emulator import Emulator
from .subprocess import ExecutionError, Subprocess

@pytest.fixture
def subprocess_mock():
    subprocess_mock = MagicMock(spec=Subprocess)
    def return_valid_completed_process(command: str, args: list[str]):
        return CompletedProcess(args=args, returncode=0, stdout='', stderr='')
    subprocess_mock.run.side_effect = return_valid_completed_process
    return subprocess_mock

def test_emulator_command_arguments_are_passed_correctly(subprocess_mock: MagicMock):
    # arrange
    emulator = Emulator(subprocess_mock)

    rom_file_path = Path('./bin/bios.bin')
    memory_dump_path = Path('memory.dmp')
    exit_label = 'breakpoint_test'

    # act
    emulator.run(
        rom_file_path=rom_file_path,
        memory_dump_path=memory_dump_path,
        exit_label=exit_label
    )
 
    # assert

    subprocess_mock.run.assert_called_once_with(
        'hbc56emu.exe',
        [
            '--rom', str(rom_file_path), 
            '--dump-memory-on-exit', 'memory.dmp', 
            '--exit-label', 'breakpoint_test'
        ]
    )

def test_emulator_command_raises_execution_error_on_nonzero_returncode(subprocess_mock: MagicMock):
    # arrange
    def error_on_running_emulator(command: str, args: list[str]):
        return CompletedProcess(args=args, returncode=123, stdout='', stderr='')

    subprocess_mock.run.side_effect = error_on_running_emulator

    emulator = Emulator(subprocess_mock)

    rom_file_path = Path('./bin/bios.bin')
    memory_dump_path = Path('memory.dmp')
    exit_label = 'breakpoint_test'

    # act
    with pytest.raises(ExecutionError) as execution_error:
        emulator.run(
            rom_file_path=rom_file_path,
            memory_dump_path=memory_dump_path,
            exit_label=exit_label
        )
 
    # assert
    assert execution_error.value.command == 'hbc56emu.exe'
    assert execution_error.value.command_args == [
            '--rom', str(rom_file_path), 
            '--dump-memory-on-exit', 'memory.dmp', 
            '--exit-label', 'breakpoint_test'
        ]
    assert execution_error.value.return_code == 123    
    assert execution_error.value.stderr == ''

def test_emulator_command_raises_execution_error_on_nonempty_stderr(subprocess_mock: MagicMock):
    # arrange
    def error_on_running_emulator(command: str, args: list[str]):
        return CompletedProcess(args=args, returncode=0, stdout='', stderr='abcd')

    subprocess_mock.run.side_effect = error_on_running_emulator

    emulator = Emulator(subprocess_mock)

    rom_file_path = Path('./bin/bios.bin')
    memory_dump_path = Path('memory.dmp')
    exit_label = 'breakpoint_test'

    # act
    with pytest.raises(ExecutionError) as execution_error:
        emulator.run(
            rom_file_path=rom_file_path,
            memory_dump_path=memory_dump_path,
            exit_label=exit_label
        )
 
    # assert
    assert execution_error.value.command == 'hbc56emu.exe'
    assert execution_error.value.command_args == [
            '--rom', str(rom_file_path), 
            '--dump-memory-on-exit', 'memory.dmp', 
            '--exit-label', 'breakpoint_test'
        ]
    assert execution_error.value.return_code == 0    
    assert execution_error.value.stderr == 'abcd'