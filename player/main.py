from services.buggy_feedback_service import BuggyFeedbackService
from services.hardcoded_guess_service import HardcodedGuessService
from util import run_gui_mode, run_cli_mode
from constants import DEFAULT_WORD
from cli.parser import CLIParser
from models.state import State

guess_service = HardcodedGuessService()
feedback_service = BuggyFeedbackService()
   
if CLIParser().parse().gui:
    state = State(DEFAULT_WORD)
    run_gui_mode(guess_service, feedback_service, state)
else: 
    run_cli_mode(guess_service, feedback_service)