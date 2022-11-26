from constants import API_URL, GET_OPENER_ENDPOINT, GET_NEXT_GUESS_ENDPOINT
from urllib3.exceptions import InsecureRequestWarning
from services.base_guess_service import BaseGuessService
from urllib3 import disable_warnings
from models.guess import Guess
import requests
import json
class HttpGuessService(BaseGuessService):
    def __init__(self):
        disable_warnings(InsecureRequestWarning)

    def get_opener(self) -> Guess:
        res = requests.get(self.__opener_url(), verify=False)
        return Guess(id="", guess_string=res.text, is_opener=True)

    def get_guess(self, pattern: str, prev_id: str) -> Guess:
        payload = json.loads(requests.get(self.__next_guess_url(pattern, prev_id), verify=False).text)
        return Guess(id=payload["id"], guess_string=payload["guessString"], is_opener=False)
    
    def __opener_url(self) -> str: 
        return API_URL + GET_OPENER_ENDPOINT

    def __next_guess_url(self, pattern, prev_id) -> str:
        return API_URL + GET_NEXT_GUESS_ENDPOINT + "/" + str(pattern) + "/" + str(prev_id)