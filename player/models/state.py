from models.guess import Guess

class State:
    def __init__(self, word_to_guess: str = "", guesses: list[Guess] = [], patterns: list[str] = []) -> None:
        self.__word_to_guess = word_to_guess
        self.__guesses  = guesses
        self.__patterns = patterns
    
    def reset(self) -> None:
        self.__guesses = []
        self.__patterns = []    
    
    def add_guess(self, guess: Guess) -> None:
        self.__guesses.append(guess)
    
    def add_pattern(self, pattern: str) -> None:
        self.__patterns.append(pattern) 
        
    def get_guesses(self) -> list[Guess]:
        return self.__guesses

    def get_patterns(self) -> list[str]:
        return self.__patterns
    
    def get_word_to_guess(self) -> str:
        return self.__word_to_guess
    
    def get_current_iteration(self) -> int: 
        return len(self.__guesses)
    
    def get_last_pattern(self) -> str: 
        return "" if len(self.__patterns) == 0 else self.__patterns[-1]

    def get_last_guess_id(self) -> str:
       return "" if len(self.__guesses) == 0 else self.__guesses[-1].id
   
    def set_word_to_guess(self, new_word: str) -> None:
        self.__word_to_guess = new_word 