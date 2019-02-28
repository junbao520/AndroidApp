using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders;
using System.Linq;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Business.ExamItems;

namespace TwoPole.Chameleon3.Business.Chongqin.RongChang.ExamItems
{
    /// <summary>
    /// 1，是否进行了加减档检测
    /// 2，加档档时间是否在合理范围内
    /// //特殊：可以配置档位流程
    /// </summary>
    public class ModifiedGear: ExamItemBase
    {
        public const Gear LowestGear = Gear.Two;

        #region 私有变量

        protected double ModifiedGearStartDrivingDistance { get; set; }

        protected List<Tuple<Gear, int>> ModifiedGears { get; set; }


        /// <summary>
        /// 是否验证通过
        /// </summary>
        protected bool IsSuccess { get; set; }

        protected Gear LastGear = Gear.Neutral;


        /// <summary>
        /// 是否播放档位语音
        /// </summary>
        protected bool IsPlayGearVocie = false;

        /// <summary>
        /// 是否播放操作语音
        /// </summary>
        protected bool IsPlayActionVocie = true;


        //自定义流程最多支持三种，这样比较简单
        protected List<List<Gear>> LstGearFlow = new List<List<Gear>>();
        //流程索引 根据第一次进来的档位判断进入的流程索引
        protected int GearFlowIndex = -1;



        protected Gear NowFlowGear = Gear.Neutral;

        protected ModifiedGearStep CommonModifiedGearStep = ModifiedGearStep.Continue;


        Gear NextGear;

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

            IsPlayActionVocie = Settings.ModifiedGearIsPlayActionVoice;
            IsPlayGearVocie = Settings.ModifiedGearIsPlayGearVoice;

            if (string.IsNullOrEmpty(Settings.ModifiedGearGearFlow))
            {
                return;
            }
            //通过；进行拆分工作。
            for (int j = 0; j < Settings.ModifiedGearGearFlow.Split(';').Length; j++)
            {
                string TempGearFlow = Settings.ModifiedGearGearFlow.Split(';')[j];
                List<Gear> lstTempGear = new List<Gear>();
                for (int i = 0; i < TempGearFlow.Split('-').Length; i++)
                {
                    lstTempGear.Add((Gear)Convert.ToInt32(TempGearFlow.Split('-')[i]));
                }
                LstGearFlow.Add(lstTempGear);
            }
            //List<Gear> lstTempGear = new List<Gear>();
            ////档位流程测试2-3-4-3-2
            //lstTempGear.Add(Gear.Two);
            //lstTempGear.Add(Gear.Three);
            //lstTempGear.Add(Gear.Four);
            //lstTempGear.Add(Gear.Three);
            //lstTempGear.Add(Gear.Two);
            //LstGearFlow.Add(lstTempGear);
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
            IsSuccess = false;
           // AdvancedCarSignal = Singleton.GetAdvancedCarSignal;

            ModifiedGearStartDrivingDistance = signalInfo.Distance;
            ModifiedGears = new List<Tuple<Gear, int>>();

            if (Settings.ModifiedGearGearMax <= (int)LowestGear)
                return false;

            List<Gear> gears = new List<Gear>();
            if (Settings.ModifiedGearAddGearCheck)
            {
                //初始化加档档位
                for (var i = Settings.ModifiedGearGearMin; i <= Settings.ModifiedGearGearMax; i++)
                {
                    gears.Add((Gear)i);
                }
            }
            if (Settings.ModifiedGearReduceGearCheck)
            {
                //初始化减档档位
                for (var i = Settings.ModifiedGearGearMax - 1; i > Settings.ModifiedGearGearMin; i--)
                {
                    gears.Add((Gear)i);
                }
            }
            if (gears.Count == 0)
                return false;



            var query = from a in gears
                        group a by a
                        into g
                        orderby g.Key
                        select Tuple.Create(g.Key, g.Count());

            ModifiedGears.AddRange(query);
            return base.InitExamParms(signalInfo);
        }

        protected override void StopCore()
        {
            if (!IsSuccess && InitializedExamParms)
                BreakRule(DeductionRuleCodes.RC30140);

            base.StopCore();
        }

