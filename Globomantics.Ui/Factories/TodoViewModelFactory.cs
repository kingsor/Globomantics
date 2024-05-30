using Globomantics.Domain;
using Globomantics.Ui.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Ui.Factories
{
    public class TodoViewModelFactory
    {
        public static IEnumerable<string> TodoTypes = new[]
        {
            nameof(Bug),
            nameof(Feature)
        };
        private readonly IServiceProvider _serviceProvider;

        public TodoViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITodoViewModel CreateViewModel(string type,
            IEnumerable<Todo> tasks,
            Todo? model = default)
        {
            ITodoViewModel? viewModel = type switch
            {
                nameof(Bug) => _serviceProvider.GetService<BugViewModel>(),
                nameof(Feature) => _serviceProvider.GetService<FeatureViewModel>(),
                _ => throw new NotImplementedException()
            };

            ArgumentNullException.ThrowIfNull(viewModel);

            if(tasks is not null && tasks.Any())
            {
                viewModel.AvailableParentTasks = tasks;
            }

            if (model is not null)
            {
                viewModel.UpdateModel(model);
            }

            return viewModel;
        }
    }
}
