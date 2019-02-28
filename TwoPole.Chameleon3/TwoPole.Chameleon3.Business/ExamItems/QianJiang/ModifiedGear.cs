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
    /// 1���Ƿ�����˼Ӽ������
    /// 2���ӵ���ʱ���Ƿ��ں���Χ��
    /// ���⣺��һ�ε�����һ�ε��ͺϸ�ǭ��
    /// </summary>
    public class ModifiedGear : ExamItemBase
    {
        public const Gear LowestGear = Gear.Two;

        #region ˽�б���

        protected double ModifiedGearStartDrivingDistance { get; set; }

        protected List<Tuple<Gear, int>> ModifiedGears { get; set; }


        /// <summary>
        /// �Ƿ���֤ͨ��
        /// </summary>
        protected bool IsSuccess { get; set; }

        #endregion

        #region ��������


        /// <summary>
        /// ��λ���ά��ʱ��
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
        /// ��ʼ������
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        protected override bool InitExamParms(CarSignalInfo signalInfo)
        {
            //ʱ��;��붼û���ã�ֱ�ӽ���
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
                //��ʼ���ӵ���λ
                for (var i = Settings.ModifiedGearGearMin; i <= Settings.ModifiedGearGearMax; i++)
                {
                    gears.Add((Gear)i);
                }
            }
            if (Settings.ModifiedGearReduceGearCheck)
            {
                //��ʼ��������λ
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
        /// ��Ŀִ��
        /// </summary>
        /// <param name="signalInfo"></param>
        protected override void ExecuteCore(CarSignalInfo signalInfo)
        {
            if (!InitializedExamParms)
                return;

            //δ�ڹ��򶨵� ����/ʱ�� ��ɼӼ������ɻ������

            //δ�ﵽ��Сʱ�����
            //if (GearLowestMilliseconds > 0 && (DateTime.Now - StartTime).TotalMilliseconds < GearLowestMilliseconds)
            //    return;

            if (IsSuccess)
            {
                //��ɲ�����ҲҪ����Ž����Ӽ�������
                //base.StopCore();
                return;
            }
            //�Զ��������е�λ
            if (Settings.LicenseC1)
            {
                IsSuccess = CheckGears();
            }
            else
            {
                //�Զ������������У��ȴ�����
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
            Logger.InfoFormat("�Ӽ�����{0}", string.Join(",", gearStates.Select(x => string.Format("{0}-{1}-{2}", x.Gear.ToDisplayGearText(), x.Count, x.Milliseconds))));

            //��һ�Σ���һ�ε��Ϳ����ˣ�20161230
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
