using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PriutForAnimal.Entities;

namespace PriutForAnimlApp.Views.Pages
{
    public partial class AnimalPage : Page, INotifyPropertyChanged
    {
        private Animal _currentAnimal;
        private ObservableCollection<Animal> _animals;
        private bool _isEditMode;

        public event PropertyChangedEventHandler PropertyChanged;

        public Animal CurrentAnimal
        {
            get => _currentAnimal;
            set
            {
                _currentAnimal = value;
                OnPropertyChanged(nameof(CurrentAnimal));
                OnPropertyChanged(nameof(AnimalInfoDisplay));
            }
        }

        public ObservableCollection<Animal> Animals
        {
            get => _animals;
            set
            {
                _animals = value;
                OnPropertyChanged(nameof(Animals));
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

        public AnimalPage()
        {
            InitializeComponent();
            DataContext = this;

            Animals = new ObservableCollection<Animal>();
            CurrentAnimal = new Animal();
            IsEditMode = false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateButtonStates()
        {
            AddButton.IsEnabled = !IsEditMode;
            DeleteButton.IsEnabled = !IsEditMode && CurrentAnimal?.AnimalId != Guid.Empty;
            EditButton.IsEnabled = !IsEditMode && CurrentAnimal?.AnimalId != Guid.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentAnimal = new Animal();
            IsEditMode = true;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAnimal != null)
            {
                IsEditMode = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAnimal != null &&
                MessageBox.Show("Delete this animal?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Animals.Remove(CurrentAnimal);
                CurrentAnimal = new Animal();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            IsEditMode = false;
            CurrentAnimal = new Animal();
        }

        private void AnimalsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Animal selectedAnimal)
            {
                CurrentAnimal = new Animal
                {
                    AnimalId = selectedAnimal.AnimalId,
                    Type = selectedAnimal.Type,
                    Name = selectedAnimal.Name,
                    Age = selectedAnimal.Age,
                    HealthStatus = selectedAnimal.HealthStatus,
                    VisitCount = selectedAnimal.VisitCount
                };
            }
            UpdateButtonStates();
        }
    }
}
