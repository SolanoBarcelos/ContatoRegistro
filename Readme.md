# ContatoRegistro

# Video explicativo do projeto e imagens est�o na pasta zipada do repositorio.

## Projeto em C#, .NET Core., Dapper, PostgreSQL, xUnit. 

### Projeto cadastro de usu�rios contendo principais opera��es CRUD.

# Rodar projeto em ambiente on premisse local:

## Comandos para containeriza��o das aplica��es separadas em ambiente de testes antes de fazer o Docker composse

### Criar network para os containers
docker network create testeconhecimento
### Ver a rede
docker network ls
### Rodar container do banco de dados (Tabela no diret�rio Infra/Persistence/Script.sql)
docker run --name postgres_db_contatoregisto --network contatoregistro -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=1234 -e POSTGRES_DB=contato_registo -p 5432:5432 -d postgres