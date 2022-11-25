from constants import API_URL, GET_OPENER_ENDPOINT, GET_NEXT_GUESS_ENDPOINT
from urllib3.exceptions import InsecureRequestWarning
from urllib3 import disable_warnings
import requests
import json

class HttpService:
    def __init__(self):
        disable_warnings(InsecureRequestWarning)

    def get_opener(self):
        res = requests.get(API_URL + GET_OPENER_ENDPOINT, verify=False)
        return res.text 

    # TODO include id 
    def get_next_guess(self, pattern, prev_id):
        res = json.loads(requests.get(self.__next_guess_url(pattern, prev_id)).text)
        return res['nextGuess']

        # "id": "cf37b7c7-9164-4b61-f294-08dacca77ad5",
        # "guessString": "ASPIC"

    def __next_guess_url(self, pattern, prev_id):
        return API_URL + GET_NEXT_GUESS_ENDPOINT + "/" + str(pattern) + "/" + str(prev_id)
