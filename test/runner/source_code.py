from collections import UserList


class SourceCode(UserList[str]):
    def include(self, file_path: str):
        self.append(f'  .include "{file_path}"')

    def assign(self, variable: str, value: int):
        lo_byte = value & 0x00FF
        self.append(f'  LDA #${lo_byte:X}')
        self.append(f'  STA {variable}')

        hi_byte = (value & 0xFF00) >> 8
        self.append(f'  LDA #${hi_byte:X}')
        self.append(f'  STA {variable}+1')

    def jump_to_subroutine(self, subroutine_name: str):
        self.append(f'  JSR {subroutine_name}')

    def label(self, label_name: str):
        self.append(f'{label_name}:')

    def nop(self):
        self.append('  NOP')