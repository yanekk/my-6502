from subprocess import CompletedProcess

import pytest

from runner.subprocess import ExecutionError, has_subprocess_failed, subprocess_execution_error

def test_assembly_error_builds_message_with_stderr():
    # arrange
    assembly_error = ExecutionError(
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
        '==stderr==',
        'error message',
        '==========',
        'Return code: 123',
        'Arguments used: a b c'
    ]

def test_assembly_error_builds_message_without_stderr():
    # arrange
    assembly_error = ExecutionError(
        command='ca65',
        stderr='',
        return_code=123,
        command_args=['a', 'b', 'c']
    )

    # act
    error_message = str(assembly_error)

    # assert
    assert error_message.splitlines() == [
        'Error while running ca65:',
        '==stderr==',
        '(no output)',
        '==========',
        'Return code: 123',
        'Arguments used: a b c'
    ]

@pytest.mark.parametrize(['returncode', 'stderr'], [
    [1, b''],
    [0, b'error']
])
def test_has_error_on_nonzero_exitcode(returncode, stderr):
    # arrange
    failed_process = CompletedProcess(args=[], returncode=returncode, stderr=stderr)

    # act & assert
    assert has_subprocess_failed(failed_process)

def test_has_no_error_on_empty_stderr_and_zero_returncode():
    # arrange
    successful_process = CompletedProcess(args=[], returncode=0, stderr='')

    # act & assert
    assert not has_subprocess_failed(successful_process)

def test_builds_error_from_completed_process():
    # arrange
    completed_process = CompletedProcess(['a', 'b', 'c'], 123, b'stdout', b'stderr')

    # act
    error = subprocess_execution_error('program.exe', completed_process)

    # assert
    assert error.command == 'program.exe'
    assert error.stderr  == 'stderr'
    assert error.command_args == ['a', 'b', 'c']
    assert error.return_code == 123