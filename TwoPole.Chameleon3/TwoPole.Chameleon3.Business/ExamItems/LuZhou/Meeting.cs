using TwoPole.Chameleon3.Infrastructure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Business.ExamItems.LuZhou
{
    public class Meeting : TwoPole.Chameleon3.Business.ExamItems.ExamItemBase
    {
  

        #region 检测规则
        /// <summary>
        /// 在规定的速度内减速慢行
        /// </summary>
        #endregion

        #region 私有变量

        protected DateTime MeetingStartTime { get; private set; }
        private bool IsForbidHighBeamCheck { get; set; }
        protected bool IsAngleCheck { get; set; }

        //是否播报过刹车持续时间规则
        protected bool IsBrake = false;
        protected DateTime? StartBrakeTime { get; set; }

        protected double PrepareDistance { get; set; }

        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.MeetingVoice;
            MaxDistance = Settings.MeetingDrivingDistance;

            PrepareDistance = Settings.MeetingPrepareD;
        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            IsBrake = false;
            if (signalInfo.BearingAngle.IsValidAngle())
            {
                StartAngle = signalInfo.BearingAngle;
                MeetingStartTime = DateTime.Now;
                return true;
            }

            return false;
        }

        //必须先松开刹车，再踩才能生效
        protected bool _hasLoosenBrake = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor.Brake == false)
                _hasLoosenBrake = true;
            if (PrepareDistance > 2)
            {
                if (signalInfo.Distance - StartDistance < PrepareDistance)
                    return;
            }
            //如果必须踩刹车
            if (IsBrake && _hasLoosenBrake)
            {
                CheckBrakeVoice(signalInfo);
            }
            //检查是否有角度变化
            if (!IsAngleCheck &&
                Settings.MeetingAngle > 0 &&
                signalInfo.BearingAngle.IsValidAngle() &&
                !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartAngle, Settings.MeetingAngle))
            {

                IsAngleCheck = true;
            }

            ////禁止远光
            //if (!IsForbidHighBeamCheck && 
            //    Settings.MeetingForbidHighBeamCheck && signalInfo.Sensor.HighBeam)
            //{
            //    IsForbidHighBeamCheck = true;
            //    BreakRule(DeductionRuleCodes.RC41604);
            //}

            //必须先松开刹车，再踩才能生效
            if (_hasLoosenBrake)
            {
                //检测刹车保持时间
                if (Settings.BrakeKeepTime > 0)
                {
                    if (signalInfo.Sensor.Brake)
                    {
                        if (!StartBrakeTime.HasValue)
                        {
                            StartBrakeTime = DateTime.Now;
                        }
                        else
                        {

                            if ((DateTime.Now - StartBrakeTime.Value).TotalSeconds >= Settings.BrakeKeepTime)
                            {
                                IsBrake = true;
                            }
                        }
                    }
                }
                else
                {
                    if (signalInfo.Sensor.Brake)
                    {
                        IsBrake = true;
                    }
                }
            }
            base.ExecuteCore(signalInfo);
        }

        protected override void StopCore()
        {
            //会车速度检查
            if (Settings.MeetingSlowSpeedInKmh > 0 && CarSignalSet.Current.SpeedInKmh > Settings.MeetingSlowSpeedInKmh)
            {
                BreakRule(DeductionRuleCodes.RC41304);
            }
            ////禁止远光
            if (!IsForbidHighBeamCheck && Settings.MeetingForbidHighBeamCheck && CarSignalSet.Current.Sensor.HighBeam)
            {
                IsForbidHighBeamCheck = true;
                BreakRule(DeductionRuleCodes.RC41604);
            }


            //检测会车刹车
            if (Settings.MeetingCheckBrake)
            {
                if (!IsBrake)
                {
                    BreakRule(DeductionRuleCodes.RC41305);
                }
            }

            //检查会车角度        
            if (Settings.MeetingAngle > 0 && !IsAngleCheck)
            {

                BreakRule(DeductionRuleCodes.RC41302);
            }
            //Logger.InfoFormat("会车结束 设定角度：{0} 开始角度：{1} 结束角度：{2}", Settings.MeetingAngle, StartAngle, CarSignalSet.Current.BearingAngle);
            base.StopCore();
        }


        public override string ItemCode
        {
            get { return ExamItemCodes.Meeting; }
        }



    }
}
