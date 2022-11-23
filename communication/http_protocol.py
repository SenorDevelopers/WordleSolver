from communication.http_protocol_constants import *
import json
import requests


class HTTPProtocol:
    def get_opener(self):
        json_answer = json.loads(requests.get(API_PATH + GET_OPENER).text)
        return json_answer['opener']
    def get_next_guess(self, pattern, prev_id):
        pass
        #get-next-guess/{pattern}/{previous-id}
        # self.con.request("GET", constants.GET_NEXT_GUESS + "?pattern={}".format(pattern))
        # response = self.con.getresponse()
        # print("Status: {} and reason: {}".format(response.status, response.reason))


# protocol = HTTPProtocol()
# print(protocol.get_opener())