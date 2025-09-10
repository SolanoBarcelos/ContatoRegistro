using ContatoRegistro.Doninio.Entidades;
using ContatoRegistro.Doninio.ObjetosValor;
using ContatoRegistro.Doninio.RepositorioGateway;
using Dapper;
using Npgsql;


namespace ContatoRegistro.Infra.Persistencia.Repository
{
    public sealed class ContatoRepository: IContatoRepository
    {
        //private readonly IDbConnection _connection;

        //public ContatoRepository(IDbConnection connection)
        //{
        //    _connection = connection;
        //}

        private readonly string _cs;

        public ContatoRepository(IConfiguration configuration)
        {
            _cs = configuration.GetConnectionString("DefaultConnection")!;
        }

        private NpgsqlConnection CriarConexao() => new NpgsqlConnection(_cs);


        public async Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default)
        {
            const string sql = @"SELECT 1 FROM contatos WHERE LOWER(email) = LOWER(@email) LIMIT 1;";

            await using var db = CriarConexao();
            await db.OpenAsync(ct);

            var ok = await db.ExecuteScalarAsync<int?>(new CommandDefinition(sql, new { email }, cancellationToken: ct));

            return ok.HasValue;
        }

        public async Task<Contato> AdicionarAsync(Contato contato, CancellationToken ct = default)
        {
            const string sql = @"
            INSERT INTO contatos (nome, email, telefone)
            VALUES (@nome, @email, @telefone)
            RETURNING id;";

            await using var db = CriarConexao();
            await db.OpenAsync(ct);

            var id = await db.ExecuteScalarAsync<int>(new CommandDefinition(
                sql,
                new { nome = contato.Nome.NomeValor, email = contato.Email.EmailValor, telefone = contato.Telefone.TelefoneValor },
                cancellationToken: ct));

            contato.AtribuirId(id);

            return contato;
        }

        public async Task<Contato?> ObterPorIdAsync(int id, CancellationToken ct = default)
        {
            const string sql = @"SELECT id, nome, email, telefone FROM contatos WHERE id = @id;";

            await using var db = CriarConexao();
            await db.OpenAsync(ct);

            var row = await db.QueryFirstOrDefaultAsync<(int id, string nome, string email, string telefone)>(
                new CommandDefinition(sql, new { id }, cancellationToken: ct));

            if (row == default) return null;

            var contato = new Contato(new NomeObjetoValor(row.nome), new EmailObjetoValor(row.email), new TelefoneObjetoValor(row.telefone));
            
            contato.AtribuirId(row.id);

            return contato;
        }

        public async Task<IEnumerable<Contato>> ObterTodosAsync(CancellationToken ct = default)
        {
            const string sql = @"SELECT id, nome, email, telefone FROM contatos ORDER BY id;";
            
            await using var db = CriarConexao();
            await db.OpenAsync(ct);

            var rows = await db.QueryAsync<(int id, string nome, string email, string telefone)>(
                new CommandDefinition(sql, cancellationToken: ct));

            var lista = new List<Contato>();
            foreach (var r in rows)
            {
                var contato = new Contato(new NomeObjetoValor(r.nome), new EmailObjetoValor(r.email), new TelefoneObjetoValor(r.telefone));

                contato.AtribuirId(r.id);

                lista.Add(contato);
            }
            return lista;
        }

        public async Task AtualizarAsync(Contato contato, CancellationToken ct = default)
        {
            const string sql = @"
            UPDATE contatos
            SET nome = @nome, email = @email, telefone = @telefone
            WHERE id = @id;";

            await using var db = CriarConexao();
            await db.OpenAsync(ct);

            await db.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    id = contato.Id,
                    nome = contato.Nome.NomeValor,
                    email = contato.Email.EmailValor,
                    telefone = contato.Telefone.TelefoneValor
                },
                cancellationToken: ct));
        }

        public async Task RemoverAsync(int id, CancellationToken ct = default)
        {
            const string sql = @"DELETE FROM contatos WHERE id = @id;";

            await using var db = CriarConexao();
            await db.OpenAsync(ct);

            await db.ExecuteAsync(new CommandDefinition(sql, new { id }, cancellationToken: ct));
        }
    }
}
