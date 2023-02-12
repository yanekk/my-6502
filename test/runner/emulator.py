from pathlib import Path
from .subprocess import ExecutionError, Subprocess

class Emulator:
    def __init__(self, subprocess: Subprocess):
        self.__subprocess = subprocess

    def run(self, rom_file_path: Path, memory_dump_path: Path, exit_label: str):
        args = [
            '--rom', str(rom_file_path), 
            '--dump-memory-on-exit', str(memory_dump_path), 
            '--exit-label', exit_label
        ]
        result = self.__subprocess.run('hbc56emu.exe', args)
        if result.returncode > 0 or result.stderr:
            raise ExecutionError(
                command='hbc56emu.exe', 
                command_args=args,
                return_code=result.returncode,
                stderr=result.stderr)