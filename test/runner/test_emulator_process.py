from .emulator_process import _build_command_line_arguments

def test_emulator_command_arguments_are_generated_correctly():
    # act
    command_line_arguments = _build_command_line_arguments(
        rom_file_path='./bin/bios.bin',
        memory_dump_path='memory.dmp',
        exit_label='breakpoint_test')
        
    # assert
    assert command_line_arguments == [
        '--rom', './bin/bios.bin', 
        '--dump-memory-on-exit', 'memory.dmp', 
        '--exit-label', 'breakpoint_test'
    ]
