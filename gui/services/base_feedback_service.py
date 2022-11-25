from abc import ABC, abstractmethod

class BaseFeedbackService(ABC):
    @abstractmethod
    def calculate(word_to_guess: str, guess: str) -> str:
        pass 