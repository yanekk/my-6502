from dataclasses import dataclass
from pathlib import Path

from .subprocess import Subprocess, subprocess_execution_error, has_subprocess_failed

class Assembler:
    def __init__(self, subprocess: Subprocess):
        self.__subprocess = subprocess
        self.__include_paths = []

    def add_include_path(self, path: str):
        self.__include_paths.append(str(Path.cwd() / Path(path)).replace('\\', '/'))
        pass

    def assemble(self, source_code_path: Path):
        work_dir = str(source_code_path.parent)
        source_file = source_code_path.name
        ca65_args = ['-g', '--cpu', '65C02']

        object_file_path = source_code_path.with_suffix('.o')
        for include_path in self.__include_paths:
            ca65_args += ['-I', include_path]
        ca65_args += ['-o', str(object_file_path), source_file]

        completed_process = self.__subprocess.run('ca65', ca65_args, work_dir)
        if has_subprocess_failed(completed_process):
            raise subprocess_execution_error('ca65', completed_process)

        return object_file_path
@dataclass
class LinkerResult:
    binary_file_path: Path
    label_file_path: Path
    
class Linker:
    def __init__(self, subprocess: Subprocess):
        self.__subprocess = subprocess
        self.__config_file = None

    def set_config_file(self, path: str):
        self.__config_file = str(Path.cwd() / Path(path)).replace('\\', '/')

    def link(self, object_file_path: Path):
        
        cl65_args = []
        if self.__config_file:
            cl65_args += ['-C', self.__config_file]

        result = LinkerResult(
            binary_file_path=object_file_path.with_suffix('.bin'),
            label_file_path=object_file_path.with_suffix('.bin.lmap')
        )

        cl65_args += ['-t', 'none', '-Ln', str(result.label_file_path), '-o', str(result.binary_file_path), str(object_file_path)]
        completed_process = self.__subprocess.run('cl65', cl65_args)
        if has_subprocess_failed(completed_process):
            raise subprocess_execution_error('cl65', completed_process)
        return result