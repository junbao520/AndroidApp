using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Business.ExamItems.LuZhou
{

    /// <summary>
    /// 通过人行横道
    /// </summary>
    public class ThroughPedestrianCrossing : TwoPole.Chameleon3.Business.ExamItems.ThroughPedestrianCrossing
    {

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            base.ExecuteCore(signalInfo);
            //如果必须踩刹车
            if (CheckBrakeRequired && _hasLoosenBrake)
            {
                CheckBrakeVoice(signalInfo);
            }
            
        }

       

    }
}
