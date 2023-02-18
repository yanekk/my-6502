from pathlib import Path

from runner.fixture import FixtureSourceFile, FixtureExecutor
from runner.assembler import Assembler, Linker
from runner.subprocess import RealSubprocess
from runner.emulator import Emulator

def test_is_executed():
    # arrange
    emulator = Emulator(RealSubprocess())

    assembler = Assembler(RealSubprocess())
    assembler.add_include_path('lib')

    linker = Linker(RealSubprocess())
    linker.set_config_file('tests/runner/assets/test.map.cfg')

    source_code = FixtureSourceFile('actual_test_name')
    source_code.include_code('zeropage.s')

    memory_dump_path = Path(__file__).parent / 'memory_dump.s'
    source_code.include_code(memory_dump_path)

    source_code.assign_variables(R1=65)
    source_code.jump_to_subroutine('store_r1_into_r3')
    
    test_executor = FixtureExecutor(assembler, linker, emulator)

    result = test_executor.execute(source_code)

    # assert
    assert result.R3 == 65
