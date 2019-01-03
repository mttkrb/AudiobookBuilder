using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudiobookBuilder.Objects
{
    public class ConvertEventArgs : EventArgs
    {
        public ConvertEventArgs(ConvertEventType eventType) : this(eventType, string.Empty)
        {

        }

        public ConvertEventArgs(ConvertEventType eventType, string message)
        {
            EventType = eventType;
            Message = message;           
        }
        
        public ConvertEventType EventType {get;private set;}
        public string Message { get; private set; }
    }
}
