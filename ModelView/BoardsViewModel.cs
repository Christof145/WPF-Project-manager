﻿using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using WpfPractice.Model;

namespace WpfPractice.ModelView
{
    class BoardsViewModel : INotifyPropertyChanged, INotifyCollectionChanged
    {
        private ObservableCollection<BoardViewModel> _boards = new ObservableCollection<BoardViewModel>();
        private BoardViewModel _selectedItem;
        private bool _editable = false;
        private TaskViewModel _selectedTask;

        public BoardsViewModel()
        {
            BoardViewModel board1 = new BoardViewModel(new Board("Board1", "1 Description"));
            Silo silo = new Silo("Sprint Backlog");
            silo.AddTask(new Task("Create a structure."));
            silo.AddTask(new Task("Establish a relationship."));
            silo.AddTask(new Task("Establish a relationship."));
            board1.Silos = new ObservableCollection<Silo>(){ { silo }, { new Silo("Things To Do") } };
            BoardViewModel board2 = new BoardViewModel(new Board("Board2", "2 Description"));
            _boards.Add(board1);
            _boards.Add(board2);
            Boards.CollectionChanged += this.OnCollectionChanged;
            _selectedItem = board1;
            _selectedTask = new TaskViewModel(silo.Tasks[0]);
        }

        public RelayCommand EditCommand
        {
            get
            {
                return new RelayCommand(
                    () => {
                        _editable = true;
                        OnPropertyChanged("Editable");
                        Debug.Write("Editable");
                    });
            }
        }

        public ObservableCollection<BoardViewModel> Boards { get => _boards; set => _boards = value; }
        private void Button_Click(object sender, EventHandler e)
        {
            Debug.Write("send");
        }
        private void Task_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.Write("Item changed");
            ListBox lb = (ListBox)sender;
            SelectedTask.ChangeModel((Task)lb.SelectedItem);
            SelectedTask.OnPropertyChanged();
        }
        public TaskViewModel SelectedTask { get => _selectedTask; set { _selectedTask = value; OnPropertyChanged(); } }
        public BoardViewModel SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); SelectedItem.OnPropertyChanged();} }
        public bool Editable { get => _editable; set => _editable = value; }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Get the sender observable collection
            ObservableCollection<BoardViewModel> obsSender = sender as ObservableCollection<BoardViewModel>;
            NotifyCollectionChangedAction action = e.Action;

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}