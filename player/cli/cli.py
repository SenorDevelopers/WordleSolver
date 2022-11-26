from services.base_feedback_service import BaseFeedbackService
from services.base_guess_service import BaseGuessService
from models.state import State

class CLIApp:
    def __init__(self, guess_service: BaseGuessService, feedback_service: BaseFeedbackService) -> None:
        self.__feedback_service = feedback_service
        self.__guess_service = guess_service
        self.__state = State(input("Enter word: ").upper())
    
    def run(self):
        guess_count = 1
        running = True
        
        while running:
            guess = self.__get_next_guess()

            if self.__determine_if_game_won(guess) == True or guess_count > 6:
                if self.__determine_if_game_won(guess) == True:
                    print("You won!")
                else:
                    print("You lost!")
                running = False
                break

            pattern = self.__feedback_service.calculate(answer=self.__state.get_word_to_guess(), word=guess.guess_string)
            
            print(f"Guess {guess_count}: {guess.guess_string}")
            
            self.__state.add_pattern(pattern)
            self.__state.add_guess(guess) 
            
            if guess.guess_string == self.__state.get_word_to_guess():
                running = False 
                break
            
            guess_count += 1
        
        expected = self.__state.get_word_to_guess()
        if guess_count == 1:
            print(f"Guessed {expected} in {guess_count} try")
        else: 
            print(f"Guessed {expected} in {guess_count} tries")
    
    def __get_next_guess(self):
        if self.__state.get_current_iteration() == 0:
            return self.__guess_service.get_opener()
        return self.__guess_service.get_guess(self.__state.get_last_pattern(), self.__state.get_last_guess_id())

    def __determine_if_game_won(self, guess) -> bool:
        last_guess = guess
        word_to_guess = self.__state.get_word_to_guess()
        return last_guess == word_to_guess