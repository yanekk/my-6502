.PHONY: build_bios build_bootloader build_program

build: build_bios build_bootloader build_program write_cfcard_file convert_lmap_files

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

write_cfcard_file:
	python .\tasks\write_all_to_cfcard_file.py .\bin\cf_card.bin

convert_lmap_files:
	python .\scripts\convert_lmap.py

piskel_to_binary:
	python scripts/piskel_to_binary/piskel_to_binary.py "data/$(FILE).piskel"