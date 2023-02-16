from unittest.mock import MagicMock

import pytest

from runner.source_code import FixtureSourceFile
from runner.fixture import Fixture

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
