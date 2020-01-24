acia_write_line_pointer = $00

acia_initialize:
    LDA #%00000000
    STA ACIA_STATUS

    LDA #%00001011
    STA ACIA_COMMAND

    LDA #%00011111
    STA ACIA_CONTROL

    RTS

acia_write_char:
  PHA
  JSR acia_wait_for_txd_empty
  PLA
  STA ACIA_DATA
  LDA #1
  JSR wait
  RTS

acia_write_line:
  STA acia_write_line_pointer
  STX acia_write_line_pointer+1
  LDY #0

@acia_write_next_char:
@acia_wait_for_txd_empty:
  JSR acia_wait_for_txd_empty
  LDA (acia_write_line_pointer), Y
  BEQ @acia_write_line_end
  STA ACIA_DATA
  INY
  LDA #1
  JSR wait
  JMP @acia_write_next_char
@acia_write_line_end:
  RTS

acia_wait_for_txd_empty:
  LDA ACIA_STATUS
  AND #$10
  BEQ acia_wait_for_txd_empty
  RTS