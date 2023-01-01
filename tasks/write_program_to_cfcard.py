import os
import sys
import msvcrt
import win32file
import winioctlcon
import contextlib
import math

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
sectorNo = int(sys.argv[2])
driveNumber = 2

file = open(sys.argv[1], "rb")
contents = file.read()
fileSize = os.stat(sys.argv[1]).st_size
dataSize = math.ceil(fileSize/sectorSize) * sectorSize
data = bytearray(dataSize)

for i in range(len(contents)):
  data[i] = contents[i]
  
with cf_card_file("bin/cf_card.bin") as disk:
  disk.seek(sectorNo * sectorSize)
  disk.write(data)