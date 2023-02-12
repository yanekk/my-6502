from .subprocess import ExecutionError

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
