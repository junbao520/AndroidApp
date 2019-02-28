using System.Collections.Specialized;
using System.Threading;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;

namespace TwoPole.Chameleon3.Business.ExamItems.LuZhou
{
    /// <summary>
    /// 起步必须播报后才能开转向灯，不能提前开
    /// </summary>
    public class VehicleStarting : TwoPole.Chameleon3.Business.ExamItems.VehicleStarting
    {

        //检测项目前不能开转向灯
        private bool checkLeft = true;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (Constants.VehicleStartingDistance==0)
            {
                Constants.VehicleStartingDistance = signalInfo.Distance;
                //Logger.Debug("起步距离进行初始化"+ Constants.VehicleStartingDistance.ToString());
            }
            //检测项目前不能提前开转向灯
            if (checkLeft)
            {
                checkLeft = false;
                if (signalInfo.Sensor.LeftIndicatorLight)
                {
                    CheckRule(true, DeductionRuleCodes.RC40212);
                }

            }
            base.ExecuteCore(signalInfo);

            if (signalInfo.Distance - StartDistance > Settings.StartStopCheckForwardDistance|| signalInfo.Distance - Constants.VehicleStartingDistance>Settings.StartStopCheckForwardDistance)
            {
                StopCore();
            }
        }


        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            Constants.VehicleStartingDistance = 0;
            Logger.Debug("起步距离初始化0");
        }
        protected override void OnDrivingTimeout()
        {
            Logger.Debug("起步超时");
            StopCore();
        }

        protected override void OnDrivingOverDistance()
        {
            Logger.Debug("起步超距");
            base.OnDrivingOverDistance();
        }


    }
}
