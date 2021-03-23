using System;
using System.Collections.Generic;

namespace Models
{
    public class ObjectGroup
    {
        public Guid Id { get; set; }
        public List<Object> Objects { get; set; }
    }
}
