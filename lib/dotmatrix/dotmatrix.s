current_page .equ $02
current_line .equ $03

current_segment .equ $04
current_segment_h .equ $05

dotmatrix_initialize:
  LDA #<DOTMATRIX_SEG1
  STA current_segment
  LDA #>DOTMATRIX_SEG1
  STA current_segment+1
  JSR dotmatrix_initialize_segment

  LDA #<DOTMATRIX_SEG2
  STA current_segment
  LDA #>DOTMATRIX_SEG2
  STA current_segment+1
  JSR dotmatrix_initialize_segment

  RTS

dotmatrix_initialize_segment:
  JSR dotmatrix_wait_for_not_busy
  
  LDA #%00111111 ; Turn display ON
  LDY #0
  STA (current_segment), Y
  JSR dotmatrix_wait_for_not_busy

  LDY #0
  STY current_page

  write_page:
  LDY current_page
  CPY #8
  BEQ dotmatrix_initialize_end

  LDA #%10111000
  ORA current_page
  LDY #0 ; command
  STA (current_segment), Y
  JSR dotmatrix_wait_for_not_busy

  LDY current_page
  INY
  STY current_page

  LDX #$0
  STX current_line

  write_line:
  LDX current_line

  CPX #$40
  BEQ write_page

  LDA #%01000000
  ORA current_line
  LDY #0 ; command
  STA (current_segment), Y
  JSR dotmatrix_wait_for_not_busy

  LDA #%11111111
  LDY #1 ; data
  STA (current_segment), Y
  JSR dotmatrix_wait_for_not_busy

  LDX current_line
  INX 
  STX current_line
  JMP write_line

dotmatrix_initialize_end:
  RTS

dotmatrix_wait_for_not_busy:
  LDY #0
  LDA (current_segment), Y
  AND #%10000000
  CMP #$0
  BNE dotmatrix_wait_for_not_busy
  RTS