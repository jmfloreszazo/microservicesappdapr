using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ObjectEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public enum EventType { Created, Deleted, Updated}
        public EventType Type { get; set; }
    }
   
}
