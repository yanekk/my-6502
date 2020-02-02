  .INCLUDE "dotmatrix/dotmatrix.h"
  .INCLUDE "bios.h"
  .INCLUDE "utils/macros.s"

  .ORG $0200

shift_line = $11

init:
  ;LDY #lcd_initialize
  ;JSR call_subroutine

  ;LDY #lcd_clear
  ;JSR call_subroutine

  LDA #<line1
  LDX #>line1
  call sub_lcd_write_line

  ;LDY #acia_initialize
  ;JSR call_subroutine

  ;LDA #<acia_initialized
  ;LDX #>acia_initialized
  ;LDY #acia_write_line
  ;JSR call_subroutine

  JSR dotmatrix_initialize

  LDA #$00
  JSR dotmatrix_clear
  JSR dotmatrix_splash_initialize
  
reset_shift_line:
  LDA #64
  STA shift_line
splash:
  JSR dotmatrix_splash
  DEC shift_line

  LDA #32
  call sub_wait

  LDA shift_line
  CMP #0
  BEQ reset_shift_line
  JMP splash

do_nothing:
  NOP
  JMP do_nothing
  .INCLUDE "dotmatrix/dotmatrix.s"
  
line1: .ASCIIZ "Hello world!"
;acia_initialized: .ASCIIZ "ACIA initialized.\r\n"