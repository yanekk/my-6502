  .INCLUDE "dotmatrix/dotmatrix.h"
  .INCLUDE "bios.h"
  .INCLUDE "utils/macros.s"

  .ORG $0200
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
splash:
  LDA #$FF
  JSR dotmatrix_clear
  LDA #60
  call sub_wait

  ;LDA #$00
  JSR dotmatrix_splash
  LDA #60
  call sub_wait
  
  JMP splash

do_nothing:
  NOP
  JMP do_nothing
  .INCLUDE "dotmatrix/dotmatrix.s"
  
line1: .ASCIIZ "Hello world!"
;acia_initialized: .ASCIIZ "ACIA initialized.\r\n"