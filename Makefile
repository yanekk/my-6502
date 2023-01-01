.PHONY: build_bios build_bootloader build_program

build_bios:
	sh scripts/assemble_file.sh -f bios.s -m memory.map.cfg

build_bootloader:
	sh scripts/assemble_file.sh -f bootloader.s

build_program:
	sh scripts/assemble_file.sh -f program.s

write_bootloader:
	python .\tasks\write_program_to_cfcard.py .\bin\bootloader.bin 3
	
write_program:
	python .\tasks\write_program_to_cfcard.py .\bin\program.bin 4