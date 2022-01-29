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
        public MessageContract(bool success, string message = null, List<string> errors = null)
        {
            IsSuccess = success;
            Message = message;
            Errors = errors;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }


    public class MessageContract<T> : MessageContract
    {

        public MessageContract() : base(success: false)
        {

        }

        public MessageContract(T data) : base(success: true)
        {
            Data = data;
        }

        public MessageContract(T data, PagedResult paged) : base(success: true)
        {
            Data = data;
            Paged = paged;
        }

        public T Data { get; set; }

        public PagedResult Paged { get; set; }
    }
}
