using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public class RequestResult
    {
        public RequestResult(bool failed, string errorMessage, TimeSpan latency)
        {
            Failed = failed;
            HasResponse = false;
            ErrorMessage = errorMessage;
            Latency = latency;
        }

        public RequestResult(bool failed, string response, string errorMessage, TimeSpan latency)
        {
            Failed = failed;
            HasResponse = true;
            Response = response;
            ErrorMessage = errorMessage;
            Latency = latency;
        }

        public bool Failed { get; }
        public bool HasResponse { get; }
        public string Response { get; }
        public string ErrorMessage { get; }
        public TimeSpan Latency { get; }
    }
}