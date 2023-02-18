import pytest

from runner.source_code import SourceCode, SourceFile, FixtureSourceFile

@pytest.mark.parametrize(['include_path'], [
    ['lcd/lcd.s'], ['via/via.s']
])
def test_include(include_path):
    # arrange
    source_code = SourceCode()

    # act
    source_code.include(include_path)

    # assert
    assert str(source_code) == f'  .include "{include_path}"'


def test_include_backward_slashes_replacement():
    # arrange
    source_code = SourceCode()

    actual_path = 'h:\\test\\test.s'
    expected_path = 'h:/test/test.s'

    # act
    source_code.include(actual_path)

    # assert
    assert str(source_code) == f'  .include "{expected_path}"'

def test_assign_byte_variable():
    # arrange
    source_code = SourceCode()

    # act
    source_code.assign('R1', 123)

    # assert
    assert str(source_code).splitlines() == [
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
    assert str(source_code).splitlines() == [
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
    assert str(source_code) == f'  JSR {subroutine_name}'

@pytest.mark.parametrize(['label_name'], [
    ['my_first_label_name'], ['my_second_label_name']
])
def test_label(label_name):
    # arrange
    source_code = SourceCode()

    # act
    source_code.label(label_name)

    # assert
    assert str(source_code) == f'{label_name}:'

def test_nop():
    # arrange
    source_code = SourceCode()

    # act
    source_code.nop()

    # assert
    assert str(source_code) == f'  NOP'

def test_empty_source_code():
    # arrange
    source_code = SourceCode()

    # assert
    assert source_code.is_empty()

def test_non_empty_source_code():
    # arrange
    source_code = SourceCode()
    source_code.nop()

    # assert
    assert not source_code.is_empty()

def test_word():
    # arrange
    source_code = SourceCode()

    # act
    source_code.word('label')

    # assert
    assert str(source_code) == f'  .word label'

def test_source_file_stores_source_code_outside_segment():
    # arrange
    source_file = SourceFile()

    source_code = SourceCode()
    source_code.word('label')

    # act
    source_file.append(source_code)

    # assert
    assert str(source_file).splitlines() == [
        '  .word label'
    ]

def test_source_file_stores_source_code_in_segment_after_segment_zero():
    # arrange
    source_file = SourceFile()

    source_code_init = SourceCode()
    source_code_init.word('segment_init_label')

    source_code_zero = SourceCode()
    source_code_zero.word('segment_zero_label')

    # act
    source_file.append(source_code_init, 'INIT')
    source_file.append(source_code_zero)

    # assert
    assert str(source_file).splitlines() == [
        '  .word segment_zero_label',
        '  .segment "INIT"',
        '  .word segment_init_label'
    ]

def test_source_file_stores_source_code_in_segment():
    # arrange
    source_file = SourceFile()

    source_code = SourceCode()
    source_code.word('label')

    # act
    source_file.append(source_code, 'INIT')

    # assert
    assert str(source_file).splitlines() == [
        '  .segment "INIT"',
        '  .word label'
    ]

def test_source_file_stores_multiple_sources_code_in_same_segment():
    # arrange
    source_file = SourceFile()

    label_source_code = SourceCode()
    label_source_code.word('label')

    irq_handler_source_code = SourceCode()
    irq_handler_source_code.word('irq_handler')

    # act
    source_file.append(label_source_code, 'INIT')
    source_file.append(irq_handler_source_code, 'INIT')

    # assert
    assert str(source_file).splitlines() == [
        '  .segment "INIT"',
        '  .word label',
        '  .word irq_handler'
    ]

def test_source_file_empty_segments_are_ignored():
    # arrange
    source_file = SourceFile()

    empty_segment = SourceCode()

    irq_handler_source_code = SourceCode()
    irq_handler_source_code.word('irq_handler')

    # act
    source_file.append(empty_segment, 'INIT')
    source_file.append(irq_handler_source_code, 'INIT')

    # assert
    assert str(source_file).splitlines() == [
        '  .segment "INIT"',
        '  .word irq_handler'
    ]

def test_source_file_stores_multiple_sources_code_in_different_segment():
    # arrange
    source_file = SourceFile()

    label_source_code = SourceCode()
    label_source_code.word('label')

    irq_handler_source_code = SourceCode()
    irq_handler_source_code.word('irq_handler')

    # act
    source_file.append(label_source_code, 'INIT')
    source_file.append(irq_handler_source_code, 'CODE')

    # assert
    assert str(source_file).splitlines() == [
        '  .segment "INIT"',
        '  .word label',
        '  .segment "CODE"',
        '  .word irq_handler'
    ]


def test_fixture_source_file_builds_test_stub():
    # arrange
    fixture_source_file = FixtureSourceFile('test_name')

    # act
    assert str(fixture_source_file).splitlines() == [
        '  .segment "CODE"',
        'test_name_start:',
        '  NOP',
        'test_name_end:',
        '  NOP',
        '  .segment "INIT"',
        '  .word test_name_start'
    ]

def test_fixture_source_assigns_exit_label():
    # arrange
    fixture_source_file = FixtureSourceFile('test_name')

    # act
    assert fixture_source_file.exit_label == 'test_name_end'

def test_fixture_source_file_variables_are_assigned():
    # arrange
    fixture_source_file = FixtureSourceFile('test_name')
    fixture_source_file.assign_variables(
        R1=0x12,
        R2=0x23,
    )
    
    # act
    assert str(fixture_source_file).splitlines() == [
        '  .segment "CODE"',
        'test_name_start:',
        '  LDA #$12',
        '  STA R1',
        '  LDA #$0',
        '  STA R1+1',
        '  LDA #$23',
        '  STA R2',
        '  LDA #$0',
        '  STA R2+1',
        'test_name_end:',
        '  NOP',
        '  .segment "INIT"',
        '  .word test_name_start'
    ]

def test_fixture_source_file_includes_are_at_the_top():
    # arrange
    fixture_source_file = FixtureSourceFile('test_name')
    fixture_source_file.assign_variables(
        R1=0x12,
    )

    # act
    fixture_source_file.include_code(
        './common/zeropage.s',
        './via/via.s'
    )
    
    # assert
    assert str(fixture_source_file).splitlines() == [
        '  .segment "CODE"',
        '  .include "./common/zeropage.s"',
        '  .include "./via/via.s"',
        'test_name_start:',
        '  LDA #$12',
        '  STA R1',
        '  LDA #$0',
        '  STA R1+1',
        'test_name_end:',
        '  NOP',
        '  .segment "INIT"',
        '  .word test_name_start'
    ]

def test_fixture_source_file_jsr_is_after_variable_assignment():
    # arrange
    fixture_source_file = FixtureSourceFile('test_name')

    # act
    fixture_source_file.jump_to_subroutine('subroutine_name')
    fixture_source_file.assign_variables(
        R1=0x12,
    )

    # assert
    assert str(fixture_source_file).splitlines() == [
        '  .segment "CODE"',
        'test_name_start:',
        '  LDA #$12',
        '  STA R1',
        '  LDA #$0',
        '  STA R1+1',
        '  JSR subroutine_name',
        'test_name_end:',
        '  NOP',
        '  .segment "INIT"',
        '  .word test_name_start'
    ]