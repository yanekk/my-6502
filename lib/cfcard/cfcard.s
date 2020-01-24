CF_INIT:
  JSR LOOP_BUSY
  
  JSR LOOP_CMD_RDY
  LDA #$04      ; Reset
  STA CFCMD
  LDA CFERR
  JSR LOOP_BUSY
  
  LDA #$01      ; LD features register to enable 8 bit
  STA CFFEAT
  JSR LOOP_BUSY
  LDA CFERR
  
  JSR LOOP_CMD_RDY
  LDA #$EF      ; Send set features command
  STA CFCMD
  JSR LOOP_BUSY
  LDA CFERR
  RTS
	
LOOP_BUSY:
  LDA CFSTAT	
  AND #$80
  CMP #$0
  BNE LOOP_BUSY
  RTS
	
LOOP_CMD_RDY:
  LDA CFSTAT    ; Read status
  AND #$C0 ; mask off busy and rdy bits
  EOR #$40 ; we want busy(7) to be 0 and drvrdy(6) to be 1
  CMP #$0
  BNE LOOP_CMD_RDY
  RTS
		
LOOP_DAT_RDY:
  LDA CFSTAT     ; Read status				
  AND #$88  ; mask off busy and drq bits
  EOR #$8	 ; we want busy(7) to be 0 and drq(3) to be 1
  CMP #$0
  BNE LOOP_DAT_RDY
  RTS
  
CF_RD_CMD:
  PHA
  JSR LOOP_CMD_RDY				;Make sure drive is ready for command
  PLA						;Prepare read command
  STA CFCMD					;Send read command
  JSR LOOP_DAT_RDY				;Wait until data is ready to be read
  LDA CFSTAT					;Read status
  AND #$1					;mask off error bit
  CMP #$0
  BNE CF_RD_CMD
  LDX #$0
@CF_RD_SECT_FIRST_HALF:
  JSR LOOP_DAT_RDY	
  LDA CFDATA
  STA CFSECT_BUFF_FIRST_HALF, X
  INX
  CPX #$FF
  BNE @CF_RD_SECT_FIRST_HALF
  LDX #$0
@CF_RD_SECT_SECOND_HALF:
  JSR LOOP_DAT_RDY	
  LDA CFDATA
  STA CFSECT_BUFF_SECOND_HALF, X
  INX
  CPX #$FF
  BNE @CF_RD_SECT_SECOND_HALF
  RTS

CF_READ_SECTOR:
  LDA #$01
  STA CFSECCO   ; Deal with only one sector at a time (512 bytes)
  JSR LOOP_BUSY

  LDA CFLBA0_BUFF
  STA CFLBA0    ; LBA 0:7
  JSR LOOP_BUSY

  LDA CFLBA1_BUFF
  STA CFLBA1    ; LBA 8:15
  JSR LOOP_BUSY

  LDA CFLBA2_BUFF
  STA CFLBA2 ;LBA 16:23
  JSR LOOP_BUSY

  LDA #$E0    ; Selects CF as master
  STA CFLBA3 ; LBA 24:27 + DRV 0 selected + bits 5:7=111
  JSR LOOP_BUSY

  LDA #$20 ; Read command
  JSR CF_RD_CMD
  RTS
