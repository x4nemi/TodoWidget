using CommunityToolkit.Mvvm.ComponentModel;
using TodoApp.Models;

namespace TodoApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ObservableObject _currentPage;

    public TodoListViewModel TodoList { get; }

    public MainWindowViewModel()
    {
        TodoList = new TodoListViewModel();
        TodoList.OnSelectItem = NavigateToDetail;
        _currentPage = TodoList;
    }

    private void NavigateToDetail(TodoItem item)
    {
        var detail = new TodoDetailViewModel(item);
        detail.OnGoBack = () => CurrentPage = TodoList;
        CurrentPage = detail;
    }
}
