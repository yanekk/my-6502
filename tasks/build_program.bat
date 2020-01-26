@echo off
cd ..
del bin\program.bin 2>nul
ca65 --cpu 65C02 -I lib -o bin\program.o --feature string_escapes program.s
cl65 -t none -o bin\program.bin bin\program.o
del bin\program.o 2>nul
powershell -Command "$binPath = 'bin\program.bin'; if(Test-Path $binPath) { $filesize = (Get-Item $binPath).Length; Write-Host -NoNewline 'Assembled. Output size: '; Write-Host $filesize; }"