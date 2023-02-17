import secrets
from unittest.mock import MagicMock

import pytest

from runner.source_code import FixtureSourceFile
from runner.fixture import Fixture, FixtureResult
from runner.label_file import Label, LabelFile

def test_property_assignment_set_variables():
    # arrange
    source_code = MagicMock(spec=FixtureSourceFile)
    fixture = Fixture(source_code)

    # act
    fixture.R1 = 123

    # assert
    source_code.assign_variables.assert_called_once_with(R1 = 123)

def test_calling_property_adds_subroutine_jump():
    # arrange
    source_code = MagicMock(spec=FixtureSourceFile)
    fixture = Fixture(source_code)

    # act
    fixture.subroutine()

    # assert
    source_code.jump_to_subroutine.assert_called_once_with('subroutine')

def test_fixture_result_allows_memory_access_by_label():
    # arrange
    label_file = LabelFile()
    label_file.append(Label('abcd', 0x1234))
    memory_dump = secrets.token_bytes(0x10000)

    # act
    fixture_result = FixtureResult(memory_dump, label_file)

    # assert
    assert fixture_result.abcd == memory_dump[0x1234]

@pytest.mark.skip
def test_calling_property_stores_source_file():
    # arrange
    source_code = MagicMock(spec=FixtureSourceFile)
    temp_directory = 'this/is/temp/directory/path'

    fixture = Fixture(source_code, temp_directory)

    # act
    fixture.subroutine()

    # assert
    source_code.save_as.assert_called_once_with(temp_directory)
