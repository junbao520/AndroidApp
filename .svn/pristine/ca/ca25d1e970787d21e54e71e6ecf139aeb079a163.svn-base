namespace TwoPole.Chameleon3.Infrastructure
{
    /// <summary>
    /// OBD信息
    /// </summary>
    public struct ObdInfo
    {
        /// <summary>
        /// 车速
        /// </summary>
        public double Speed { get; internal set; }

        /// <summary>
        /// 发动机转速
        /// </summary>
        public int EngineRpm { get; internal set; }
        
        public bool IsInvalidate
        {
            get
            {
                if (Speed < 0 && EngineRpm < 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}