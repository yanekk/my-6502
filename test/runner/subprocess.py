
from subprocess import CompletedProcess
import subprocess
from typing import Protocol


class Subprocess(Protocol):
    def run(self, command: str, args: list[str]) -> CompletedProcess:
        ...


class RealSubprocess:
    def run(self, command: str, args: list[str]) -> CompletedProcess:
        return subprocess.run([command] + args, capture_output=True)


class ExecutionError(Exception):
    def __init__(self, command: str, stderr: str, return_code: int, command_args: list[str]):
        self.command = command
        self.stderr = stderr
        self.return_code = return_code
        self.command_args = command_args

        msg = '\n'.join([
            f'Error while running {command}:',
            '==stderr==',
            stderr if stderr else '(no output)',
            '==========',
            f'Return code: {return_code}',
            f'Arguments used: {" ".join(command_args)}'
        ])

        super().__init__(msg)