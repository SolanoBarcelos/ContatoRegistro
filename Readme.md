# ContatoRegistro - API RESTful para cadastro de usu�rios

#### Video explicativo do projeto e imagens do Sonar est�o na pasta "Info_Entrega_Trabalho" do repositorio.

#### by Solano Barcelos - https://www.linkedin.com/in/solanobarcelos/

## Projeto em C#, .NET Core., Dapper, PostgreSQL, xUnit, SonarCloud, Docker. 

### Projeto cadastro de usu�rios contendo principais opera��es CRUD.

## Cleam Architecture + DDD 
### Projeto dividido em camadas: Domain, Application, Infra, WEBAPI, Testes.

## Domain: Entidades, Objetos de valor, Reposit�rios (interfaces - Gateway Reposit�rio - abstra��o para que o reposit�rio n�o acesse a entidade) => Esta camada condensa regras de neg�cio e valida��es.

## Application: Servi�os de aplica��o (implementa��o das interfaces do dom�nio), DTOs, Mapeamentos => Esta camada orquestra as opera��es do sistema.

## Infra: Implementa��o dos reposit�rios (acesso a dados), Configura��o do banco de dados, Dapper => Esta camada lida com a persist�ncia dos dados.

## API: Controladores, Presenters, Inje��o de depend�ncia => Esta camada exp�e os endpoints da aplica��o, usa controladores MVC que nessa caso tamb�m s�o os controladores da Cleam Architecture e apresenta os retornos tratados em forma de Presenters.

# Rodar projeto em ambiente on premisse local:

## Comandos para containeriza��o das aplica��es separadas em ambiente de testes antes de fazer o Docker composse

### Criar network para os containers
docker network create contatoregistro
### Ver a rede
docker network ls
### Rodar container do banco de dados (Tabela no diret�rio Infra/Persistence/criar_tabela_contatos.sql)
docker run --name postgres_db_contatoregisto --network contatoregistro -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=1234 -e POSTGRES_DB=contato_registo -p 5432:5432 -d postgres