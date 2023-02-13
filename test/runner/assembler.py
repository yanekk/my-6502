from pathlib import Path

from .subprocess import Subprocess, has_execution_error, build_execution_error

class Assembler:
    def __init__(self, subprocess: Subprocess):
        self.__subprocess = subprocess

    def assemble(self, source_code_path: Path):
        ca65_args = ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path)]
        completed_process = self.__subprocess.run('ca65', ca65_args)
        if has_execution_error(completed_process):
            raise build_execution_error('ca65', completed_process)
                
        cl65_args = ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))]
        completed_process = self.__subprocess.run('cl65', cl65_args)
        if has_execution_error(completed_process):
            raise build_execution_error('cl65', completed_process)
