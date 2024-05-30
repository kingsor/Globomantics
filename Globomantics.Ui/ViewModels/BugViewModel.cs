using CommunityToolkit.Mvvm.Input;
using Globomantics.Domain;
using Globomantics.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Ui.ViewModels
{
    public class BugViewModel : BaseTodoViewModel<Bug>
    {
        private readonly IRepository<Bug> _repository;

        private string? _description;
        private string? _affectedVersion;
        private int _affectedUsers;
        private DateTimeOffset _dueDate;
        private Severity _severity;

        public string? Description
        {
            get => _description;

            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string AffectedVersion
        {
            get => _affectedVersion;

            set
            {
                _affectedVersion = value;
                OnPropertyChanged();
            }
        }

        public int AffectedUsers
        {
            get => _affectedUsers;

            set
            {
                _affectedUsers = value;
                OnPropertyChanged();
            }
        }

        public DateTimeOffset DueDate
        {
            get => _dueDate;

            set
            {
                _dueDate = value;
                OnPropertyChanged();
            }
        }

        public Severity Severity
        {
            get => _severity;

            set
            {
                _severity = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Severity> SeverityLevels { get; } = new[]
        {
            Severity.Critical,
            Severity.Annoying,
            Severity.Major,
            Severity.Minor
        };

        public ObservableCollection<byte[]> Screenshots { get; set; } = new();

        public ICommand AttachScreenshotCommand { get; set; }

        public BugViewModel(IRepository<Bug> repository) : base()
        {
            _repository = repository;

            SaveCommand = new RelayCommand(async() => await SaveAsync());

            AttachScreenshotCommand = new RelayCommand(() =>
            {
                var filenames = ShowOpenFileDialog?.Invoke();

                if (filenames is null || !filenames.Any()) 
                {
                    return;
                }

                foreach (var filename in filenames)
                {
                    Screenshots.Add(File.ReadAllBytes(filename));
                }
            });
        }

        public override async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                ShowError?.Invoke($"{nameof(Title)} cannot be empty");

                return;
            }

            if (Model is null)
            {
                Model = new Bug(Title, 
                    Description ?? "No description", 
                    Severity,
                    AffectedVersion,
                    AffectedUsers, 
                    App.CurrentUser, 
                    App.CurrentUser,
                    Screenshots.ToArray())
                {
                    DueDate = DueDate,
                    Parent = Parent,
                    IsCompleted = IsCompleted
                };
            }
            else
            {
                Model = Model with
                {
                    Title = Title,
                    Description = Description ?? "No description",
                    Severity = Severity,
                    AffectedVersion = AffectedVersion,
                    AffectedUsers = AffectedUsers,
                    DueDate = DueDate,
                    Parent = Parent,
                    IsCompleted = IsCompleted,
                    Images = Screenshots.ToArray()
                };
            }

            await _repository.AddAsync(Model);
            await _repository.SaveChangesAsync();

            // TODO: Send message that the item is saved
        }

        public override void UpdateModel(Todo model)
        {
            if (model is not Bug bug) return;

            base.UpdateModel(model);

            Description = bug.Description;
            AffectedVersion = bug.AffectedVersion;
            AffectedUsers = bug.AffectedUsers;
            Severity = bug.Severity;
            Screenshots = new(bug.Images);
            DueDate = bug.DueDate;
        }
    }
}
