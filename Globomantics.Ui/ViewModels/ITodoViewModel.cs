using Globomantics.Domain;
using System.Windows.Input;

namespace Globomantics.Ui.ViewModels;

public interface ITodoViewModel : IViewModel
{
    IEnumerable<Todo>? AvailableParentTasks { get; set; }

    ICommand DeleteCommand { get; }
    ICommand SaveCommand { get; set; }
    Task SaveAsync();
    void UpdateModel(Todo model);
}
