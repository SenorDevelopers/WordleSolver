from services.base_feedback_service import BaseFeedbackService
from constants import GRID_COLS

class BuggyFeedbackService(BaseFeedbackService):
    def calculate(self, answer: str, word: str) -> str:
        ans = ["0" for x in range(GRID_COLS)]
        copy_word = [word[i] for i in range(GRID_COLS)]
        copy_ans = [answer[i] for i in range(GRID_COLS)]
        
        for i in range(GRID_COLS):
            if answer[i] == word[i]:
                ans[i] = "2"
                copy_word[i] = "0"        

        for i in range(GRID_COLS):
            if copy_word[i] != "0" and copy_word[i] in copy_ans:
                ans[i] = "1"
        
        return "".join(ans)