using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoPole.Chameleon3.Domain
{
    public class LightRule 
    {

        public  int Id { get; set; }
        public bool IsEnable { get; set; }

        public int Order { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string VoiceText { get; set; }
        public string VoiceFile { get; set; }
        public string LightRuleType { get; set; }

        /// <summary>
        /// 项目名称和路径
        /// </summary>
        public  Dictionary<string, string> dicType { get; set; }
        /// <summary>
        /// 操作说明
        /// </summary>
        public string OperDes { get; set; }
    }
}
