using System.Windows;
using Core.Context;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services;
using WpfApp.ViewModels;
using WpfApp.Views.ClientViews;
using WpfApp.Views.InnerViews;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var services = new ServiceCollection();
                // Конфігурація
                IConfiguration config = new ConfigurationBuilder()
                    .AddUserSecrets<DataContext>()
                    .Build();
                services.AddSingleton(config);
                // DbContext
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
                // Репозиторії та сервіси
                services.AddScoped<FeedbackRepository>();
                services.AddScoped<UserRepository>();
                services.AddScoped<CategoryRepository>();
                services.AddScoped<FeedbackCategoryRepository>();
                services.AddScoped<LogRepository>();

                services.AddScoped<CategoryService>();
                services.AddScoped<UserService>();
                services.AddScoped<LogService>();
                services.AddScoped<FeedbackService>();
                services.AddScoped<AuthenticationService>();

                services.AddSingleton<IUserSession, UserSession>();
                // ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddSingleton<FeedbackViewModel>();
                services.AddTransient<StatisticsViewModel>();
                services.AddTransient<LogViewModel>();
                // One instance ViewModels
                services.AddTransient<MainViewModel>();
                services.AddSingleton<UsersViewModel>();
                services.AddSingleton<CategoriesViewModel>();
                // Views
                services.AddTransient<MainWindow>();
                services.AddTransient<ClientInfoDialog>();
                services.AddTransient<LogInWindow>();
                services.AddTransient<AnalystsView>();
                services.AddTransient<AdminView>();
                services.AddTransient<AddUserWindow>();
                services.AddTransient<AddTagWindow>();
                services.AddTransient<AddNewCategoryWindow>();
                services.AddTransient<StatsWindow>();
                services.AddTransient<LogWindow>();

                ServiceProvider = services.BuildServiceProvider();
                // Запускаємо MainWindow
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup error: {ex.Message}\n\n{ex.StackTrace}",
                      "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
