
from subprocess import CompletedProcess
import subprocess
from typing import Optional, Protocol


class Subprocess(Protocol):
    def run(self, command: str, args: list[str], work_dir: Optional[str] = None) -> CompletedProcess:
        ...


class RealSubprocess:
    def run(self, command: str, args: list[str], work_dir: Optional[str] = None) -> CompletedProcess:
        kwargs = {}
        if work_dir:
            kwargs['cwd'] = work_dir

        return subprocess.run([command] + args, capture_output=True, **kwargs)


class ExecutionError(Exception):
    def __init__(self, command: str, stderr: str, return_code: int, command_args: list[str]):
        self.command = command
        self.stderr = stderr
        self.return_code = return_code
        self.command_args = command_args
        #TODO: add current working directory

        msg = '\n'.join([
            f'Error while running {command}:',
            '==stderr==',
            stderr if stderr else '(no output)',
            '==========',
            f'Return code: {return_code}',
            f'Arguments used: {" ".join(command_args)}'
        ])

        super().__init__(msg)

def has_subprocess_failed(completed_process: CompletedProcess):
    return completed_process.returncode != 0 or bool(completed_process.stderr)

def subprocess_execution_error(program_path: str, completed_process: CompletedProcess):
    return ExecutionError(
        command=program_path,
        stderr=completed_process.stderr.decode(),
        return_code=completed_process.returncode,
        command_args=completed_process.args
    )
