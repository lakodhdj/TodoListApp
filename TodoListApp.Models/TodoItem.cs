using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace TodoListApp.Models
{
    public class TodoItem : INotifyPropertyChanged
    {
        private string _title = "";
        private string _description = "";
        private bool _isCompleted;

        public string Id { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                _title = value ?? "";
                OnPropertyChanged();
            }
        }


        public string Description
        {
            get => _description;
            set
            {
                _description = value ?? "";
                OnPropertyChanged();
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
                // Автоматически сохраняем изменение статуса
                CompletedDate = value ? DateTime.Now : null;
            }
        }

        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TodoItem()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}