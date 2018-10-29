import requests
import json
import unittest

BASE_URL = "https://webshop-userdb-api.herokuapp.com/api/TableUsers"

class TestingMethod(unittest.TestCase):

    def testResponse(self):
        b = 200

        response = requests.get(BASE_URL)
        jsonResponse = response.json()

        self.assertEqual(response.status_code, b)

if __name__ == '__main__':
    unittest.main()



