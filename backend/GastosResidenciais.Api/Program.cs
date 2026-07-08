using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Middleware;
using GastosResidenciais.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------
// Configuração de serviços (injeção de dependência)
// ---------------------------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Persistência: SQLite, com o arquivo de banco definido em appsettings.json.
// Os dados permanecem salvos em disco entre execuções da aplicação.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Serviços de domínio (regras de negócio), registrados como Scoped
// (uma instância por requisição HTTP, alinhado ao ciclo de vida do DbContext).
builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITotalsService, TotalsService>();

// CORS liberado para o front-end React (em desenvolvimento, portas do Vite).
// Em produção, restrinja para o domínio real do front-end.
const string CorsPolicyName = "FrontendPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ---------------------------------------------------------------------
// Garante que o banco de dados e as tabelas existam ao iniciar a aplicação.
// Como estamos usando SQLite em arquivo, os dados persistem entre execuções;
// EnsureCreated apenas cria o schema caso o arquivo/tabelas ainda não existam.
// ---------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ---------------------------------------------------------------------
// Pipeline HTTP
// ---------------------------------------------------------------------

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
