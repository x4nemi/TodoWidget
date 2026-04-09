using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;

namespace TodoApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<TodoItem> Todos { get; } = new ObservableCollection<TodoItem>
    {
        new TodoItem { Title = "Buy groceries" },
        new TodoItem { Title = "Walk the dog" },
        new TodoItem { Title = "Finish project" }
    };

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddTodoCommand))]
    private string _newTodoText = string.Empty;

    [RelayCommand(CanExecute = nameof(CanAddTodo))]
    private void AddTodo()
    {
        Todos.Add(new TodoItem { Title = NewTodoText.Trim() });
        NewTodoText = string.Empty;
    }

    private bool CanAddTodo() => !string.IsNullOrWhiteSpace(NewTodoText);

    [RelayCommand]
    private void DeleteTodo(TodoItem item) => Todos.Remove(item);
}
