  .INCLUDE "via/via.h"
  .INCLUDE "acia/ACIA.h"
  .INCLUDE "dotmatrix/dotmatrix.h"

  .ORG $0200
  
init:
  ;LDA #$ff       ; set all LCD 
  ;STA VIA_DDRA   ; data lines to output
  
  ;LDA #$07       ; set first three B lines to output
  ;STA VIA_DDRB   ; set all B lines to output
  
  ;JSR lcd_initialize
  ;JSR lcd_clear
  
  ;LDA #<line1
  ;LDX #>line1
  ;JSR lcd_write_line

  JSR acia_initialize

  LDA #<acia_initialized
  LDX #>acia_initialized
  JSR acia_write_line
  JSR dotmatrix_initialize
  
do_nothing:
  NOP
  JMP do_nothing
  
  .include "acia/ACIA.s"
  .include "utils/wait.s"
  ;.INCLUDE "lcd/lcd.s"
  .INCLUDE "dotmatrix/dotmatrix.s"
  
line1 .ASCII "Sending data..."
acia_initialized .ASCII "ACIA initialized.\r\n"
acia_initialized2 .ASCII "ACIA initialized.\r\n"
a .byte "a"