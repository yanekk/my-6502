from pathlib import Path
from .subprocess import Subprocess, has_subprocess_failed, subprocess_execution_error

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
        if has_subprocess_failed(result):
            raise subprocess_execution_error('hbc56emu.exe', result)