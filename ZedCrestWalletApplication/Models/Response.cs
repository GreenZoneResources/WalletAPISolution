using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrestWalletApplication.Models
{
    public class Response
    {
        public string RequestId => $"{Guid.NewGuid().ToString()}";
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public Object Data { get; set; }
    }
}
