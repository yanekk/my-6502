from pathlib import Path
from subprocess import CompletedProcess
import subprocess
from typing import Protocol

class AssemblyError(Exception):
    def __init__(self, command: str, stderr: str, return_code: int, command_args: list[str]):
        self.command = command
        self.stderr = stderr
        self.return_code = return_code
        self.command_args = command_args

        msg = '\n'.join([
            f'Error while running {command}:',
            '===',
            stderr,
            '===',
            f'Return code: {return_code}',
            f'Arguments used: {" ".join(command_args)}'
        ])

        super().__init__(msg)
    pass

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
        ca65_args = ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path)]
        completed_process = self.__subprocess.run('ca65', ca65_args)
        if completed_process.returncode or completed_process.stderr:
            raise AssemblyError(
                command='ca65', command_args=ca65_args,
                return_code=completed_process.returncode,
                stderr=completed_process.stderr)

        self.__subprocess.run('cl65',
            ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))]
        )
