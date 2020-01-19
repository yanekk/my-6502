acia_initialize:
    LDA #%00000000
    STA ACIA_STATUS

    LDA #%00001011
    STA ACIA_COMMAND

    LDA #%00011111
    STA ACIA_CONTROL

write:
    LDY #0

next_char:
wait_txd_empty:
    LDA ACIA_STATUS
    AND #$10
    BEQ wait_txd_empty
    LDA text, Y
    BEQ read
    STA ACIA_DATA
    INY
    LDA #1
    JSR wait
    JMP next_char

read:
wait_rxd_full:
    LDA ACIA_STATUS
    AND #$08
    BEQ wait_rxd_full
    LDA ACIA_DATA
    JMP write

text:
    .byte "Hallo World!", $0d, $0a, $00