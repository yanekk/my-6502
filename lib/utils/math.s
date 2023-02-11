.zeropage
R1: .word 0
R2: .word 0
R3: .word 0

.code

.export add
; add two 16-bit numbers
; @param R1: operand
; @param R2: operand
; @return R3: addition result
add:
    RTS