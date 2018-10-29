import requests
import json


BASE_URL = "https://webshop-userdb-api.herokuapp.com/Users/register"

test_data = {
            "Username": "test", 
            "Email": "test", 
            "Password": "test", 
            "Adress": "test", 
            "Postnummer": 1, 
            "Stad": "test"
            }

test_data_json = json.dumps(test_data)
print(test_data_json)
response = requests.post(url = BASE_URL, data = test_data_json)

#usersresponse = requests.get("https://webshop-userdb-api.herokuapp.com/api/TableUsers")
usersJsonResponse = response.json()
print(usersJsonResponse)



