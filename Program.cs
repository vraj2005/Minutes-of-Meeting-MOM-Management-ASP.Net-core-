namespace MOM_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

			if (builder.Configuration.GetValue<bool>("SeedData:All") ||
				string.Equals(Environment.GetEnvironmentVariable("MOM_SEED_ALL"), "true", StringComparison.OrdinalIgnoreCase))
			{
				Infrastructure.DatabaseSeeder.SeedAll(builder.Configuration);
			}

			if (builder.Configuration.GetValue<bool>("SeedData:Departments") ||
				string.Equals(Environment.GetEnvironmentVariable("MOM_SEED_DEPARTMENTS"), "true", StringComparison.OrdinalIgnoreCase))
			{
				Infrastructure.DatabaseSeeder.SeedDepartmentsOnly(builder.Configuration);
			}

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
