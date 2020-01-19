; general equates
LCD_COMMAND .equ $00 ; 00000000
LCD_CHAR    .equ $01 ; 00000001

LCD_ENABLE  .equ $04 ; 00000100

LCD_FUNCTION_SET      .equ $3b ; 00111011
LCD_CMD_DISPLAY_ON    .equ $0c ; 00001100
LCD_CMD_DISPLAY_CLEAR .equ $01 ; 00000001
LCD_ENTRYMODE_SET     .equ $06 ; 00000110

; variables
write_line_pointer = $00

lcd_initialize:
  LDA #$70 ; wait 101ms
  JSR wait
  
  LDX #LCD_FUNCTION_SET
  JSR lcd_write_command
  
  LDA #$05 ; wait 5ms
  JSR wait
  
  LDX #LCD_FUNCTION_SET
  JSR lcd_write_command
  
  LDA #$01 ; wait 1ms
  JSR wait
  
  LDX #LCD_FUNCTION_SET
  JSR lcd_write_command
  
  LDA #$01 ; wait 1ms
  JSR wait
  
  LDX #LCD_FUNCTION_SET
  JSR lcd_write_command
  
  LDA #$01 ; wait 1ms
  JSR wait
  
  LDX #LCD_CMD_DISPLAY_ON
  JSR lcd_write_command
  
  LDA #$01 ; wait 1ms
  JSR wait
  
  LDX #LCD_CMD_DISPLAY_CLEAR
  JSR lcd_write_command
  
  LDA #$01 ; wait 1ms
  JSR wait
  
  LDX #LCD_ENTRYMODE_SET
  JSR lcd_write_command
  
  LDA #$01 ; wait 1ms
  JSR wait
  RTS
  
lcd_clear:
  LDX #LCD_CMD_DISPLAY_CLEAR
  JSR lcd_write_command
  
  LDA #$02 ; wait 5ms
  JSR wait
  
  RTS
  
lcd_write_line:
  STA write_line_pointer
  STX write_line_pointer+1
  
  LDY #$00
.write_next_line1_char:
  LDA (write_line_pointer), Y
  TAX
  CPX #$00
  BEQ .write_line_return
  JSR lcd_write_char
  INY
  JMP .write_next_line1_char
.write_line_return:
  RTS
  
lcd_write_char:
  LDA #LCD_CHAR
  AND #$07     ; mask 00000111
  STA VIA_PB
  STX VIA_PA
  JSR lcd_pulse_enable
  RTS
  
lcd_write_command:
  LDA #LCD_COMMAND
  STA VIA_PB
  STX VIA_PA
  JSR lcd_pulse_enable
  RTS
  
lcd_pulse_enable:
  EOR #LCD_ENABLE
  STA VIA_PB
  NOP ; wait 1 microsecond

  EOR #LCD_ENABLE
  STA VIA_PB
  
  PHA
  LDA #$01 ; wait 1 milsecond
  JSR wait
  PLA
  
  RTS