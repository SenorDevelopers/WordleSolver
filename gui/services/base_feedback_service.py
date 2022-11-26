from abc import ABC, abstractmethod

class BaseFeedbackService(ABC):
    @abstractmethod
    def calculate(self, answer: str, word: str) -> str:
        pass 