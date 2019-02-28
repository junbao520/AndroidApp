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

namespace TwoPole.Chameleon3.Business.ExamItems
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
    public class CommonExamItem : ExamItemBase
    {
        #region 私有变量

        ///是否已经触犯发动机规则
        private bool IsBrokenSafetyBelt = false;
        private bool IsBrokenTaxiingOnN = false;
        private bool IsBrokenSpeedAndGear = false;
        private bool IsBrokenMaxSpeed = false;
        private bool StopCheckEngine = false;

        #endregion
        private DateTime? GearChangeTime { get; set; }

        private bool IsCheckReleaseHandbrake = false;
        private DateTime? StartCheckReleaseHandbrake { get; set; }

        private DateTime? StartCheckGearOnNTime { get; set; }

        /// <summary>
        /// 开始检测全程档位速度
        /// </summary>
        private DateTime? StartCheckGlobalGearContinuous { get; set; }

        protected bool IsReachedGlobalGearContinuous = false;

        private bool IsCheckGearOneDistanceAndTime = false;
        private DateTime? GearOneStartTime { get; set; }
        private double? GearOneStartDistance { get; set; }
        private double? TaxiingStartDistance { get; set; }

        private bool IsCheckGearTwoDistanceAndTime = false;
        private DateTime? GearTwoStartTime { get; set; }
        private double? GearTwoStartDistance { get; set; }

        private DateTime? ExamItemEndTime = DateTime.Now;

        //发动机转速过高播报
        private bool IsEngineRpmSpeaked = false;

        private bool _isInExamItem = false;

        /// <summary>
        /// 当前速度超过 全程要求速度
        /// </summary>
        private bool NowSignalOverLowestSpeed = false;
        /// <summary>
        /// 当前速度没有超过 全程要求速度
        /// </summary>
        private bool NowSignalNotOverLowsetSpeed = true;

        private double SignalOverLowestSpeedTempBeginDistince = 0;

        private DateTime SignalOverLowestSpeedTempBeginTime = DateTime.Now;

        protected double OverLowestSpeedDistince = 0;
        protected double OverLowestSpeedTime = 0;
        protected double IndicatorLightInitDistance = 0;

        private DateTime? FirstSafeBeltTime { get; set; }

        private DateTime? FirstEngineTime { get; set; }

        private bool IsBrokenEngineStop = false;



        private bool tempExamitemSkip = false;
        //本次结束的项目进行变道规则的延时,掉头，变道
        private bool tempExamItem = false;
        //掉头变更车道结束时计时
        private DateTime? finishTime { get; set; }

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

        private bool IsInSlowExam { get; set; }
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

            if(Settings.LicenseC1)
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
                messenger.Register<IsNeutralChangedMessage>(this, OnIsNeutralChanged);
            }
            base.RegisterMessages(messenger);
        }
        private bool _isVehicleStarting = false;
        private DateTime? _rpmTime { get; set; }
        PriorityQueue<int> _maxEngineRpm = new PriorityQueue<int>();
        /// <summary>
        /// 转速过高（连续3个信号才评判，由于偶尔误判）
        /// </summary>
        /// <param name="signalInfo"></param>
        /// 
        private void CheckEngineRpm(CarSignalInfo signalInfo)
        {
            //起步项目时，不进行综合评判的转速检测
            if (_isVehicleStarting)
                return;
            if (Settings.MaxRpmTime.Equals(0))
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
            else if (Settings.MaxRpmTime > 0 && Settings.MaxEngineRpm > 0)
            {
                if (!_rpmTime.HasValue && signalInfo.EngineRpm >= Settings.MaxEngineRpm)
                    _rpmTime = DateTime.Now;
                if (signalInfo.EngineRpm < Settings.MaxEngineRpm)
                {
                    _rpmTime = null;
                    IsEngineRpmSpeaked = false;
                }

                if (_rpmTime.HasValue && signalInfo.EngineRpm >= Settings.MaxEngineRpm && IsEngineRpmSpeaked == false &&
                    (DateTime.Now - _rpmTime.Value).TotalSeconds > Settings.MaxRpmTime)
                {
                    IsEngineRpmSpeaked = true;
                    BreakRule(DeductionRuleCodes.RC30110, DeductionRuleCodes.SRC3011001);
                }
            }

        }

        #region 消息事件 
        /// <summary>
        /// 这个事件已经没有注册消息
        /// </summary>
        /// <param name="message"></param>
        private void OnEngineStop(EngineStopMessage message)
        {
            if (!StopCheckEngine)
            {
            
                BreakRule(DeductionRuleCodes.RC30210);
              //  Logger.Error("OnEngineStop");
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
                    //触犯规则:非空挡打火 
                    //软件软处理!//
                    BreakRule(DeductionRuleCodes.RC40204,null,100);
                    //if (Settings.CommonExamItemsCheckEngineStall)
                    //{
                    //    if (LastEngineTime.HasValue)
                    //    {
                    //        BreakRule(DeductionRuleCodes.RC40204);
                    //    }
                    //}
                    //else
                    //{
                    //    BreakRule(DeductionRuleCodes.RC40204);
                    //}

                }
            }
        }

        /// <summary>
        /// 两次挂档不进
        /// </summary>
        /// <param name="message"></param>
        private void OnIsNeutralChanged(IsNeutralChangedMessage message)
        {
            //当前空挡
            if (message.NewValue)
            {

                var historyTime = DateTime.Now.AddSeconds(-Settings.TwiceGearNoSuccess);
                IAdvancedCarSignal advancedCarSignal = Singleton.GetAdvancedCarSignal;
                var changingGearFailed = advancedCarSignal.CheckDoubleChangingGearFailed(historyTime);
                //触犯规则：2次挂档不进扣分
                //触犯规则
                CheckRule(!changingGearFailed, DeductionRuleCodes.RC30111);
            }
        }

        //是否在靠边停车项目
        private bool _isPullover = false;

        private void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            var examManager = Singleton.GetExamManager;
           
            IsInExamItem = examManager.ExamItems.Any(x => x.State == ExamItemState.Progressing && x.ItemCode != ExamItemCodes.CommonExamItem);
            _isVehicleStarting = examManager.ExamItems.Any(x => x.State == ExamItemState.Progressing && x.ItemCode == ExamItemCodes.Start);
            _isPullover = examManager.ExamItems.Any(x => x.State == ExamItemState.Progressing && x.ItemCode == ExamItemCodes.PullOver);
            //过滤掉变更车道和会车,超车，掉头，左转，右转,起步，靠边停车，直线行驶，20150712
            tempExamitemSkip = examManager.ExamItems.Any(x => x.State == ExamItemState.Progressing &&
                (x.ItemCode == ExamItemCodes.ChangeLanes || x.ItemCode == ExamItemCodes.Meeting ||
                x.ItemCode == ExamItemCodes.Overtaking || x.ItemCode == ExamItemCodes.TurnRound ||
                x.ItemCode == ExamItemCodes.TurnLeft || x.ItemCode == ExamItemCodes.TurnRight ||
                 x.ItemCode == ExamItemCodes.Start || x.ItemCode == ExamItemCodes.PullOver ||
                 x.ItemCode == ExamItemCodes.StraightDriving));
            //变道掉头,超车左右转结束后延迟10S
            if (!tempExamItem)
            {
                if (examManager.ExamItems.Any((x => x.State == ExamItemState.Progressing &&
                                                    (x.ItemCode == ExamItemCodes.ChangeLanes ||
                                                     x.ItemCode == ExamItemCodes.TurnLeft ||
                                                     x.ItemCode == ExamItemCodes.TurnRight ||
                                                     x.ItemCode == ExamItemCodes.TurnRound ||
                                                     x.ItemCode == ExamItemCodes.Overtaking))))
                {
                    tempExamItem = true;
                }
            }

            IsInExamItem = examManager.ExamItems.Any(x => x.State == ExamItemState.Progressing && x.ItemCode != ExamItemCodes.CommonExamItem);
            //变道，掉头结束时开始计时，用于延时
            if (!finishTime.HasValue && tempExamItem && !IsInExamItem)
            {
                finishTime = DateTime.Now;
            }
            if (IsInExamItem)
            {
                List<string> lstSlowExam = new List<string> { ExamItemCodes.BusArea, ExamItemCodes.Meeting, ExamItemCodes.StraightDriving };
                examManager.ExamItems.Any(x => x.State == ExamItemState.Progressing && x.ItemCode != ExamItemCodes.CommonExamItem);
            }
        }
        private double StartChangeLanesAngle { get; set; }
        //记录信号个数
        private int RecordSignalCount = 0;

        private DateTime? RecordSignalTime = null;
        //播报后延迟时间
        private DateTime delayDateTime { get; set; }
        //变道扣分播报
        private bool IsChangeLineSpeak = false;

        //上一次发动机熄火时间
        private DateTime? LastEngineTime { get; set; }


        protected void ChangeLineAngleCheck(CarSignalInfo signalInfo)
        {
            //由于停车和起步的时候会报触发规则，所以过滤
            //全程变道最低速度

            if (signalInfo.SpeedInKmh < 10)
                return;
            //过滤掉变更车道和会车,超车，掉头，左转，右转,起步，靠边停车
            if (tempExamitemSkip)
                return;
            //因为掉头，变道,角度达到就结束了，太快，所以延迟15s
            if (tempExamItem && finishTime.HasValue)
            {
                if ((DateTime.Now - finishTime.Value).TotalSeconds > 15)
                {
                    finishTime = null;
                    tempExamItem = false;
                    StartChangeLanesAngle = signalInfo.BearingAngle;
                }
                else
                {
                    return;
                }

            }
            //播报后进行延时
            if (IsChangeLineSpeak)
            {
                if ((DateTime.Now - delayDateTime).TotalSeconds > 12)
                {
                    IsChangeLineSpeak = false;
                    //重置角度
                    RecordSignalCount = 0;
                    RecordSignalTime = DateTime.Now;
                    StartChangeLanesAngle = signalInfo.BearingAngle;
                }
                else
                {
                    //重置角度
                    RecordSignalCount = 0;
                    RecordSignalTime = DateTime.Now;
                    StartChangeLanesAngle = signalInfo.BearingAngle;
                    return;
                }
            }
            RecordSignalCount++;
            if (!RecordSignalTime.HasValue)
            {
                RecordSignalTime = DateTime.Now;
            }
            //过滤掉大于30度的跳动
            if (GeoHelper.GetDiffAngle(StartChangeLanesAngle, signalInfo.BearingAngle) > 30)
            {
                StartChangeLanesAngle = signalInfo.BearingAngle;
                return;
            }
            //判断车辆是否在进行变道
            if ((DateTime.Now - RecordSignalTime.Value).TotalSeconds >= Settings.CommonExamItemsChangeLanesTimeOut && IsChangeLineSpeak == false && signalInfo.BearingAngle.IsValidAngle() &&
                StartChangeLanesAngle.IsValidAngle() &&
                !GeoHelper.IsBetweenDiffAngle(signalInfo.BearingAngle, StartChangeLanesAngle, Settings.CommonExamItemsChangeLanesAngle))
            {
                //if (!signalInfo.Sensor.LeftIndicatorLight || !signalInfo.Sensor.RightIndicatorLight)
                if (!CarSignalSet.Query(DateTime.Now - TimeSpan.FromSeconds(12)).Any(d => d.Sensor.LeftIndicatorLight ||
                    d.Sensor.RightIndicatorLight))
                {
                    delayDateTime = DateTime.Now;
                    IsChangeLineSpeak = true;
                    RecordSignalCount = 0;
                    RecordSignalTime = null;
                    StartChangeLanesAngle = signalInfo.BearingAngle;
                    BreakRule(DeductionRuleCodes.RC30205, DeductionRuleCodes.SRC3020503);
                }
                else
                {
                    delayDateTime = DateTime.Now;
                    IsChangeLineSpeak = true;

                    //转向灯是否超过3秒,检测12秒前
                    var leftLight = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.LeftIndicatorLight, DateTime.Now - TimeSpan.FromSeconds(12), Settings.TurnLightAheadOfTime); ;
                    if (leftLight)
                        return;

                    var rightLight = AdvancedSignal.CheckOperationAheadSeconds(x => x.Sensor.RightIndicatorLight, DateTime.Now - TimeSpan.FromSeconds(12), Settings.TurnLightAheadOfTime); ;
                    if (rightLight)
                        return;

                    BreakRule(DeductionRuleCodes.RC30206, DeductionRuleCodes.SRC3020603);
                }

            }

            if (RecordSignalTime != null && (DateTime.Now - RecordSignalTime.Value).TotalSeconds > Settings.CommonExamItemsChangeLanesTimeOut)
            {
                //重置角度
                RecordSignalTime = null;
                StartChangeLanesAngle = signalInfo.BearingAngle;
            }
        }


        private void OnEngineRuleMessage(EngineRuleMessage message)
        {
            StopCheckEngine = !message.Enable;
        }
        #endregion
        
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            #region 踩刹车“噔”声音
            //Todo:现在只有黔江那个客户需要

            CheckBrakeVoice(signalInfo);

            #endregion
            #region 检测全程发动机转速
            CheckEngineRpm(signalInfo);
            #endregion

            #region 检测转向灯超时  项目中不评判
           
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

            #region 检测转向灯超距
            if (!IsInExamItem)
            {
                if ((signalInfo.Sensor.LeftIndicatorLight || signalInfo.Sensor.RightIndicatorLight) && IndicatorLightInitDistance == 0)
                {
                    IndicatorLightInitDistance = signalInfo.Distance;
                }
                else if (!signalInfo.Sensor.LeftIndicatorLight && !signalInfo.Sensor.RightIndicatorLight)
                {
                    IndicatorLightInitDistance = 0;
                }
                if (signalInfo.Distance - IndicatorLightInitDistance >= Settings.IndicatorLightDistanceout && IndicatorLightInitDistance != 0&& Settings.IndicatorLightDistanceout>0)
                {
                    IndicatorLightInitDistance = 0;
                    BreakRule(DeductionRuleCodes.RC30215);
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

            #region 检测全程变道
            if (Settings.CommonExamItemsCheckChangeLanes)
            {
                ChangeLineAngleCheck(signalInfo);
            }
            #endregion

            #region 检测长时间使用离合

            //CheckClutchTaxiingTimeout(signalInfo);

            CheckClutchDistance(signalInfo);
            #endregion

            #region 检测空档滑行
            CheckGearNeutralDistanceAndTime(signalInfo);
            #endregion

            #region 检测速度和档位是否匹配

            CheckGearAndSpeed(signalInfo);
            #endregion

            #region 检测全程限制

            //TODO:超过全程速度限制改为 需要超过并且维持指定时间
            if (!IsBrokenMaxSpeed && Settings.MaxSpeed > 0)
            {
                if (signalInfo.SpeedInKmh > Settings.MaxSpeed&&!ReachMaxSpeedTime.HasValue)
                {
                    ReachMaxSpeedTime = DateTime.Now;
                }
               if (signalInfo.SpeedInKmh > Settings.MaxSpeed &&ReachMaxSpeedTime.HasValue
                 &&(DateTime.Now-ReachMaxSpeedTime).Value.TotalMilliseconds>=Settings.CommonExamItemsMaxSpeedKeepTime)
                {
                    var SignalInfo = CarSignalSet.Query(DateTime.Now.AddSeconds(-3));
                    Logger.ErrorFormat("检测全程限制S:{0},s1:{1}", signalInfo.SpeedInKmh, Settings.MaxSpeed);
                    Logger.ErrorFormat("最近3秒速度值S:{0}", string.Join(",", SignalInfo.Select(S => S.SpeedInKmh).ToArray()));
                    IsBrokenMaxSpeed = true;
                    BreakRule(DeductionRuleCodes.RC40001);
                }
                if (signalInfo.SpeedInKmh<=Settings.MaxSpeed)
                {
                    ReachMaxSpeedTime = null;
                }
  
            }

            #endregion

            //#region 检测是否达到全程最低速度要求
            //if (signalInfo.SpeedInKmh >= Settings.GlobalLowestSpeed && NowSignalOverLowestSpeed == false)
            //{
            //    IsReachedGlobalLowestSpeed = true;
            //    SignalOverLowestSpeedTempBeginDistince = signalInfo.Distance;
            //    SignalOverLowestSpeedTempBeginTime = DateTime.Now;
            //    NowSignalOverLowestSpeed = true;
            //    NowSignalNotOverLowsetSpeed = true;
            //}
            //else if (NowSignalNotOverLowsetSpeed)
            //{
            //    if (signalInfo.SpeedInKmh < Settings.GlobalLowestSpeed)
            //    {
            //        NowSignalNotOverLowsetSpeed = false;
            //        NowSignalOverLowestSpeed = false;
            //        OverLowestSpeedDistince += (signalInfo.Distance - SignalOverLowestSpeedTempBeginDistince);
            //        OverLowestSpeedTime += (DateTime.Now - SignalOverLowestSpeedTempBeginTime).TotalSeconds;
            //    }
            //}



            //#endregion

            #region 起步后松手刹
            //TODO:这个先暂时屏蔽掉,已经把这个项目移动到起步
           // CheckHandbrake(signalInfo);

            #endregion

            #region 全程档位速度检测
            CheckGlobalGearAndSpeed(signalInfo);
            #endregion

            #region 检测档位超距
            CheckGearOneDistanceAndTime(signalInfo);
            CheckGearTwoDistanceAndTime(signalInfo);

           
            #endregion

            #region 监测全程最低速度要求
            if (Settings.LicenseC1)
                CheckOverOnceSpeed(signalInfo);
            #endregion

            #region 3档累计距离
            AccumulativeGear3Distance(signalInfo);
            #endregion


            base.ExecuteCore(signalInfo);
        }

        /// <summary>
        /// 转向灯超时，停着不计算时间
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected bool CheckIndicatorTimeout(CarSignalInfo signalInfo)
        {
           // Logger.Error(Settings.IndicatorLightTimeout.ToString());
         
            if (Settings.IndicatorLightTimeout <= 0 || !ExamItemEndTime.HasValue)
                return true;


            //项目结束到现在超过设置时间才有效
            if ((DateTime.Now - ExamItemEndTime.Value).TotalSeconds < Settings.IndicatorLightTimeout)
                return true;

         
            //刚开始的时候数据未超过超时时间
            if (CarSignalSet.Count == 0 ||(DateTime.Now - CarSignalSet.Last().RecordTime).TotalSeconds < Settings.IndicatorLightTimeout)
                return true;
          
            //查询15秒以前的数据
            var sensors = CarSignalSet.QueryCachedSeconds(Settings.IndicatorLightTimeout).Where(x => x.IsValid);


            //需要处理一下重新计算时间
            var carSignalInfos = sensors.ToArray();
            if (signalInfo.Sensor.LeftIndicatorLight)
            {
                if (carSignalInfos.All(x => x.Sensor.LeftIndicatorLight))
                {
                    BreakRule(DeductionRuleCodes.RC30215, null);
                  
                    return false;
                }
            }
            if (signalInfo.Sensor.RightIndicatorLight)
            {
                if (carSignalInfos.All(x => x.Sensor.RightIndicatorLight))
                {
                    BreakRule(DeductionRuleCodes.RC30215, null);
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
        /// TODO:信号闪引起手刹误判，把这个移动
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
        private bool IsBrokenClutchTaxiing = false;
        private DateTime? StartCheckClutchTaxiing { get; set; }
        /// <summary>
        /// 检测长时间使用离合
        /// </summary>
        /// <param name="signalInfo"></param>
        private void CheckClutchTaxiingTimeout(CarSignalInfo signalInfo)
        {
            if (Settings.ClutchTaxiingTimeout <= 0)
                return;

            ///重置状态
            if (IsBrokenClutchTaxiing)
            {
                if (!signalInfo.Sensor.Clutch || signalInfo.CarState == CarState.Stop)
                {
                    StartCheckClutchTaxiing = null;
                    IsBrokenClutchTaxiing = false;
                }
                return;
            }

            ///计时离合使用时间
            if (signalInfo.CarState == CarState.Moving && signalInfo.Sensor.Clutch)
            {
                if (!StartCheckClutchTaxiing.HasValue)
                    StartCheckClutchTaxiing = DateTime.Now;

                if ((DateTime.Now - StartCheckClutchTaxiing.Value).TotalSeconds > Settings.ClutchTaxiingTimeout)
                {
                    IsBrokenClutchTaxiing = true;
                    StartCheckClutchTaxiing = null;
                    BreakRule(DeductionRuleCodes.RC40403);
                }
            }
            else
            {
                StartCheckClutchTaxiing = null;
            }

        }



        #region  长时间检测离合（时间/距离+速度）

        private double? _clutchDistance { get; set; }
        private DateTime? _clutchTime { get; set; }

        private bool _clutchMoving { get; set; }

        /// <summary>
        /// 检测长时使用离合
        /// </summary>
        /// <param name="signalInfo"></param>
        private void CheckClutchDistance(CarSignalInfo signalInfo)
        {
            //设置靠边停车不检测离合，且正在靠边停项目
            if (_isPullover)
            {
                IsBrokenClutchTaxiing = false;
                return;
            }

            if (Settings.ClutchTaxiingTimeout < 2 && Settings.ClutchTaxiingDistance < 2)
                return;

            //车停，则重置评判离合
            if (signalInfo.CarState == CarState.Stop)
                IsBrokenClutchTaxiing = false;

            //该速度以上，则为移动
            if (Settings.ClutchTaxiingSpeedLimit > 1)
            {
                _clutchMoving = signalInfo.SpeedInKmh > Settings.ClutchTaxiingSpeedLimit ? true : false;
            }
            //有距离，则只判距离
            if (Settings.ClutchTaxiingDistance > 2)
            {
                if (signalInfo.Sensor.Clutch && _clutchMoving)
                {
                    if (!_clutchDistance.HasValue)
                        _clutchDistance = signalInfo.Distance;
                    if (signalInfo.Distance - _clutchDistance.Value > Settings.ClutchTaxiingDistance && IsBrokenClutchTaxiing == false)
                    {
                        IsBrokenClutchTaxiing = true;
                        BreakRule(DeductionRuleCodes.RC40403);
                    }
                }
                else
                {
                    _clutchDistance = null;
                }

            }
            else if (Settings.ClutchTaxiingTimeout > 2)
            {
                if (signalInfo.Sensor.Clutch && _clutchMoving)
                {
                    if (!_clutchTime.HasValue)
                        _clutchTime = DateTime.Now;
                    if (IsBrokenClutchTaxiing == false && (DateTime.Now - _clutchTime.Value).TotalSeconds > Settings.ClutchTaxiingTimeout)
                    {
                        IsBrokenClutchTaxiing = true;
                        BreakRule(DeductionRuleCodes.RC40403);
                    }
                }
                else
                {
                    _clutchTime = null;
                }
            }


        }
        #endregion



        /// <summary>
        /// 检查空挡滑行
        /// </summary>
        /// <param name="signalInfo"></param>
        private void CheckGearNeutralDistanceAndTime(CarSignalInfo signalInfo)
        {
            ///如果设置为否则不检测
            if(Settings.NeutralTaxiingTimeout <= 0&&Settings.NeutralTaxiingMaxDistance<=0)
                return;

            if (IsBrokenTaxiingOnN)
            {
                //如果档位不是空挡,都以空挡传感器为准
                if (!signalInfo.Sensor.IsNeutral|| signalInfo.CarState == CarState.Stop)
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
                //已安装空挡传感器，20160315，李
                if (Settings.IsNeutralGear)
                {
                    //不是1档，直接重置
                    if ((int)signalInfo.Sensor.Gear > (int)Gear.One)
                    {
                        IsCheckGearOneDistanceAndTime = false;
                        GearOneStartTime = null;
                        GearOneStartDistance = null;
                        return;
                    }
                    //档杆空挡，重置距离
                    if (signalInfo.Sensor.IsNeutral)
                    {
                        IsCheckGearOneDistanceAndTime = false;
                        GearOneStartTime = null;
                        GearOneStartDistance = null;
                    }
                }
                else
                {
                    IsCheckGearOneDistanceAndTime = false;
                    GearOneStartTime = null;
                    GearOneStartDistance = null;
                }
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
                //已安装空挡传感器，20160315，李
                if (Settings.IsNeutralGear)
                {
                    //不是2档，直接重置
                    if ((int)signalInfo.Sensor.Gear > (int)Gear.Two)
                    {
                        IsCheckGearTwoDistanceAndTime = false;
                        GearTwoStartTime = null;
                        GearTwoStartDistance = null;
                        return;
                    }
                    if ((int)signalInfo.Sensor.Gear==(int)Gear.One)
                    {
                        IsCheckGearTwoDistanceAndTime = false;
                        GearTwoStartTime = null;
                        GearTwoStartDistance = null;
                        return;
                    }
                    //档杆空挡，重置距离
                    if (signalInfo.Sensor.IsNeutral)
                    {
                        IsCheckGearTwoDistanceAndTime = false;
                        GearTwoStartTime = null;
                        GearTwoStartDistance = null;
                    }
                }
                else
                {
                    IsCheckGearTwoDistanceAndTime = false;
                    GearTwoStartTime = null;
                    GearTwoStartDistance = null;
                }
            }
        }


        private DateTime? _lastSignalTime { get; set; }
        //3档累计距离
        private double Gear3TotalDistance { get; set; }
        /// <summary>
        /// 3档累计距离
        /// </summary>
        /// <param name="signalInfo"></param>
        protected void AccumulativeGear3Distance(CarSignalInfo signalInfo)
        {

            //记录上个信号时间
            if (!_lastSignalTime.HasValue)
                _lastSignalTime = signalInfo.RecordTime;

            double timespan = (signalInfo.RecordTime - _lastSignalTime.Value).TotalMilliseconds / 1000;
            timespan = Convert.ToDouble(timespan.ToString("N3"));
            _lastSignalTime = signalInfo.RecordTime;
            //3档累计距离
            if (signalInfo.Sensor.SpeedInKmh > 0 && signalInfo.Sensor.Gear == Gear.Three)
            {
                Gear3TotalDistance = Gear3TotalDistance + (signalInfo.Sensor.SpeedInKmh / 3.6) * timespan;

            }

        }



        protected bool brakeVoiceSpeaked = false;
        /// <summary>
        /// 检测刹车，播报“咚”声音
        /// </summary>
        /// <param name="signalInfo"></param>
        protected virtual void CheckBrakeVoice(CarSignalInfo signalInfo)
        {
            if (!Settings.CommonExamItemsBreakVoice)
            {
                return;
            }
            if (signalInfo.Sensor.Brake && !brakeVoiceSpeaked)
            {
                brakeVoiceSpeaked = true;
                Speaker.SpeakBreakeVoice();
                //   Speaker.PlayAudioAsync("咚");
            }
            if (!signalInfo.Sensor.Brake)
            {
                brakeVoiceSpeaked = false;
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
            //3档累计距离要求
            if (Settings.GearThreeDrivingDistance > 10 && Gear3TotalDistance < Settings.GearThreeDrivingDistance)
            {
                BreakRule(DeductionRuleCodes.RC30156);
            }

            if (!IsReachedGlobalLowestSpeed)
            {
                BreakRule(DeductionRuleCodes.RC40002);
            }
            //该速度累计的距离没有达到 设定的值
            //else if (OverLowestSpeedDistince < Settings.GlobalLowestSpeedHoldDistince && Settings.GlobalLowestSpeedHoldDistince > 0)
            //{
            //    BreakRule(DeductionRuleCodes.RC40002);
            //}
            //else if (OverLowestSpeedTime < Settings.GlobalLowestSpeedHoldTimeSeconds && Settings.GlobalLowestSpeedHoldTimeSeconds > 0)
            //{
            //    BreakRule(DeductionRuleCodes.RC40002);
            //}
            //TODO:功能重复
            //if (!IsReachedGlobalGearContinuous)
            //{
            //    BreakRule(DeductionRuleCodes.RC40003);
            //}
            base.StopCore();
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.CommonExamItem; }
        }

        protected void BreakRule(string ruleCode, string subRuleCode = null, int DeductedScores = 0,string DeductedVoiceFile="")
        {
            ExamScore.BreakRule(ItemCode, Name, ruleCode, subRuleCode, null, DeductedScores, DeductedVoiceFile);
        }


    }
}
