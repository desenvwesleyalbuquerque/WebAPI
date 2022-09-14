using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSolutions.Desenv.WebApi.Entities.Models
{
    public class ConnectionStrings
    {   
        public string AppDbContext { get; set; }
        public string RedisCache { get; set; }
    }
}
