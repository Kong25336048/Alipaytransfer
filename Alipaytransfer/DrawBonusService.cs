using Loogn.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alipaytransfer
{
    public class DrawBonusService
    {
        static int preLimit;
        static string conn;
        static DrawBonusService()
        {
            var econn = ConfigurationManager.ConnectionStrings["MsSql"].ConnectionString;

            conn = CoreHelper.EncryptHelper.DecryptString(econn, "htx12345");
            preLimit = int.Parse(ConfigurationManager.AppSettings.Get("preLimit"));
        }


        static SqlConnection Open()
        {
            return new SqlConnection(conn);
        }

        public static List<DrawBonus> GetList()
        {
            using (var db = Open())
            {
                return db.SelectFmt<DrawBonus>("select top {0} Id,Mobile, ExtractMoney,Replay from DrawBonus WHERE IsCheck=0 ORDER BY AddTime", preLimit);
            }
        }
        public static int UpdateStatus(DrawBonus m)
        {

            if (m.Replay.Length > 200)
            {
                m.Replay = m.Replay.Substring(0, 200);
            }

            m.ModifyDate = DateTime.Now;


            using (var db = Open())
            {
                return db.UpdateById<DrawBonus>(DictBuilder
                    .Assign("IsCheck", m.IsCheck)
                    .Assign("Replay", m.Replay)
                    .Assign("ModifyDate", m.ModifyDate), m.Id);
            }
        }

    }
}
