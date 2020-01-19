WAIT_TIME = $1000

wait:
  STA WAIT_TIME
  PHA
  TXA
  PHA
.wait_milisecond:
  LDX #$7F
.wait_some_more:
  DEX
  CPX #$0
  BNE .wait_some_more
  LDX WAIT_TIME
  DEX
  CPX #$0
  BEQ .end_wait
  STX WAIT_TIME
  JMP .wait_milisecond
.end_wait:
  PLA
  TAX
  PLA
  RTS