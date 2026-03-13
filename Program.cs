namespace MOM_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
         builder.Services.AddDistributedMemoryCache();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromHours(8);
            });

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
           app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                // Prevent browser caching so the Back button can't show restricted pages after logout.
                context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
                context.Response.Headers.Pragma = "no-cache";
                context.Response.Headers.Expires = "0";
                await next();
            });

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
