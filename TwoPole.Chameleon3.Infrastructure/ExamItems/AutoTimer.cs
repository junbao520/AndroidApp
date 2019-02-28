using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation;

namespace TwoPole.Chameleon3.Infrastructure
{
    public sealed class AutoTimer : DisposableBase
    {
        public Action Worker { get; private set; }
        protected Timer Timer { get; private set; }

        public AutoTimer(Action worker)
        {
            Worker = worker;
        }

        public void Start(int milseconds)
        {
            ReleaseTimer();
            Timer = new Timer((obj) => Worker());
            Timer.Change(milseconds, Timeout.Infinite);
        }

        //public static AutoTimer Start(Action worker, int milseconds)
        //{
        //    //Task.Delay(milseconds).ContinueWith()
        //}

        private void ReleaseTimer()
        {
            if (Timer != null)
            {
                Timer.Dispose();
                Timer = null;
            }
        }

        protected override void Free(bool disposing)
        {
            if (disposing)
                ReleaseTimer();
        }
    }
}
