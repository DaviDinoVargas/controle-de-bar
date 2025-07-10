using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.ModuloProduto;
using eAgenda.Infraestrutura.SqlServer.Compartilhado;
using System.Data;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloProduto
{
    public class RepositorioProdutoEmSql : RepositorioBaseEmSql<Produto>, IRepositorioProduto
    {
        public RepositorioProdutoEmSql(IDbConnection conexaoComBanco) : base(conexaoComBanco)
        {
        }

        protected override string SqlInserir => @"
            INSERT INTO [TBPRODUTO]
            (
                [ID],
                [NOME],
                [VALOR]
            )
            VALUES
            (
                @ID,
                @NOME,
                @VALOR
            );";

        protected override string SqlEditar => @"
            UPDATE [TBPRODUTO]
            SET
                [NOME] = @NOME,
                [VALOR] = @VALOR
            WHERE
                [ID] = @ID";

        protected override string SqlExcluir => @"
            DELETE FROM [TBPRODUTO]
            WHERE [ID] = @ID";

        protected override string SqlSelecionarPorId => @"
            SELECT 
                [ID],
                [NOME],
                [VALOR]
            FROM [TBPRODUTO]
            WHERE [ID] = @ID";

        protected override string SqlSelecionarTodos => @"
            SELECT 
                [ID],
                [NOME],
                [VALOR]
            FROM [TBPRODUTO]";

        protected override void ConfigurarParametrosRegistro(Produto produto, IDbCommand comando)
        {
            comando.AdicionarParametro("ID", produto.Id);
            comando.AdicionarParametro("NOME", produto.Nome);
            comando.AdicionarParametro("VALOR", produto.Valor);
        }

        protected override Produto ConverterParaRegistro(IDataReader leitor)
        {
            return new Produto
            {
                Id = Guid.Parse(leitor["ID"].ToString()!),
                Nome = leitor["NOME"].ToString()!,
                Valor = Convert.ToDecimal(leitor["VALOR"])
            };
        }
    }
}
