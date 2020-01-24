ptr1 = $FE
ptr1_h = $FF

  .include "cfcard/cfcard.h"
  .include "via/via.h"
  .include "lcd/lcd.h"
  .include "acia/acia.h"

CFSECT_BUFF_FIRST_HALF  = $3e00 
CFSECT_BUFF_SECOND_HALF = CFSECT_BUFF_FIRST_HALF + $FF

  .segment "CODE"
  .include "cfcard/cfcard.s"
  .include "lcd/lcd.s"
  .include "utils/wait.s"
  .include "acia/acia.s"
  
  .segment "CODE"
reset: 
  JSR CF_INIT
  
  LDA #$3
  STA CFLBA0_BUFF
  LDA #$0
  STA CFLBA1_BUFF
  LDA #$0
  STA CFLBA2_BUFF
  JSR CF_READ_SECTOR
  
  JMP CFSECT_BUFF_FIRST_HALF
  
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
  .word $0000