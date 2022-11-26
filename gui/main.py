from main_window import MainWindow 
from models.main_window_state import MainWindowState
from services.buggy_feedback_service import BuggyFeedbackService
from services.hardcoded_guess_service import HardcodedGuessService
from services.http_guess_service import HttpGuessService

state = MainWindowState(word_to_guess="STATS")
guess_service = HardcodedGuessService()
feedback_service = BuggyFeedbackService()

window = MainWindow(state, guess_service, feedback_service)
window.mainloop()