using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveAsOcr
{
    public class SaveResult
    {
        public SaveResultStatus Status { get; set; }
        public string message { get; set; }

        public SaveResult(SaveResultStatus status, string message)
        {
            Status = status;
            this.message = message;
        }
    }

    public enum SaveResultStatus
    {
        FAILURE,
        SUCCESS
    }
}
