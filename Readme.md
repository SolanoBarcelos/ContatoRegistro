# ContatoRegistro - API RESTful para cadastro de usuários

#### Video explicativo do projeto e imagens do Sonar estão na pasta "Info_Entrega_Trabalho" do repositorio.

#### by Solano Barcelos - https://www.linkedin.com/in/solanobarcelos/

## Projeto em C#, .NET Core., Dapper, PostgreSQL, xUnit, SonarCloud, Docker. 

### Projeto cadastro de usuários contendo principais operações CRUD.

## Cleam Architecture + DDD 
### Projeto dividido em camadas: Domain, Application, Infra, WEBAPI, Testes.

## Domain: Entidades, Objetos de valor, Repositórios (interfaces - Gateway Repositório - abstração para que o repositório não acesse a entidade) => Esta camada condensa regras de negócio e validações.

## Application: Serviços de aplicação (implementação das interfaces do domínio), DTOs, Mapeamentos => Esta camada orquestra as operações do sistema.

## Infra: Implementação dos repositórios (acesso a dados), Configuração do banco de dados, Dapper => Esta camada lida com a persistência dos dados.

## API: Controladores, Presenters, Injeção de dependência => Esta camada expõe os endpoints da aplicação, usa controladores MVC que nessa caso também são os controladores da Cleam Architecture e apresenta os retornos tratados em forma de Presenters.

# Rodar projeto em ambiente on premisse local:

## Comandos para containerização das aplicações separadas em ambiente de testes antes de fazer o Docker composse

### Criar network para os containers
docker network create contatoregistro
### Ver a rede
docker network ls
### Rodar container do banco de dados (Tabela no diretório Infra/Persistence/criar_tabela_contatos.sql)
docker run --name postgres_db_contatoregisto --network contatoregistro -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=1234 -e POSTGRES_DB=contato_registo -p 5432:5432 -d postgres