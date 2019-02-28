using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders;
using System.Linq;


namespace TwoPole.Chameleon3.Business.Areas.Chongqin.QianJiang.ExamItems
{
    /// <summary>
    /// 1，是否进行了加减档检测
    /// 2，加档档时间是否在合理范围内
    /// 特殊：加一次档，减一次档就合格，黔江
    /// </summary>
    public class ModifiedGear : ExamItemBase
    {
        public const Gear LowestGear = Gear.Two;

        #region 私有变量

        protected double ModifiedGearStartDrivingDistance { get; set; }

        protected List<Tuple<Gear, int>> ModifiedGears { get; set; }


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
            GearRecord = new List<Gear>();
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

            //未在规则定的 距离/时间 完成加减档，由基类完成

            //未达到最小时间忽略
            //if (GearLowestMilliseconds > 0 && (DateTime.Now - StartTime).TotalMilliseconds < GearLowestMilliseconds)
            //    return;

            if (IsSuccess)
            {
                //完成操作后也要距离才结束加减档操作
                //base.StopCore();
                return;
            }
            //自动挡不评判档位
            if (Settings.LicenseC1)
            {
                IsSuccess = CheckGears();
            }
            else
            {
                //自动挡不进行评判，等待超距
                IsSuccess = true;
            }
        }

        protected Gear? InitGear { get; set; }
        protected List<Gear> GearRecord { get; set; }

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
            Logger.InfoFormat("加减档：{0}", string.Join(",", gearStates.Select(x => string.Format("{0}-{1}-{2}", x.Gear.ToDisplayGearText(), x.Count, x.Milliseconds))));

            //加一次，减一次档就可以了，20161230
            if (InitGear.HasValue)
            {
                var tempGear = gearStates.FirstOrDefault().Gear;
                if (InitGear != tempGear && !GearRecord.Contains(tempGear))
                {
                    InitGear = tempGear;
                    GearRecord.Add(tempGear);
                }
                if (GearRecord.Count >= 2)
                    return true;

            }
            if (!InitGear.HasValue && gearStates.Length > 0)
                InitGear = gearStates.FirstOrDefault().Gear;
            return false;
        }

        public override string ItemCode
        {
            get { return ExamItemCodes.ModifiedGear; }
        }
    }
}
