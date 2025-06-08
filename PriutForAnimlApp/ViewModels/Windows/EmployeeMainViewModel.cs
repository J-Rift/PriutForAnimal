using PriutForAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PriutForAnimalDbContext;

namespace PriutForAnimlApp.ViewModels.Windows
{
    public class EmployeeMainViewModel
    {
        private ObservableCollection<Visitor> _visitors;
        private Visitor _selectedVisitor;
        private ObservableCollection<Animal> _animals;
        private Animal _selectedAnimal;

        public ObservableCollection<Visitor> Visitors
        {
            get => _visitors;
            set { _visitors = value; OnPropertyChanged(); }
        }

        public Visitor SelectedVisitor
        {
            get => _selectedVisitor;
            set { _selectedVisitor = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Animal> Animals
        {
            get => _animals;
            set { _animals = value; OnPropertyChanged(); }
        }

        public Animal SelectedAnimal
        {
            get => _selectedAnimal;
            set { _selectedAnimal = value; OnPropertyChanged(); }
        }

        public ICommand AddVisitorCommand { get; }
        public ICommand EditVisitorCommand { get; }
        public ICommand DeleteVisitorCommand { get; }
        public ICommand AddAnimalCommand { get; }
        public ICommand EditAnimalCommand { get; }
        public ICommand DeleteAnimalCommand { get; }

        public EmployeeMainViewModel()
        {
            // Initialize commands
            AddVisitorCommand = new RelayCommand(AddVisitor);
            EditVisitorCommand = new RelayCommand(EditVisitor, CanEditVisitor);
            DeleteVisitorCommand = new RelayCommand(DeleteVisitor, CanDeleteVisitor);

            AddAnimalCommand = new RelayCommand(AddAnimal);
            EditAnimalCommand = new RelayCommand(EditAnimal, CanEditAnimal);
            DeleteAnimalCommand = new RelayCommand(DeleteAnimal, CanDeleteAnimal);

            // Load data
            LoadVisitors();
            LoadAnimals();
        }

        private void LoadVisitors()
        {
            // Implement data loading
            Visitors = new ObservableCollection<Visitor>();
        }

        private void LoadAnimals()
        {
            // Implement data loading
            Animals = new ObservableCollection<Animal>();
        }

        private void AddVisitor(object obj)
        {
            // Implement add visitor logic
        }

        private void EditVisitor(object obj)
        {
            // Implement edit visitor logic
        }

        private bool CanEditVisitor(object obj) => SelectedVisitor != null;

        private void DeleteVisitor(object obj)
        {
            // Implement delete visitor logic
        }

        private bool CanDeleteVisitor(object obj) => SelectedVisitor != null;

        private void AddAnimal(object obj)
        {
            // Implement add animal logic
        }

        private void EditAnimal(object obj)
        {
            // Implement edit animal logic
        }

        private bool CanEditAnimal(object obj) => SelectedAnimal != null;

        private void DeleteAnimal(object obj)
        {
            // Implement delete animal logic
        }

        private bool CanDeleteAnimal(object obj) => SelectedAnimal != null;
    }
}
