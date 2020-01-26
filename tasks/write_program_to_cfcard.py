import os
import sys
import msvcrt
import win32file
import winioctlcon
import contextlib
import math

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
  
with open(fr"\\.\F:", 'r+b') as disk:
  with lock_volume(disk):
    disk.seek(sectorNo * sectorSize)
    disk.write(data)