using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class Constants
    {
        /// <summary>
        /// 默认总的车道数
        /// </summary>
        public const int DefaultTotalDrivingLane = 1;
        /// <summary>
        /// 默认道路宽度，单位：米；
        /// </summary>
        public const double DefaultRoadWidth = 3.5;

        /// <summary>
        /// 道路宽度的分隔符号
        /// </summary>
        public const string RoadWidthSeparator = "-";

        public const string ApplicationName = "Chameleon3";
        public const string MapPointCodeAttributeName = "Code";
        public const string MapPointNameAttributeName = "Name";
        public const string MapRoadCountAttributeName = "RoadCount";
        public const string MapRoadWidthAttributeName = "RoadWidth";
        public const string MapSpeedLimitAttributeName = "SpeedLimit";
        /// <summary>
        /// 档位闪烁过滤，800ms；实际应该会增加200ms
        /// </summary>
        public const int GearMinPeriodMilliseconds = 800;

        public static string[] MapAttributeNames = new[]
        {
            MapPointCodeAttributeName,
            MapPointNameAttributeName,
            MapRoadCountAttributeName,
            MapRoadWidthAttributeName
        };

        public const int WGS84SRID = 4326;
        public const int ExamItemBaseCode = 40000;

        #region LicenseTypes

        public const string LicenseTypeC1 = "C1";
        public const string LicenseTypeC2 = "C2";
        public const string LicenseTypeB1 = "B1";

        #endregion

        /// <summary>
        /// 信号误差范围值
        /// 在这个数值之内表明信号无效
        /// </summary>
        public const int ErrorSignalCount =5;

        /// <summary>
        /// 发动机转速要求达到多少，判断发动机为启动
        /// </summary>
        public const int EngineRpmLimit = 20;


        /// <summary>
        /// 无效的速度限制
        /// </summary>
        public const int InvalidSpeedLimit = 200;

        /// <summary>
        /// 记录上次选择地图ID的key,在setting表中
        /// </summary>
        public const string ExamMapId = "ExamMapId";


        /// <summary>
        ///地图中遇到的第一个左转项目（泸州）
        /// </summary>
        public static bool IsFirstTurnLeft = false;
        /// <summary>
        ///地图中遇到的第一个右转项目（泸州）
        /// </summary>
        public static bool IsFirstTurnRight = false;
        /// <summary>
        ///是否考试模式（泸州，判定比context更早，所以加一个标志来判定）
        /// </summary>
        public static bool IsExamMode_Luzhou = false;

        public static double VehicleStartingDistance = 0;

        public static double ChangeLaneDistance = 0;

        public static double PullOverDistance = 0;

        public static bool IsHaveRightLight = false;


        //是否是第二次掉头
        public static bool IsSecondTurnRound = false;

        public static bool IsTriggerStartExam = false;


    }
}
