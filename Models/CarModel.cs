using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_api_crud.Models
{
    public class CarModel
    {
        public int id { get; set; }
        public int num_doors { get; set; }

        public string brand { get; set; }

        public string model { get; set; }

    }
}
