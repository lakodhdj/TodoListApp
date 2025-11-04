using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TodoListApp.Models;
using TodoListApp.Services;

namespace TodoListApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private TodoItem _selectedTodo;
        private string _newTaskTitle;
        private string _newTaskDescription;

        public ObservableCollection<TodoItem> Todos { get; set; }

        public TodoItem SelectedTodo
        {
            get => _selectedTodo;
            set
            {
                _selectedTodo = value;
                OnPropertyChanged();
            }
        }

        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set
            {
                _newTaskTitle = value;
                OnPropertyChanged();
            }
        }

        public string NewTaskDescription
        {
            get => _newTaskDescription;
            set
            {
                _newTaskDescription = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public MainViewModel()
        {
            _dataService = new DataService();
            Todos = new ObservableCollection<TodoItem>();
            LoadTodos();

            AddTaskCommand = new RelayCommand(AddTask);
            EditTaskCommand = new RelayCommand(EditTask, CanEdit);
            SaveEditCommand = new RelayCommand(SaveEdit, CanSaveEdit);
            CancelEditCommand = new RelayCommand(CancelEdit, CanCancelEdit);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
        }

        private void LoadTodos()
        {
            var todos = _dataService.LoadTodos();
            Todos.Clear();
            foreach (var todo in todos)
            {
                Todos.Add(todo);
            }
        }

        private void AddTask(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewTaskTitle))
            {
                var newTodo = new TodoItem
                {
                    Title = NewTaskTitle,
                    Description = NewTaskDescription ?? ""
                };
                _dataService.AddTodo(newTodo);
                Todos.Add(newTodo);
                NewTaskTitle = "";
                NewTaskDescription = "";
            }
        }

        // ✅ РЕДАКТИРОВАНИЕ через отдельное окно
        private void EditTask(object parameter)
        {
            if (SelectedTodo != null)
            {
                var editWindow = new EditTaskWindow(SelectedTodo);
                if (editWindow.ShowDialog() == true)
                {
                    _dataService.UpdateTodo(SelectedTodo);
                    MessageBox.Show("Задача обновлена!");
                }
            }
        }

        private bool CanEdit(object parameter)
        {
            return SelectedTodo != null;
        }

        private void SaveEdit(object parameter)
        {
            if (SelectedTodo != null)
            {
                _dataService.UpdateTodo(SelectedTodo);
                MessageBox.Show("Сохранено!");
            }
        }

        private bool CanSaveEdit(object parameter)
        {
            return SelectedTodo != null;
        }

        private void CancelEdit(object parameter)
        {
            LoadTodos();
            MessageBox.Show("Отменено!");
        }

        private bool CanCancelEdit(object parameter)
        {
            return true;
        }

        private void DeleteTask(object parameter)
        {
            if (SelectedTodo != null)
            {
                var result = MessageBox.Show($"Удалить '{SelectedTodo.Title}'?",
                                           "Удаление", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _dataService.DeleteTodo(SelectedTodo.Id);
                    Todos.Remove(SelectedTodo);
                    SelectedTodo = null;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}