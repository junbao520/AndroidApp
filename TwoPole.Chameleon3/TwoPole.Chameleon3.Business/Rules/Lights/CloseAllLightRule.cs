using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using TwoPole.Chameleon3.Infrastructure;

namespace TwoPole.Chameleon3.Business.Rules
{
    /// <summary>
    /// 关闭所有灯光，小灯、远光、近光、雾灯、左转、右转、报警灯
    /// </summary>
    public class CloseAllLightRule : LightRule
    {

        protected override bool HasErrorLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            return propertyNames.Count > 0;
        }
        //TODO:关闭所有灯光的规则修改 只有超时才会判
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
            //估计是超时
            if (!StartDateTime.HasValue)
            {
                StartDateTime = DateTime.Now;
                return RuleExecutionResult.Continue;
            }
            else if (IsTimeout())
            {
                string str = string.Format("{0}-灯光超时：{1}，起始时间：{2:yyyy-MM-dd HH-mm-ss}", Name, LightTimeout, StartDateTime);
                //
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
           // Logger.InfoFormat(changedProperties.ToString());
            var result = CheckLights(changedProperties, newSensor);
            //等待一定的间隔
            Logger.Info("Result" + result.ToString());
            if (result)
            {
                Logger.Info(Name + "判定灯光成功");
                //判定成功之后还需要判断是否关闭所有的灯光
                return RuleExecutionResult.Finish;
            }
            return RuleExecutionResult.Continue;
        }
        protected override bool CheckLights(IList<string> propertyNames, CarSensorInfo sensor)
        {
            if (!sensor.HighBeam &&
                !sensor.LowBeam &&
                !sensor.OutlineLight &&
                !sensor.FogLight &&
                !sensor.CautionLight &&
                !sensor.LeftIndicatorLight &&
                !sensor.RightIndicatorLight)
                return true;

            return false;
        }
    }
}
