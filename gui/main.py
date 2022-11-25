from main_window import MainWindow 
from models.main_window_state import MainWindowState
from services.hardcoded_guess_service import HardcodedGuessService
from models.guess import Guess

# state = MainWindowState(word_to_guess="STATS", guesses=[Guess(id="", guess_string="THINK", is_opener=True),Guess(id="", guess_string="ABOUT", is_opener=False),Guess(id="", guess_string="STATS", is_opener=False)], patterns=["10000", "10001", "22222"])

state = MainWindowState(word_to_guess="TAREI")
guess_service = HardcodedGuessService()

window = MainWindow(state, guess_service)
window.mainloop()