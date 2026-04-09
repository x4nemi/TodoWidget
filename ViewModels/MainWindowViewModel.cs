using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;

namespace TodoApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<TodoItem> Todos { get; } = new ObservableCollection<TodoItem>
    {
        new TodoItem { Title = "Buy groceries", Priority = 1 },
        new TodoItem { Title = "Walk the dog", Priority = 2 },
        new TodoItem { Title = "Finish project", Priority = 3 }
    };

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddTodoCommand))]
    private string _newTodoText = string.Empty;

    [RelayCommand(CanExecute = nameof(CanAddTodo))]
    private void AddTodo()
    {
        var lastPriority = Todos.Count > 0 ? Todos.Max(t => t.Priority) : 0;
        Todos.Add(new TodoItem { Title = NewTodoText.Trim(), Priority = lastPriority + 1 });
        NewTodoText = string.Empty;
    }

    private bool CanAddTodo() => !string.IsNullOrWhiteSpace(NewTodoText);

    [RelayCommand]
    private void DeleteTodo(TodoItem item) => Todos.Remove(item);
}
