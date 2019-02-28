namespace TwoPole.Chameleon3.Infrastructure
{
    public enum ExamItemTriggerSource : byte
    {
        /// <summary>
        /// 地图点位
        /// </summary>
        Map,
        /// <summary>
        /// 人工操作
        /// </summary>
        Manual,
        /// <summary>
        /// 程序自动触发
        /// </summary>
        Auto,
    }
}