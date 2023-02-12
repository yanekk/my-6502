from pathlib import Path

from .subprocess import Subprocess, ExecutionError

class Assembler:
    def __init__(self, subprocess: Subprocess):
        self.__subprocess = subprocess

    def assemble(self, source_code_path: Path):
        ca65_args = ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path)]
        completed_process = self.__subprocess.run('ca65', ca65_args)
        if completed_process.returncode or completed_process.stderr:
            raise ExecutionError(
                command='ca65', command_args=ca65_args,
                return_code=completed_process.returncode,
                stderr=completed_process.stderr)
                
        cl65_args = ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))]
        completed_process = self.__subprocess.run('cl65', cl65_args)
        if completed_process.returncode or completed_process.stderr:
            raise ExecutionError(
                command='cl65', command_args=cl65_args,
                return_code=completed_process.returncode,
                stderr=completed_process.stderr)
