using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 灯光模拟检测的规则，如果错误打灯，立即失败；
    /// </summary>
    public abstract class LightRule : RuleBase, ILightRule
    {
        public int Id { get; set; } 
        /// <summary>
        /// 开始考试时的语音提示
        /// </summary>
        public string VoiceText { get; set; }
        /// <summary>
        /// 语音文件
        /// </summary>
        ///
        public string VoiceFile { get; set; }

        public   ILog Logger { get; set; }

        public IAdvancedCarSignal AdvancedSignal { get; private set; }
        protected DateTime? StartDateTime { get; set; }
        public bool IsPlayedVoice { get; set; }
        public bool IsFirstRule { get; set; }

        private bool isBrokenRule = false;

        protected LightRule()
        {
            AdvancedSignal = Singleton.GetAdvancedCarSignal;
            Logger = Singleton.GetLogManager;
        }
        public virtual void Reset()
        {
            _hasLightError = false;
            _watingBeginTime = null;
            StartDateTime = null;
            IsPlayedVoice = false;
            IsWaitLightInterval = false;
            isBrokenRule = false;
        }
        protected virtual bool IsTimeout()
        {
            return StartDateTime.HasValue && DateTime.Now > StartDateTime.Value.AddSeconds(LightTimeout);
        }
        public double LightTimeout { get; protected set; }
        public bool   IsPlayDingVoice { get; set; }

        protected IList<string> GetLightChangedProperties(CarSensorInfo oldSensor, CarSensorInfo newSensor)
        {
            List<string> propertyNames = new List<string>();
            if (oldSensor == null)
                return propertyNames;

            //灯光被改变
            if (oldSensor.CautionLight != newSensor.CautionLight && newSensor.CautionLight)
                propertyNames.Add("CautionLight");
            if (oldSensor.LeftIndicatorLight != newSensor.LeftIndicatorLight && newSensor.LeftIndicatorLight)
                propertyNames.Add("LeftIndicatorLight");
            if (oldSensor.RightIndicatorLight != newSensor.RightIndicatorLight && newSensor.RightIndicatorLight)
                propertyNames.Add("RightIndicatorLight");
            if (oldSensor.FogLight != newSensor.FogLight && newSensor.FogLight)
                propertyNames.Add("FogLight");
            if (oldSensor.HighBeam != newSensor.HighBeam && newSensor.HighBeam)
                propertyNames.Add("HighBeam");
            if (oldSensor.LowBeam != newSensor.LowBeam && newSensor.LowBeam)
                propertyNames.Add("LowBeam");
            if (oldSensor.OutlineLight != newSensor.OutlineLight && newSensor.OutlineLight)
                propertyNames.Add("OutlineLight");

            return propertyNames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true:继续等待，false:已播放完毕</returns>
        protected virtual bool WaitingForPlayingVoice()
        {
            if (Speaker.HasVoice)
            {
                return true;
            }
            //todo:是否播放语音
            if (!IsPlayedVoice)
            {
                 var voiceTime = DateTime.Now;
                Logger.InfoFormat("灯光模拟:加入灯光模拟规则:{0}",VoiceFile);
                //需要等到语音播放完成之后再进行计算开始时间
                Speaker.PlayAudioAsync(VoiceFile,SpeechPriority.Normal,() =>
                {
                    //TODO这样有可能死循环 //需要加一个
                    while (Speaker.HasVoice);
                    Thread.Sleep(150);
                    while (Speaker.HasVoice) ;
                    //等待语音播报完成之后才可以进行
                    StartDateTime = DateTime.Now;
                    Logger.InfoFormat("灯光模拟:{0}-语音播报完成:", VoiceFile);
                    IsPlayedVoice = true;
                });
                //写死
                //聊城在每句灯光后，添加“噔”的声音,第一句和最后句不加
                if (Settings.IsPlayDingVoice)
                {
                    if (VoiceFile.Contains("请开启前照灯") || VoiceFile.Contains("关闭所有灯光"))
                    {
                        //这两句不播放叮的一声
                    }
                    else
                    {

                        if (DataBase.Currentversion.DataBaseName.Contains("haikou"))
                        {
                            //使用合成语音“滴”
                            Speaker.PlayAudioAsync("滴", SpeechPriority.Normal);

                        }
                        else
                        { 
                            //播放刹车语音
                            Speaker.SpeakBreakeVoice();
                        }
                    }
                }
               
             

                //客户需要

                return true;
            }

            return false;
        }

        private bool IsWaitLightInterval = false;
        private DateTime? _watingBeginTime;
        protected bool WaitLightInterval()
        {
            if (Speaker.HasVoice)
                return true;

            if (IsWaitLightInterval)
                return false;

            if (_watingBeginTime == null)
                _watingBeginTime = DateTime.Now;

            if (Settings.SimulationLightInterval <= 0)
                return false;

            if ((DateTime.Now - _watingBeginTime.Value).TotalSeconds > Settings.SimulationLightInterval)
            {
                _watingBeginTime = null;
                IsWaitLightInterval = true;
                //
                Logger.InfoFormat("{0}-灯光间隔{1:yyyyMMdd HHmmss}-{2:yyyyMMdd HHmmss}",  Name, _watingBeginTime, DateTime.Now);
                return false;
            }
            
            return true;
        }

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            LightTimeout = Settings.SimulationLightTimeout;
            IsPlayDingVoice = Settings.IsPlayDingVoice;
        }

        public override RuleExecutionResult Check(CarSignalInfo signalInfo)
        {
            if (signalInfo == null)
                return RuleExecutionResult.Continue;

            var oldSignal = CarSignalSet.Skip(1).FirstOrDefault();
            if (oldSignal == null)
                return RuleExecutionResult.Continue;

            //等待播放语音
            if (WaitingForPlayingVoice())
                return RuleExecutionResult.Continue;

            //评定是否超时
            if (!StartDateTime.HasValue)
            {
                StartDateTime = DateTime.Now;
                return RuleExecutionResult.Continue;
            }
            else if (IsTimeout())
            {
                string str = string.Format("{0}-灯光超时：{1}，起始时间：{2:yyyy-MM-dd HH-mm-ss}", Name, LightTimeout, StartDateTime);
                Logger.Info(str);
                if (!TimeoutRuleCheck(signalInfo.Sensor))
                    BreakRule();

                return RuleExecutionResult.Break;
            }

            //如果灯光有错误
            if (_hasLightError)
            {
                //等待一段时间后，返回错误；
                if (WaitLightInterval())
                {
                    return RuleExecutionResult.Continue;
                }
                else
                {
                    return RuleExecutionResult.Break;
                }
            }

            var oldSensor = oldSignal.Sensor;
            var newSensor = signalInfo.Sensor;
            var changedProperties = GetLightChangedProperties(oldSensor, newSensor);
            Logger.Info("changedProperties:", changedProperties.ToJson().ToString());
            if (changedProperties.Count > 0)
            {
                var hasErrorLights = HasErrorLights(changedProperties, newSensor);
                Logger.Info("HasErrorLights:",hasErrorLights.ToString());
                if (hasErrorLights)
                {
                    BreakRule();
                    _hasLightError = true;
                    Logger.InfoFormat("{0}-灯光错误：当前灯光，上一个灯光：{1}", Name, string.Join(", ", changedProperties, newSensor.ToJson(), oldSensor.ToJson()));
                    return RuleExecutionResult.Break;
                }
                else
                {
                    var result = CheckLights(changedProperties, newSensor);
                    //等待一定的间隔
                    Logger.Info("NoErrorLightsResult" + result.ToString());
                    if (result && !WaitLightInterval())
                    {
                        Logger.Info(Name + "判定灯光成功");
                        //判定成功之后还需要判断是否关闭所有的灯光
                        return RuleExecutionResult.Finish;
                    }
                }
            }
            else
            {
               
                var result = CheckLights(changedProperties, newSensor);
                //等待一定的间隔
                Logger.Info("Result"+result.ToString());
                if (result && !WaitLightInterval())
                {
                    Logger.Info(Name+"判定灯光成功");
                    //判定成功之后还需要判断是否关闭所有的灯光
                    return RuleExecutionResult.Finish;
                }
            }

            return RuleExecutionResult.Continue;
        }

        protected bool _hasLightError = false;

        protected abstract bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor);

        protected abstract bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor);

        /// <summary>
        /// 对超时的规则进行验证是否
        /// </summary>
        /// <returns></returns>
        protected virtual bool TimeoutRuleCheck(CarSensorInfo signalInfo)
        {
            return false;
        }

        public override void BreakRule()
        {
            if (!isBrokenRule)
            {
                isBrokenRule = true;
                base.BreakRule();
            }
        }
    }
}
