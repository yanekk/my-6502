@echo off
cd ..
del emulator\6502.Emulator\6502.Emulator.Integration.Tests\test.bin 2>nul
ca65 --cpu 65C02 -I lib -o bin\test.o --feature string_escapes test.s
cl65 -t none -C memory.map.cfg -o emulator\6502.Emulator\6502.Emulator.Integration.Tests\test.bin bin\test.o
del bin\bios.o 2>nul
powershell -Command "$binPath = 'emulator\6502.Emulator\6502.Emulator.Integration.Tests\test.bin'; if(Test-Path $binPath) { $filesize = (Get-Item $binPath).Length; Write-Host -NoNewline 'Assembled. Output size: '; Write-Host $filesize; }"