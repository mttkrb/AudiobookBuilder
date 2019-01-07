using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudiobookBuilder.Objects
{
    public class InputFileItem : FileItem
    {
        public InputFileItem(string path) : base(path)
        {
        }

        private string _workingPath;

        public string WorkingPath
        {
            get { return _workingPath; }
            set { _workingPath = value; OnPropertyChanged(); }
        }
    }
}
