using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Areas.Chongqin.Fuling.ExamItems
{
    /// <summary>
    /// 1，是否进行了加减档检测
    /// 2，加档档时间是否在合理范围内
    /// 3. 涪陵考场新规则:4-3-2-3  或者 2-1-2-3 或者 3-2-3  //三种都是可以的
    /// 4. 
    /// </summary>
    public class ModifiedGear : ExamItemBase
    {
        public const Gear LowestGear = Gear.Two;

        public ModifiedGearStep ModifiedGearStepState = ModifiedGearStep.None;

        #region 私有变量

        protected double ModifiedGearStartDrivingDistance { get; set; }

        protected List<Tuple<Gear, int>> ModifiedGears { get; set; }

        //protected IAdvancedCarSignal AdvancedCarSignal { get; private set; }


        /// <summary>
        /// 语音文件
        /// </summary>
        private string jjdw23 = "jjd_a3.wav";//请加到3档
        private string jjdw32 = "jjd_d2.wav";//请减到2档
        private string jjdw54 = "jjd_d4.wav"; //请减到4档
        private string jjdw43 = "jjd_d3.wav"; //请减到3档


    


        /// <summary>
        /// 是否验证通过
        /// </summary>
        protected bool IsSuccess { get; set; }

        #endregion

        #region 参数配置
        /// <summary>
        /// 档位最低维持时间
        /// </summary>
        protected double ModifiedGearIgnoreMilliseconds { get; set; }

        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            VoiceExamItem = Settings.ModifiedGearVoice;
            if (Settings.ModifiedGearDrivingDistance > 0)
            {
                MaxDistance = Settings.ModifiedGearDrivingDistance;
            }
            ModifiedGearIgnoreMilliseconds = Settings.ModifiedGearIgnoreSeconds * 1000;
            if (Settings.ModifiedGearTimeout > 0)
            {
                MaxElapsedTime = TimeSpan.FromSeconds(Settings.ModifiedGearTimeout);
            }
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //时间和距离都没设置，直接结束
            if (Settings.ModifiedGearDrivingDistance <= 0 && Settings.ModifiedGearTimeout <= 0)
            {
                base.StopCore();
            }

            return base.InitExamParms(signalInfo);
        }

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            var query = from a in AdvancedSignal.GetGearChangedStates(StartTime)
                where a.Gear != Gear.Neutral &&
                      a.PeriodMilliseconds >= ModifiedGearIgnoreMilliseconds
                group a by a.Gear
                into g
                select new
                {
                    Gear = g.Key,
                    Milliseconds = g.Max(x => x.PeriodMilliseconds),
                    Count = g.Count()
                };

            var gearStates = query.ToArray();
            if (gearStates.Length < 1)
                return;

            var tempGear = gearStates.FirstOrDefault().Gear;

            Logger.Debug("Gear" + tempGear.ToString());
            if (ModifiedGearStepState == ModifiedGearStep.Finshed)
            {
                return;
            }

            //涪陵考场新规则: 4 - 3 - 2 - 3  或者 2 - 1 - 2 - 3 或者 3 - 2 - 3 ，或者1-2-1 //三种都是可以的
            if (ModifiedGearStepState == ModifiedGearStep.None)
            {

                if (tempGear == Gear.Four)
                {
                    ModifiedGearStepState = ModifiedGearStep.GearFlowFirst43;

                    Speaker.PlayAudioAsync("请减到3档",SpeechPriority.Normal);
                }
                else if (tempGear == Gear.Three)
                {
                    ModifiedGearStepState = ModifiedGearStep.GearFlowThrid32;

                    Speaker.PlayAudioAsync("请减到2档", SpeechPriority.Normal);
                }
                else if (tempGear == Gear.Two)
                {
                    ModifiedGearStepState = ModifiedGearStep.GearFlowSecond21;

                    Speaker.PlayAudioAsync("请减到1档",SpeechPriority.Normal);
                }
                else if (tempGear == Gear.One)
                {
                    ModifiedGearStepState = ModifiedGearStep.GearFlowFuorth12;

                    Speaker.PlayAudioAsync("请加到2档", SpeechPriority.Normal);
                }
            }
                #region 第一种流程4-3-2-3

            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowFirst43)
            {
                if (tempGear == Gear.Three)
                {
                    ModifiedGearStepState = ModifiedGearStep.GearFlowFirst32;

                    Speaker.PlayAudioAsync("请减到2档", SpeechPriority.Normal);
                }
            }
            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowFirst32)
            {
                if (tempGear == Gear.Two)
                {
                    ModifiedGearStepState = ModifiedGearStep.GearFlowFirst23;

                    Speaker.PlayAudioAsync("请加到3档", SpeechPriority.Normal);
                }
            }
            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowFirst23)
            {
                if (tempGear == Gear.Three)
                {
                   ModifiedGearStepState = ModifiedGearStep.Finshed;
                }
            }
            #endregion

            #region 第二种流程 2-1-2-3 
            //客户特殊要求  2-1-2就可以了，不需要加到3档
            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowSecond21)
            {
                if (tempGear == Gear.One)
                {
                    Speaker.PlayAudioAsync("请加到2档", SpeechPriority.Normal);
                    ModifiedGearStepState = ModifiedGearStep.GearFlowSecond12;
                }
            }
            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowSecond12)
            {
                if (tempGear == Gear.Two)
                {
                  //  Speaker.PlayAudioAsync("请加到3档", SpeechPriority.Normal);
                  
                    ModifiedGearStepState = ModifiedGearStep.Finshed;
                   // ModifiedGearStepState = ModifiedGearStep.GearFlowSecond23;
                }
            }
            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowSecond23)
            {
                if (tempGear == Gear.Three)
                {
                    // PlayAudioAsync("AddToGear2.wav", SpeechPriority.Normal);
                    ModifiedGearStepState = ModifiedGearStep.Finshed;
                }
            }
                #endregion

                #region 第三种流程3-2-3

            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowThrid32)
            {
                if (tempGear == Gear.Two)
                {
                    Speaker.PlayAudioAsync("请加到3档", SpeechPriority.Normal);
                    ModifiedGearStepState = ModifiedGearStep.GearFlowThrid23;
                }
            }
            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowThrid23)
            {
                if (tempGear == Gear.Three)
                {
                    ModifiedGearStepState = ModifiedGearStep.Finshed;
                }
            }

            #endregion

            #region  第四流程 1-2-1

            if (ModifiedGearStepState == ModifiedGearStep.GearFlowFuorth12)
            {
                if (tempGear == Gear.Two)
                {
                    ModifiedGearStepState = ModifiedGearStep.GearFlowFuorth21;
                    Speaker.PlayAudioAsync("请减到1档", SpeechPriority.Normal);
                }
            }
            else if (ModifiedGearStepState == ModifiedGearStep.GearFlowFuorth21)
            {
                if (tempGear == Gear.One)
                {
                    ModifiedGearStepState = ModifiedGearStep.Finshed;
                }

            }


            #endregion

            if (ModifiedGearStepState == ModifiedGearStep.Finshed)
            {
                IsSuccess = true;
            }


            base.ExecuteCore(signalInfo);
        }

        protected double DelayTime = 2;
        protected bool IsFirst = true;
        /// <summary>
        /// 延时2秒播报语音
        /// </summary>
        /// <param name="voiceFile"></param>
        /// <param name="priority"></param>
        protected void PlayAudioAsync(string voiceFile, SpeechPriority priority)
        {
            Task.Run(() =>
            {
                if (IsFirst)
                {
                    //第一次报档位多延迟3秒，避免和项目语音连起来
                    IsFirst = false;
                    Thread.Sleep(3000);
                }
                int delay = Convert.ToInt32(DelayTime*1000);
                Thread.Sleep(delay);
                Speaker.PlayAudioAsync(voiceFile, priority);
            });
           
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.ModifiedGear; }
        }
        protected override void StopCore()
        {
            if (!IsSuccess)
                BreakRule(DeductionRuleCodes.RC30140);

            base.StopCore();
        }

        public enum ModifiedGearStepFirst
        {
            None,
            Gear43,
            Gear32,
            Gear23
        }
        public enum ModifiedGearStepTwo
        {
            None,
            Gear21,
            Gear12
        }
        public enum ModifiedGearStepThree
        {
            None,
            Gear32,
            Gear23
        }
        public enum ModifiedGearStep
        {
            None,
            GearFlowFirst43,
            GearFlowFirst32,
            GearFlowFirst23,
            GearFlowSecond21,
            GearFlowSecond12,
            GearFlowSecond23,
            GearFlowThrid32,
            GearFlowThrid23,
            GearFlowFuorth12,
            GearFlowFuorth21,

            Finshed

        }

    }
}
