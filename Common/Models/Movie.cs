using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Movie : MongoDocument
    {
        public string? Name { get; set; }
        public List<string?> Actors { get; set; } = new();
        public decimal? Budget { get; set; }
        public string? Description { get; set; }
    }
}