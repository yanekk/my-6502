from sys import argv
from pathlib import Path

lmap_lines = []
for lmap_file_path in Path('.').rglob('bin/*.bin.lmap'):
    with open(lmap_file_path, 'rt', encoding='utf-8') as lmap_file:
        while line := lmap_file.readline():
            if not line.startswith('al'):
                lmap_lines.append(f'{line}\n')
                continue

            line = line.split(' ')
            label = line[2][1:].strip()
            address = line[1][2:]
            if not label.startswith('__'):
                lmap_lines.append(f'{label} = ${address}\n')
              

with open('bin/bios.bin.lmap', 'wt', encoding='utf-8') as lmap_file:
    lmap_file.writelines(lmap_lines)
