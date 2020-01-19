CFBASE: .equ $5000
CFDATA:		.EQU	CFBASE + $00		; Data (R/W)
CFERR:		.EQU	CFBASE + $01		; Error register (R)
CFFEAT:		.EQU	CFBASE + $01		; Features (W)
CFSECCO:	.EQU	CFBASE + $02		; Sector count (R/W)
CFLBA0:		.EQU	CFBASE + $03		; LBA bits 0-7 (R/W, LBA mode)
CFLBA1:		.EQU	CFBASE + $04		; LBA bits 8-15 (R/W, LBA mode)
CFLBA2:		.EQU	CFBASE + $05		; LBA bits 16-23 (R/W, LBA mode)
CFLBA3:		.EQU	CFBASE + $06		; LBA bits 24-27 (R/W, LBA mode)
CFSTAT:		.EQU	CFBASE + $07		; Status (R)
CFCMD:		.EQU	CFBASE + $07		; Command (W)

CFLBA0_BUFF: .EQU $0400
CFLBA1_BUFF: .EQU $0401
CFLBA2_BUFF: .EQU $0402