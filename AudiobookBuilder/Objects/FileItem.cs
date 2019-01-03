using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AudiobookBuilder.Objects
{
    public class FileItem : INotifyPropertyChanged
    {
        public FileItem(string path)
        {
            Path = path;
        }

        public string FileName
        {
            get { return Path?.Split('\\').Last(); }            
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            private set { _path = value; OnPropertyChanged(); }
        }

        private string _workingPath;

        public string WorkingPath
        {
            get { return _workingPath; }
            set { _workingPath = value;OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
