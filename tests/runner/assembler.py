from pathlib import Path

from .subprocess import Subprocess, subprocess_execution_error, has_subprocess_failed

class Assembler:
    def __init__(self, subprocess: Subprocess):
        self.__subprocess = subprocess
        self.__include_paths = []
        self.__config_file = None

    def add_include_path(self, path: str):
        self.__include_paths.append(str(Path.cwd() / Path(path)).replace('\\', '/'))
        pass

    def set_config_file(self, path: str):
        self.__config_file = str(Path.cwd() / Path(path)).replace('\\', '/')

    def assemble(self, source_code_path: Path):
        work_dir = str(source_code_path.parent)
        source_file = source_code_path.name
        ca65_args = ['--cpu', '65C02']

        for include_path in self.__include_paths:
            ca65_args += ['-I', include_path]
        ca65_args += ['-o', str(source_code_path.with_suffix('.o')), source_file]

        completed_process = self.__subprocess.run('ca65', ca65_args, work_dir)
        if has_subprocess_failed(completed_process):
            raise subprocess_execution_error('ca65', completed_process)

        cl65_args = []
        if self.__config_file:
            cl65_args += ['-C', self.__config_file]

        cl65_args += ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))]
        completed_process = self.__subprocess.run('cl65', cl65_args)
        if has_subprocess_failed(completed_process):
            raise subprocess_execution_error('cl65', completed_process)
