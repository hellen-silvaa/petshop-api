using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PetShopApi.Data;
using PetShopApi.Middleware;
using PetShopApi.Services;
using PetShopApi.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PetShopApi",
        Version = "v1",
        Description = "API RESTful para gestão de tutores, pets, agendamentos, produtos e pedidos de um pet shop."
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PetShopApi v1");
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
