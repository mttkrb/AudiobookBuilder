using AudiobookBuilder.Commands;
using AudiobookBuilder.Objects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AudiobookBuilder
{
    public class AudioBookBuilderViewModel : INotifyPropertyChanged
    {
        public AudioBookBuilderViewModel()
        {
            _audioConverter = new AudioConverter();
            _audioConverter.OnConvert += _audioConverter_OnConvert;

            FileItems = new ObservableCollection<FileItem>();            
            ConvertCommand = new RelayCommand<ObservableCollection<FileItem>>(o =>_audioConverter.Convert(o), o => !IsBusy && o?.Any() == true);
            AbortCommand = new RelayCommand<object>(o => _audioConverter.Abort(), o=>IsBusy);
        }

        private void _audioConverter_OnConvert(object sender, ConvertEventArgs e)
        {
            switch(e.EventType)
            {
                case ConvertEventType.Aborting:
                    break;
                case ConvertEventType.BeginConvert:
                    IsBusy = true;                    
                    break;
                case ConvertEventType.Cancelled:
                    IsBusy = false;
                    break;
                case ConvertEventType.ConvertToAAC:
                    break;
                case ConvertEventType.EndConvert:
                    IsBusy = false;
                    break;                
                case ConvertEventType.Error:
                    IsBusy = false;
                    break;
                default:
                    break;
            }
            BusyMessage = e.Message;
            CommandManager.InvalidateRequerySuggested();
        }


        private AudioConverter _audioConverter;

        private string _busyMessage;
        public string BusyMessage
        {
            get { return _busyMessage; }
            set { _busyMessage = value; OnPropertyChanged(); }
        }
        
        public RelayCommand<ObservableCollection<FileItem>> ConvertCommand { get; private set; }
        public RelayCommand<object> AbortCommand { get; private set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value;OnPropertyChanged(); }
        }

        private ObservableCollection<FileItem> _fileItems;
        public ObservableCollection<FileItem> FileItems
        {
            get { return _fileItems; }
            set { _fileItems = value;OnPropertyChanged(); }
        }

        private FileItem _selectedFileItem;
        public FileItem SelectedFileItem
        {
            get { return _selectedFileItem; }
            set { _selectedFileItem = value;OnPropertyChanged(); }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
