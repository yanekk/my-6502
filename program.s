.include "dotmatrix/dotmatrix.inc"
.include "via/via.inc"
.include "bios.inc"
.include "utils/macros.inc"

interrupt_timeout = $FD

.segment "INIT"
program_start:
jmp init

.code
  .INCLUDE "dotmatrix/dotmatrix.s"  

init:
  ;LDY #lcd_initialize
  ;JSR call_subroutine

  ;call sub_lcd_clear

;  LDA #<line1
;  LDX #>line1
;  call sub_lcd_write_line

  LDA #<acia_initialized
  LDX #>acia_initialized
  call sub_acia_write_line

  LDA #<line1
  LDX #>line1
  call sub_acia_write_line

  ;LDY #acia_write_line
  ;JSR call_subroutine

  JSR dotmatrix_initialize

  LDA #$00
  JSR dotmatrix_clear
  JSR dotmatrix_splash_reset

  ; initialize interrupt
  lda #$FF
  sta VIA_T1CL
  sta VIA_T1CH
  lda #%01000000 ; Continuous interrupts / no PB7 output
  sta VIA_ACR
  LDA #%11000000 ; enable T1
  STA VIA_IER
  CLI ; enable interrupts
  JSR dotmatrix_splash
  JMP wait_for_interrupt

splash:
  JSR dotmatrix_move

  JSR dotmatrix_splash_next_frame
  JSR dotmatrix_splash
  DEC dotmatrix_move_arg_shift_line

  LDA dotmatrix_move_arg_shift_line
  CMP #0
  BNE wait_for_interrupt

reset_shift_line:
  LDA #64
  STA dotmatrix_move_arg_shift_line

wait_for_interrupt:
  LDA interrupt_timeout
  BEQ wait_for_interrupt
  
  LDA #0
  STA interrupt_timeout

  JMP splash



  
line1: .ASCIIZ "Program loaded"
acia_initialized: .ASCIIZ "ACIA initialized.\r\n"