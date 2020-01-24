@echo off
cd ..
del bin\bootloader.bin 2>nul
ca65 --cpu 65C02 -I lib -o bin\bootloader.o --feature string_escapes bootloader.s
cl65 -t none -o bin\bootloader.bin bin\bootloader.o
del bin\bootloader.o 2>nul
powershell -Command "$binPath = 'bin\bootloader.bin'; if(Test-Path $binPath) { $filesize = (Get-Item $binPath).Length; Write-Host -NoNewline 'Assembled. Output size: '; Write-Host $filesize; }"