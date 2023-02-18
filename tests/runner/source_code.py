from collections import defaultdict
from pathlib import Path
from typing import Optional, Union


class SourceCode:
    def __init__(self):
        self.__lines = []

    def include(self, file_path: Union[str, Path]):
        normalized_path = str(file_path).replace('\\', '/')
        self.__lines.append(f'  .include "{normalized_path}"')

    def assign(self, variable: str, value: int):
        lo_byte = value & 0x00FF
        self.__lines.append(f'  LDA #${lo_byte:X}')
        self.__lines.append(f'  STA {variable}')

        hi_byte = (value & 0xFF00) >> 8
        self.__lines.append(f'  LDA #${hi_byte:X}')
        self.__lines.append(f'  STA {variable}+1')

    def jump_to_subroutine(self, subroutine_name: str):
        self.__lines.append(f'  JSR {subroutine_name}')

    def label(self, label_name: str):
        self.__lines.append(f'{label_name}:')

    def nop(self):
        self.__lines.append('  NOP')

    def word(self, label):
        self.__lines.append(f'  .word {label}')

    def is_empty(self) -> bool:
        return len(self.__lines) == 0

    def __str__(self) -> str:
        return '\n'.join(self.__lines)

class SourceFile:
    def __init__(self):
        self.__segment_zero: list[SourceCode] = []
        self.__segments: dict[str, list[SourceCode]] = defaultdict(list)
    
    def append(self, source_code: SourceCode, segment: Optional[str] = None):
        if segment:
            self.__segments[segment].append(source_code)
            return
        self.__segment_zero.append(source_code)

    def __str__(self) -> str:
        lines = []

        non_empty_source_codes = [_ for _ in self.__segment_zero if not _.is_empty()]
        for source_code in non_empty_source_codes:
            lines.append(str(source_code))

        for segment, source_codes in self.__segments.items():
            lines.append(f'  .segment "{segment}"')
            non_empty_source_codes = [_ for _ in source_codes if not _.is_empty()]
            for source_code in non_empty_source_codes:
                lines.append(str(source_code))
        return '\n'.join(lines)

class FixtureSourceFile:
    def __init__(self, test_name):
        self.__source_file = SourceFile()
        
        self.__includes_segment = SourceCode()
        self.__source_file.append(self.__includes_segment, 'CODE')
        
        test_start_segment = SourceCode()
        test_start_segment.label(f'{test_name}_start')
        self.__source_file.append(test_start_segment, 'CODE')

        self.__variable_assignment_segment = SourceCode()
        self.__source_file.append(self.__variable_assignment_segment, 'CODE')

        self.__test_code_segment = SourceCode()
        self.__source_file.append(self.__test_code_segment, 'CODE')
        
        self.__code_segments: list[SourceCode] = [
            self.__includes_segment,
            self.__variable_assignment_segment,
            self.__test_code_segment]

        self.exit_label = f'{test_name}_end'
        test_end_segment = SourceCode()
        test_end_segment.label(self.exit_label)
        test_end_segment.nop()
        self.__source_file.append(test_end_segment, 'CODE')

        init_segment = SourceCode()
        init_segment.word(f'{test_name}_start')
        self.__source_file.append(init_segment, 'INIT')

    def assign_variables(self, **variables):
        for variable, value in variables.items():
            self.__variable_assignment_segment.assign(variable, value)

    def include_code(self, *source_file_paths: Union[str, Path]):
        for source_file_path in source_file_paths:
            self.__includes_segment.include(source_file_path)

    def jump_to_subroutine(self, subroutine_name):
        self.__test_code_segment.jump_to_subroutine(subroutine_name)
        
    def __str__(self) -> str:
        if all([_.is_empty() for _ in self.__code_segments]):
            self.__test_code_segment.nop()

        return str(self.__source_file)