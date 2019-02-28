using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TwoPole.Chameleon3;
using Java.Security;

namespace TwoPole.Chameleon3.Infrastructure.Services
{
    public class DataBase
    {
        //TODO:数据库是根据-分割字符串最后一位字符串命名的。是通过反射进行动态创建的
        public static string FuLing = "content-chongqin-fuling.db";
        public static string LongQuan = "content-chengdu-longquan.db";
        public static string JingTang = "content-chengdu-jingtang.db";
        public static string QiQiHaEr = "content-heilongjiang-qiqihaer.db";
        public static string RongChang = "content-chongqin-rongchang.db";
        public static string WanZhou = "content-chongqin-wanzhou.db";
        public static string FengDu = "content-chongqin-fengdu.db";

        public static string QianJiang = "content-chongqin-qianjiang.db";
        public static string MianYang = "content-sichuan-mianyang.db";
        public static string XuZhou = "content-jiangsu-xuzhou.db";
        public static string HuaiAn = "content-jiangsu-huaian.db";
        public static string GuangXi = "content-guangxi-guangxi.db";
        public static string ZunYi = "content-guizhou-zunyi.db";

        public static string SanYa = "content-hainan-sanya.db";
        public static string QiongBei = "content-hainan-qiongbei.db";
        public static string HaiKou = "content-hainan-haikou.db";
        public static string DongFang = "content-hainan-dongfang.db";

        public static string DuoLun = "content-chengdu-duolun.db";
        public static string HuaZhong = "content-chengdu-huazhong.db";
        public static string JingYing = "content-chengdu-jingying.db";
        public static string BeiYong = "content-chengdu-beiyong.db";
        public static string LuXian = "content-sichuan-luxian.db";
        public static string LuZhou = "content-sichuan-luzhou.db";
        //TODO:不知道你这个数据库是那个？
        public static string Foshan = "content-guangdong-foshan.db";
        public static string QingBaiJiang = "content-chengdu-qingbaijiang.db";
        public static string YunFu = "content-guangdong-yunfu.db";

        public static string GuLin = "content-sichuan-gulin.db";

        public static MasterControlBoxVersion Version = MasterControlBoxVersion.USB;
        public static SystemUIType SystemType = SystemUIType.SanLian;
        public static string VersionNumber = string.Empty;
        public static VersionInfo Currentversion;
        public static string APPID = "5a09389a548b7a32a400016c";
        public static string Token = "63e5feeec1c40892fd3db309596aa71d";
        //是否是租赁方案,如果是True 则会要求二维码验证，租赁方案授权码同样生效
        public static bool IsLeaseScheme = true;

        public static int ByMinute = 100;
        public static DateTime ByTimes = DateTime.Now;

        public static string DefaultDataFolderName = "ykx";
        //区分海振兴(1)和易考星(0)
        public static int ProgramType= 0;


