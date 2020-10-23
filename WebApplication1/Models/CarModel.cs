using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class CarModel
    { 
        public long id { get; set; }
        public string name { get; set; }
        public IEnumerable<IList> attribute { get; set; }

    }
}
