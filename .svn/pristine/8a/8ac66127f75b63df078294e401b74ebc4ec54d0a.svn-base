using System.ComponentModel;

namespace TwoPole.Chameleon3.Infrastructure
{
    public enum ExamMode : byte
    {
        [Description("未初始化")]
        None = 0,
        [Description("考试模式")]
        Examming = 1,
        [Description("训练模式")]
        Training = 2,
    }

    [Description("考试模式")]
    public enum ExamTimeMode : byte
    {
        //[Description("未初始化")]
        //None = 0,
        [Description("白天")]
        Day = 1,
        [Description("夜间")]
        Night = 2,
    }

    [Description("结束标志")]
    public enum PullOverEndMark : byte
    {
        [Description("无")]
        None = 0,
        [Description("警告灯检测")]
        CautionLightCheck = 1,
        [Description("近光灯检测")]
        LowBeamCheck = 2,
        [Description("发动机熄火检测")]
        EngineExtinctionCheck = 3,
        [Description("安全带检测")]
        SafetyBeltCheck = 4,
        [Description("开关车门检测")]
        OpenCloseDoorCheck = 5,
        [Description("拉紧驻车制动")]
        Handbrake = 6,
        [Description("开车门结束")]
        OpenDoorCheck =7,
    }

    [Description("停车标志")]
    public enum PullOverMark : byte
    {
        [Description("无")]
        None = 0,
        [Description("车停")]
        CarStop = 1,
        [Description("手刹")]
        Handbrake = 2
    }
}
