.zeropage
current_segment_command: .addr 0
current_segment_data: .addr 0
page: .byte 0

current_frame: .addr 0, 0
current_frame_index: .byte 0
.code

clear_value = $10

command_turn_on    = %00111111
command_set_page   = %10111000
command_start_line = %11000000
command_set_column = %01000000

  .include "dotmatrix.i"
  .include "dotmatrix/macros.s"
  .include "../utils/macros.i"

dotmatrix_initialize:
  dotmatrix_set_segment DOTMATRIX_SEG1
  JSR @dotmatrix_initialize_segment  

  dotmatrix_set_segment DOTMATRIX_SEG2
  JSR @dotmatrix_initialize_segment  

  RTS

@dotmatrix_initialize_segment:
  LDA #command_turn_on ; Turn display ON
  STA (current_segment_command)
  RTS

dotmatrix_clear:
  STA clear_value
  dotmatrix_set_segment DOTMATRIX_SEG1
  JSR @clear_segment 

  dotmatrix_set_segment DOTMATRIX_SEG2
  JSR @clear_segment
  RTS

@clear_segment:
  LDY #0

@write_page:
  CPY #8
  BEQ @clear_end

  TYA 
  ORA #command_set_page
  STA (current_segment_command)
  INY
  LDX #0
@write_line:
  CPX #$40
  BEQ @write_page

  LDA clear_value
  STA (current_segment_data)
  
  INX 
  JMP @write_line

@clear_end:
  RTS

dotmatrix_move_arg_shift_line: .byte 64

dotmatrix_move:
  dotmatrix_set_segment DOTMATRIX_SEG1
  LDA dotmatrix_move_arg_shift_line
  ORA #command_start_line
  STA (current_segment_command)

  dotmatrix_set_segment DOTMATRIX_SEG2
  LDA dotmatrix_move_arg_shift_line
  ORA #command_start_line
  STA (current_segment_command)
  RTS

dotmatrix_splash_reset:
  copy_pointer star_0, current_frame
  set_variable current_frame_index, #0
  RTS

dotmatrix_splash_next_frame:
  LDY #8
@dotmatrix_splash_next_byte:
  CLV

  INC current_frame
  BNE @dotmatrix_splash_next_byte_no_overflow
  INC current_frame+1

@dotmatrix_splash_next_byte_no_overflow:
  DEY
  CPY #0
  BNE @dotmatrix_splash_next_byte

  INC current_frame_index
  LDA current_frame_index
  CMP #8
  BNE @dotmatrix_splash_next_frame_end
  JSR dotmatrix_splash_reset
@dotmatrix_splash_next_frame_end:
  RTS

dotmatrix_splash:
  set_variable page, #0

  dotmatrix_set_segment DOTMATRIX_SEG1
  dotmatrix_set_page page
  dotmatrix_set_column #0
  LDY #0 

@draw_byte:
  LDA (current_frame), Y
  STA (current_segment_data)
  INY
  CPY #8
  BNE @draw_byte
  RTS

  .include "data/star.s"
  