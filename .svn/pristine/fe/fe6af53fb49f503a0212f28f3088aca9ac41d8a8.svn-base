using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    /// <summary>
    /// 车载所有信号，车载GPS，传感器等
    /// </summary>
    public class CarSignalInfo
    {
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime { get; set; }

        /// <summary>
        /// Gps信息
        /// </summary>
        public GpsInfo Gps { get; set; }

        /// <summary>
        /// 方向角度
        /// </summary>
        public double DirectionDegrees { get; set; }

        /// <summary>
        /// 车载传感器
        /// </summary>
        public CarSensorInfo Sensor { get; set; }

    

        public ObdCarSensorInfo ObdSensor { get; set; }

        /// <summary>
        /// 车辆运行状态
        /// </summary>
        public CarState CarState { get; set; }

        /// <summary>
        /// 车辆的转向角度（经过计算）
        /// </summary>
        public double BearingAngle { get; set; }

        /// <summary>
        /// 经过计算后的发动机转速
        /// </summary>
        public int EngineRpm { get; set; }

        /// <summary>
        /// 车辆的速度（单位：千米每小时）取车载信号或者gps信号
        /// </summary>
        public double SpeedInKmh { get; set; }

        /// <summary>
        /// 当前行驶距离
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// 考试耗时
        /// </summary>
        public TimeSpan UsedTime{ get; set; }

        /// <summary>
        /// 引擎的速率
        /// </summary>
        public int EngineRatio { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get { return IsGpsValid && IsSensorValid; } }

        /// <summary>
        /// Gps是否有效
        /// </summary>
        public bool IsGpsValid { get; set; }

        /// <summary>
        /// 传感器是否有效
        /// </summary>
        public bool IsSensorValid { get; set; }

        #region 陀螺仪新增角度的扩展属性

        /// <summary>
        /// 角度
        /// </summary>
        public double AngleX { get; set; }
        public double AngleY { get; set; }
        public double AngleZ { get; set; }

        /// <summary>
        /// 角速度
        /// </summary>
        public double AngleSpeedX { get; set; }
        public double AngleSpeedY { get; set; }
        public double AngleSpeedZ { get; set; }

        /// <summary>
        /// 角加速度
        /// </summary>
        public double AccelerationX { get; set; }
        public double AccelerationY { get; set; }
        public double AccelerationZ { get; set; }

        #endregion

        public CarSignalInfo()
        {
            RecordTime = DateTime.Now;
        }
        /// <summary>
        /// 破线板信号
        /// </summary>
        public string SensorBody { get; set; }

        /// <summary>
        /// 免破线信号
        /// </summary>
        public string OBDSensorBody { get; set; }
        
        /// <summary>
        /// 陀螺仪信号
        /// </summary>
        public string GyroSensorBody { get; set; }
        
        /// <summary>
        /// 传感器信号数组
        /// </summary>
        public int[] inputs { get; set; }

        /// <summary>
        /// 底层原始信号
        /// </summary>
        public string[] commands { get; set; }

        /// <summary>
        /// 单片机中输出的Count
        /// </summary>
        public UInt32 Count;
        

        /// <summary>
        /// 单片机中输出的Time
        /// </summary>
        public UInt32 Time;


    }
}
