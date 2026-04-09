using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using TodoApp.Models;
using TodoApp.ViewModels;

namespace TodoApp.Views;

public partial class TodoListView : UserControl
{
    private TodoItem? _draggedItem;
    private Grid? _draggedGrid;
    private bool _isDragging;
    private Point _startPoint;
    private const double DragThreshold = 5;

    public TodoListView()
    {
        InitializeComponent();

        var input = this.FindControl<TextBox>("InputBox");
        if (input != null)
            input.KeyDown += (_, e) =>
            {
                if (e.Key == Key.Enter && DataContext is TodoListViewModel vm && vm.AddTodoCommand.CanExecute(null))
                    vm.AddTodoCommand.Execute(null);
            };
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (e.Source is Control source && !IsButton(source))
        {
            var grid = FindParentGrid(source);
            if (grid?.DataContext is TodoItem item)
            {
                _draggedItem = item;
                _draggedGrid = grid;
                _startPoint = e.GetPosition(this);
                _isDragging = false;
                e.Pointer.Capture(this);
            }
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (_draggedItem == null || _draggedGrid == null) return;

        var pos = e.GetPosition(this);
        var delta = pos - _startPoint;

        if (!_isDragging && (System.Math.Abs(delta.X) > DragThreshold || System.Math.Abs(delta.Y) > DragThreshold))
        {
            _isDragging = true;
            _draggedGrid.Opacity = 0.4;
        }

        if (_isDragging)
        {
            ResetAllRowBackgrounds();
            var targetGrid = FindGridAtPoint(pos);
            if (targetGrid != null && targetGrid != _draggedGrid && targetGrid.DataContext is TodoItem)
            {
                targetGrid.Background = new SolidColorBrush(Color.Parse("#333333"));
            }
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_draggedItem != null && _isDragging && DataContext is TodoListViewModel vm)
        {
            var pos = e.GetPosition(this);
            var targetGrid = FindGridAtPoint(pos);
            if (targetGrid?.DataContext is TodoItem targetItem && targetItem != _draggedItem)
            {
                var oldIndex = vm.Todos.IndexOf(_draggedItem);
                var newIndex = vm.Todos.IndexOf(targetItem);
                vm.MoveTodo(oldIndex, newIndex);
            }
        }

        if (_draggedGrid != null)
            _draggedGrid.Opacity = 1.0;

        ResetAllRowBackgrounds();

        _draggedItem = null;
        _draggedGrid = null;
        _isDragging = false;
        e.Pointer.Capture(null);
    }

    private Grid? FindGridAtPoint(Point point)
    {
        var visual = this.GetVisualAt(point);
        if (visual is Control control)
            return FindParentGrid(control);
        return null;
    }

    private void ResetAllRowBackgrounds()
    {
        if (DataContext is not TodoListViewModel vm) return;
        var itemsControl = this.FindControl<ItemsControl>("TodoList");
        if (itemsControl == null) return;

        for (int i = 0; i < vm.Todos.Count; i++)
        {
            var container = itemsControl.ContainerFromIndex(i) as ContentPresenter;
            if (container?.Child is Grid g)
                g.Background = Brushes.Transparent;
        }
    }

    private static bool IsButton(Control control)
    {
        Control? current = control;
        while (current != null)
        {
            if (current is Button) return true;
            current = current.Parent as Control;
        }
        return false;
    }

    private static Grid? FindParentGrid(Control control)
    {
        Control? current = control;
        while (current != null)
        {
            if (current is Grid g && g.DataContext is TodoItem)
                return g;
            current = current.Parent as Control;
        }
        return null;
    }
}
