  .segment "CODE"
reset:
  LDA #$BA
  STA $01
  JMP reset
  .segment "INIT"
  .word reset
  .word 0