using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSocketConnection
{
    public class SocketErrorEventArgs
    {

        public SocketErrorEventArgs()
        {

        }
        public SocketErrorEventArgs(string error_message, string error_stack)
        {
            this.ErrorMessage = error_message;
            this.ErrorStack = error_stack;
        }

        public string ErrorMessage { private set; get; } = "";      //エラーメッセージ

        public string ErrorStack { private set; get; } = "";        //エラースタック
    }
}
