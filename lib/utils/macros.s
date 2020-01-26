.macro increment_variable var
  LDY var
  INY
  STY var
.endmacro

.macro call subroutine_name
  LDY #subroutine_name
  JSR call_subroutine
.endmacro

.macro copy_pointer source, destination
  LDA #<source
  STA destination

  LDA #>source
  STA destination+1
.endmacro

.macro set_variable variable, value
  LDA value
  STA variable
.endmacro