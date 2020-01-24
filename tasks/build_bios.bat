@echo off
cd ..
del bin\bios.bin 2>nul
ca65 --cpu 65C02 -I lib -o bin\bios.o --feature string_escapes bios.s
cl65 -t none -C memory.map.cfg -o bin\bios.bin bin\bios.o
del bin\bios.o 2>nul
powershell -Command "$binPath = 'bin\bios.bin'; if(Test-Path $binPath) { $filesize = (Get-Item $binPath).Length; Write-Host -NoNewline 'Assembled. Output size: '; Write-Host $filesize; }"