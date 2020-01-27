current_page = $02
current_line = $03

current_segment_command = $04
current_segment_command_h = $05

current_segment_data = $06
current_segment_data_h = $07

page = $09

current_char = $0A
current_char_h = $0B

current_tile_h = $0C
current_tile_h_h = $0D
current_tile_l = $0E
current_tile_l_h = $0F

clear_value = $10
;shift_line = $11

command_turn_on    = %00111111
command_set_page   = %10111000
command_start_line = %11000000
command_set_column = %01000000
  .include "dotmatrix/macros.s"
  ;.include "utils/macros.s"

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
  JSR @dotmatrix_clear_segment 

  dotmatrix_set_segment DOTMATRIX_SEG2
  JSR @dotmatrix_clear_segment

@dotmatrix_clear_segment:
  LDY #0

@write_page:
  CPY #8
  BEQ @dotmatrix_clear_end

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

@dotmatrix_clear_end:
  RTS

;
; dotmatrix_splash
;

dotmatrix_splash:
  copy_pointer splash_01, current_char
  set_variable page, #0
  LDX #0 ; X ALWAYS contain current column

@display_next_char:
  CPX #0 ; check if first column
  BNE @check_if_half_of_screen

  dotmatrix_set_segment DOTMATRIX_SEG1

  LDA shift_line
  ORA #command_start_line
  STA (current_segment_command)

  dotmatrix_set_page page
  dotmatrix_set_column #0

@check_if_half_of_screen:
  CPX #16 ; switch to second segment
  BNE @load_chars

  dotmatrix_set_segment DOTMATRIX_SEG2

  LDA shift_line
  ORA #command_start_line
  STA (current_segment_command)

  dotmatrix_set_page page
  dotmatrix_set_column #0

@load_chars:
  TXA
  TAY
  LDA (current_char), Y ; A contains relative address of upper tile
  TAY

  LDA tiles_l, Y
  STA current_tile_h
  LDA tiles_h, Y
  STA current_tile_h+1 ; current_tile_h now contain upper tile address
  
  CLC
  LDA #32    ; start loading second line
  ADC index_map, X
  TAY        ; Y contains column for second row

  LDA (current_char), Y ; A contains relative address of lower tile
  TAY

  LDA tiles_l, Y
  STA current_tile_l
  LDA tiles_h, Y
  STA current_tile_l+1 ; current_tile_l now contain lower tile address

  LDY #0
  PHX
@write_tile_line:  
  LDA (current_tile_l), Y
  TAX
  LDA times_sixteen, X ; move tile to upper nibble to make space of the lower tile
  ORA (current_tile_h), Y ; load upper tile to higher nibble
  STA (current_segment_data)
  INY
  CPY #4 ; width of the tile
  BNE @write_tile_line
  PLX
  INX
  
  CPX #32
  BNE @jmp_display_next_char

  LDY page
  INY 
  STY page
  
  CPY #8
  BEQ @finish_drawing

  LDA lines_l, Y
  STA current_char
  LDA lines_h, Y
  STA current_char+1

  LDX #0
@jmp_display_next_char:
  JMP @display_next_char

@finish_drawing:
  RTS

index_map: .byte 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33

splash_01: .byte 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3
splash_02: .byte 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1
splash_03: .byte 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2
splash_04: .byte 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0
splash_05: .byte 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3
splash_06: .byte 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1
splash_07: .byte 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2
splash_08: .byte 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0
splash_09: .byte 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3
splash_10: .byte 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1
splash_11: .byte 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2
splash_12: .byte 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0
splash_13: .byte 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3
splash_14: .byte 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1
splash_15: .byte 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2, 3, 4, 2
splash_16: .byte 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0, 1, 4, 0

lines_l: .byte <splash_01, <splash_03, <splash_05, <splash_07, <splash_09, <splash_11, <splash_13, <splash_15
lines_h: .byte >splash_01, >splash_03, >splash_05, >splash_07, >splash_09, >splash_11, >splash_13, >splash_15

tiles_l: .byte <tile_0, <tile_1, <tile_2, <tile_3, <tile_4
tiles_h: .byte >tile_0, >tile_1, >tile_2, >tile_3, >tile_4

tiles:
tile_0: .byte %00000011, %00000111, %00001111, %00001111
tile_1: .byte %00001111, %00001111, %00000111, %00000011
tile_2: .byte %00001100, %00001110, %00001111, %00001111
tile_3: .byte %00001111, %00001111, %00001110, %00001100
tile_4: .byte %00000000, %00000000, %00000000, %00000000

times_sixteen:
  .byte $00, $10, $20, $30, $40, $50, $60, $70
  .byte $80, $90, $A0, $B0, $C0, $D0, $E0, $F0