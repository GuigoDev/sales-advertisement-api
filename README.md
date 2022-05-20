API for announcement platforms.

<br/>

Tecnologias ultilizadas:
  - .NET 6
  - ASP.NET Core
  - Entity Framework Core
  - Microsoft SQL Server
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
  4 - Atualize a string de conecção do SQL Server no arquivo Program.cs, na linha 22.
  <br/>
  5 - Gere as migrations com o comando: dotnet migrations add TestApi --context DatabaseContext
  <br/>
  6 - Aplique as migrations no banco de dados com o comando: dotnet database update --context DatabaseContext
  <br/>
  7 - Execute a api com o comando "dotnet watch" no VS Code ou Ctrl+F5 no Visual Studio.
  <br/>
  8 - Voçê deve ver uma aba do navegador se abrir com o Swagger UI com várias informações da api.
  
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
