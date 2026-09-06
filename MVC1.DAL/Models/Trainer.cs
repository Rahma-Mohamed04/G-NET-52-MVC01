using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC1.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialty Specialty { get; set; }
    }

    public enum Specialty
    {
        GeneralFitness = 1,
        Yoga,
        Boxing,
        CrossFit,
    }


}
