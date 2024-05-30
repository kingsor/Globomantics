using Globomantics.Domain;
using Globomantics.Infrastructure.Data.Repositories;
using Globomantics.Ui.Factories;
using Globomantics.Ui.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Globomantics.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User CurrentUser { get; set; } = default!;

        public IServiceProvider ServiceProvider { get; init; }
        public IConfiguration Configuration { get; init; }

        public App()
        {
            var builder = new ConfigurationBuilder();

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IRepository<Bug>, TodoInMemoryRepository<Bug>>();
            serviceCollection.AddSingleton<IRepository<Feature>, TodoInMemoryRepository<Feature>>();
            serviceCollection.AddSingleton<IRepository<TodoTask>, TodoInMemoryRepository<TodoTask>>();

            serviceCollection.AddTransient<TodoViewModelFactory>();

            serviceCollection.AddTransient<BugViewModel>();
            serviceCollection.AddTransient<FeatureViewModel>();

            serviceCollection.AddTransient<MainViewModel>();
            serviceCollection.AddTransient<MainWindow>();
            serviceCollection.AddTransient(_ => ServiceProvider!);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = ServiceProvider.GetRequiredService<MainWindow>();

            MainWindow?.Show();

            base.OnStartup(e);
        }
    }

}
