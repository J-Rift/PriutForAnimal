using PriutForAnimal.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PriutForAnimlApp.ViewModels
{
    public class AnimalPageViewModel : INotifyPropertyChanged
    {
        private Animal _currentAnimal;
        private bool _isEditMode;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Animal> Animals { get; private set; }

        public Animal CurrentAnimal
        {
            get => _currentAnimal;
            set
            {
                _currentAnimal = value;
                OnPropertyChanged(nameof(CurrentAnimal));
                OnPropertyChanged(nameof(AnimalInfoDisplay));
                UpdateButtonStates();
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
                UpdateButtonStates();
            }
        }

        public string AnimalInfoDisplay => CurrentAnimal != null
            ? $"ID: {CurrentAnimal.AnimalId}\nType: {CurrentAnimal.Type}\nName: {CurrentAnimal.Name}\nAge: {CurrentAnimal.Age}\nHealth: {CurrentAnimal.HealthStatus}\nVisits: {CurrentAnimal.VisitCount}"
            : "No animal selected";

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public bool CanAddOrEdit => !IsEditMode;
        public bool CanDelete => !IsEditMode && CurrentAnimal?.AnimalId != Guid.Empty;

        public AnimalPageViewModel()
        {
            Animals = new ObservableCollection<Animal>();
            CurrentAnimal = new Animal();

            // Инициализация команд
            AddCommand = new RelayCommand(_ => AddAnimal());
            EditCommand = new RelayCommand(_ => EditAnimal(), _ => CanAddOrEdit && CurrentAnimal?.AnimalId != Guid.Empty);
            DeleteCommand = new RelayCommand(_ => DeleteAnimal(), _ => CanDelete);
            SaveCommand = new RelayCommand(_ => SaveAnimal());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void AddAnimal()
        {
            CurrentAnimal = new Animal();
            IsEditMode = true;
        }

        private void EditAnimal()
        {
            if (CurrentAnimal != null)
            {
                IsEditMode = true;
            }
        }

        private void DeleteAnimal()
        {
            if (CurrentAnimal != null &&
                MessageBox.Show("Delete this animal?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Animals.Remove(CurrentAnimal);
                CurrentAnimal = new Animal();
            }
        }

        private void SaveAnimal()
        {
            if (!ValidateAnimalData())
                return;

            if (CurrentAnimal.AnimalId == Guid.Empty) // New animal
            {
                CurrentAnimal.AnimalId = Guid.NewGuid();
                CurrentAnimal.VisitCount = 0;
                Animals.Add(CurrentAnimal);
            }
            else // Existing animal
            {
                var existingAnimal = Animals.FirstOrDefault(a => a.AnimalId == CurrentAnimal.AnimalId);
                if (existingAnimal != null)
                {
                    existingAnimal.Name = CurrentAnimal.Name;
                    existingAnimal.Type = CurrentAnimal.Type;
                    existingAnimal.Age = CurrentAnimal.Age;
                    existingAnimal.HealthStatus = CurrentAnimal.HealthStatus;
                }
            }

            ResetForm();
        }

        private bool ValidateAnimalData()
        {
            if (string.IsNullOrWhiteSpace(CurrentAnimal.Name))
            {
                MessageBox.Show("Please enter animal name", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CurrentAnimal.Age <= 0)
            {
                MessageBox.Show("Age must be positive number", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Cancel()
        {
            ResetForm();
        }

        private void ResetForm()
        {
            IsEditMode = false;
            CurrentAnimal = new Animal();
        }

        private void UpdateButtonStates()
        {
            AddCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Реализация RelayCommand для MVVM
    public class RelayCommand : System.Windows.Input.ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() =>
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
    }
}
