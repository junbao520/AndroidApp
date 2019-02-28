using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Providers;

namespace TwoPole.Chameleon3.Infrastructure.Triggers
{
    public abstract class TriggerBase : DisposableBase, ITrigger, IProvider
    {
       // protected ILog Logger { get;  set; }
        public virtual string Name { get;  set; }
        public virtual int Order { get;  set; }
        public bool IsRunning { get; private set; }

        protected TriggerBase()
        {
   
        } 

        public virtual void Start(ExamContext context)
        {
            if(IsRunning)
                throw  new ApplicationException("触发器正在运行，不能被重复执行");

          //  Logger.DebugFormat("正在启动触发器：{0}", Name);
            IsRunning = true;
        }
        public virtual void Stop()
        {
            //Logger.DebugFormat("正在停止触发器：{0}", Name);
            IsRunning = false;
        }

        public virtual void Init(NameValueCollection settings)
        {
     
        }

        protected override void Free(bool disposing)
        {
            if(IsRunning)
                Stop();
        }
    }
}
