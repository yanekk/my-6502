.macro dotmatrix_set_segment segment
  LDA #<segment
  STA current_segment_command
  LDA #>segment
  STA current_segment_command+1
  LDA #<segment+1
  STA current_segment_data
  LDA #>segment
  STA current_segment_data+1
.endmacro

.macro dotmatrix_set_page page
  LDA #command_set_page
  ORA page
  STA (current_segment_command)
.endmacro

.macro dotmatrix_set_column column
  LDA #command_set_column
  ORA column
  STA (current_segment_command)
.endmacro