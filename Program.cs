
using Task_Management_System.NewFolder;
using Task_Management_System.Repositories;
using Task_Management_System.Services;

namespace Task_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddScoped<ITaskService, TaskService>();
            //builder.Services.AddScoped<IUserService, UserService>();
            //builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            //builder.Services.AddScoped<IUserRepository, UserRepository>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
           


            app.MapControllers();

            app.Run();
        }
    }
}
