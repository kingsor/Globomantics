using CommunityToolkit.Mvvm.Input;
using Globomantics.Domain;
using Globomantics.Infrastructure.Data.Repositories;

namespace Globomantics.Ui.ViewModels
{
    public class FeatureViewModel : BaseTodoViewModel<Feature>
    {
        private readonly IRepository<Feature> _repository;

        private string? _description;

        public string? Description
        {
            get => _description;

            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public FeatureViewModel(IRepository<Feature> repository) : base()
        {
            _repository = repository;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
        }
        public override async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                ShowError?.Invoke($"{nameof(Title)} cannot be empty");

                return;
            }

            if(Model is null)
            {
                Model = new Feature(Title, Description, "UI?", 1, App.CurrentUser, App.CurrentUser)
                {
                    DueDate = DateTimeOffset.Now.AddDays(10),
                    Parent = Parent,
                    IsCompleted = IsCompleted
                };
            }
            else
            {
                Model = Model with
                {
                    Title = Title,
                    Description = Description,
                    Parent = Parent,
                    IsCompleted = IsCompleted
                };
            }

            await _repository.AddAsync(Model);
            await _repository.SaveChangesAsync();

            // TODO: Send message that the item is saved
        }

        public override void UpdateModel(Todo model)
        {
            if (model is not Feature feature) return;

            base.UpdateModel(feature);

            Description = feature.Description;
        }
    }
}
