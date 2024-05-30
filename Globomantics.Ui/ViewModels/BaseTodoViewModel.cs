using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Globomantics.Domain;
using Microsoft.Win32;
using System.Windows.Input;

namespace Globomantics.Ui.ViewModels;

public abstract class BaseTodoViewModel<T> : ObservableObject, ITodoViewModel
    where T : Todo
{
    private T? _model;
    private string? _title;
    private bool _isCompleted;
    private Todo? _parent;

    public BaseTodoViewModel()
    {
        DeleteCommand = new RelayCommand(() =>
        {
            if (Model is not null)
            {
                Model = Model with { IsDeleted = true };

                // TODO: Send message that Model is deleted
            };
        });
    }

    public T? Model
    {
        get => _model;
        set
        {
            _model = value;
            OnPropertyChanged(nameof(Model));
            OnPropertyChanged(nameof(IsExisting));
        }
    }

    public bool IsExisting => Model is not null;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            _isCompleted = value;
            OnPropertyChanged(nameof(IsCompleted));
        }
    }

    public Todo? Parent
    {
        get => _parent;
        set
        {
            _parent = value;
            OnPropertyChanged(nameof(Parent));
        }
    }

    #region From ITodoViewModel and IViewModel

    public IEnumerable<Todo>? AvailableParentTasks { get; set; }

    public ICommand DeleteCommand { get; }

    public ICommand SaveCommand { get; set; } = default!;

    public Action<string>? ShowAlert { get; set; }
    public Action<string>? ShowError { get; set; }
    public Func<IEnumerable<string>>? ShowOpenFileDialog { get; set; }
    public Func<string>? ShowSaveFileDialog { get; set; }
    public Func<string, bool>? AskForConfirmation { get; set; }

    #endregion


    public abstract Task SaveAsync();

    public virtual void UpdateModel(Todo model)
    {
        if(model is null)
        {
            return;
        }

        var parent = AvailableParentTasks?.SingleOrDefault(
            t => t.Parent is not null && t.Parent?.Id == model.Parent?.Id
        );

        Model = model as T;
        Title = model.Title;
        IsCompleted = model.IsCompleted;
        Parent = parent;
    }

    
}
