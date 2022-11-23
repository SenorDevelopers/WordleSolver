import game_data
from communication.http_protocol import HTTPProtocol

class game_data_manager:
    def __init__(self):
        self.protocol = HTTPProtocol()
    # Getter for game_data.py
    def get_game_data(self):
        return game_data.game_letters
    def get_opener(self):
        return self.protocol.get_opener()
    def get_word_to_guess(self):
        return self.protocol.get_next_guess()
    
    # Setter for game_data.py
    def reset_game(self):
        game_data.current_guess = 0
        game_data.game_letters = [["", "", "", "", ""],
                                ["", "", "", "", ""],
                                ["", "", "", "", ""],
                                ["", "", "", "", ""],
                                ["", "", "", "", ""],
                                ["", "", "", "", ""]]
    def set_new_guess(self, new_guess):
        for letter_id in range(len(new_guess)):
            game_data.game_letters[game_data.current_guess][letter_id] = new_guess[letter_id]
        game_data.current_guess += 1

    def set_word_to_guess(self, new_word_to_guess):
        game_data.word_to_guess = new_word_to_guess
    



