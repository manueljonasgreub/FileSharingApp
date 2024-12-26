using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FSAClient.Classes
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ChatMessage> Messages { get; set; }

        public ChatViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) // Suggestion from ChatGPT
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddMessage(string message, Brush color) // Suggestion from ChatGPT
        {
            Messages.Add(new ChatMessage { Message = message, Color = color });
        }
    }

    public class ChatMessage
    {
        public string Message { get; set; }
        public Brush Color { get; set; }
    }
}