import requests
import json

BASE_URL = "https://webshop-userdb-api.herokuapp.com/api/TableUsers"

response = requests.get(BASE_URL)

print(response)
