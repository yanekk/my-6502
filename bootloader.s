  .include "cfcard/cfcard.inc"
  .include "utils/macros.inc"
  .INCLUDE "bios.inc"
  .ORG $3e00

init:
  call sub_acia_initialize

  LDA #<loading
  LDX #>loading
  call sub_acia_write_line

;  call sub_lcd_initialize
;  call sub_lcd_clear

;  LDA #<loading
;  LDX #>loading
;  call sub_lcd_write_line

  LDA #$3
  STA CFSECCO_BUFF
  LDA #$4
  STA CFLBA0_BUFF
  LDA #$0
  STA CFLBA1_BUFF
  LDA #$0
  STA CFLBA2_BUFF
  LDX #<$0200
  LDY #>$0200
  JSR CF_READ_SECTOR

;  call sub_lcd_clear

  JMP $0200

  .include "cfcard/cfcard.s"
loading: .ASCIIZ "Loading..."