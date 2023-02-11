from pathlib import Path
from subprocess import CompletedProcess
import subprocess
from typing import Protocol


class Subprocess(Protocol):
    def run(self, command: str, args: list[str]) -> CompletedProcess:
        ...

class RealSubprocess:
    def run(self, command: str, args: list[str]) -> CompletedProcess:
        return subprocess.run([command] + args, capture_output=True)

class Assembler:
    def __init__(self, subprocess: Subprocess):
        self.__subprocess = subprocess

    def assemble(self, source_code_path: Path):
        self.__subprocess.run('ca65',
            ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path)]
        )
        self.__subprocess.run('cl65',
            ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))]
        )
