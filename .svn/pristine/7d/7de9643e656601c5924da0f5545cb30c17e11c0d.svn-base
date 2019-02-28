using System.Collections.Specialized;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Triggers;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Services;
using System.Collections.Generic;

namespace TwoPole.Chameleon3.Business.ExamItems.LuZhou
{



    /// <summary>
    /// 拉手刹，播报“请下车”，维持开门3秒后，再播报一次请下车，才关车门
    /// </summary>
    public class PullOver : TwoPole.Chameleon3.Business.ExamItems.PullOver
    {

        /// <summary>
        /// 车门开后过是否3秒
        /// </summary>
        private bool isCheckOverOpendoor = false;

        Queue<bool> queueOpenDoor = new Queue<bool>(3);

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            //设置了变道角度后
            //todo：bug
            if (Settings.PulloverAngle > 0.5 && signalInfo.BearingAngle.IsValidAngle() &&
                StartAngle.IsValidAngle() &&
                !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartAngle, Settings.PulloverAngle))
            {
                CheckRightIndicatorLight();
            }

             queueOpenDoor.Enqueue(signalInfo.Sensor.Door);
            if (queueOpenDoor.Count>3)
            {
                queueOpenDoor.Dequeue();
            }
            //有可能错误检测到了开门信号导致直接结束考试流程
            //判断下是否停车肯定是停车状态下
            //判断下最近
            if (Settings.PullOverEndMark == PullOverEndMark.OpenCloseDoorCheck &&
                //停车状态下至少是3个连续信号
                //保证OpenDoor都是开门信号
                //保证基本上是车停的时候
                PullOverStepState < PullOverStep.StopCar&& signalInfo.Sensor.SpeedInKmh<=1&& signalInfo.Sensor.Door&&queueOpenDoor.Where(s=>s==true).Count()>=2)
            {
                //闪过一个信号后判断
                if(!signalInfo.Sensor.Handbrake)
                    CheckRule(true,DeductionRuleCodes.RC40607, DeductionRuleCodes.SRC4060701);

                CheckEndMark(signalInfo);
            }

            if (PullOverStepState == PullOverStep.None)
            {

                if ((signalInfo.CarState == CarState.Stop && Settings.PullOverMark == PullOverMark.CarStop) ||
                    (signalInfo.Sensor.Handbrake && Settings.PullOverMark == PullOverMark.Handbrake))
                {
                    if (signalInfo.Sensor.Handbrake)
                        Speaker.PlayAudioAsync("请下车", SpeechPriority.High);

                    PullOverStepState = PullOverStep.StopCar;
                    StopCarTime = DateTime.Now;
                    Messenger.Send(new EngineRuleMessage(false));
                }
            }
            else if (PullOverStepState == PullOverStep.StopCar)
            {
                //停车转向灯检查
                PullOverStepState = PullOverStep.OpenPullOverLight;

                CheckRightIndicatorLight();
            }
            else if (PullOverStepState == PullOverStep.OpenPullOverLight)
            {
                CheckHandbrake(signalInfo);
            }
            else if (PullOverStepState == PullOverStep.PullHandbrake)
            {
                //判断靠边停车是否结束
                CheckEndMark(signalInfo);
            }
            else if (PullOverStepState == PullOverStep.CheckStop)
            {
                if (!_isCheckedPulloverStop)
                {
                    _isCheckedPulloverStop = true;
                    CheckPullOverStop(signalInfo);
                }
                if (Settings.PullOverEndMark == PullOverEndMark.OpenDoorCheck)
                {
                    StopCore();
                    return;
                }
                //true false
                if (!(Settings.PullOverEndMark == PullOverEndMark.None ||
                    Settings.PullOverEndMark == PullOverEndMark.OpenCloseDoorCheck))
                {
                    StopCore();
                    return;
                }
                if (!OpenDoorTime.HasValue)
                    OpenDoorTime = DateTime.Now;

                //开车门维持3秒
                if (OpenDoorTime.HasValue && (DateTime.Now - OpenDoorTime.Value).TotalSeconds > 3 &&
                    signalInfo.Sensor.Door && isCheckOverOpendoor == false)
                {
                    isCheckOverOpendoor = true;
                    Speaker.PlayAudioAsync("请下车", SpeechPriority.High);
                }

                if (!signalInfo.Sensor.Door && isCheckOverOpendoor)
                {
                    PullOverStepState = PullOverStep.CloseDoor;
                    Messenger.Send(new DoorChangedMessage(signalInfo.Sensor.Door));
                    StopCore();
                    return;
                }


                //在规定的时间内没有关闭车门
                if (CloseDoorTimeOut() && isCheckOverOpendoor)
                {
                    BreakRule(DeductionRuleCodes.RC40605);
                    StopCore();
                }
            }
        }
    }
}
