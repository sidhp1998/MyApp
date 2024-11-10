using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;


namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<DataContext>(opt => {
            opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        var app = builder.Build();
        
        app.MapControllers();

        app.Run();
    }
}
