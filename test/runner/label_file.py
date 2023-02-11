from collections import UserList
from dataclasses import dataclass
import io


@dataclass
class Label:
    label: str
    address: int

    @classmethod
    def parse(cls, label_assignment: str):
        label, address = label_assignment.split(' = $')
        return Label(label=label, address=int(address, 16))

class LabelFile(UserList[Label]):
    @classmethod
    def parse(cls, buffer: io.TextIOWrapper):
        return LabelFile([Label.parse(label) for label in buffer.readlines()])