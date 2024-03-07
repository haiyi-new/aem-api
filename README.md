To run this:- docker compose build docker compose up

3 container will be up -Dotnet Web Api -Mysql -Phpmyadmin: http://localhost:8080/index.php?route=/&route=%2F&db=db

Communicate via REST API with http://test-demo.aemenersol.com/index.html

Login and view dashboard. http://localhost/api/DataSync/SyncData

view Platform Well Actual http://localhost/api/DataSync/GetPlatformWellActual

view Platform Well Dummy http://localhost/api/DataSync/GetPlatformWellDummy

Sync Data via REST API with http://test-demo.aemenersol.com/index.html 1)creating table in MySQL http://localhost/api/Schema/InitializeSchema (Fail)

login, getdata, save data -> saving process -> http://localhost/api/DataSave/SaveData
