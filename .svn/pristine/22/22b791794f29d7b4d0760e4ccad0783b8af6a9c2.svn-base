using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace TwoPole.Chameleon3.Domain
{

    public class MapLine 
    {
        public MapLine()
        {
            this.Points = new List<MapLinePoint>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public double Distance { get; set; }

        public double CenterLongitude { get; set; }

        public double CenterLatitude { get; set; }

        public double MinLongitude { get; set; }

        public double MinLatitude { get; set; }

        public double MaxLongitude { get; set; }

        public double MaxLatitude { get; set; }

        public DateTime CreateOn { get; set; }
        
 

        public virtual IList<MapLinePoint> Points { get; set; }
    }
}
