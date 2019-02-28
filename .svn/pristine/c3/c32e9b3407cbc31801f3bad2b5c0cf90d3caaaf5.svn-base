using System;
using System.IO;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;

namespace TwoPole.Chameleon3.Business.ExamItems.LuXian
{
    /// <summary>
    /// 4.0 版本绕车一周采用按钮形式
    /// 1，可以配置（无锡新规），20170912
    /// </summary>
    public class PrepareDrivingV4 : ExamItemBase
    {
        #region 检测项目

        /// <summary>
        /// 1，是否启用项目
        /// 2，检测是否绕车一周检查
        /// 3，绕车一周是否超时
        /// </summary>

        #endregion

        #region 私有变量
        protected bool ArrivedTailstock { get; set; }

        protected bool ArrivedHeadstock { get; set; }
        protected bool InitDoorState { get; set; }

        /// <summary>
        /// 逆时针绕车
        /// </summary>
        private string Anticlockwise = string.Empty;

        //座椅
        protected bool _hasSeat = false;
        //内后视镜
        protected bool _hasInnerMirror = false;
        //外后视镜
        protected bool _hasExteriorMirror = false;

        #endregion

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);

            VoiceExamItem = Settings.AroundCarVoiceEnable;
            MaxElapsedTime = TimeSpan.FromSeconds(Settings.AroundCarTimeout);

        }

        protected override void OnDrivingTimeout()
        {
            //超时且没有经过车头车尾
            if (Settings.AroundCarEnable && (!ArrivedHeadstock || !ArrivedTailstock))
            {
                CheckRule(true, DeductionRuleCodes.RC40101);
            }
            base.OnDrivingTimeout();
        }

        protected override bool BeforeExecute(CarSignalInfo signalInfo)
        {
            return this.State == ExamItemState.Progressing && signalInfo.Sensor != null;
        }



        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            InitDoorState = signalInfo.Sensor.Door;
            if (Settings.AroundCarVoiceEnable && Settings.AroundCarEnable)
            {
                //绕车一周语音
                Speaker.PlayAudioAsync("请绕车一周检测车辆外观及安全情况");
            }
          


            return base.InitExamParms(signalInfo);
        }

        /// <summary>
        /// 开门次数
        /// </summary>
        private int _openDoorCount = 0;

        /// <summary>
        /// 允许开门车门计数
        /// </summary>
        private bool _allowCounter = false;

        private string carHead = "绕车二";
        private string carTrail = "绕车一";

        protected string seatVoice = "luxian/dong.wav";
        protected string innerMirrorVoice = "luxian/dong.wav";
        protected string exteriorMirrorVoice = "luxian/dong.wav";

        /// <summary>
        /// 检测传感器
        /// </summary>
        /// <param name="signalInfo"></param>
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (Settings.AroundCarTimeout <= 0)
            {
                StopCore();
                return;
            }



            if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.EngineAndSafeBelt)
            {
                if (signalInfo.Sensor.Engine)
                {
                    if (signalInfo.Sensor.SafetyBelt)
                    {
                        if (Settings.AroundCarEnable && (!ArrivedHeadstock || !ArrivedTailstock))
                        {
                            CheckRule(true, DeductionRuleCodes.RC40101);
                        }
                        StopCore();
                        return;
                    }
                }
            }

            if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.SafeBelt)
            {
                if (signalInfo.Sensor.SafetyBelt)
                {
                    if (Settings.AroundCarEnable && (!ArrivedHeadstock || !ArrivedTailstock))
                    {
                        CheckRule(true, DeductionRuleCodes.RC40101);
                    }
                    StopCore();
                    return;
                }
            }

            if (Settings.PrepareDrivingEndFlag == PrepareDrivingEndFlag.Door)
            {
                if (signalInfo.Sensor.Door == true)
                {
                    _allowCounter = true;
                    InitDoorState = signalInfo.Sensor.Door;
                  

                }
                else if (signalInfo.Sensor.Door == false)
                {
                    if (_allowCounter)
                    {
                        _allowCounter = false;
                        _openDoorCount++;
                    }
                    if (_openDoorCount >= 2)
                    {

                        //关门
                        if (Settings.AroundCarEnable && (!ArrivedHeadstock || !ArrivedTailstock))
                        {
                            CheckRule(true, DeductionRuleCodes.RC40101);

                        }
                        StopCore();
                        return;
                    }
                }

            }
            if (Settings.AroundCarEnable)
            {
                if (Settings.PrepareDriving3TouchVoice)
                {
                    if (!_hasSeat && signalInfo.Sensor.Seats)
                    {
                        _hasSeat = true;
                        //Speaker.PlayAudioAsync(seatVoice, SpeechPriority.Normal);
                        Speaker.SpeakDongVoice();
                    }
                    if (!_hasInnerMirror && signalInfo.Sensor.InnerMirror)
                    {
                        _hasInnerMirror = true;
                        //Speaker.PlayAudioAsync(innerMirrorVoice, SpeechPriority.Normal);
                        Speaker.SpeakDongVoice();
                    }
                    if (!_hasExteriorMirror && signalInfo.Sensor.ExteriorMirror)
                    {
                        _hasExteriorMirror = true;
                        //Speaker.PlayAudioAsync(exteriorMirrorVoice, SpeechPriority.Normal);
                        Speaker.SpeakDongVoice();
                    }
                }

                //还有安全带加发动机的情况

                if (ArrivedHeadstock && ArrivedTailstock)
                {
                    return;
                }




                var arrivedHead = signalInfo.Sensor.ArrivedHeadstock;
                var arrivedTail = signalInfo.Sensor.ArrivedTailstock;

                if (Settings.PrepareDrivingTailStockEnable)
                {
                    if (!ArrivedTailstock && arrivedTail)
                    {
                        if (headCount == HeadCount.One)
                            headCount = HeadCount.Two;
                        Anticlockwise = string.IsNullOrEmpty(Anticlockwise) ? "Tail" : Anticlockwise;
                        ArrivedTailstock = true;
                        //学员正经过车尾
                        Speaker.PlayAudioAsync(carTrail, SpeechPriority.Normal);
                        return;
                    }
                }
                
                if (Settings.PrepareDrivingHeadStockEnable)
                {
                    if (!ArrivedHeadstock && arrivedHead)
                    {
                        if (headCount == HeadCount.One)
                            return;
                        if (headCount == HeadCount.None)
                            headCount = HeadCount.One;
                        Anticlockwise = string.IsNullOrEmpty(Anticlockwise) ? "Head" : Anticlockwise;

                        ArrivedHeadstock = true;

                        //学员正经过车头
                        Speaker.PlayAudioAsync(carHead, SpeechPriority.Normal);
                        return;
                    }
                }
              


            }

            base.ExecuteCore(signalInfo);
        }

        private HeadCount headCount;
        protected override void StopCore()
        {
            //有三模的时候
            if (Settings.PrepareDriving3TouchVoice)
            {
                if (!_hasExteriorMirror || !_hasInnerMirror)
                {
                    CheckRule(true, DeductionRuleCodes.RC40203);
                }
                if (!_hasSeat)
                    CheckRule(true, DeductionRuleCodes.RC40211, DeductionRuleCodes.SRC4021101);
            }
            //不逆时针绕车
            if (Anticlockwise == "Head")
            {
                //暂时使用不按考试员指令来评判
                CheckRule(true, DeductionRuleCodes.RC40101);
            }
            base.StopCore();
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.PrepareDriving; }
        }


    }

    public enum HeadCount
    {
        None,
        One,
        Wait,
        Two,
        Over
    }
}
