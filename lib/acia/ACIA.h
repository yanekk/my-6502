ACIA: .equ $4800
ACIA_DATA:    .EQU	ACIA + $00		; Data register
ACIA_STATUS:  .EQU	ACIA + $01		; Status register
ACIA_COMMAND: .EQU	ACIA + $02		; Command register
ACIA_CONTROL: .EQU	ACIA + $03		; Control register