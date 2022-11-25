from abc import ABC, abstractmethod
from models.guess import Guess

class BaseGuessService(ABC):
    @abstractmethod
    def get_opener(self) -> Guess:
        pass 
    
    @abstractmethod
    def get_guess(self) -> Guess:
        pass 