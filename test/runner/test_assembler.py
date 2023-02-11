
from unittest.mock import MagicMock, call
import pytest
from pathlib import Path
from .assembler import Assembler, Subprocess

@pytest.fixture
def testdata_path():
    return Path(__file__).parent / 'testdata'

def test_assembler_valid_file_returns_no_error(testdata_path: Path):
    # arrange
    subprocess_mock = MagicMock(spec=Subprocess)
    assembler = Assembler(subprocess_mock)

    source_code_path = testdata_path / 'valid_code.s'

    # act
    assembler.assemble(source_code_path=source_code_path)

    # assert
    assert subprocess_mock.run.call_count == 2
    subprocess_mock.run.assert_has_calls([
        call('ca65', ['--cpu', '65C02', '-o', str(source_code_path.with_suffix('.o')), str(source_code_path)]),
        call('cl65', ['-t', 'none', '-o', str(source_code_path.with_suffix('.bin')), str(source_code_path.with_suffix('.o'))])
    ])

