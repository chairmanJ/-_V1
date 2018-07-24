using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace 二进四出_V1.channel
{
    [Serializable]
    public enum _phase
    {
        Phase,
        UnPhase
    }
    /// <summary>
    /// 相位
    /// </summary>
    [Serializable]
    public class _ephase
    {
        _phase phase = _phase.UnPhase;

        public _phase getPhase()
        {
            return this.phase;
        }
        public int setPhase(_phase phase)
        {
            int ret = 0;
            this.phase = phase;
            return ret;
        }
        public _ephase()
        {
            phase = _phase.UnPhase;
        }
    }
}
