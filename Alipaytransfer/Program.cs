using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alipaytransfer
{
    class Program
    {
        static Timer timer;
        static string serverUrl;
        static string privateKeyPem;
        static string alipayPulicKey;
        static string app_id;
        static int periodSeconds;
        static string showName;
        static void Main(string[] args)
        {

            //加载配置
            serverUrl = ConfigurationManager.AppSettings.Get("serverUrl");
            privateKeyPem = ConfigurationManager.AppSettings.Get("privateKeyPem");
            alipayPulicKey = ConfigurationManager.AppSettings.Get("alipayPulicKey");
            app_id = ConfigurationManager.AppSettings.Get("app_id");
            periodSeconds = int.Parse(ConfigurationManager.AppSettings.Get("periodSeconds"));
            showName = ConfigurationManager.AppSettings.Get("showName");

            timer = new Timer(RunTask, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(periodSeconds));
            string str = null;
            while ((str = Console.ReadLine()) != "exit") ;
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        static void RunTask(object state)
        {
            var list = DrawBonusService.GetList();
            Console.WriteLine("{1} 获取数据：{0}", list.Count, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (list.Count == 0) return;

            var aopClient = new DefaultAopClient(serverUrl, app_id, privateKeyPem, "json", "1.0", "RSA2", alipayPulicKey, "GBK", false);
            foreach (var item in list)
            {
                try
                {
                    AlipayFundTransToaccountTransferRequest request = new AlipayFundTransToaccountTransferRequest();
                    BizContentInfo bizContentInfo = new BizContentInfo();
                    bizContentInfo.out_biz_no = item.Id.ToString(); //商户流水号
                    bizContentInfo.payee_account = item.Mobile;//账号
                    bizContentInfo.amount = item.ExtractMoney;//金额 两位小数，最小0.1
                    bizContentInfo.payer_show_name = showName;
                    bizContentInfo.payee_real_name = "";
                    bizContentInfo.remark = "提现结算";
                    request.BizContent = bizContentInfo.ToString();
                    var response = aopClient.Execute(request);
                    if (response.Code == "10000") //成功了
                    {
                        item.IsCheck = 1;
                        item.Replay += string.Format(" code:{0},msg:{1}", response.Code, response.Msg);
                        Console.Write("{3} 结算成功,编号{0},账号:{1},金额:{2} ", item.Id, item.Mobile, item.ExtractMoney, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        item.IsCheck = 2;
                        item.Replay += string.Format(" code:{0},subcode:{1},submsg:{2}", response.Code, response.SubCode, response.SubMsg);
                        Console.Write("{4} 结算失败,编号:{0},账号:{1},金额:{2},错误:{3} ", item.Id, item.Mobile, item.ExtractMoney, response.SubMsg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }

                }
                catch (Exception exp)
                {
                    item.IsCheck = 2;
                    item.Replay += exp.Message;
                    Console.Write("{4} 结算异常,编号:{0},账号:{1},金额:{2},错误:{3} ", item.Id, item.Mobile, item.ExtractMoney, exp.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                var flag = DrawBonusService.UpdateStatus(item);
                Console.WriteLine(" 存储:{0}", flag);

                Thread.Sleep(300);
            }
        }
    }
}
