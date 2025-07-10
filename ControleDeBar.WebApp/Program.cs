using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.ModuloMesa;
using ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;
using ControleDeBar.Infraestrutura.SqlServer.ModuloConta;
using ControleDeBar.Infraestrutura.SqlServer.ModuloGarcom;
using ControleDeBar.Infraestrutura.SqlServer.ModuloMesa;
using ControleDeBar.Infraestrutura.SqlServer.ModuloProduto;
using ControleDeBar.WebApp.DependencyInjection;
using eAgenda.WebApp.ActionFilters;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ControleDeBar.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Adiciona filtros globais para validação e logging de ações
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<ValidarModeloAttribute>();
            options.Filters.Add<LogarAcaoAttribute>();
        });

        // Registra conexão com banco SQL usando connection string do appsettings.json
        builder.Services.AddScoped<IDbConnection>(provider =>
        {
            var connectionString = builder.Configuration["SQL_CONNECTION_STRING"];
            return new SqlConnection(connectionString);
        });

        // Registra repositórios usando implementações SQL
        builder.Services.AddScoped<IRepositorioGarcom, RepositorioGarcomEmSql>();
        builder.Services.AddScoped<IRepositorioProduto, RepositorioProdutoEmSql>();
        builder.Services.AddScoped<IRepositorioMesa, RepositorioMesaEmSql>();
        builder.Services.AddScoped<IRepositorioConta, RepositorioContaEmSql>();

        builder.Services.AddSerilogConfig(builder.Logging);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/erro");
        else
            app.UseDeveloperExceptionPage();

        app.UseAntiforgery();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.MapDefaultControllerRoute();

        app.Run();
    }
}
