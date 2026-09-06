using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC1.DAL.Models
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
