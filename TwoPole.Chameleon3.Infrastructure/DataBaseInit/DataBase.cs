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
        //TODO:���ݿ��Ǹ���-�ָ��ַ������һλ�ַ��������ġ���ͨ��������ж�̬������
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
        //TODO:��֪����������ݿ����Ǹ���
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
        //�Ƿ������޷���,�����True ���Ҫ���ά����֤�����޷�����Ȩ��ͬ����Ч
        public static bool IsLeaseScheme = true;

        public static int ByMinute = 100;
        public static DateTime ByTimes = DateTime.Now;

        public static string DefaultDataFolderName = "ykx";
        //���ֺ�����(1)���׿���(0)
        public static int ProgramType= 0;


        public static bool CanUse()
        {
            return true;
            if (ByMinute > 0)
            {
                //���򵽼���ʱ����һ������ʱ�� ��ʱ��5����
                if ((DateTime.Now - ByTimes).TotalMilliseconds <= ByMinute + 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetFolderName(string DataBaseName)
        {
            //�ļ�������
            var FolderName = DataBaseName.Split('-')[2].Substring(0, Currentversion.DataBaseName.Split('-')[2].Length - 3);
            return FolderName;
        }
        //�豸���
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
        #region ��Դ���ļ�
        //public static keyVlauePair<string,string> SimulationDataKevVale
        //public static KeyValuePair<string, string> SimulationDataKeyValue = new KeyValuePair<string, string>("Simulation.ini",Sqlr);
        // public static KeyValuePair<string, string> BreakeVoiceKeyValue = new KeyValuePair<string, string>("breake.wav", BreakeVoice);
        #endregion
        
        public static void SaveVersionInfo(VersionInfo versionInfo)
        {
            //�ѵ�ǰ�İ汾�����ھ�̬������
            Currentversion = versionInfo;
            //�ɶ���ȪUSB170612:
            //TODO;����д��չ����ѽ ֱ�Ӷ�ö��ע��

            VersionNumber = versionInfo.masterBoxVersion.GetDescription();

            //switch (versionInfo.masterBoxVersion)
            //{
            //    case MasterControlBoxVersion.SimulatedData:
            //        VersionNumber = "����";
            //        break;
            //    case MasterControlBoxVersion.WifiTcp:
            //        VersionNumber = "WIFI";
            //        break;
            //    case MasterControlBoxVersion.WifiUdp:
            //        VersionNumber = "WIFI";
            //        break;
            //    case MasterControlBoxVersion.Bluetooth:
            //        VersionNumber = "����";
            //        break;
            //    case MasterControlBoxVersion.USB:
            //        VersionNumber = "USB";
            //        break;
            //    case MasterControlBoxVersion.Serial:
            //        VersionNumber = "����";
            //        break;
            //    default:
            //        break;
            //}
            //TODO:����������һ���ֵ����ά���� Ȼ����Ling��ѯ�Ϳ����� û�б�Ҫд��ô��
            //TODO:���� ֱ��ʹ�������lstUIIfo
            //TODO:Ϊ�β��Ż�����ʵ�����Ż���ѽ��̫��If else if ����ͷ��  ��������Ż���  ���� 2018-09-03

            //��ѯ�汾��
           // versionInfo = lstVersionInfo.Select(s => s.DataBaseName == versionInfo.DataBaseName).FirstOrDefault();

            if (versionInfo.DataBaseName.Contains(FuLing))
            {
                VersionNumber = VersionNumber + "���츢��";
            }
            else if (versionInfo.DataBaseName.Contains(LongQuan))
            {
                VersionNumber = VersionNumber + "�ɶ���Ȫ";
            }
            else if (versionInfo.DataBaseName.Contains(JingTang))
            {
                VersionNumber = VersionNumber + "�ɶ�����";
            }
            else if (versionInfo.DataBaseName.Contains(QiQiHaEr))
            {
                VersionNumber = VersionNumber + "�������������";
            }
            else if (versionInfo.DataBaseName.Contains(RongChang))
            {
                VersionNumber = VersionNumber + "�����ٲ�";
            }
            else if (versionInfo.DataBaseName.Contains(WanZhou))
            {
                VersionNumber = VersionNumber + "��������";
            }
            else if (versionInfo.DataBaseName.Contains(FengDu))
            {
                VersionNumber = VersionNumber + "����ᶼ";
            }
            else if (versionInfo.DataBaseName.Contains(SanYa))
            {
                VersionNumber = VersionNumber + "��������";
            }
            else if (versionInfo.DataBaseName.Contains(QianJiang))
            {
                VersionNumber = VersionNumber + "����ǭ��";
            }
            else if (versionInfo.DataBaseName.Contains(MianYang))
            {
                VersionNumber = VersionNumber + "�Ĵ�����";
            }
            else if (versionInfo.DataBaseName.Contains(XuZhou))
            {
                VersionNumber = VersionNumber + "��������";
            }
            else if (versionInfo.DataBaseName.Contains(HuaiAn))
            {
                VersionNumber = VersionNumber + "���ջ���";
            }
            else if (versionInfo.DataBaseName.Contains(GuangXi))
            {
                VersionNumber = VersionNumber + "����";
            }
            else if (versionInfo.DataBaseName.Contains(ZunYi))
            {
                VersionNumber = VersionNumber + "��������";
            }
            else if (versionInfo.DataBaseName.Contains(HaiKou))
            {
                VersionNumber = VersionNumber + "���Ϻ���";
            }
            else if (versionInfo.DataBaseName.Contains(DongFang))
            {
                VersionNumber = VersionNumber + "���϶���";
            }
            else if (versionInfo.DataBaseName.Contains(QiongBei))
            {
                VersionNumber = VersionNumber + "������";
            }
            else if (versionInfo.DataBaseName.Contains(DuoLun))
            {
                VersionNumber = VersionNumber + "�ɶ�����";
            }
            else if (versionInfo.DataBaseName.Contains(HuaZhong))
            {
                VersionNumber = VersionNumber + "�ɶ�����";
            }
            else if (versionInfo.DataBaseName.Contains(JingYing))
            {
                VersionNumber = VersionNumber + "�ɶ���Ӣ";
            }
            else if (versionInfo.DataBaseName.Contains(BeiYong))
            {
                VersionNumber = VersionNumber + "�ɶ�����";
            }
            else if (versionInfo.DataBaseName.Contains(LuXian))
            {
                VersionNumber = VersionNumber + "�Ĵ�����";
            }
            else if (versionInfo.DataBaseName.Contains(LuZhou))
            {
                VersionNumber = VersionNumber + "�Ĵ�����";
            }
            else if (versionInfo.DataBaseName.Contains(Foshan))
            {
                VersionNumber = VersionNumber + "�㶫��ɽ";
            }
            else if (versionInfo.DataBaseName.Contains(QingBaiJiang))
            {
                VersionNumber = VersionNumber + "��׽�";
            }
            else if (versionInfo.DataBaseName.Contains(YunFu))
            {
                VersionNumber = VersionNumber + "�㶫�Ƹ�";
            }
            else if (versionInfo.DataBaseName.Contains(GuLin))
            {
                VersionNumber = VersionNumber += "�Ĵ�����";
            }
        }

        //TODO����Щ������ֱ�ӱ���ö�ٵõ���
        public static List<KeyValuePair<string, string>> lstUIInfo = new List<KeyValuePair<string, string>>(){
            new KeyValuePair<string, string>("���׾����","DuoLun"),
            new KeyValuePair<string, string>("����","DuoLunSensor"),
            new KeyValuePair<string, string>("������","DuoLunNew"),
            new KeyValuePair<string, string>("���ݻ���","HuaZhongLuZhou"),
            new KeyValuePair<string, string>("����","HuaZhong"),
            new KeyValuePair<string, string>("����","SanLian"),
            new KeyValuePair<string, string>("̩��","TaiPu"),
            new KeyValuePair<string, string>("�����ֻ���","DuoLunMobilePhone"),
            new KeyValuePair<string, string>("�����ֻ���","SanLianMobilePhone"),
            new KeyValuePair<string, string>("��Ӣ","JingYing"),
            new KeyValuePair<string, string>("����","BeiKe"),
            new KeyValuePair<string, string>("����","LongChuang"),
            new KeyValuePair<string, string>("�Ʒ���","KeFeiTe"),
            new KeyValuePair<string, string>("�����㶫","SanLianGuangDong"),
        };
        //TODO����Щ������ֱ�ӱ���ö�ٵõ���
        public static List<KeyValuePair<string, string>> lstSensorInfo = new List<KeyValuePair<string, string>>(){
            new KeyValuePair<string, string>("USB","USB"),
            new KeyValuePair<string, string>("Wifi","WifiUdp"),
            new KeyValuePair<string, string>("����","Bluetooth"),
            new KeyValuePair<string, string>("ģ������","SimulatedData"),
            new KeyValuePair<string, string>("����","Serial")

        };
        public static List<VersionInfo> lstVersionInfo = new List<VersionInfo> {
            new VersionInfo {VersionName="����",DataBaseName=FuLing,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=FuLing,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="��Ȫ",DataBaseName=LongQuan,UIType=SystemUIType.DuoLunSensor,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=JingTang,UIType=SystemUIType.JingTang,IsShowAds=true },
            new VersionInfo {VersionName="�������",DataBaseName=QiQiHaEr,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="�ٲ�",DataBaseName=RongChang,UIType=SystemUIType.DuoLunSensor,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=WanZhou,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="�ᶼ",DataBaseName=FengDu,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=SanYa,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="ǭ��",DataBaseName=QianJiang,UIType=SystemUIType.TaiPu,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=MianYang,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=XuZhou,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=HuaiAn,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=GuangXi,UIType=SystemUIType.DuoLun,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=ZunYi,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=HaiKou,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="��",DataBaseName=QiongBei,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=DongFang,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=LuXian,UIType=SystemUIType.JingYing,IsShowAds=true },
            new VersionInfo {VersionName="����",DataBaseName=LuZhou,UIType=SystemUIType.LongChuang,IsShowAds=true },
            new VersionInfo {VersionName="�㶫��ɽ",DataBaseName=Foshan,UIType=SystemUIType.SanLian,IsShowAds=true },
            new VersionInfo{VersionName="��׽�",DataBaseName=QingBaiJiang,UIType=SystemUIType.KeFeiTe,IsShowAds=true },
            new VersionInfo{VersionName="�㶫�Ƹ�",DataBaseName=YunFu,UIType=SystemUIType.SanLianGuangDong,IsShowAds=true },
            new VersionInfo{VersionName="�Ĵ�����",DataBaseName=GuLin,UIType=SystemUIType.HuaZhongLuZhou,IsShowAds=true },
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

             Build.User; //13 λ

            //�ڽ���MD5����
            return md5(md5(m_szDevIDShort));

        }
    }

    public class VersionInfo
    {
        //�汾����������---//----
        public string VersionName;
        //���ݿ�����
        private string dataBaseName;

        //�ļ�����Ҳ�ͺ����ݿ�������дһ����
        public string FolderName;
        //Ĭ�ϵĽ���
        public SystemUIType UIType;
        //�Ƿ���ʾ���
        public bool IsShowAds;
        //�������ź���Դ
        public MasterControlBoxVersion masterBoxVersion;
        //�Ƿ񲥷Ų�������
        public bool IsPlayActionVoice;
        //�Ƿ��ֹ�汾�л�
        public bool IsForbiddenVersionChange;
        //�����ʾʱ�� ��λ����
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