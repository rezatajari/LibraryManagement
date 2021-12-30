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

        }

        public bool IsFaild { get; set; }

        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }

        public List<string> Success { get; set; }
    }


    public class MessageContract<T> : MessageContract
    {
        public MessageContract()
        {

        }

        public T Data { get; set; }
    }
}
