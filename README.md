Api para para plataformas de anúncios de vendas.

Publicado em: https://sales-advertisement-api.herokuapp.com/advertisement/

<br/>

Tecnologias ultilizadas:
  - .NET 6
  - ASP.NET Core
  - Entity Framework Core
  - PostgreSQL
  - AWS S3
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

Rotas:
  - Buscar todos os usuários: https://sales-advertisement-api.herokuapp.com/User
  - Buscar um único usuário ultilizando seu id (ex: 1): https://sales-advertisement-api.herokuapp.com/User/1
  - Criar um novo usuário: https://sales-advertisement-api.herokuapp.com/User/register
 
        Dados para criar um novo usuário: 
            
            Name: string
            Email: string
            Password: string
            
  - Atualizar um novo usuário ultilizando seu id (ex: 1): https://sales-advertisement-api.herokuapp.com/User/1
  
        Dados atualizáveis de um usuário:
                  
                  Email: string
                  Password: string
                  
  - Deletar um usuário ultilizando seu id (ex: 1): https://sales-advertisement-api.herokuapp.com/User/1

  - Buscar todos os anúncios: https://sales-advertisement-api.herokuapp.com/advertisement/
  - Buscar um único anúncio ultilizando seu id (ex: 1): https://sales-advertisement-api.herokuapp.com/advertisement/1
  - Criar um novo anúncio: https://sales-advertisement-api.herokuapp.com/advertisement/Create
    
        Dados ao criar um novo anúncio:
            
            Image: file
            Title: string
            Description: string
            Price: float
            userId: int (no cabeçalho da rota (header))
            
   - Atualizar um anúncio ultilizando seu id (ex: 1): https://sales-advertisement-api.herokuapp.com/Advertisement/1
  
          Dados atualizáveis de um anúncio:
            
            Title: string
            Description: string
            Price: float
            
   - Deletar um anúncio ultilizando seu id (ex: 1): https://sales-advertisement-api.herokuapp.com/advertisement/1
