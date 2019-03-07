using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alipaytransfer
{
    public class DrawBonus
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public decimal ExtractMoney { get; set; }

        public DateTime ModifyDate { get; set; }
        public string Replay { get; set; }
        /// <summary>
        /// 0等待 1成功 2 作废
        /// </summary>
        public byte IsCheck { get; set; }

    }
}
