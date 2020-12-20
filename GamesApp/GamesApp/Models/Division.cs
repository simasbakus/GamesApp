using GamesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GamesApp.Models
{
    public class Division : INotifyPropertyChanged
    {
        public string Label { get; set; }
        private bool _IsChecked { get; set; }
        public bool IsChecked
        {
            get { return _IsChecked; }
            set 
            { 
                _IsChecked = value; 
                OnPropertyChanged(nameof(IsChecked)); 
            }
        }
        public string SearchTerm { get; set; }

        public ICommand ToggleIsChecked { get; }

        public Division(string label, bool isChecked, string searchTerm)
        {
            this.Label = label;
            this.IsChecked = isChecked;
            this.SearchTerm = searchTerm;

            ToggleIsChecked = new Command( () => { this.IsChecked = !this.IsChecked; } );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
