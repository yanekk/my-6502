from collections import defaultdict


class SourceCode:
    def __init__(self):
        self.__lines = []

    def include(self, file_path: str):
        self.__lines.append(f'  .include "{file_path}"')

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

    def __str__(self) -> str:
        return '\n'.join(self.__lines)

class SourceFile:
    def __init__(self):
        self.__segments: dict[str, list[SourceCode]] = defaultdict(list)
    
    def append(self, segment: str, source_code: SourceCode):
        self.__segments[segment].append(source_code)    

    def __str__(self) -> str:
        lines = []
        for segment, source_codes in self.__segments.items():
            lines.append(f'  .segment "{segment}"')
            for source_code in source_codes:
                lines.append(str(source_code))
        return '\n'.join(lines)