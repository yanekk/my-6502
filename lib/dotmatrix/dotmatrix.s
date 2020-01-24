current_page = $02
current_line = $03

current_segment = $04
current_segment_h = $05

current_splash_byte = $06
current_splash_byte_h = $07

dotmatrix_initialize:
  LDA #<DOTMATRIX_SEG1
  STA current_segment
  LDA #>DOTMATRIX_SEG1
  STA current_segment+1
  JSR @dotmatrix_initialize_segment

  LDA #<DOTMATRIX_SEG2
  STA current_segment
  LDA #>DOTMATRIX_SEG2
  STA current_segment+1
  JSR @dotmatrix_initialize_segment
  
  RTS

@dotmatrix_initialize_segment:  
  LDA #%00111111 ; Turn display ON
  LDY #0
  STA (current_segment), Y

  LDY #0
  STY current_page

@write_page:
  LDY current_page
  CPY #8
  BEQ @dotmatrix_initialize_end

  LDA #%10111000
  ORA current_page
  LDY #0 ; command
  STA (current_segment), Y

  LDY current_page
  INY
  STY current_page

  LDX #$0
  STX current_line

@write_line:
  LDX current_line

  CPX #$40
  BEQ @write_page

  LDA #%11111111
  LDY #1 ; data
  STA (current_segment), Y

  LDX current_line
  INX 
  STX current_line
  JMP @write_line

@dotmatrix_initialize_end:
  RTS

splash_01: .byte %11000011, %00001100, %00110000, %11000011
splash_02: .byte %00100100, %10010010, %01001001, %00100100
splash_03: .byte %00011000, %01100001, %10000110, %00011000
splash_04: .byte %10001000, %10001000, %10001000, %10001000
splash_05: .byte %01010101, %01010101, %01010101, %01010101
splash_06: .byte %00100010, %00100010, %00100010, %00100010
splash_07: .byte %11100001, %11111000, %01111110, %00011111
splash_08: .byte %01110011, %10011100, %11100111, %00111000
splash_09: .byte %00111111, %00001111, %11000011, %11110000
splash_10: .byte %00000000, %00000000, %00000000, %00000000
splash_11: .byte %00000000, %00000000, %00000000, %00000000
splash_12: .byte %00000000, %00000000, %00000000, %00000000
splash_13: .byte %00000000, %00000000, %00000000, %00000000
splash_14: .byte %00000000, %00000000, %00000000, %00000000
splash_15: .byte %00000000, %00000000, %00000000, %00000000
splash_16: .byte %00000000, %00000000, %00000000, %00000000