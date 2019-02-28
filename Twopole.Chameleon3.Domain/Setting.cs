using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Domain
{
    public class Setting 
    {
        public int Id { get; set; }
        /// <summary>
        /// 配置信息键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 配置信息的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 配置信息分组
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 配置信息描述
        /// </summary>
        public string Remark { get; set; }
    }
}
