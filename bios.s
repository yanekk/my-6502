interrupt_timeout = $FD

ptr1 = $FE
ptr1_h = $FF

  .include "utils/macros.inc"
  .include "cfcard/cfcard.inc"
  .include "via/via.inc"
  .include "lcd/lcd.inc"
  .include "acia/acia.inc"
  .include "bios.inc"

INIT  = $3e00 

  .segment "CODE"
  .include "lcd/lcd.s"
  .include "utils/wait.s"
  .include "acia/acia.s"
  .include "cfcard/cfcard.s"
  
  .segment "CODE"
reset:
  CLD
  LDA #0
  STA interrupt_timeout

;  call sub_lcd_initialize
;  call sub_lcd_clear

  JSR CF_INIT

  LDA #$1
  STA CFSECCO_BUFF
  LDA #$3
  STA CFLBA0_BUFF
  LDA #$0
  STA CFLBA1_BUFF
  LDA #$0
  STA CFLBA2_BUFF
  LDX #<INIT
  LDY #>INIT
  JSR CF_READ_SECTOR

  JMP INIT

irq_handler:
  LDA #1
  STA interrupt_timeout
  STA VIA_T1CH
  RTI

  .segment "API"
call_subroutine:
  PHA
  LDA funcs_low, Y
  STA ptr1
  LDA funcs_high, Y
  STA ptr1_h
  PLA
  JMP (ptr1)

funcs_low:
  .byte <lcd_initialize, <lcd_clear, <lcd_write_line, <wait, <acia_initialize, <acia_write_char, <acia_write_line
funcs_high:
  .byte >lcd_initialize, >lcd_clear, >lcd_write_line, >wait, >acia_initialize, >acia_write_char, >acia_write_line

  .segment "INIT" 
  .word reset
  .word irq_handler