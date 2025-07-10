using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrutura.Orm.Compartilhado;

namespace ControleDeBar.Infraestrutura.Orm.ModuloMesa;

public class RepositorioMesaEmOrm : IRepositorioMesa
{
    private readonly ControleDeBarDbContext contexto;

    public RepositorioMesaEmOrm(ControleDeBarDbContext contexto)
    {
        this.contexto = contexto;
    }

    public void CadastrarRegistro(Mesa novoRegistro)
    {
        contexto.Mesas.Add(novoRegistro);
    }

    public bool EditarRegistro(Guid idRegistro, Mesa registroEditado)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registroSelecionado.AtualizarRegistro(registroEditado);

        return true;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        contexto.Mesas.Remove(registroSelecionado);

        return true;
    }

    public Mesa? SelecionarRegistroPorId(Guid idRegistro)
    {
        return contexto.Mesas.FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public List<Mesa> SelecionarRegistros()
    {
        return contexto.Mesas.ToList();
    }
}