        public static bool CanUse()
        {
            return true;
            if (ByMinute > 0)
            {
                //购买到计算时间有一个冗余时间 暂时给5分钟
                if ((DateTime.Now - ByTimes).TotalMilliseconds <= ByMinute + 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetFolderName(string DataBaseName)
        {
            //文件夹名词
            var FolderName = DataBaseName.Split('-')[2].Substring(0, Currentversion.DataBaseName.Split('-')[2].Length - 3);
            return FolderName;
        }
        //设备编号
        public static string EquipmentNumber
        {
            get
            {
                return md5(getUniquePsuedoID());
            }
        }


        public static string GetUpdateApiUrl()
        {
            string url = string.Format("http://api.fir.im/apps/latest/{0}?api_token={1}", APPID, Token);
            return url;
        }
        #region 资源文文件
        //public static keyVlauePair<string,string> SimulationDataKevVale
        //public static KeyValuePair<string, string> SimulationDataKeyValue = new KeyValuePair<string, string>("Simulation.ini",Sqlr);
        // public static KeyValuePair<string, string> BreakeVoiceKeyValue = new KeyValuePair<string, string>("breake.wav", BreakeVoice);
        #endregion
        
        public static void SaveVersionInfo(VersionInfo versionInfo)
        {
            //把当前的版本保存在静态变量中
            Currentversion = versionInfo;
            //成都龙泉USB170612:
            //TODO;可以写扩展方法呀 直接读枚举注释

            VersionNumber = versionInfo.masterBoxVersion.GetDescription();

            //switch (versionInfo.masterBoxVersion)
            //{
            //    case MasterControlBoxVersion.SimulatedData:
            //        VersionNumber = "测试";
            //        break;
            //    case MasterControlBoxVersion.WifiTcp:
            //        VersionNumber = "WIFI";
            //        break;
            //    case MasterControlBoxVersion.WifiUdp:
            //        VersionNumber = "WIFI";
            //        break;
            //    case MasterControlBoxVersion.Bluetooth:
            //        VersionNumber = "蓝牙";
            //        break;
            //    case MasterControlBoxVersion.USB:
            //        VersionNumber = "USB";
            //        break;
            //    case MasterControlBoxVersion.Serial:
            //        VersionNumber = "串口";
            //        break;
            //    default:
            //        break;
            //}
            //TODO:这点可以做成一个字典可以维护的 然后用Ling查询就可以了 没有必要写这么长
            //TODO:可以 直接使用下面的lstUIIfo
            //TODO:为何不优化？这实可以优化的呀，太多If else if 看着头疼  亮哥等你优化下  鲍君 2018-09-03

            //查询版本号
           // versionInfo = lstVersionInfo.Select(s => s.DataBaseName == versionInfo.DataBaseName).FirstOrDefault();

            if (versionInfo.DataBaseName.Contains(FuLing))
            {
                VersionNumber = VersionNumber + "重庆涪陵";
            }
            else if (versionInfo.DataBaseName.Contains(LongQuan))
            {
                VersionNumber = VersionNumber + "成都龙泉";
            }
            else if (versionInfo.DataBaseName.Contains(JingTang))
            {
                VersionNumber = VersionNumber + "成都金堂";
            }
            else if (versionInfo.DataBaseName.Contains(QiQiHaEr))
            {
                VersionNumber = VersionNumber + "黑龙江齐齐哈尔";
            }
            else if (versionInfo.DataBaseName.Contains(RongChang))
            {
                VersionNumber = VersionNumber + "重庆荣昌";
            }
            else if (versionInfo.DataBaseName.Contains(WanZhou))
            {
                VersionNumber = VersionNumber + "重庆万州";
            }
            else if (versionInfo.DataBaseName.Contains(FengDu))
            {
                VersionNumber = VersionNumber + "重庆丰都";
            }
            else if (versionInfo.DataBaseName.Contains(SanYa))
            {
                VersionNumber = VersionNumber + "海南三亚";
            }
            else if (versionInfo.DataBaseName.Contains(QianJiang))
            {
                VersionNumber = VersionNumber + "重庆黔江";
            }
            else if (versionInfo.DataBaseName.Contains(MianYang))
            {
                VersionNumber = VersionNumber + "四川绵阳";
            }
            else if (versionInfo.DataBaseName.Contains(XuZhou))
            {
                VersionNumber = VersionNumber + "江苏徐州";
            }
            else if (versionInfo.DataBaseName.Contains(HuaiAn))
            {
                VersionNumber = VersionNumber + "江苏淮安";
            }
            else if (versionInfo.DataBaseName.Contains(GuangXi))
            {
                VersionNumber = VersionNumber + "广西";
            }
            else if (versionInfo.DataBaseName.Contains(ZunYi))
            {
                VersionNumber = VersionNumber + "贵州遵义";
            }
            else if (versionInfo.DataBaseName.Contains(HaiKou))
            {
                VersionNumber = VersionNumber + "海南海口";
            }
            else if (versionInfo.DataBaseName.Contains(DongFang))
            {
                VersionNumber = VersionNumber + "海南东方";
            }
            else if (versionInfo.DataBaseName.Contains(QiongBei))
            {
                VersionNumber = VersionNumber + "海南琼北";
            }
            else if (versionInfo.DataBaseName.Contains(DuoLun))
            {
                VersionNumber = VersionNumber + "成都多伦";
            }
            else if (versionInfo.DataBaseName.Contains(HuaZhong))
            {
                VersionNumber = VersionNumber + "成都华众";
            }
            else if (versionInfo.DataBaseName.Contains(JingYing))
            {
                VersionNumber = VersionNumber + "成都精英";
            }
            else if (versionInfo.DataBaseName.Contains(BeiYong))
            {
                VersionNumber = VersionNumber + "成都备用";
            }
            else if (versionInfo.DataBaseName.Contains(LuXian))
            {
                VersionNumber = VersionNumber + "四川泸县";
            }
            else if (versionInfo.DataBaseName.Contains(LuZhou))
            {
                VersionNumber = VersionNumber + "四川泸州";
            }
            else if (versionInfo.DataBaseName.Contains(Foshan))
            {
                VersionNumber = VersionNumber + "广东佛山";
            }
            else if (versionInfo.DataBaseName.Contains(QingBaiJiang))
            {
                VersionNumber = VersionNumber + "青白江";
            }
            else if (versionInfo.DataBaseName.Contains(YunFu))
            {
                VersionNumber = VersionNumber + "广东云浮";
            }
            else if (versionInfo.DataBaseName.Contains(GuLin))
            {
                VersionNumber = VersionNumber += "四川古蔺";
            }
        }

        //TODO：这些都可以直接遍历枚举得到的
        public static List<KeyValuePair<string, string>> lstUIInfo = new List<KeyValuePair<string, string>>(){
            new KeyValuePair<string, string>("多伦精简版","DuoLun"),
            new KeyValuePair<string, string>("多伦","DuoLunSensor"),
            new KeyValuePair<string, string>("多轮新","DuoLunNew"),
            new KeyValuePair<string, string>("泸州华众","HuaZhongLuZhou"),
            new KeyValuePair<string, string>("华众","HuaZhong"),
            new KeyValuePair<string, string>("三联","SanLian"),
            new KeyValuePair<string, string>("泰普","TaiPu"),
            new KeyValuePair<string, string>("多伦手机版","DuoLunMobilePhone"),
            new KeyValuePair<string, string>("三联手机版","SanLianMobilePhone"),
            new KeyValuePair<string, string>("精英","JingYing"),
            new KeyValuePair<string, string>("北科","BeiKe"),
            new KeyValuePair<string, string>("龙创","LongChuang"),
            new KeyValuePair<string, string>("科飞特","KeFeiTe"),
            new KeyValuePair<string, string>("三联广东","SanLianGuangDong"),
        };
        //TODO：这些都可以直接遍历枚举得到的
        public static List<KeyValuePair<string, string>> lstSensorInfo = new List<KeyValuePair<string, string>>(){
            new KeyValuePair<string, string>("USB","USB"),
            new KeyValuePair<string, string>("Wifi","WifiUdp"),
            new KeyValuePair<string, string>("蓝牙","Bluetooth"),
            new KeyValuePair<string, string>("模拟数据","SimulatedData"),
            new KeyValuePair<string, string>("串口","Serial")

        };
        public static List<VersionInfo> lstVersionInfo = new List<VersionInfo> {
            new VersionInfo {VersionName="涪陵",DataBaseName=FuLing,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="涪陵",DataBaseName=FuLing,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="龙泉",DataBaseName=LongQuan,UIType=SystemUIType.DuoLunSensor,IsShowAds=true },
            new VersionInfo {VersionName="金堂",DataBaseName=JingTang,UIType=SystemUIType.JingTang,IsShowAds=true },
            new VersionInfo {VersionName="齐齐哈尔",DataBaseName=QiQiHaEr,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="荣昌",DataBaseName=RongChang,UIType=SystemUIType.DuoLunSensor,IsShowAds=true },
            new VersionInfo {VersionName="万州",DataBaseName=WanZhou,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="丰都",DataBaseName=FengDu,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="三亚",DataBaseName=SanYa,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="黔江",DataBaseName=QianJiang,UIType=SystemUIType.TaiPu,IsShowAds=true },
            new VersionInfo {VersionName="绵阳",DataBaseName=MianYang,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="徐州",DataBaseName=XuZhou,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="淮安",DataBaseName=HuaiAn,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="广西",DataBaseName=GuangXi,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="遵义",DataBaseName=ZunYi,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="海口",DataBaseName=HaiKou,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="琼北",DataBaseName=QiongBei,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="东方",DataBaseName=DongFang,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="泸县",DataBaseName=LuXian,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="泸州",DataBaseName=LuZhou,UIType=SystemUIType.LongChuang,IsShowAds=true },
            new VersionInfo {VersionName="广东佛山",DataBaseName=Foshan,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo{VersionName="清白江",DataBaseName=QingBaiJiang,UIType=SystemUIType.KeFeiTe,IsShowAds=true },
            new VersionInfo{VersionName="广东云浮",DataBaseName=YunFu,UIType=SystemUIType.SanLianGuangDong,IsShowAds=true },
            new VersionInfo{VersionName="四川古蔺",DataBaseName=GuLin,UIType=SystemUIType.HuaZhongLuZhou,IsShowAds=true },
        };

        public static VersionInfo GetVersionInfo(string Info = "")
        {
            // string Info = string.Format("{0};{1};{2}", DataBaseName, UIType, IsShowAds);
            try
            {
                if (string.IsNullOrEmpty(Info))
                {
                    return null;
                }
                string DataBaseName = Info.Split(';')[0];
                string UIType = Info.Split(';')[1];
                string IsShowAds = Info.Split(';')[2];
                string masterBoxVersion = Info.Split(';')[3];
                string IsPlayActionVoice = Info.Split(';')[4];
                VersionInfo version = new VersionInfo();
                version.DataBaseName = DataBaseName;
                version.UIType = (SystemUIType)Enum.Parse(typeof(SystemUIType), UIType);
                version.masterBoxVersion = (MasterControlBoxVersion)Enum.Parse(typeof(MasterControlBoxVersion), masterBoxVersion);
                version.IsShowAds = IsShowAds == "True" ? true : false;
                version.IsPlayActionVoice = IsPlayActionVoice == "True" ? true : false;
                return version;
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog("GetVersionInfoEx:" + ex.Message);
                return null;
            }

        }


        public static string md5(string str)
        {

            MessageDigest md5 = null;
            try
            {
                md5 = MessageDigest.GetInstance("MD5");
                byte[] bytes = md5.Digest(System.Text.Encoding.Default.GetBytes(str));
                string result = "";
                foreach (byte b in bytes)
                {
                    string temp = Java.Lang.Integer.ToHexString(b & 0xff);
                    if (temp.Length == 1)
                    {
                        temp = "0" + temp;
                    }
                    result += temp;
                }
                return result;
            }
            catch (NoSuchAlgorithmException e)
            {
            }
            return "";
        }
        public static string getUniquePsuedoID()
        {


            string m_szDevIDShort =
             Build.Board + Build.Brand +

             Build.CpuAbi + Build.Device +

             Build.Display + Build.Host +

             Build.Id + Build.Manufacturer +

             Build.Model + Build.Product +

             Build.Tags + Build.Type +

             Build.User; //13 位

            //在进行MD5加密
            return md5(md5(m_szDevIDShort));

        }
    }

    public class VersionInfo
    {
        //版本的中文名称---//----
        public string VersionName;
        //数据库名字
        private string dataBaseName;

        //文件名称也就和数据库名称缩写一样的
        public string FolderName;
        //默认的界面
        public SystemUIType UIType;
        //是否显示广告
        public bool IsShowAds;
        //主控箱信号来源
        public MasterControlBoxVersion masterBoxVersion;
        //是否播放操作语音
        public bool IsPlayActionVoice;
        //是否禁止版本切换
        public bool IsForbiddenVersionChange;
        //广告显示时间 单位毫秒
        public int AdsShowTime = 3000;

        public string WelcomeVoice = string.Empty;

        public bool IsPlayBackgroundMusic = true;

        public bool IsCreateFile = true;


        public bool IsUseIoc = false;

        public bool IsUserAop = false;

   




        public string DataBaseName
        {
            get
            {
                return dataBaseName;
            }
            set
            {
                if (value.Contains(Android.OS.Environment.ExternalStorageDirectory.ToString()))
                {
                    dataBaseName = value;
                }
                else
                {
                    //content-chongqin-fuling.db
                    //LogManager.WriteSystemLog(value);
                    var dbname = value.Split('-')[2].Substring(0, value.Split('-')[2].Length - 3);
                    this.FolderName = dbname;
                    dataBaseName = Android.OS.Environment.ExternalStorageDirectory + "/" + dbname + "/" + value;
                }
                //else
                //{
                //    //luzhou/
                //    dataBaseName = Android.OS.Environment.ExternalStorageDirectory + "/" + value;
                //}
            

            }
        }
    }
}