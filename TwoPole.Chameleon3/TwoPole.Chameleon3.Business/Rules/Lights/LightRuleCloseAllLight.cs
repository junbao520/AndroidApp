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

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 灯光模拟检测的规则，如果错误打灯，立即失败；
    /// </summary>
    public abstract class LightRuleCloseAllLight: RuleBase, ILightRule
    {
        public int Id { get; set; }
        /// <summary>
        /// 开始考试时的语音提示
        /// </summary>
        public string VoiceText { get; set; }
        /// <summary>
        /// 语音文件
        /// </summary>
        public string VoiceFile { get; set; }

        public IAdvancedCarSignal AdvancedSignal { get; private set; }
        protected DateTime? StartDateTime { get; set; }
        public bool IsPlayedVoice { get; set; }
        public bool IsFirstRule { get; set; }

        private bool isBrokenRule = false;


        private bool IsSuccess = false;
        protected LightRuleCloseAllLight()
        {
            AdvancedSignal = Singleton.GetAdvancedCarSignal;
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
               // Logger.DebugFormat("灯光模拟:加入灯光模拟规则:{0}", VoiceFile);
                //Speaker.PlayAudioAsync(VoiceFile, SpeechPriority.Normal, () =>
                //{
                //    StartDateTime = DateTime.Now;
                //   // Logger.InfoFormat("灯光模拟:{0}-语音播报完成:", VoiceFile);
                //    //IsPlayedVoice = true;
                //});

                IsPlayedVoice = true;
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
                //Logger.DebugFormat("{0}-灯光间隔{1:yyyyMMdd HHmmss}-{2:yyyyMMdd HHmmss}", Name, _watingBeginTime, DateTime.Now);
                return false;
            }

            return true;
        }

        public override void Init(NameValueCollection settings)
        {
            base.Init(settings);
            LightTimeout = Settings.SimulationLightTimeout;
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
                //Logger.DebugFormat("{0}-灯光超时：{1}，起始时间：{2:yyyy-MM-dd HH-mm-ss}", Name, LightTimeout, StartDateTime);
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
            if (changedProperties.Count > 0)
            {
                var hasErrorLights = HasErrorLights(changedProperties, newSensor);
                if (hasErrorLights)
                {
                    BreakRule();
                    _hasLightError = true;
                    //Logger.DebugFormat("{0}-灯光错误：当前灯光，上一个灯光：{1}", Name, string.Join(", ", changedProperties, newSensor.ToJson(), oldSensor.ToJson()));
                    return RuleExecutionResult.Break;
                }
            }
            else
            {
                var result = CheckLights(changedProperties, newSensor);

                if (result)
                {
                    IsSuccess = true;
                }
                //等待一定的间隔
                if (IsSuccess && !WaitLightInterval())
                {

                    //有一个等待 //其实我们可以这样
                    //首先判断是否关闭所有灯光
                    if (!newSensor.HighBeam &&
                        !newSensor.LowBeam &&
                        !newSensor.OutlineLight &&
                        !newSensor.FogLight &&
                        !newSensor.CautionLight &&
                        !newSensor.LeftIndicatorLight &&
                        !newSensor.RightIndicatorLight)
                    {
                        //Logger.DebugFormat("{0}-判定灯光成功", Name);
                        return RuleExecutionResult.Finish;
                    }
                    else
                    {
                        //Logger.DebugFormat("CloseAllLight{0}", newSensor.ToString());
                        return RuleExecutionResult.Continue;
                    }
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
