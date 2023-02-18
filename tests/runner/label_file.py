from collections import UserList
from dataclasses import dataclass
import io


@dataclass
class Label:
    label: str
    address: int

    @classmethod
    def parse(cls, label_assignment: str):
        _, address, label = label_assignment.strip().split(' ')
        return Label(label=label.lstrip('.'), address=int(address, 16))

class LabelFile(UserList[Label]):
    @classmethod
    def parse(cls, buffer: io.TextIOWrapper):
        return LabelFile([Label.parse(label) for label in buffer.readlines()])

    def address_at(self, label: str):
        return [_ for _ in self if _.label == label][0].address