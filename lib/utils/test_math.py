def test_add_single_number(emu6502):
    assert emu6502.add(R1=1, R2=0).R3 == 1
    assert emu6502.add(R1=0, R2=1).R3 == 1