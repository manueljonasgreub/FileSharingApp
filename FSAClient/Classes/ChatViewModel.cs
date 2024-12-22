using System.Collections.ObjectModel;
using System.ComponentModel;

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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}