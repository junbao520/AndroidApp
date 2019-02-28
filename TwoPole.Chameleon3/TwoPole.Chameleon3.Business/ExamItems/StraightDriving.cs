using System.Collections.Specialized;
using System.Linq;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using System;


namespace TwoPole.Chameleon3.Business.ExamItems
{
    /// <summary>
    /// 直线行驶
    /// </summary>
    public class StraightDriving : ExamItemBase
    {
        #region 项目检测规则
        /// <summary>
        /// 1，检测直线行驶距离
        /// 2，检测直线行驶最大偏移角度
        /// 3，检测直线行驶速度要求
        /// 4, 直线准备距离
        /// 5, 停车不检测角度（王涛 2015-12-08）
        /// </summary>
        #endregion

        #region 私有变量

        ///开始直线行驶时记录转向角
        private double StraightDrivingStartOffsetAngle { get; set; }
        private DateTime? StraightDrivingStartTime { get; set; }

        private double StraightDrivingStartDistance { get; set; }

        /// <summary>
        /// 是否触犯规则
        /// </summary>
        private bool IsBroken_RC40301 = false;
        /// <summary>
        /// 低于最低速度
        /// </summary>
        private bool IsUnderSpeedMinLimit = false;
        /// <summary>
        /// 高于最高速度
        /// </summary>
        private bool IsAboveSpeedMaxLimit = false;

        private bool IsReachSpeed = false;

        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            StraightDrivingStartOffsetAngle = double.NaN;
            VoiceExamItem = Settings.StraightDrivingVoice;
            MaxDistance = Settings.StraightDrivingDistance + Settings.StraightDrivingPrepareDistance;
        }

        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            StraightDrivingStartDistance = signalInfo.Distance;
            if (signalInfo.BearingAngle.IsValidAngle())
            {
                StraightDrivingStartOffsetAngle = signalInfo.BearingAngle;
                return base.InitExamParms(signalInfo);
            }
            return false;
        }

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            //在准备距离不检测
            if (signalInfo.Distance - StraightDrivingStartDistance < Settings.StraightDrivingPrepareDistance)
                return;

            ///停车不检测角度
            if (signalInfo.CarState == CarState.Stop)
                return;
            //判断是否超过速度限制，20160406
            if (Settings.StraightDrivingSpeedMinLimit > 0 && signalInfo.SpeedInKmh < Settings.StraightDrivingSpeedMinLimit)
            {
                Logger.Error("blow DrivingSpeedMinLimit");
                IsUnderSpeedMinLimit = true;
            }

            if (Settings.StraightDrivingReachSpeed> 0 && signalInfo.SpeedInKmh > Settings.StraightDrivingReachSpeed)
            {

                IsReachSpeed = true;
            }
            //
            if (Settings.StraightDrivingSpeedMaxLimit > 0 &&
                signalInfo.SpeedInKmh > Settings.StraightDrivingSpeedMaxLimit)
            {
                IsAboveSpeedMaxLimit = true;
            }
            //
            //直线行驶开始时间，开始的时候记录时间
            if (!StraightDrivingStartTime.HasValue)
            {
           
                if (signalInfo.BearingAngle.IsValidAngle())
                {
                    StraightDrivingStartOffsetAngle = signalInfo.BearingAngle;
                    StraightDrivingStartTime = DateTime.Now;
                }
            }
            //如果角度无效返回不检测
            if (!signalInfo.BearingAngle.IsValidAngle() || !StraightDrivingStartOffsetAngle.IsValidAngle())
                return;

            if (!IsBroken_RC40301&&Settings.StraightDrivingMaxOffsetAngle > 0 && 
               !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StraightDrivingStartOffsetAngle,Settings.StraightDrivingMaxOffsetAngle))
            {
                IsBroken_RC40301 = true;
                //当偏移角度过大时触发规则
                BreakRule(DeductionRuleCodes.RC40301);
                return;
            }

            base.ExecuteCore(signalInfo);
        }

        protected override void StopCore()
        {
            //当直线行驶时速度不在规则范围内时
            var isOverMax = CheckMaxSpeedLimit();
            var isBelowMin = CheckMiniSpeedLimit();

            var isReach = CheckReachSpeedLimit();
            if (!isOverMax ||!isBelowMin||!isReach)
            {
                BreakRule(DeductionRuleCodes.RC30116);
            }
            //Logger.InfoFormat("直线行驶结束  设定角度：{0} 开始角度：{1} 结束角度：{2}", Settings.StraightDrivingMaxOffsetAngle,StraightDrivingStartOffsetAngle, CarSignalSet.Current.BearingAngle);
            base.StopCore();
        }

        protected bool CheckMiniSpeedLimit()
        {
            //if (Settings.StraightDrivingSpeedMinLimit > 0 && StraightDrivingStartTime.HasValue)
            //{
            //    return CarSignalSet.Query(StraightDrivingStartTime.Value).All(x => x.SpeedInKmh >= Settings.StraightDrivingSpeedMinLimit);
            //}
            if (IsUnderSpeedMinLimit&& Settings.StraightDrivingSpeedMinLimit > 0)
                return false;
            return true;
        }
        protected bool CheckReachSpeedLimit()
        {
            //if (Settings.StraightDrivingSpeedMinLimit > 0 && StraightDrivingStartTime.HasValue)
            //{
            //    return CarSignalSet.Query(StraightDrivingStartTime.Value).All(x => x.SpeedInKmh >= Settings.StraightDrivingSpeedMinLimit);
            //}
            if (!IsReachSpeed && Settings.StraightDrivingReachSpeed > 0)
                return false;
            return true;
        }

        protected bool CheckMaxSpeedLimit()
        {
            //if (Settings.StraightDrivingSpeedMaxLimit > 0 && StraightDrivingStartTime.HasValue)
            //{
            //    return CarSignalSet.Query(StraightDrivingStartTime.Value).All(x => x.SpeedInKmh <= Settings.StraightDrivingSpeedMaxLimit);
            //}
            if (IsAboveSpeedMaxLimit&&Settings.StraightDrivingSpeedMaxLimit>0)
                return false;
            return true;
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.StraightDriving; }
        }
    }
}
