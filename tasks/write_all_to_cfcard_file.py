import io
import os
import sys
import msvcrt
import win32file
import winioctlcon
import contextlib
import math

files = [
  {
    'filename': 'bootloader.bin',
    'sectorNo': 3
  },
  {
    'filename': 'program.bin',
    'sectorNo': 4
  },
]

@contextlib.contextmanager
def cf_card():
  with open(fr"\\.\F:", 'r+b') as disk:
    with lock_volume(disk):
      yield disk

@contextlib.contextmanager
def cf_card_file(file_name):
  with open(file_name, 'wb') as disk_file:
    yield disk_file

@contextlib.contextmanager
def lock_volume(vol):
    hVol = msvcrt.get_osfhandle(vol.fileno())
    win32file.DeviceIoControl(hVol, winioctlcon.FSCTL_LOCK_VOLUME,
                              None, None)
                              
    try:
        yield vol
    finally:
        try:
            vol.flush()
        finally:
            win32file.DeviceIoControl(hVol, winioctlcon.FSCTL_UNLOCK_VOLUME,
                                      None, None)

sectorSize = 512
driveNumber = 2
sectorsNeeded = 0

for file_entry in files:
  with open('bin/'+file_entry['filename'], "rb") as file:
    contents = file.read()
  
  fileSize = os.stat(sys.argv[1]).st_size
  fileSectors = math.ceil(fileSize/sectorSize)

  fileSectors = fileSectors + file_entry['sectorNo']
  if(fileSectors > sectorsNeeded):
    sectorsNeeded = fileSectors

  file_entry['data'] = contents


buffer = io.BytesIO(b'\0' * (sectorsNeeded * sectorSize))
for file_entry in files:
  buffer.seek(file_entry['sectorNo'] * sectorSize)
  buffer.write(file_entry['data'])

with cf_card_file("bin/cf_card.bin") as disk:
  disk.write(buffer.getbuffer())