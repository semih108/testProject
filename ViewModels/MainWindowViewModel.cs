using ReactiveUI;
using System;
using System.Reactive.Linq;
using testProject.Models;
using testProject.Services;

namespace testProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _contentViewModel;
        public TodoListViewModel List { get; }

        public MainWindowViewModel(Database db)
        {
            List = new TodoListViewModel(db.GetItems());
            _contentViewModel = List;
        }

        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }
        

        public void AddItem()
        {
            AddItemViewModel addItemViewModel = new();
            Observable.Merge(
                addItemViewModel.OkCommand,
                addItemViewModel.CancelCommand.Select(_ => (TodoItem?)null))
                .Take(1)
                .Subscribe(newItem =>
                {
                    if (newItem != null)
                    {
                        List.Items.Add(newItem);
                    }
                    ContentViewModel = List;
                });
            ContentViewModel = addItemViewModel;
        }
    }
}