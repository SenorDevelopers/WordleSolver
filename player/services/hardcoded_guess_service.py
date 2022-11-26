from services.base_guess_service import BaseGuessService
from models.guess import Guess

# for testing purposes only 
class HardcodedGuessService(BaseGuessService):
    def __init__(self):
        self.__guesses =  ["GITTY", "ABOUT", "MORAU", "STATS"]
        self.__counter = 1
    
    def get_opener(self) -> Guess:
        return Guess(id="", guess_string=self.__guesses[0], is_opener=True)
    
    def get_guess(self, pattern: str, prev_id: str) -> Guess:
        guess = Guess(id="", guess_string=self.__guesses[self.__counter], is_opener=False)
        self.__counter = (self.__counter + 1) % len(self.__guesses)
        return guess