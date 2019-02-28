
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Areas.Chongqin.Fengdu.ExamItems
{
    /// <summary>
    /// 超车 1. 是否返回原车道 通过参数配置
    /// 超车 2. 是否播放返回原车道的语音通过参数配置
    /// 超车 3. 涪陵特殊情况两种超车选择其中的一种
    /// 超车 4. 
    /// </summary>
    public class Overtake: ExamItemBase
    {
        #region 私有变量
        /// <summary>
        /// 开始变道的角度
        /// </summary>
        protected double StartChangeLanesAngle { get; set; }

        private bool IsCheckedOvertakeLeftLight { get; set; }

        private bool IsCheckedOvertakeRightLight { get; set; }


        private bool IsLoudSpeakerCheck = false;


        private double StartChangingLanesDistance { get; set; }

        /// <summary>
        /// 变更车道步骤状态
        /// </summary>
        protected OvertakeStep OvertakeStepState = OvertakeStep.None;
        /// <summary>
        /// 变更车道距离
        /// </summary>
        //当车辆开始变道到变道结束距离参数
        protected double ChangingLanesDistance = 30;

        //
        private string ItemVoice1 = "请在当前车道完成超车动作";
        private string ItemVoice2 = "请完成超车动作";

        private bool IsDefaultRule = true;

        #endregion


    
        ///
        /// <summary>
        /// 是否已经完成超车动作
        /// </summary>
        private bool IsSuccess = false;
        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            MaxDistance = Settings.OvertakeMaxDistance;
            MaxElapsedTime = TimeSpan.FromSeconds(Settings.OvertakeTimeout);
            VoiceExamItem = false;
            //返回原车道的播报距离
            ChangingLanesDistance = Settings.ReturnToOriginalLaneDistince;

           //如果数字是偶数,**************************,***************************,*****************//
            if (DateTime.Now.Second%2==0)
            {
                IsDefaultRule = false;
            }
            if (IsDefaultRule)
            {
                Speaker.PlayAudioAsync(ItemVoice2,SpeechPriority.High);
            }
            else
            {
                Speaker.PlayAudioAsync(ItemVoice1, SpeechPriority.High);
            }
           
        }

        private bool IsArrivedLowSpeed = false;

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //记录开始超车时的初始角度
            if (signalInfo.BearingAngle.IsValidAngle())
            {
                StartAngle = signalInfo.BearingAngle;
                StartTime = DateTime.Now;
                StartDistance = signalInfo.Distance;

                return base.InitExamParms(signalInfo);
            }
            else
            {
                return false;
            }
        }

        protected override void OnDrivingOverDistance()
        {
            StopCore();
        }

        protected override void OnDrivingTimeout()
        {
            StopCore();
        }

        private bool isOverPrepareDistance = false;
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
          if (!IsLoudSpeakerCheck && signalInfo.Sensor.Loudspeaker)
                IsLoudSpeakerCheck = true;
            //过滤10m
            if (!isOverPrepareDistance)
            {
                //超车10米后才评判
                if (signalInfo.Distance - StartDistance < Settings.OvertakePrepareDistance)
                {
                    return;
                }
                if (signalInfo.BearingAngle.IsValidAngle())
                {
                    isOverPrepareDistance = true;
                    StartAngle = signalInfo.BearingAngle;
                }
                return;
            }
            //检测是否超速
            if (Settings.OvertakeSpeedLimit > 0 && signalInfo.SpeedInKmh > Settings.OvertakeSpeedLimit)
            {
                CheckRule(true, DeductionRuleCodes.RC30116);
            }

            //超车只要达到最低速度就可以了
            if (Settings.OvertakeLowestSpeed >=0 && signalInfo.SpeedInKmh > Settings.OvertakeLowestSpeed)
            {
                IsArrivedLowSpeed = true;
            }

            //检测开始超车转向角度
            if (OvertakeStepState == OvertakeStep.None)
            {
                if (signalInfo.BearingAngle.IsValidAngle() &&
                   !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartAngle, Settings.OvertakeChangeLanesAngle))
                {
                    //检测是否反向超车，20161107
                    //if (Settings.ReverseOvertakeCheck)
                    //{
                    //    var current = Locator.Current.Resolve<DirectionJudgeClass>();
                    //    var direction = current.JudgeDirectoin(StartAngle, signalInfo.BearingAngle, Settings.OvertakeChangeLanesAngle, Settings.AngleSource);
                    //    if (direction == Direction.Right)
                    //    {
                    //        CheckRule(true, DeductionRuleCodes.RC41409);
                    //    }
                    //}
                    //设置开始变道时的距离
                    OvertakeStepState = OvertakeStep.StartChangeLanesToLeft;
                    StartChangingLanesDistance = signalInfo.Distance;

                    if (Settings.OvertakeLightCheck && !IsCheckedOvertakeLeftLight)
                    {
                        IsCheckedOvertakeLeftLight = true;
                        //超车,左转向灯***********/
                        if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight))
                        {
                            BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
                            Logger.InfoFormat("OverTakeTest Not LeftIndicatorLight");
                        }
                        else
                        {
                            if (!AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime))
                            {
                                BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020604);
                                Logger.InfoFormat("OverTakeTest LeftIndicatorLight Time:{0}", Settings.TurnLightAheadOfTime);
                            }
                        }
                    }
                }
            }
            else if (OvertakeStepState == OvertakeStep.StartChangeLanesToLeft)
            {
                //当开始变道后向前行驶15米则认为变道成功
                Logger.DebugFormat("OvertakeDistince :{0}",signalInfo.Distance - StartChangingLanesDistance);
                if (signalInfo.Distance - StartChangingLanesDistance >= ChangingLanesDistance &&
                    signalInfo.BearingAngle.IsValidAngle())
                {
                    OvertakeStepState = OvertakeStep.EndChangeLanesToLeft;
                    StartChangeLanesAngle = signalInfo.BearingAngle;
                }
            }
            else if (OvertakeStepState == OvertakeStep.EndChangeLanesToLeft)
            {
                //结束考试项目
                IsSuccess = true;
                return;
            }



            base.ExecuteCore(signalInfo);
        }


        protected override void StopCore()
        {
            //全程没有达到最低速度
            if (!IsArrivedLowSpeed)
            {
                CheckRule(true, DeductionRuleCodes.RC30116);
            }
            if (IsDefaultRule)
            {
                if (!IsSuccess)
                {
                    //BreakRule(DeductionRuleCodes.RC30103);
                }
            }
            else
            {  
                //如果不是默认规则就只需要检查转向灯
                if (!CarSignalSet.Query(StartTime).Any(d => d.Sensor.LeftIndicatorLight))
                {
                    BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020504);
                    Logger.InfoFormat("OverTakeTest Not LeftIndicatorLight");
                }
                else
                {
                    if (!AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, StartTime, Settings.TurnLightAheadOfTime))
                    {
                        BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020604);
                        Logger.InfoFormat("OverTakeTest LeftIndicatorLight Time:{0}", Settings.TurnLightAheadOfTime);
                    }
                }
            }

            //检查喇叭
            if (!IsLoudSpeakerCheck)
            {
                if (Settings.OvertakeLoudSpeakerNightCheck && Context.ExamTimeMode == ExamTimeMode.Night)
                {
                    BreakRule(DeductionRuleCodes.RC30212);
                }
                if (Settings.OvertakeLoudSpeakerDayCheck && Context.ExamTimeMode == ExamTimeMode.Day)
                {
                    BreakRule(DeductionRuleCodes.RC30212);
                }
            }

            //夜考双闪
            if (Settings.OvertakeLowAndHighBeamCheck && Context.ExamTimeMode == ExamTimeMode.Night)
            {
                if (!AdvancedSignal.CheckHighBeam(StartTime))
                {
                    BreakRule(DeductionRuleCodes.RC41603);
                }
            }
            Logger.InfoFormat("超车结束  当前流程状态:{0} 设定角度：{1} 开始角度：{2} 结束角度：{3}", OvertakeStepState.ToString(), Settings.OvertakeChangeLanesAngle, StartChangeLanesAngle, CarSignalSet.Current.BearingAngle);

            //Speaker.PlayAudioAsync(EndVoiceFile, Infrastructure.Speech.SpeechPriority.Highest);
            base.StopCore();
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.Overtaking; }
        }
    }
}
