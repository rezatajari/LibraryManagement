using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service
{
    public class MessageContract
    {
        public MessageContract()
        {
            Errors = new List<string>();
        }

        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }
    }


    public class MessageContract<T> : MessageContract
    {
        public MessageContract()
        {

        }

        public T Data { get; set; }
    }
}
