using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    /// <summary>
    /// 考试结束的触发器
    /// </summary>
    public abstract class ExamEndTrigger : TriggerBase
    {
        public override void Stop()
        {
            if(IsRunning)
                Valid();

            base.Stop();
        }

        protected abstract void Valid();
    }
}
