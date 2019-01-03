using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudiobookBuilder.Objects
{
    public enum ConvertEventType
    {
        BeginConvert,
        EndConvert,
        ConvertToAAC,
        Error,
        Cancelled,
        Aborting
    }
}
