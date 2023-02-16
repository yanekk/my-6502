from typing import Any
from .source_code import FixtureSourceFile

class Fixture:
    def __init__(self, source_file: FixtureSourceFile):
        self._source_file = source_file
    
    def __setattr__(self, name: str, value: Any) -> None:
        if(name.startswith('_')):
            self.__dict__[name] = value
            return

        self._source_file.assign_variables(**{
            name: value
        })

    def __getattr__(self, name: str):
        self._source_file.jump_to_subroutine(name)
        return lambda: 0