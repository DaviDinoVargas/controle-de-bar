using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.ModuloMesa;
using eAgenda.Infraestrutura.SqlServer.Compartilhado;
using System.Data;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloMesa
{
    public class RepositorioMesaEmSql : RepositorioBaseEmSql<Mesa>, IRepositorioMesa
    {
        public RepositorioMesaEmSql(IDbConnection conexaoComBanco) : base(conexaoComBanco)
        {
        }

        protected override string SqlInserir => @"
            INSERT INTO [TBMESA]
            (
                [ID],
                [NUMERO],
                [CAPACIDADE],
                [ESTA_OCUPADA]
            )
            VALUES
            (
                @ID,
                @NUMERO,
                @CAPACIDADE,
                @ESTA_OCUPADA
            );";

        protected override string SqlEditar => @"
            UPDATE [TBMESA]
            SET
                [NUMERO] = @NUMERO,
                [CAPACIDADE] = @CAPACIDADE,
                [ESTA_OCUPADA] = @ESTA_OCUPADA
            WHERE
                [ID] = @ID";

        protected override string SqlExcluir => @"
            DELETE FROM [TBMESA]
            WHERE [ID] = @ID";

        protected override string SqlSelecionarPorId => @"
            SELECT 
                [ID],
                [NUMERO],
                [CAPACIDADE],
                [ESTA_OCUPADA]
            FROM [TBMESA]
            WHERE [ID] = @ID";

        protected override string SqlSelecionarTodos => @"
            SELECT 
                [ID],
                [NUMERO],
                [CAPACIDADE],
                [ESTA_OCUPADA]
            FROM [TBMESA]";

        protected override void ConfigurarParametrosRegistro(Mesa mesa, IDbCommand comando)
        {
            comando.AdicionarParametro("ID", mesa.Id);
            comando.AdicionarParametro("NUMERO", mesa.Numero);
            comando.AdicionarParametro("CAPACIDADE", mesa.Capacidade);
            comando.AdicionarParametro("ESTA_OCUPADA", mesa.EstaOcupada);
        }

        protected override Mesa ConverterParaRegistro(IDataReader leitor)
        {
            return new Mesa
            {
                Id = Guid.Parse(leitor["ID"].ToString()!),
                Numero = Convert.ToInt32(leitor["NUMERO"]),
                Capacidade = Convert.ToInt32(leitor["CAPACIDADE"]),
                EstaOcupada = Convert.ToBoolean(leitor["ESTA_OCUPADA"])
            };
        }
    }
}
