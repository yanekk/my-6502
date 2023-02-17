import io

from runner.label_file import Label, LabelFile

def test_parse_label():
    # arrange
    label_string = 'funcs_high = $FF16'

    # act
    label = Label.parse(label_string)

    # assert
    assert label == Label('funcs_high', 0xFF16)

def test_parse_label_file():
    # arrange
    label_file = """
funcs_high = $FF16
funcs_low = $FF0F
call_subroutine = $FF00
    """.strip()

    # act
    labels = LabelFile.parse(io.StringIO(label_file))

    # assert
    assert labels == [
        Label('funcs_high', 0xFF16),
        Label('funcs_low', 0xFF0F),
        Label('call_subroutine', 0xFF00)
    ]

def test_get_address_at_label():
    # arrange
    expected_address = 0xFF16
    label_file = LabelFile(
        [Label('abcd', 0xFF16)]
    )

    # act
    actual_address = label_file.address_at('abcd')

    # assert
    assert actual_address == expected_address