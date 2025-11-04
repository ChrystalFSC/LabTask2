using System.Windows.Input;
using Prism.Commands;
using LabTask2.Models;

namespace LabTask2.ViewModels.Pages
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private string _message = string.Empty;
        private MyModel _model = new();
        private int _counter;
        private bool _isFavorite;

        public MyModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (SetProperty(ref _isFavorite, value))
                    RaisePropertyChanged(nameof(FavoriteIconText));
            }
        }

        public string FavoriteIconText => IsFavorite ? "♥" : "♡";

        public ICommand TestCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }

        public MainPageViewModel()
        {
            Model = new MyModel
            {
                Image = "pet.jpg",
                Hello = "Golden",
                Welcome = "Golden Retriever"
            };

            Message = "Adopt Me";
            TestCommand = new DelegateCommand(Count);
            ToggleFavoriteCommand = new DelegateCommand(() => IsFavorite = !IsFavorite);
        }

        private void Count()
        {
            _counter++;
            Message = $"Clicked {_counter}";
        }
    }
}
