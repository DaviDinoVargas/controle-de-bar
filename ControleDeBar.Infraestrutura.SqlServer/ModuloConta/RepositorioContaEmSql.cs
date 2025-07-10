using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using eAgenda.Infraestrutura.SqlServer.Compartilhado;
using System.Data;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloConta
{
    public class RepositorioContaEmSql : RepositorioBaseEmSql<Conta>, IRepositorioConta
    {
        public RepositorioContaEmSql(IDbConnection conexaoComBanco) : base(conexaoComBanco)
        {
        }

        protected override string SqlInserir => @"
            INSERT INTO [TBCONTA]
            (
                [ID],
                [TITULAR],
                [MESA_ID],
                [GARCOM_ID],
                [ABERTURA],
                [FECHAMENTO],
                [ESTA_ABERTA]
            )
            VALUES
            (
                @ID,
                @TITULAR,
                @MESA_ID,
                @GARCOM_ID,
                @ABERTURA,
                @FECHAMENTO,
                @ESTA_ABERTA
            );";

        protected override string SqlEditar => @"
            UPDATE [TBCONTA]
            SET
                [TITULAR] = @TITULAR,
                [MESA_ID] = @MESA_ID,
                [GARCOM_ID] = @GARCOM_ID,
                [ABERTURA] = @ABERTURA,
                [FECHAMENTO] = @FECHAMENTO,
                [ESTA_ABERTA] = @ESTA_ABERTA
            WHERE
                [ID] = @ID";

        protected override string SqlExcluir => @"
            DELETE FROM [TBCONTA]
            WHERE [ID] = @ID";

        protected override string SqlSelecionarPorId => @"
            SELECT 
                [ID],
                [TITULAR],
                [MESA_ID],
                [GARCOM_ID],
                [ABERTURA],
                [FECHAMENTO],
                [ESTA_ABERTA]
            FROM [TBCONTA]
            WHERE [ID] = @ID";

        protected override string SqlSelecionarTodos => @"
            SELECT 
                [ID],
                [TITULAR],
                [MESA_ID],
                [GARCOM_ID],
                [ABERTURA],
                [FECHAMENTO],
                [ESTA_ABERTA]
            FROM [TBCONTA]";

        private string SqlSelecionarPorStatus => SqlSelecionarTodos + " WHERE [ESTA_ABERTA] = @STATUS";

        private string SqlSelecionarPorData => SqlSelecionarTodos + " WHERE CAST([FECHAMENTO] AS DATE) = @DATA";

        public void CadastrarConta(Conta conta)
        {
            CadastrarRegistro(conta);
        }

        public Conta SelecionarPorId(Guid idRegistro)
        {
            return SelecionarRegistroPorId(idRegistro);
        }

        public List<Conta> SelecionarContas()
        {
            return SelecionarRegistros();
        }

        public List<Conta> SelecionarContasAbertas()
        {
            return SelecionarContasPorStatus(true);
        }

        public List<Conta> SelecionarContasFechadas()
        {
            return SelecionarContasPorStatus(false);
        }

        public List<Conta> SelecionarContasPorPeriodo(DateTime data)
        {
            using var comando = conexaoComBanco.CreateCommand();

            comando.CommandText = SqlSelecionarPorData;
            comando.AdicionarParametro("DATA", data.Date);

            conexaoComBanco.Open();
            var leitor = comando.ExecuteReader();

            var contas = new List<Conta>();

            while (leitor.Read())
                contas.Add(ConverterParaRegistro(leitor));

            conexaoComBanco.Close();

            return contas;
        }

        private List<Conta> SelecionarContasPorStatus(bool status)
        {
            using var comando = conexaoComBanco.CreateCommand();

            comando.CommandText = SqlSelecionarPorStatus;
            comando.AdicionarParametro("STATUS", status);

            conexaoComBanco.Open();
            var leitor = comando.ExecuteReader();

            var contas = new List<Conta>();

            while (leitor.Read())
                contas.Add(ConverterParaRegistro(leitor));

            conexaoComBanco.Close();

            return contas;
        }

        protected override void ConfigurarParametrosRegistro(Conta conta, IDbCommand comando)
        {
            comando.AdicionarParametro("ID", conta.Id);
            comando.AdicionarParametro("TITULAR", conta.Titular);
            comando.AdicionarParametro("MESA_ID", conta.Mesa.Id);
            comando.AdicionarParametro("GARCOM_ID", conta.Garcom.Id);
            comando.AdicionarParametro("ABERTURA", conta.Abertura);
            comando.AdicionarParametro("FECHAMENTO", conta.Fechamento);
            comando.AdicionarParametro("ESTA_ABERTA", conta.EstaAberta);
        }

        protected override Conta ConverterParaRegistro(IDataReader leitor)
        {
            return new Conta
            {
                Id = Guid.Parse(leitor["ID"].ToString()!),
                Titular = leitor["TITULAR"].ToString()!,
                Abertura = Convert.ToDateTime(leitor["ABERTURA"]),
                Fechamento = Convert.ToDateTime(leitor["FECHAMENTO"]),
                EstaAberta = Convert.ToBoolean(leitor["ESTA_ABERTA"]),
                Mesa = new Mesa { Id = Guid.Parse(leitor["MESA_ID"].ToString()!) },
                Garcom = new Garcom { Id = Guid.Parse(leitor["GARCOM_ID"].ToString()!) },
                Pedidos = new List<Pedido>()
            };
        }
    }
}
