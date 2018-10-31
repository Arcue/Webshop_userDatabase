import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import requests
import json
import unittest

BASE_URL = "https://webshop-userdb-api.herokuapp.com/"

class TestingMethod(unittest.TestCase):
    Token = None

    def test_Response(self):
        ok = 200
        response = requests.get(BASE_URL + "api/TableUsers") 
        jsonResponse = response.json()
        #print(jsonResponse[0]["authtoken"])        
        global Token
        Token = jsonResponse[0]["authtoken"]      
        self.assertEqual(response.status_code, ok)
        
 
    def test_usrId(self):
        ok = 200
        header = {"authToken" : Token}
        response = requests.get(BASE_URL + "Users/user", headers = header)
        jsonResponse = response.json()
        self.assertEqual(response.status_code, ok)

    def test_registration(self):
        userInfo = {
            "UserId": 4,
            "Email": "test4",
            "Username": "test4",
            "Password": "test4",
            "Adress": "test4",
            "Postnummer": 1,
            "Stad": "test4"
        }

        data_json = json.dumps(userInfo)
        

        response = requests.post(BASE_URL + "Users/register", headers = {"Content-type": "application/json"}, data=data_json)
        print(response.status_code)
        self.assertEqual(response.status_code, 200)

        

if __name__ == '__main__':
    unittest.main()