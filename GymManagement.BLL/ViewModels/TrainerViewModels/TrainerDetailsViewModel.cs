using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModels
{
    public class TrainerDetailsViewModel : TrainerViewModel
    {
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}
