from services.base_feedback_service import BaseFeedbackService
from services.base_guess_service import BaseGuessService
from models.state import State
from gui.main_window import MainWindow
from cli.cli import CLIApp

def run_gui_mode(guess_service: BaseGuessService, feedback_service: BaseFeedbackService, state: State):
    window = MainWindow(state, guess_service, feedback_service)
    window.mainloop()
    
def run_cli_mode(guess_service: BaseGuessService, feedback_service: BaseFeedbackService):
    cli_app = CLIApp(guess_service, feedback_service)
    cli_app.run()