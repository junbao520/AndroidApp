using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 下一个项目
    /// </summary>
    public class NextExamItem:ExamItemBase
    {

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            base.ExecuteCore(signalInfo);
            //语音播报后结束
            StopCore();
        }
        public override string ItemCode
        {
            get { return ExamItemCodes.NextItem; }
        }
    }
}
