.macro dotmatrix_set_segment segment
    LDA #<segment
    STA current_segment
    LDA #>segment
    STA current_segment+1
.endmacro

.macro dotmatrix_send_command
    LDY #0
    STA (current_segment), Y
.endmacro

.macro dotmatrix_set_page page
  LDA #command_set_page
  ORA page
  dotmatrix_send_command
.endmacro

.macro dotmatrix_set_column column
  LDA #command_set_column
  ORA column
  dotmatrix_send_command
.endmacro

.macro dotmatrix_write_data
    PHY
    LDY #1
    STA (current_segment), Y
    PLY
.endmacro