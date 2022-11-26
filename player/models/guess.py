from dataclasses import dataclass

@dataclass
class Guess:
    id: str 
    guess_string: str
    is_opener: bool 