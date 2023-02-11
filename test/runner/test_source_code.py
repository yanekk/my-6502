import pytest

from .source_code import SourceCode

@pytest.mark.parametrize(['include_path'], [
    ['lcd/lcd.s'], ['via/via.s']
])
def test_include(include_path):
    # arrange
    source_code = SourceCode()

    # act
    source_code.include(include_path)

    # assert
    assert source_code == [f'  .include "{include_path}"']

def test_assign_byte_variable():
    # arrange
    source_code = SourceCode()

    # act
    source_code.assign('R1', 123)

    # assert
    assert source_code == [
        '  LDA #$7B',
        '  STA R1',
        '  LDA #$0',
        '  STA R1+1'
    ]
    
def test_assign_word_variable():
    # arrange
    source_code = SourceCode()

    # act
    source_code.assign('R1', 0x1234)

    # assert
    assert source_code == [
        '  LDA #$34',
        '  STA R1',
        '  LDA #$12',
        '  STA R1+1'
    ]

@pytest.mark.parametrize(['subroutine_name'], [
    ['my_first_subroutine'], ['my_second_subroutine']
])
def test_jump_to_subroutine(subroutine_name):
    # arrange
    source_code = SourceCode()

    # act
    source_code.jump_to_subroutine(subroutine_name)

    # assert
    assert source_code == [f'  JSR {subroutine_name}']

@pytest.mark.parametrize(['label_name'], [
    ['my_first_label_name'], ['my_second_label_name']
])
def test_label(label_name):
    # arrange
    source_code = SourceCode()

    # act
    source_code.label(label_name)

    # assert
    assert source_code == [f'{label_name}:']

def test_nop():
    # arrange
    source_code = SourceCode()

    # act
    source_code.nop()

    # assert
    assert source_code == [f'  NOP']