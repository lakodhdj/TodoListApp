using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TodoListApp.Models;

namespace TodoListApp.Services
{
    public class DataService
    {
        private const string DATA_FILE = "todos.json";

        public List<TodoItem> LoadTodos()
        {
            try
            {
                if (File.Exists(DATA_FILE))
                {
                    var json = File.ReadAllText(DATA_FILE);
                    var todos = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
                    return todos;
                }
            }
            catch { }
            return new List<TodoItem>();
        }

        public void AddTodo(TodoItem todo)
        {
            var todos = LoadTodos();
            todos.Add(todo);
            SaveTodos(todos);
        }

        public void UpdateTodo(TodoItem updatedTodo)
        {
            var todos = LoadTodos();
            var existing = todos.FirstOrDefault(t => t.Id == updatedTodo.Id);
            if (existing != null)
            {
                existing.Title = updatedTodo.Title;
                existing.Description = updatedTodo.Description;
                existing.IsCompleted = updatedTodo.IsCompleted;
                existing.CompletedDate = updatedTodo.CompletedDate;
            }
            SaveTodos(todos);
        }

        public void DeleteTodo(string id)
        {
            var todos = LoadTodos();
            var todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                todos.Remove(todo);
                SaveTodos(todos);
            }
        }

        private void SaveTodos(List<TodoItem> todos)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(todos, options);
                File.WriteAllText(DATA_FILE, json);
            }
            catch { }
        }
    }
}