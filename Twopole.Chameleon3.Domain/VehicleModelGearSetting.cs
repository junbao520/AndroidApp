using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoPole.Chameleon3.Domain
{
    public class VehicleModelGearSetting 
    {
        public VehicleModelGearSetting()
        {
            UpdatedOn = DateTime.Now;
        }

        public Gear Gear { get; set; }
        public int MinRatio { get; set; }
        public int MaxRatio { get; set; }

        public DateTime UpdatedOn { get; set; }
        public int VehicleModelId { get; set; }

        public virtual VehicleModel VehicleModel { get; set; }
    }
}
