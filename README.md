Prerequiste: Need Docker            <br />
Windows: WSL2, Linux:Debian

To run this:-                       <br />
-docker compose up --build          <br />
-attach shell, inside container of MyWebApi: <br />

  1)dotnet ef migrations add InitialCreate <br />
  2)dotnet dotnet-ef database update <br />
  
  If fail to do 1) <br />
  3)dotnet new tool-manifest --force  <br />
  4)dotnet tool install --local dotnet-ef --version 7.0.7 <br />
  Then run again 1)
  --for WSL/WSL2--
  5)dotnet clean
  Then run again 1)

To view the table,Phpmyadmin:<br /> http://localhost:8080/index.php

login,getdata, view in json->      <br />
http://localhost/DataSave/Actual   <br />
http://localhost/DataSave/Dummy    <br />


login, getdata, save data -> saving process: Get <br />
http://localhost/DataSave/Actualx                <br />
http://localhost/DataSave/Dummyx                 <br />
