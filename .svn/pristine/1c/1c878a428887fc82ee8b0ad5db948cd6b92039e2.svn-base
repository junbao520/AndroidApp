using GalaSoft.MvvmLight.Messaging;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation;
using System;
using System.Linq;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Messages;
using System.Collections.Generic;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Business.Areas.GuiZhou.ZunYi.ExamItems
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
    /// 11,发动机转速过高或过低（屏蔽，三亚不进行评判 王涛2015.12.01）  //2016 06 06 客户要求加上检测发动机转速  鲍君
    /// 12,全程限制速度
    /// 13，全程速度必须达到最低值
    /// 14, 起步时是否松驻车制动器
    /// 15，加减档位要求（档位、速度、时间）
    /// 16，转向灯超时检测
    /// 17, 三模以及车头语音
    /// </summary>
    public class CommonExamItem : ExamItemBase
    {
        #region 私有变量

        ///是否已经触犯发动机规则
        private bool IsBrokenSafetyBelt = false;
        private bool IsBrokenTaxiingOnN = false;
        private bool IsBrokenSpeedAndGear = false;
        private bool IsBrokenMaxSpeed = false;
        private bool StopCheckEngine = false;


        /// <summary>
        /// 触发后视镜
        /// </summary>
        private bool TouchInnerMirror = false;

       /// <summary>
       /// 触发坐椅
       /// </summary>
        private bool TouchSeats = false;

       /// <summary>
       /// 绕车一
       /// </summary>
        private bool ArrivedHeadstock = false;

        /// <summary>
        /// 绕车二
        /// </summary>
        private bool ArrivedTailstock = false;

        /// <summary>
        /// 触发观望镜
        /// </summary>
        private bool TouchExteriorMirror = false;

        #endregion

        private DateTime? GearChangeTime { get; set; }

        private bool IsReachedGlobalLowestSpeed = false;

        private bool IsCheckReleaseHandbrake = false;
        private DateTime? StartCheckReleaseHandbrake { get; set; }

        private DateTime? StartCheckGearOnNTime { get; set; }

        /// <summary>
        /// 开始检测全程档位速度
        /// </summary>
        private DateTime? StartCheckGlobalGearContinuous { get; set; }

        private bool IsReachedGlobalGearContinuous = false;

        private bool IsCheckGearOneDistanceAndTime = true;
        private DateTime? GearOneStartTime { get; set; }
        private double? GearOneStartDistance { get; set; }
        private double? TaxiingStartDistance { get; set; }

        private bool IsCheckGearTwoDistanceAndTime = true;
        private DateTime? GearTwoStartTime { get; set; }
        private double? GearTwoStartDistance { get; set; }

        private DateTime? ExamItemEndTime = DateTime.Now;

        //发动机转速过高播报
        private bool IsEngineRpmSpeaked = false;

        private bool _isInExamItem = false;
        private DateTime? ReachMaxSpeedTime { get; set; }
        /// <summary>
        /// 是否在项目里面
        /// </summary>
        private bool IsInExamItem
        {
            get
            {
                if (!_isInExamItem && !ExamItemEndTime.HasValue)
                    ExamItemEndTime = DateTime.Now;
                return _isInExamItem;
            }
            set
            {
                if (_isInExamItem != value)
                {
                    if (!_isInExamItem)
                        ExamItemEndTime = DateTime.Now;
                    else
                        ExamItemEndTime = null;
                    _isInExamItem = value;
                }
            }
        }



        /// <summary>
        /// 有无转向灯超时
        /// </summary>
        private bool HasIndicatorTimeout = false;

        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            Name = "综合评判";
            if (Settings.GlobalLowestSpeed <= 0)
                IsReachedGlobalLowestSpeed = true;

            if (signalInfo.CarState == CarState.Stop)
                IsCheckReleaseHandbrake = true;

            return base.InitExamParms(signalInfo);
        }

        protected override void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<EngineRuleMessage>(this, OnEngineRuleMessage);
            messenger.Register<EngineStopMessage>(this, OnEngineStop);
            messenger.Register<ExamItemStateChangedMessage>(this, OnExamItemStateChanged);

            if (Settings.NeutralStart)
            {
                //界面配置，启用空挡打火
                messenger.Register<EngineChangedMessage>(this, OnEngineChanged);
            }
            if (Settings.TwiceGearNoSuccess > 0)
            {
                //界面配置，启用两次挂档不进
               // messenger.Register<IsNeutralChangedMessage>(this, OnIsNeutralChanged);
            }
            base.RegisterMessages(messenger);
        }

        private void CheckEngineRpm(CarSignalInfo signalInfo)
        {
            if (Settings.MaxEngineRpm > 0 && signalInfo.EngineRpm > Settings.MaxEngineRpm)
            {
                if (IsEngineRpmSpeaked == false)
                {
                    IsEngineRpmSpeaked = true;
                    BreakRule(DeductionRuleCodes.RC30110, DeductionRuleCodes.SRC3011001);
                }

            }
            else
            {
                IsEngineRpmSpeaked = false;
            }

        }

        private void CheckTouchSensor(CarSignalInfo signalInfo)
        {
   
            if (!TouchInnerMirror)
            {
                if (signalInfo.Sensor.InnerMirror)
                {
                    Logger.Info("CheckTouchSensor", "InnerMirror");
                    TouchInnerMirror = true;
                    Speaker.PlayAudioAsync("后视镜确认");
                }
            }

            if (!TouchSeats)
            {
                if (signalInfo.Sensor.Seats)
                {
                    Logger.Info("CheckTouchSensor","Seats");
                    TouchSeats = true;
                    Speaker.PlayAudioAsync("调整座椅确认");
                }
            }

            if (!ArrivedHeadstock)
            {
                if (signalInfo.Sensor.ArrivedHeadstock)
                {
                    Logger.Info("CheckTouchSensor","ArrivedHeadstock");
                    ArrivedHeadstock = true;
                    Speaker.PlayAudioAsync("绕车一");
                }
            }
            if (!ArrivedTailstock && Settings.PrepareDrivingTailStockEnable)
            {
                if (signalInfo.Sensor.ArrivedTailstock)
                {
                    Logger.Info("ArrivedTailstock");
                    ArrivedTailstock = true;
                    Speaker.PlayAudioAsync("绕车二");
                }
            }
            if (!TouchExteriorMirror)
            {
                if (signalInfo.Sensor.ExteriorMirror)
                {
                    Logger.Info("CheckTouchSensor","ExteriorMirror");
                    TouchExteriorMirror = true;
                    Speaker.PlayAudioAsync("观望镜确认");
                }
            }


        }

        #region 消息事件
        private void OnEngineStop(EngineStopMessage message)
        {
            if (!StopCheckEngine)
            {
                Logger.DebugFormat("发动机熄火，规则：{0}", DeductionRuleCodes.RC30210);
                BreakRule(DeductionRuleCodes.RC30210);
            }
        }

        /// <summary>
        /// 发动机启动检查是否空挡
        /// </summary>
        /// <param name="message"></param>
        private void OnEngineChanged(EngineChangedMessage message)
        {
            if (message.NewValue)
            {
                if (!CarSignalSet.Current.Sensor.IsNeutral)
                {
                    //触犯规则：非空挡打火
                    BreakRule(DeductionRuleCodes.RC40204);
                }
            }
        }

        /// <summary>
        /// 两次挂档不进
        /// </summary>
        /// <param name="message"></param>
        //private void OnIsNeutralChanged(IsNeutralChangedMessage message)
        //{
        //    //当前空挡
        //    if (message.NewValue)
        //    {
        //        var historyTime = DateTime.Now.AddSeconds(-Settings.TwiceGearNoSuccess);
        //        IAdvancedCarSignal advancedCarSignal = Resolve<IAdvancedCarSignal>();
        //        var changingGearFailed = advancedCarSignal.CheckDoubleChangingGearFailed(historyTime);
        //        //触犯规则：2次挂档不进扣分
        //        CheckRule(!changingGearFailed, DeductionRuleCodes.RC30111);
        //    }
        //}

        private void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            var examManager = Singleton.GetExamManager;
            IsInExamItem = examManager.ExamItems.Any(x => x.State == ExamItemState.Progressing && x.ItemCode != ExamItemCodes.CommonExamItem);
        }

        private void OnEngineRuleMessage(EngineRuleMessage message)
        {
            StopCheckEngine = !message.Enable;
        }
        #endregion

        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {

            #region 检测全程发动机转速

            CheckEngineRpm(signalInfo);

            #endregion


            #region 检测三模进行播报

            CheckTouchSensor(signalInfo);
            #endregion
            #region 检测转向灯超时
            if (!IsInExamItem)
            {
                if (!HasIndicatorTimeout)
                {
                    HasIndicatorTimeout = !CheckIndicatorTimeout(signalInfo);
                }
                else
                {
                    if (!signalInfo.Sensor.LeftIndicatorLight && !signalInfo.Sensor.RightIndicatorLight)
                    {
                        HasIndicatorTimeout = false;
                    }
                }
            }
            #endregion

            #region 检测安全带

            if (!IsBrokenSafetyBelt)
            {
                if (signalInfo.CarState == CarState.Moving && !signalInfo.Sensor.SafetyBelt)
                {
                    IsBrokenSafetyBelt = true;
                    BreakRule(DeductionRuleCodes.RC30135);
                }
            }
            #endregion

            //改用消息机制
            //#region 检测发动机是否熄火
            //if (!StopCheckEngine && !IsBrokenEngineStop)
            //{
            //    if (signalInfo.CarState == CarState.Moving && !signalInfo.Sensor.Engine)
            //    {
            //        IsBrokenEngineStop = true;
            //        BreakRule(DeductionRuleCodes.RC30210);
            //    }
            //}
            //else
            //{
            //    if (signalInfo.Sensor.Engine)
            //        IsBrokenEngineStop = false;
            //}
            //#endregion

            #region 检测空档滑行

            CheckGearNeutralDistanceAndTime(signalInfo);
            #endregion

            #region 检测速度和档位是否匹配

            CheckGearAndSpeed(signalInfo);

            #endregion

            #region 检测全程限制
            if (!IsBrokenMaxSpeed && Settings.MaxSpeed > 0)
            {
                if (signalInfo.SpeedInKmh > Settings.MaxSpeed && !ReachMaxSpeedTime.HasValue)
                {
                    ReachMaxSpeedTime = DateTime.Now;
                }
                if (signalInfo.SpeedInKmh > Settings.MaxSpeed && ReachMaxSpeedTime.HasValue
                  && (DateTime.Now - ReachMaxSpeedTime).Value.TotalMilliseconds >= Settings.CommonExamItemsMaxSpeedKeepTime)
                {
                    var SignalInfo = CarSignalSet.Query(DateTime.Now.AddSeconds(-3));
                    Logger.ErrorFormat("检测全程限制S:{0},s1:{1}", signalInfo.SpeedInKmh, Settings.MaxSpeed);
                    Logger.ErrorFormat("最近3秒速度值S:{0}", string.Join(",", SignalInfo.Select(S => S.SpeedInKmh).ToArray()));
                    IsBrokenMaxSpeed = true;
                    BreakRule(DeductionRuleCodes.RC40001);
                }
                if (signalInfo.SpeedInKmh <= Settings.MaxSpeed)
                {
                    ReachMaxSpeedTime = null;
                }

            }

            #endregion

            #region 检测是否达到全程最低速度要求

            if (!IsReachedGlobalLowestSpeed && signalInfo.SpeedInKmh > Settings.GlobalLowestSpeed)
                IsReachedGlobalLowestSpeed = true;

            #endregion

            //#region 起步后松手刹

            //TODO:手刹这个移动到起步项目
            //CheckHandbrake(signalInfo);

            //#endregion

            #region 全程档位速度检测

            //当档位和时间不为空时
            CheckGlobalGearAndSpeed(signalInfo);
            #endregion

            #region 低速档位不匹配检测

            CheckGearOneDistanceAndTime(signalInfo);
            CheckGearTwoDistanceAndTime(signalInfo);

            #endregion

            base.ExecuteCore(signalInfo);
        }

        protected bool CheckIndicatorTimeout(CarSignalInfo signalInfo)
        {
            if (Settings.IndicatorLightTimeout <= 0 || !ExamItemEndTime.HasValue)
                return true;

            //项目结束到现在超过设置时间才有效
            if ((DateTime.Now - ExamItemEndTime.Value).TotalSeconds < Settings.IndicatorLightTimeout)
                return true;

            //刚开始的时候数据未超过超时时间
            if (CarSignalSet.Count == 0 ||
                (DateTime.Now - CarSignalSet.Last().RecordTime).TotalSeconds < Settings.IndicatorLightTimeout)
                return true;

            var sensors = CarSignalSet.QueryCachedSeconds(Settings.IndicatorLightTimeout).Where(x => x.IsValid);
            var carSignalInfos = sensors.ToArray();
            if (signalInfo.Sensor.LeftIndicatorLight)
            {
                if (carSignalInfos.All(x => x.Sensor.LeftIndicatorLight))
                {
                    BreakRule(DeductionRuleCodes.RC30215);
                    return false;
                }
            }
            if (signalInfo.Sensor.RightIndicatorLight)
            {
                if (carSignalInfos.All(x => x.Sensor.RightIndicatorLight))
                {
                    BreakRule(DeductionRuleCodes.RC30215);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查全程档位速度、时间
        /// </summary>
        /// <param name="signalInfo"></param>
        private void CheckGlobalGearAndSpeed(CarSignalInfo signalInfo)
        {
            if (IsReachedGlobalGearContinuous)
                return;

            //无档位要求、或者有档位但速度和时间都小于0
            if (Settings.GlobalContinuousGear == Gear.Neutral ||
               (Settings.GlobalContinuousSpeed <= 0 && Settings.GlobalContinuousSeconds <= 0))
            {
                IsReachedGlobalGearContinuous = true;
                return;
            }


            if (signalInfo.Sensor.Gear != Settings.GlobalContinuousGear)
            {
                StartCheckGlobalGearContinuous = null;
                return;

            }
            //速度和时间都必须达到
            if (Settings.GlobalContinuousSpeed > 0 && Settings.GlobalContinuousSeconds > 0)
            {
                if (signalInfo.SpeedInKmh >= Settings.GlobalContinuousSpeed)
                {
                    if (!StartCheckGlobalGearContinuous.HasValue)
                        StartCheckGlobalGearContinuous = DateTime.Now;
                }
                else
                {
                    StartCheckGlobalGearContinuous = null;
                }
            }
            else if (Settings.GlobalContinuousSpeed > 0)
            {
                //速度达到，无时间要求
                if (signalInfo.SpeedInKmh >= Settings.GlobalContinuousSpeed)
                    IsReachedGlobalGearContinuous = true;
            }
            else if (Settings.GlobalContinuousSeconds > 0)
            {
                //时间达到，无速度要求
                if (!StartCheckGlobalGearContinuous.HasValue)
                    StartCheckGlobalGearContinuous = DateTime.Now;
            }

            if (!IsReachedGlobalGearContinuous &&
                StartCheckGlobalGearContinuous.HasValue &&
                (DateTime.Now - StartCheckGlobalGearContinuous.Value).TotalSeconds >= Settings.GlobalContinuousSeconds)
            {
                IsReachedGlobalGearContinuous = true;
            }
        }

        /// <summary>
        /// 只有停车、并且起步松手刹时间大于0，才有效
        /// </summary>
        /// <param name="signalInfo"></param>
        private void CheckHandbrake(CarSignalInfo signalInfo)
        {
            if (Settings.StartReleaseHandbrakeTimeout <= 0)
                return;

            if (signalInfo.CarState == CarState.Stop)
            {
                IsCheckReleaseHandbrake = false;
                StartCheckReleaseHandbrake = null;
                return;
            }

            if (signalInfo.Sensor.Handbrake && !StartCheckReleaseHandbrake.HasValue)
            {
                StartCheckReleaseHandbrake = DateTime.Now;
                return;
            }


            //
            //未在规定时间内完成拉起手刹

            if (!IsCheckReleaseHandbrake && StartCheckReleaseHandbrake.HasValue)
            {
                if (!signalInfo.Sensor.Handbrake)
                {
                    IsCheckReleaseHandbrake = true;
                    StartCheckReleaseHandbrake = null;
                    BreakRule(DeductionRuleCodes.RC40214);
                }

                if ((DateTime.Now - StartCheckReleaseHandbrake.Value).TotalSeconds > Settings.StartReleaseHandbrakeTimeout)
                {
                    IsCheckReleaseHandbrake = true;
                    StartCheckReleaseHandbrake = null;
                    BreakRule(DeductionRuleCodes.RC40205);
                }

            }
        }

        /// <summary>
        /// 检测速度和档位不匹配
        /// </summary>
        /// <param name="signalInfo"></param>
        private void CheckGearAndSpeed(CarSignalInfo signalInfo)
        {
            if (Settings.SpeedAndGearTimeout > 0 && signalInfo.CarState == CarState.Moving)
            {
                if (!GearChangeTime.HasValue)
                    GearChangeTime = DateTime.Now;

                switch (signalInfo.Sensor.Gear)
                {
                    case Gear.One:
                        if (signalInfo.SpeedInKmh <= Settings.GearOneMaxSpeed)
                        {
                            GearChangeTime = DateTime.Now;
                            IsBrokenSpeedAndGear = false;
                        }
                        break;
                    case Gear.Two:
                        if (Settings.GearTwoMinSpeed <= signalInfo.SpeedInKmh &&
                            signalInfo.SpeedInKmh <= Settings.GearTwoMaxSpeed)
                        {
                            GearChangeTime = DateTime.Now;
                            IsBrokenSpeedAndGear = false;
                        }
                        break;
                    case Gear.Three:
                        if (Settings.GearThreeMinSpeed <= signalInfo.SpeedInKmh && signalInfo.SpeedInKmh <= Settings.GearThreeMaxSpeed)
                        {
                            GearChangeTime = DateTime.Now;
                            IsBrokenSpeedAndGear = false;
                        }
                        break;
                    case Gear.Four:
                        if (Settings.GearFourMinSpeed <= signalInfo.SpeedInKmh && signalInfo.SpeedInKmh <= Settings.GearFourMaxSpeed)
                        {
                            GearChangeTime = DateTime.Now;
                            IsBrokenSpeedAndGear = false;
                        }
                        break;
                    case Gear.Five:
                        if (Settings.GearFiveMinSpeed <= signalInfo.SpeedInKmh && signalInfo.SpeedInKmh <= Settings.GearFiveMaxSpeed)
                        {
                            GearChangeTime = DateTime.Now;
                            IsBrokenSpeedAndGear = false;
                        }
                        break;
                    default:
                        {
                            GearChangeTime = DateTime.Now;
                            IsBrokenSpeedAndGear = false;
                        }
                        break;
                }

                if (!IsBrokenSpeedAndGear && IsSpeedAndGearTimeOut())
                {
                    IsBrokenSpeedAndGear = true;
                    BreakRule(DeductionRuleCodes.RC40402);

                    //检测是否造成发动机转速过高或过低
                    if ((Settings.MaxEngineRpm > 0 && signalInfo.EngineRpm > Settings.MaxEngineRpm) ||
                        (Settings.MinEngineRpm > 0 && signalInfo.EngineRpm < Settings.MinEngineRpm))
                    {
                        BreakRule(DeductionRuleCodes.RC30141);
                    }
                }
            }
        }

        /// <summary>
        /// 检查空挡滑行
        /// </summary>
        /// <param name="signalInfo"></param>
        private void CheckGearNeutralDistanceAndTime(CarSignalInfo signalInfo)
        {
            ///如果设置为否则不检测
            if (Settings.NeutralTaxiingTimeout <= 0 && Settings.NeutralTaxiingMaxDistance <= 0)
                return;

            if (IsBrokenTaxiingOnN)
            {
                //如果档位不是空挡,都以空挡传感器为准
                if (!signalInfo.Sensor.IsNeutral || signalInfo.CarState == CarState.Stop)
                {
                    StartCheckGearOnNTime = null;
                    IsBrokenTaxiingOnN = false;
                    TaxiingStartDistance = null;
                }
                return;
            }
            //TODO:修改为有以空挡传感器为准，如果不装空挡传感器
            if (signalInfo.CarState == CarState.Moving && signalInfo.Sensor.IsNeutral)
            {
                if (!StartCheckGearOnNTime.HasValue)
                    StartCheckGearOnNTime = DateTime.Now;

                if (!TaxiingStartDistance.HasValue)
                    TaxiingStartDistance = signalInfo.Distance;

                if (Settings.NeutralTaxiingTimeout > 0 && Settings.NeutralTaxiingMaxDistance > 0)
                {
                    if ((signalInfo.Distance - TaxiingStartDistance.Value) > Settings.NeutralTaxiingMaxDistance &&
                        (DateTime.Now - StartCheckGearOnNTime.Value).TotalSeconds > Settings.NeutralTaxiingTimeout)
                    {
                        IsBrokenTaxiingOnN = true;
                        BreakRule(DeductionRuleCodes.RC30112);
                    }
                }
                else if (Settings.NeutralTaxiingTimeout > 0)
                {
                    if ((DateTime.Now - StartCheckGearOnNTime.Value).TotalSeconds > Settings.NeutralTaxiingTimeout)
                    {
                        IsBrokenTaxiingOnN = true;
                        BreakRule(DeductionRuleCodes.RC30112);
                    }
                }
                else if (Settings.NeutralTaxiingMaxDistance > 0)
                {
                    if ((signalInfo.Distance - TaxiingStartDistance.Value) > Settings.NeutralTaxiingMaxDistance)
                    {
                        IsBrokenTaxiingOnN = true;
                        BreakRule(DeductionRuleCodes.RC30112);
                    }
                }
            }
            else
            {
                StartCheckGearOnNTime = null;
                TaxiingStartDistance = null;
            }
        }

        private void CheckGearOneDistanceAndTime(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor.Gear == Gear.One && signalInfo.CarState == CarState.Moving)
            {
                if (IsCheckGearOneDistanceAndTime)
                    return;

                if (!GearOneStartTime.HasValue)
                    GearOneStartTime = DateTime.Now;

                if (!GearOneStartDistance.HasValue)
                    GearOneStartDistance = signalInfo.Distance;

                if (Settings.GearOneTimeout > 0 && Settings.GearOneDrivingDistance > 0)
                {
                    if ((signalInfo.Distance - GearOneStartDistance) > Settings.GearOneDrivingDistance &&
                        (DateTime.Now - GearOneStartTime.Value).TotalSeconds > Settings.GearOneTimeout)
                    {
                        IsCheckGearOneDistanceAndTime = true;
                        BreakRule(DeductionRuleCodes.RC40406);
                    }
                }
                else if (Settings.GearOneTimeout > 0)
                {
                    if ((DateTime.Now - GearOneStartTime.Value).TotalSeconds > Settings.GearOneTimeout)
                    {
                        IsCheckGearOneDistanceAndTime = true;
                        BreakRule(DeductionRuleCodes.RC40406);
                    }
                }
                else if (Settings.GearOneDrivingDistance > 0)
                {
                    if ((signalInfo.Distance - GearOneStartDistance) > Settings.GearOneDrivingDistance)
                    {
                        IsCheckGearOneDistanceAndTime = true;
                        BreakRule(DeductionRuleCodes.RC40406);
                    }
                }
            }
            else
            {
                IsCheckGearOneDistanceAndTime = false;
                GearOneStartTime = null;
                GearOneStartDistance = null;
            }
        }

        private void CheckGearTwoDistanceAndTime(CarSignalInfo signalInfo)
        {
            if (signalInfo.Sensor.Gear == Gear.Two && signalInfo.CarState == CarState.Moving)
            {
                if (IsCheckGearTwoDistanceAndTime)
                    return;

                if (!GearTwoStartTime.HasValue)
                    GearTwoStartTime = DateTime.Now;

                if (!GearTwoStartDistance.HasValue)
                    GearTwoStartDistance = signalInfo.Distance;

                if (Settings.GearTwoTimeout > 0 && Settings.GearTwoDrivingDistance > 0)
                {
                    if ((signalInfo.Distance - GearTwoStartDistance) > Settings.GearTwoDrivingDistance &&
                        (DateTime.Now - GearTwoStartTime.Value).TotalSeconds > Settings.GearTwoTimeout)
                    {
                        IsCheckGearTwoDistanceAndTime = true;
                        BreakRule(DeductionRuleCodes.RC40408);
                    }
                }
                else if (Settings.GearTwoTimeout > 0)
                {
                    if ((DateTime.Now - GearTwoStartTime.Value).TotalSeconds > Settings.GearTwoTimeout)
                    {
                        IsCheckGearTwoDistanceAndTime = true;
                        BreakRule(DeductionRuleCodes.RC40408);
                    }
                }
                else if (Settings.GearTwoDrivingDistance > 0)
                {
                    if ((signalInfo.Distance - GearTwoStartDistance) > Settings.GearTwoDrivingDistance)
                    {
                        IsCheckGearTwoDistanceAndTime = true;
                        BreakRule(DeductionRuleCodes.RC40408);
                    }
                }
            }
            else
            {
                IsCheckGearTwoDistanceAndTime = false;
                GearTwoStartTime = null;
                GearTwoStartDistance = null;
            }
        }

        private bool IsSpeedAndGearTimeOut()
        {
            if (!GearChangeTime.HasValue)
                return false;

            return (DateTime.Now - GearChangeTime.Value).TotalSeconds > Settings.SpeedAndGearTimeout;
        }


        protected override void StopCore()
        {
            if (!IsReachedGlobalLowestSpeed)
            {
                BreakRule(DeductionRuleCodes.RC40002);
            }

            if (!IsReachedGlobalGearContinuous)
            {
                BreakRule(DeductionRuleCodes.RC40003);
            }
            base.StopCore();
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.CommonExamItem; }
        }
    }
}
