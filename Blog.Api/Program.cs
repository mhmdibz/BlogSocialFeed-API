using Blog.Api.Extensions;
using Blog.Api.Middleware;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Seed_Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

 builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

 using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    await context.Database.MigrateAsync();
    await Seed_Data.SeedUsersAsync(context);
}

 app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();