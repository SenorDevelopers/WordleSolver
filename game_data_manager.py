import game_data

class game_data_manager:
    def __init__(self):
        pass
    # Getter for game_data.py
    def get_game_data(self):
        return game_data.GAME_LETTERS

    def get_word_to_guess(self):
        return game_data.WORD_TO_GUESS
    
    # Setter for game_data.py
    def set_game_data(self, new_game_data):
        game_data.GAME_LETTERS = new_game_data

    def set_word_to_guess(self, new_word_to_guess):
        game_data.WORD_TO_GUESS = new_word_to_guess
    



