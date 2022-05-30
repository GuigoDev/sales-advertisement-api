API for advertisement platforms.

Deploy on: https://sales-advertisement-api.herokuapp.com/

<br/>

Tecnologias ultilizadas:
  - .NET 6
  - ASP.NET Core
  - Entity Framework Core
  - PostgreSQL
  <br/>
  <br/>
Como usar?
  <br/>
  <br/>
  1 - Clone o repositório para um diretório de sua preferência.
  <br/>
  2 - Abra o projeto com o Visual Studio ou Vs Code.
  <br/>
  3 - Execute no terminal: dotnet tool install --global dotnet-ef
  <br/>
  4 - Atualize a string de conecção do PostgreSQL no arquivo Program.cs, na linha 21.
  <br/>
  5 - Aplique as migrations no banco de dados com o comando: dotnet database update --context DatabaseContext
  <br/>
  6 - Execute a api com o comando "dotnet watch" no VS Code ou Ctrl+F5 no Visual Studio.
  <br/>
  7 - Voçê deve ver uma aba do navegador se abrir com o Swagger UI com várias informações da api.
  
  <br/>
  <br/>
  
 Dados para criar um novo usuário: 
    <br/>
    Name: string
    <br/>
    Email: string
    <br/>
    Password: string
    
Dados ao criar um novo anúncio:
    <br/>
    Title: string
    <br/>
    Description: string
    <br/>
    Price: float
    <br/>
    userId: int (no cabeçalho da rota (header))
