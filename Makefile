build_bios: 
	vasm6502_oldstyle -Fbin -Ilib -dotdir -o bios.bin bios.s
build_program:
	vasm6502_oldstyle -Fbin -Ilib -dotdir -o program.bin program.s
write_program:
	python3 tools\write_sector.py program.bin
