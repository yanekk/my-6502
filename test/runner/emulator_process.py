def _build_command_line_arguments(rom_file_path: str, memory_dump_path: str, exit_label: str):
    return [
        '--rom', rom_file_path, 
        '--dump-memory-on-exit', memory_dump_path, 
        '--exit-label', exit_label
    ]