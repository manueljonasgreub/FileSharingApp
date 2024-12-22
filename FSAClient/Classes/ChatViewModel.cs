using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAClient.Classes
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Messages { get; set; }

        public ChatViewModel()
        {
            Messages = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) // Suggestion from ChatGPT
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}