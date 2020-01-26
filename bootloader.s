  .INCLUDE "dotmatrix/dotmatrix.h"
  .INCLUDE "bios.h"

  .ORG $3e00
  
init:
  ;LDY #lcd_initialize
  ;JSR call_subroutine

  ;LDY #lcd_clear
  ;JSR call_subroutine

  ;LDA #<line1
  ;LDX #>line1
  ;LDY #lcd_write_line
  ;JSR call_subroutine

  ;LDY #acia_initialize
  ;JSR call_subroutine

  ;LDA #<acia_initialized
  ;LDX #>acia_initialized
  ;LDY #acia_write_line
  ;JSR call_subroutine

  JSR dotmatrix_initialize
  JSR dotmatrix_splash
  
do_nothing:
  NOP
  JMP do_nothing
  .INCLUDE "dotmatrix/dotmatrix.s"
  
;line1: .ASCIIZ "Sending data..."
;acia_initialized: .ASCIIZ "ACIA initialized.\r\n"