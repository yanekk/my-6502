import pytest
from runner.fixture import Fixture, FixtureSourceFile

@pytest.mark.skip
def test_memory_dump():
    # arrange
    source_file = FixtureSourceFile('test_memory_dump')
    fixture = Fixture(source_file)
    fixture.R1 = 123

    # act
    result = fixture.store_variable_into_r3()

    # assert
    assert result.R3 == 123