        /// <summary>
        /// 项目执行
        /// </summary>
        /// <param name="signalInfo"></param>
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!InitializedExamParms)
                return;

            if (IsSuccess || CommonModifiedGearStep == ModifiedGearStep.Finished)
            {
                return;
            }

            //如果没有启用自定义流程 /**********
            if (string.IsNullOrEmpty(Settings.ModifiedGearGearFlow))
            {
                IsSuccess = true;
                //IsSuccess = CheckGears();
                return;
            }
            
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
            if (query.Count() == 0)
            {
                return;
            }
            var NowGear = query.ToArray().FirstOrDefault().Gear;
            signalInfo.Sensor.IsNeutral = signalInfo.Sensor.Clutch;
            if (signalInfo.Sensor.IsNeutral)
                NowGear = Gear.Neutral;
            if (NowGear == Gear.Neutral)
            {
                return;
            }
            if (NowGear != LastGear)
            {
                if (IsPlayGearVocie)
                {
                    string GearVocie = string.Format("{0}档",Convert.ToInt32(NowGear));
                    Speaker.PlayAudioAsync(GearVocie);
                }
                if (GearFlowIndex == -1)
                {
                    SetGearFlowIndex(NowGear);
                }
                CheckFlowGear(NowGear);
                LastGear = NowGear;
            }
        }

        protected ModifiedGearStep CheckFlowGear(Gear NowGear)
        {
            //如果已经成功走完流程就表示正确的

            //如果正在流程中 //如果两个档位发生了变化才继续
            if (CommonModifiedGearStep == ModifiedGearStep.Continue)
            {
                if (NowGear == NextGear)
                {
                    CommonModifiedGearStep = ModifiedGearStep.Continue;
                    NextGear = GetNextGear();

                    if (NextGear == Gear.Neutral)
                    {
                        IsSuccess = true;
                        CommonModifiedGearStep = ModifiedGearStep.Finished;
                    }
                    else
                    {   
                        PlayActionVoice(NowGear, NextGear);
                    }
                }
            }

            return CommonModifiedGearStep;
        }


        //设置流程索引,
        protected void SetGearFlowIndex(Gear NowGear)
        {
            for (int i = 0; i < LstGearFlow.Count; i++)
            {
                if (LstGearFlow[i].FirstOrDefault() == NowGear)
                {
                    GearFlowIndex = i;
                    NextGear = NowGear;
                    return;
                }
            }
            return;
        }
        protected Gear GetNextGear()
        {
            //现在主要是通过集合在区分//
            //判断是否是第一次进入，如果是第一次进入的话需要标记进入的是第一个还是第二个流程状态
            if (LstGearFlow[GearFlowIndex].Count == 0)
            {
                return Gear.Neutral;
            }
            LstGearFlow[GearFlowIndex].RemoveAt(0);
            Gear gear = LstGearFlow[GearFlowIndex][0];
            return gear;
        }

        protected void PlayActionVoice(Gear NowGear, Gear NextGear)
        {
            //如果当前档位等于下一个档位 或者 不播放档位操作语音就直接Retrun
            if (NowGear == NextGear || !IsPlayActionVocie)
            {
                return;
            }
            int NextGearNumber = Convert.ToInt32(NextGear);
            string VocieText = string.Empty;
            //如果当前档位大于下一个档位 则提示请减档  
            //当前档位已经是5档了，下一个档位是4档
         
            if (Convert.ToInt32(NowGear) > Convert.ToInt32(NextGear))
            {
                if (NowGear==Gear.Three)
                {
                    VocieText = "请把3档减到2档";
                }
                else
                {
                    VocieText = "请减到" + NextGearNumber.ToString() + "档";
                }
               
            }
            else
            {
                if (NowGear==Gear.Two)
                {
                    VocieText = "请把2档加到3档";
                }
                else
                {
                    VocieText = "请加到" + NextGearNumber.ToString() + "档";
                }
               
            }
            Speaker.PlayAudioAsync(VocieText);
        }

        //CheckGear
        private bool CheckGears()
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
            //Logger.InfoFormat("加减档：{0}", string.Join(",", gearStates.Select(x => string.Format("{0}-{1}-{2}", x.Gear.ToDisplayGearText(), x.Count, x.Milliseconds))));
            if (gearStates.Length < ModifiedGears.Count)
                return false;

            foreach (var item in ModifiedGears)
            {
                var a = gearStates.FirstOrDefault(x => x.Gear == item.Item1);
                if (a == null)
                    return false;

                if (a.Count < item.Item2)
                    return false;
            }

            return true;
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.ModifiedGear; }
        }
        public enum ModifiedGearStep
        {
            Continue,
            Finished,
            Error

        }
    }
}
