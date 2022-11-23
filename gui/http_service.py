from constants import API_URL, GET_OPENER_ENDPOINT, GET_NEXT_GUESS_ENDPOINT
import json
import requests

class HttpService:
    # TODO include id 
    def get_opener(self):
        res = json.loads(requests.get(API_URL + GET_OPENER_ENDPOINT).text)
        return res['opener']

    # TODO include id 
    def get_next_guess(self, pattern, prev_id):
        res = json.loads(requests.get(self.__next_guess_url(pattern, prev_id)).text)
        return res['nextGuess']

    def __next_guess_url(self, pattern, prev_id):
        return API_URL + GET_NEXT_GUESS_ENDPOINT + "/" + str(pattern) + "/" + str(prev_id)
