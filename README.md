API for announcement platforms.

Tecnologias ultilizadas:
  - .NET 6
  - ASP.NET Core
  - Entity Framework Core
  - Microsoft SQL Server

Como usar?
  1 - Clone o repositório para um diretório de sua preferência.
  2 - Abra o projeto com o Visual Studio ou Vs Code.
  3 - Execute no terminal: dotnet tool install --global dotnet-ef
  4 - Atualize a string de conecção do SQL Server no arquivo Program.cs, na linha 22.
  5 - Gere as migrations com o comando: dotnet migrations add TestApi --context DatabaseContext
  6 - Aplique as migrations no banco de dados com o comando: dotnet database update --context DatabaseContext
  7 - Execute a api com o comando "dotnet watch" no VS Code ou Ctrl+F5 no Visual Studio.
  8 - Voçê deve ver uma aba do navegador se abrir com o Swagger UI com várias informações da api.
  
  
 Dados para criar um novo usuário: 
    Name: string
    Email: string
    Password: string
    
Dados ao criar um novo anúncio:
    Title: string
    Description: string
    Price: float
    userId: int (no cabeçalho da rota (header))
