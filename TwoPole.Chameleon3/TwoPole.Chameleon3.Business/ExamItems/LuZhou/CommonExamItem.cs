using GalaSoft.MvvmLight.Messaging;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using System;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure.Messages;
using System.Collections.Generic;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.ExamItems.LuZhou
{
    /// <summary>
    /// 1,发动机转速大于3000转/分
    /// 2,按规则使用安全带
    /// 3,车辆偏离正确行驶方向(未完)
    /// 4,因操作不当造成发动机熄火一次
    /// 5,检查行驶中空挡滑行
    /// 7,不使用或错误使用转向灯或转向灯少于3 s(在通过规则平时不判断灯光)
    /// 8,未按指令平稳加、减挡（一档）
    /// 9,未按指令平稳加、减挡（二档）
    /// 10,车辆运行速度和挡位不匹配
    /// 11,发动机转速过高或过低
    /// 12,全程限制速度
    /// 13，全程速度必须达到最低值
    /// 14,起步时是否松驻车制动器
    /// 15，加减档位要求（档位、速度、时间）
    /// 16，转向灯超时检测
    /// 特殊：踩刹车有“瞪”的声音
    /// 空挡传感器，1，2档距离使用档杆中断累计
    /// 特殊要求 要求全程30码 以上必须达到500米，由于是新考场 故特别修改 2016/8/8 鲍君
    /// 重庆涪陵特殊要求  全程评判转向灯超时，不管是不是在项目中   停着不计算时间 2016/8/13
    /// 重庆涪陵 添加全程自主变道评判 两个配置 一个是否启用，一个变道时间。一个变道角度  2016/8/23 鲍君  
    /// TwoPole.Chameleon3.Business.ExamItems.CommonExamItem,TwoPole.Chameleon3.Business
    /// </summary>
    public class CommonExamItem : TwoPole.Chameleon3.Business.ExamItems.CommonExamItem
    {
        protected override void CheckBrakeVoice(CarSignalInfo signalInfo)
        {
            //泸州版本在减速项目里面才报
        }
        private int BrakeIrregularitySignal;
        private int BrakeIrregularitySpeed;
        private int BrakeIrregularitySpeedZero;
        private int BrakeIrregularitySpeedOver;
        private Queue<CarSignalInfo> currentSigSet;
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            BrakeIrregularitySignal = Settings.BrakeIrregularitySignal;
            BrakeIrregularitySpeed = Settings.BrakeIrregularitySpeed;
            BrakeIrregularitySpeedZero = Settings.BrakeIrregularitySpeedZero;
            BrakeIrregularitySpeedOver = Settings.BrakeIrregularitySpeedOver;
            //重新初始化
            Constants.IsSecondTurnRound = false;
            currentSigSet = new Queue<CarSignalInfo>(Settings.BrakeIrregularitySignal);
            return base.InitExamParms(signalInfo);
        }
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            //是否检测制动部平顺
            if (Settings.IsEnableBrakeIrregularity)
            {
                CheckRushing(signalInfo);
            }

            base.ExecuteCore(signalInfo);

        }



        private bool hasBreak = true;


        /// <summary>
        /// 检测制动部平顺
        /// </summary>
        /// <param name="signalInfo"></param>
        private void CheckRushing(CarSignalInfo signalInfo)
        {
            if (signalInfo.SpeedInKmh > 15)
                hasBreak = false;

            currentSigSet.Enqueue(signalInfo);
            if (currentSigSet.Count > Settings.BrakeIrregularitySignal)
                currentSigSet.Dequeue();

            if (!hasBreak && signalInfo.Sensor.Brake && currentSigSet.Where(x => x.SpeedInKmh <=1).Count() >= Settings.BrakeIrregularitySpeedZero)
            {
                if (currentSigSet.Count > 10)
                {
                    //也可以使用陀螺仪来评判，正常A_Y加速度是正的，急减速时会变成负的
                    //非正常......
                    //非正常....
                    //踩轻点.... 速度>10的信号个数多一些
                    //踩重一点.... 速度大于10的信号少一些
                    //3个正常
                    //综合参数
                    var signal = currentSigSet.Where(s => s.SpeedInKmh>Settings.BrakeIrregularitySpeed);
                     
                    //非正常操作 说明速度信号个数没有大于3个
                    //正常操作的化 速度1
                    if (signal.Count() > Settings.BrakeIrregularitySpeedOver)
                    {
                        hasBreak = true;
                        BreakRule(DeductionRuleCodes.RC30221);
                    }
                    var info = string.Join(",", currentSigSet.Select(s => s.SpeedInKmh));
                    Logger.Info(info);


                }
            }

        }

    }
}