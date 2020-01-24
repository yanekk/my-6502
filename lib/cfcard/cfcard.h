CFBASE  = $5000
CFDATA  = CFBASE + $00		; Data (R/W)
CFERR   = CFBASE + $01		; Error register (R)
CFFEAT  = CFBASE + $01		; Features (W)
CFSECCO = CFBASE + $02		; Sector count (R/W)
CFLBA0  = CFBASE + $03		; LBA bits 0-7 (R/W, LBA mode)
CFLBA1  = CFBASE + $04		; LBA bits 8-15 (R/W, LBA mode)
CFLBA2  = CFBASE + $05		; LBA bits 16-23 (R/W, LBA mode)
CFLBA3  = CFBASE + $06		; LBA bits 24-27 (R/W, LBA mode)
CFSTAT  = CFBASE + $07		; Status (R)
CFCMD   = CFBASE + $07		; Command (W)

CFLBA0_BUFF = $00
CFLBA1_BUFF = $01
CFLBA2_BUFF = $02