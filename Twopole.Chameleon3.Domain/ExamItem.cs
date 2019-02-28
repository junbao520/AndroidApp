using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace TwoPole.Chameleon3.Domain
{
    public class ExamItem
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string VoiceText { get; set; }
        public string VoiceFile { get; set; }
        public string Remark { get; set; }
        public string IconFile { get; set; }
        public string ExamItemType { get; set; }
        public bool IsEnable { get; set; }

        public string EndVoiceText { get; set; }
        public string EndVoiceFile { get; set; }
        public int SequenceNumber { get; set; }
        public MapPointType MapPointType { get; set; }


        /// <summary>
        /// 是通用 还是地区
        /// </summary>
        public string LocalityName { get; set; }
    }
}
