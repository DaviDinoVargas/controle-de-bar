using ControleDeBar.Dominio.ModuloGarcom;
using eAgenda.Infraestrutura.SqlServer.Compartilhado;
using System.Data;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloGarcom
{
    public class RepositorioGarcomEmSql : RepositorioBaseEmSql<Garcom>, IRepositorioGarcom
    {
        public RepositorioGarcomEmSql(IDbConnection conexaoComBanco) : base(conexaoComBanco)
        {
        }

        protected override string SqlInserir => @"
            INSERT INTO [TBGARCOM]
            (
                [ID],
                [NOME],
                [CPF]
            )
            VALUES
            (
                @ID,
                @NOME,
                @CPF
            );";

        protected override string SqlEditar => @"
            UPDATE [TBGARCOM]
            SET
                [NOME] = @NOME,
                [CPF] = @CPF
            WHERE
                [ID] = @ID";

        protected override string SqlExcluir => @"
            DELETE FROM [TBGARCOM]
            WHERE [ID] = @ID";

        protected override string SqlSelecionarPorId => @"
            SELECT 
                [ID],
                [NOME],
                [CPF]
            FROM [TBGARCOM]
            WHERE [ID] = @ID";

        protected override string SqlSelecionarTodos => @"
            SELECT 
                [ID],
                [NOME],
                [CPF]
            FROM [TBGARCOM]";

        protected override void ConfigurarParametrosRegistro(Garcom garcom, IDbCommand comando)
        {
            comando.AdicionarParametro("ID", garcom.Id);
            comando.AdicionarParametro("NOME", garcom.Nome);
            comando.AdicionarParametro("CPF", garcom.Cpf);
        }

        protected override Garcom ConverterParaRegistro(IDataReader leitor)
        {
            return new Garcom
            {
                Id = Guid.Parse(leitor["ID"].ToString()!),
                Nome = leitor["NOME"].ToString()!,
                Cpf = leitor["CPF"].ToString()!
            };
        }
    }
}
