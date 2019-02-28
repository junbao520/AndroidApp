using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure
{
    public class CarSensorInfo
    {
        public CarSensorInfo()
        {
            this.RecordTime = DateTime.Now;
            this.Heading = -1;
        }

        #region 车载控制信号
        /// <summary>
        /// 离合器；true：踩到底
        /// </summary>
        public bool Clutch { get; set; }

        /// <summary>
        /// 喇叭
        /// </summary>
        public bool Loudspeaker { get; set; }

        /// <summary>
        /// 雨刷
        /// </summary>
        //public bool WindshieldWiper { get; set; }

        /// <summary>
        /// 安全带
        /// </summary>
        public bool SafetyBelt { get;  set; }

        /// <summary>
        /// 档位
        /// </summary>
        public Gear Gear { get; set; }

        /// <summary>
        /// OBD 档位来源
        /// </summary>
        public Gear OBDGear { get; set; }

        /// <summary>
        /// 手刹
        /// </summary>
        public bool Handbrake { get; set; }

        /// <summary>
        /// 刹车
        /// </summary>
        public bool Brake { get; set; }

        /// <summary>
        /// 车门
        /// </summary>
        public bool Door { get; set; }

        /// <summary>
        /// 发动机
        /// </summary>
        public bool Engine { get; set; }

        /// <summary>
        /// 外接的发动机线 
        /// </summary>
        public bool SpecialEngine { get; set; }

        /// <summary>
        /// 发动机转速,这个转速未经处理，请用CarSignal类里面的EngineRpm;
        /// </summary>
        public int EngineRpm { get; set; }
        #endregion

        #region 车载灯光
        /// <summary>
        /// 雾灯
        /// </summary>
        public bool FogLight { get; set; }

        /// <summary>
        /// 示廓灯
        /// </summary>
        public bool OutlineLight { get; set; }

        /// <summary>
        /// 原始实时左转向灯信号
        /// </summary>
        public bool OriginalLeftIndicatorLight { get; set; }

        /// <summary>
        /// 左转向指示灯
        /// </summary>
        public bool LeftIndicatorLight { get; set; }


        /// <summary>
        /// 原始实时右转向指示灯
        /// </summary>
        public bool OriginalRightIndicatorLight { get; set; }

        /// <summary>
        /// 右转向指示灯
        /// </summary>
        public bool RightIndicatorLight { get; set; }

        /// <summary>
        /// 警示灯
        /// </summary>
        public bool CautionLight { get; set; }

        /// <summary>
        /// 远光灯
        /// </summary>
        public bool HighBeam { get; set; }

        /// <summary>
        /// 近光灯
        /// </summary>
        public bool LowBeam { get; set; }

        /// <summary>
        /// 倒车灯
        /// </summary>
        public bool ReversingLight { get; set; }

        #endregion

        #region 扩展的属性
        /// <summary>
        /// 扩展的属性-进过车头
        /// </summary>
        public bool ArrivedHeadstock { get; set; }

        /// <summary>
        /// 扩展的属性-进过车头2
        /// </summary>
        public bool ArrivedHeadstock2 { get; set; }

        /// <summary>
        /// 扩展的属性-进过车尾
        /// </summary>
        public bool ArrivedTailstock { get; set; }

        /// <summary>
        /// 扩展的属性-进过车尾2
        /// </summary>
        public bool ArrivedTailstock2 { get; set; }

        /// <summary>
        /// 汽车座椅
        /// </summary>
        public bool Seats { get; set; }

        /// <summary>
        /// 外后视镜
        /// </summary>
        public bool ExteriorMirror { get; set; }

        /// <summary>
        /// 内后视镜
        /// </summary>
        public bool InnerMirror { get; set; }


        /// <summary>
        /// 指纹仪
        /// </summary>
        public bool Fingerprint { get; set; }

        /// <summary>
        /// 是否空挡
        /// </summary>
        public bool IsNeutral { get; set; }



        #region 档位显示信号

        public bool GearDisplayD1 { get; set; }
        public bool GearDisplayD2 { get; set; }
        public bool GearDisplayD3 { get; set; }
        public bool GearDisplayD4 { get; set; }

        #endregion


        #region 拉线信号
        #region 定义拉线地址 
 
        public  bool PullLineD1 { get; set; }
 
        public bool PullLineD2 { get; set; }
  
        public bool PullLineD3 { get; set; }

    
        public bool PullLineD4 { get; set; }

        public bool PullLineD5 { get; set; }

        public bool PullLineD6 { get; set; }


    #endregion
    #endregion

    #endregion

    /// <summary>
    /// 读取的车速
    /// </summary>
    public double SpeedInKmh { get; set; }

        /// <summary>
        /// 航向角
        /// </summary>
        public double Heading { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime { get; set; }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb
                .AppendFormat("记录时间: {0:yyyy-MM-dd HH:mm:ss.ff}", this.RecordTime)
                .AppendFormat(" 离合器: {0}", this.Clutch)
                .AppendFormat(" 喇叭: {0}", this.Loudspeaker)
                .AppendFormat(" 安全带: {0}", this.SafetyBelt)
                .AppendFormat(" 档位: {0}", "")
                .AppendFormat(" 手刹: {0}", this.Handbrake)
                .AppendFormat(" 刹车: {0}", this.Brake)
                .AppendFormat(" 发动机: {0}", this.Engine)
                .AppendFormat(" 发动机转速(RPM): {0}", this.EngineRpm)
                .AppendFormat(" 雾灯: {0}", this.FogLight)
                .AppendFormat(" 示廓灯: {0}", this.OutlineLight)
                .AppendFormat(" 左转向灯: {0}", this.LeftIndicatorLight)
                .AppendFormat(" 右转向灯: {0}", this.RightIndicatorLight)
                .AppendFormat(" 警报灯: {0}", this.CautionLight)
                .AppendFormat(" 远光灯: {0}", this.HighBeam)
                .AppendFormat(" 近光灯: {0}", this.LowBeam)
                .AppendFormat(" 车速(Kmh): {0}", this.SpeedInKmh);
            return sb.ToString();
        }
    }
}
