current_page = $02
current_line = $03

current_segment = $04
current_segment_h = $05

column = $06
page = $07

current_char = $08
current_char_h = $09

current_tile_h = $0A
current_tile_h_h = $0B
current_tile_l = $0C
current_tile_l_h = $0D

command_turn_on    = %00111111
command_set_page   = %10111000
command_set_column = %01000000
  .include "dotmatrix/macros.s"
  .include "utils/macros.s"

dotmatrix_initialize:
  dotmatrix_set_segment DOTMATRIX_SEG1
  JSR @dotmatrix_initialize_segment  

  dotmatrix_set_segment DOTMATRIX_SEG2
  JSR @dotmatrix_initialize_segment  

  RTS

@dotmatrix_initialize_segment:
  LDA #command_turn_on ; Turn display ON
  dotmatrix_send_command
  set_variable current_page, #0

@write_page:
  LDY current_page
  CPY #8
  BEQ @dotmatrix_initialize_end

  dotmatrix_set_page current_page
  increment_variable current_page
  set_variable current_line, #0

@write_line:
  LDX current_line

  CPX #$40
  BEQ @write_page

  LDA #$FF
  dotmatrix_write_data
  
  increment_variable current_line
  JMP @write_line

@dotmatrix_initialize_end:
  RTS

dotmatrix_splash:
  copy_pointer splash_01, current_char
  set_variable column, #0
  set_variable page, #0

@display_next_char:
  LDX column
  CPX #0 ; check if first column
  BNE @check_if_half_of_screen

  dotmatrix_set_segment DOTMATRIX_SEG1
  dotmatrix_set_page page
  dotmatrix_set_column #0

@check_if_half_of_screen:
  LDX column
  CPX #16 ; switch to second segment
  BNE @load_chars

  dotmatrix_set_segment DOTMATRIX_SEG2
  dotmatrix_set_page page
  dotmatrix_set_column #0

@load_chars:
  LDY column
  LDA (current_char), Y ; A contains relative address of upper tile
  TAY

  LDA tiles_l, Y
  STA current_tile_h
  LDA tiles_h, Y
  STA current_tile_h+1 ; current_tile_h now contain upper tile address

  LDY #32 ; start loading second line
  LDX column

@set_current_char_index:
  CPX #0
  BEQ @set_current_char_index_e
  DEX ; for each value in column, decrease X and increase Y to get the value of second line in a row
  INY
  JMP @set_current_char_index 
  
@set_current_char_index_e:
  LDA (current_char), Y ; A contains relative address of lower tile
  TAY

  LDA tiles_l, Y
  STA current_tile_l
  LDA tiles_h, Y
  STA current_tile_l+1 ; current_tile_h now contain upper tile address

  LDY #0
@write_tile_line:  
  LDA (current_tile_h), Y
  LSR ; move tile lower nibble to make space of the upper tile
  LSR 
  LSR 
  LSR 
  ORA (current_tile_l), Y ; load upper tile to higher nibble
  dotmatrix_write_data
  INY
  CPY #4 ; width of the tile
  BNE @write_tile_line

  increment_variable column
  LDX column
  CPX #32
  BNE @goto_display_next_char

  increment_variable page
  LDY page
  CPY #3
  BEQ @finish_drawing

  LDA lines_l, Y
  STA current_char
  LDA lines_h, Y
  STA current_char+1

  LDX #0
  STX column
@goto_display_next_char:
  JMP @display_next_char

@finish_drawing:
  RTS

splash_01: .byte 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3
splash_02: .byte 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1
splash_03: .byte 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2
splash_04: .byte 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0
splash_05: .byte 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3
splash_06: .byte 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1

lines_l: .byte <splash_01, <splash_03, <splash_05
lines_h: .byte >splash_01, >splash_03, >splash_05

tiles_l: .byte <tile_0, <tile_1, <tile_2, <tile_3, <tile_4
tiles_h: .byte >tile_0, >tile_1, >tile_2, >tile_3, >tile_4

tile_0: .byte %00110000, %01110000, %11110000, %11110000
tile_1: .byte %11110000, %11110000, %01110000, %00110000
tile_2: .byte %11000000, %11100000, %11110000, %11110000
tile_3: .byte %11110000, %11110000, %11100000, %11000000
tile_4: .byte %00000000, %00000000, %00000000, %00000000