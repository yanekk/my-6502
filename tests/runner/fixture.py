from pathlib import Path
from tempfile import TemporaryDirectory
from typing import Any
import uuid

from .label_file import LabelFile
from .source_code import FixtureSourceFile
from .assembler import Assembler, Linker
from .emulator import Emulator

class Fixture:
    def __init__(self, source_file: FixtureSourceFile):
        self._source_file = source_file
    
    def __setattr__(self, name: str, value: Any) -> None:
        if(name.startswith('_')):
            self.__dict__[name] = value
            return

        self._source_file.assign_variables(**{
            name: value
        })

    def __getattr__(self, name: str):
        self._source_file.jump_to_subroutine(name)
        return lambda: 0

class FixtureResult:
    def __init__(self, memory: bytes, label_file: LabelFile):
        self._memory = memory
        self._label_file = label_file

    def __getattr__(self, name: str):
        return self._memory[self._label_file.address_at(name)]

class FixtureExecutor:
    def __init__(self, assembler: Assembler, linker: Linker, emulator: Emulator):
        self.__assembler = assembler
        self.__linker = linker
        self.__emulator = emulator

    def execute(self, source_code: FixtureSourceFile) -> FixtureResult:
        test_id = str(uuid.uuid4())

        with TemporaryDirectory() as temp_dir:
            source_file_path = Path(temp_dir) / f'{test_id}.s'
            source_file_path.write_text(str(source_code))

            object_file_path = self.__assembler.assemble(source_file_path)
            linker_result = self.__linker.link(object_file_path)

            with open(linker_result.label_file_path, 'rt') as label_file_contents:
                label_file = LabelFile.parse(label_file_contents)

            memory_dump_path = source_file_path.with_suffix('.dmp')
            self.__emulator.run(
                linker_result.binary_file_path,
                memory_dump_path,
                label_file.address_at(source_code.exit_label))

            with open(memory_dump_path, 'rb') as memory_dump:
                return FixtureResult(memory_dump.read(), label_file)