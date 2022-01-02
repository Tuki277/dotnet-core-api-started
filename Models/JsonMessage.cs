using System.Collections.Generic;

namespace APIStarted
{
    public class JSONMessage<T>
    {
        public string Message { get; set; }
        public bool Code { get; set; }
        public List<T> Data { get; set; }
    }
}