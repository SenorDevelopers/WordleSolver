from http_service import HttpService
from constants import GRID_COLS

class State:
    def __init__(self):
        self.__http_service = HttpService() 
        self.__current_guess_count = 0
        self.__state = [["", "", "", "", ""] for x in range(GRID_COLS)]
        self.__patterns = [["", "", "", "", ""] for x in range(GRID_COLS)]
        self.__guess_history = []

    def reset_state(self):
        self.__current_guess_count = 0
        self.__state = [["", "", "", "", ""] for x in range(GRID_COLS)]
        self.__patterns = [["", "", "", "", ""] for x in range(GRID_COLS)]
        self.__guess_history = []

    def get_opener(self):
        print("AICI")
        opener = self.__http_service.get_opener()        
        for i in range(len(opener)):
            self.__state[0][i] = opener[i]
        self.__current_guess_count = 1
        
        cnt = 0
        for c in "01201":
            self.__patterns[0][cnt] = c 
            cnt += 1

        return opener

    def get_pattern(self, row, col):
        return self.__patterns[row][col]

    def get_state(self, row, col):
        return self.__state[row][col]

    def get_curent_guess_count(self):
        return self.__current_guess

    def get_next_guess(self):
        if len(self.__history) == 0:
            guess = self.get_opener()
        else: 
            guess = self.__http_service.get_next_guess()
        
        self.__guess_history.append(guess)
        return guess 
    
    def set_new_guess(self, new_guess):
        for i in range(len(new_guess)):
            self.__state[self.__current_guess_count][i] = new_guess[i]
        self.__current_guess_count += 1