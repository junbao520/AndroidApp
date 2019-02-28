using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Util;
using System.Collections.Generic;
using Java.Util;
using Android.Speech;
using Android.Speech.Tts;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;
using System.Threading;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Devices.CarSignal;
using TwoPole.Chameleon3.Infrastructure.Instance;
using CN.Wch.Ch34xuartdriver;
using Android.Hardware.Usb;
using System.IO;
using TwoPole.Chameleon3.Infrastructure.Services;
using Org.Apache.Http.Client.Methods;
using System.Net.Http;
using Org.Apache.Http.Impl.Client;
using Org.Apache.Http;
using System.Net;
using System.Text;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Java.Net;
using System.Diagnostics;
namespace TwoPole.Chameleon3
{
    //[Activity(Label = "迈使惊", MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainActivityYunFu : Activity
    {
        IMessenger messager;
        ISpeaker speaker;
        ImageView ImageAds;
        Handler InitSystemHandler;
        VersionInfo Currentversion;

        static string DBNAME = Android.OS.Environment.ExternalStorageDirectory + "/" + DataBase.RongChang;
        VersionInfo DefaultVersion = new VersionInfo
        {
            //主控箱版本 USB
            masterBoxVersion = MasterControlBoxVersion.WifiUdp,
            //考试界面 TODO:感觉UI问题太多 可以考虑完全重构下UI，1.不能很好的自适应 2.重复性代码比较多 3.比较杂乱 
            //todo:客户需求一台车装多个软件 软件机制要稍微修改下 所有的数据文件或者日志文件都存放在自己的目录文件中不在散放在根目录中
            //todo:有空时可以研究下使用Aop进行日志拦截以及性能拦截 
            UIType = SystemUIType.SanLianGuangDong,

            DataBaseName = DataBase.YunFu,
            //是否显示广告
            IsShowAds = false,
            //是否播放操作语音
            IsPlayActionVoice = true,
            //是否禁止版本切换 
            IsForbiddenVersionChange = false,
            //是否播放背景音乐
            IsPlayBackgroundMusic = true,
            //广告显示时间
            AdsShowTime = 3000,
            //是否创建文件
            IsCreateFile = true,
            //TODO:是否使用IOC 框架 2018.3.23 鲍君
            IsUseIoc = true,
            //是否使用AOP进行日志拦截 还未开发 2018.9.14
            IsUserAop = false,
            //广告ShowTime
            WelcomeVoice = "欢迎使用易考星"
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainFuYun);
            InitControl();
            try
            {
                InitVersionInfo();
                CreateDataBase(Currentversion.DataBaseName);
                if (Currentversion == null)
                {
                    LogManager.WriteSpeechInfo("Currentversion==null");
                }
                Bootstrapper.InitializeServices(this, Currentversion.DataBaseName, Currentversion.IsPlayActionVoice, DefaultVersion.IsUseIoc);
                InitSystemHandler = new Handler();
                InitSystemHandler.PostDelayed(InitSystem, 10);
                Task.Run(() =>
                {
                    Singleton.GetDataService.Cache();
                    if (Currentversion.IsCreateFile)
                    {


                        CreateFile(SqlDataRepertory.SimulationDataFileName, SqlDataRepertory.SimulationData);
                        CreateFile(SqlDataRepertory.loopMusicFileName, SqlDataRepertory.loopMusic);
                        //TODO:海河版本的刹车语音有一点一样
                        //TODO:如果觉得不合适请自行替换
                        
                        if (DataBase.VersionNumber.Contains("海南"))
                        {
                            CreateFile(SqlDataRepertory.BreakeVocieFileName, SqlDataRepertory.BreakeVoiceHaiHe);
                        }
                        else
                        {
                            //todo:这个创建的文件在有些中控上面显示文件损坏无法播放 有时间还要多测试
                            CreateFile(SqlDataRepertory.BreakeVocieFileName, SqlDataRepertory.BreakeVoice);
                        }
                        //泸县三模“冬冬”声
                        if (DataBase.VersionNumber.Contains("泸县"))
                            CreateFile(SqlDataRepertory.DongFileName, SqlDataRepertory.DongVoice);
                    }
                    //进行背景图片的设置
                    //TODO:可能存在Bug
                    //ImageView imageView = new ImageView(this);
                    ////这个点没有办法获取数据库设置
                    //if (Currentversion.IsShowAds)
                    //{
                    //    //背景图显示公司Logo
                    //    imageView.SetBackgroundResource(Resource.Drawable.voice_main_bg_new);
                    //}
                    //else
                    //{
                    //    //背景图不显示公司Logo
                    //    imageView.SetBackgroundResource(Resource.Drawable.bg);
                    //}

                });
            }
            catch (Exception ex)
            {
                //异常信息尽量记录详细一些
                LogManager.WriteSystemLog("CreateDataBaseOrInitSystemError:" + ex.InnerException.Message + ex.Source + ex.StackTrace + ex.Message);
            }



            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            // Create your application here
        }
        public void CreateFile(string FileName, string FileContent)
        {

            //如果不包含
            if (!FileName.Contains(Android.OS.Environment.ExternalStorageDirectory.ToString()))
            {
                //不包含
                var FolderName = DataBase.GetFolderName(Currentversion.DataBaseName);

                //文件生成到指定文件夹里面
                FileName = Android.OS.Environment.ExternalStorageDirectory + "/" + FolderName + "/" + FileName;
            }
            if (System.IO.File.Exists(FileName))
            {
                return;
            }
            else
            {
                var databytes = Convert.FromBase64String(FileContent);
                System.IO.File.WriteAllBytes(FileName, databytes);
            }
        }
        public void InitControl()
        {
            var mgViewStartExam = (ImageView)FindViewById(Resource.Id.mgViewStartExam);
            var mgViewMap = (ImageView)FindViewById(Resource.Id.mgViewMap);
            var mgViewRuleModified = (ImageView)FindViewById(Resource.Id.mgViewRuleModified);
            var mgViewSetting = (ImageView)FindViewById(Resource.Id.mgViewSetting);

            mgViewStartExam.Click += MgViewStartExam_Click;
            mgViewMap.Click += MgViewMap_Click;
            mgViewRuleModified.Click += MgViewRuleModified_Click;
            mgViewSetting.Click += MgViewSetting_Click;

        }

        private void MgViewSetting_Click(object sender, EventArgs e)
        {
            //PlayActionText(sender);
            //广州云浮特殊版本 不可以选择考试界面
            Intent intent = new Intent();
            intent.SetClass(this, typeof(SanLianYunFuSetting));
            StartActivity(intent);
        }

        private Type GetActivityType()
        {
            var ActivityType = typeof(DuoLunSensor);
            switch (Currentversion.UIType)
            {
                case SystemUIType.DuoLun:
                    ActivityType = typeof(DuoLunZhongKongNew);
                    break;
                case SystemUIType.DuoLunSensor:
                    ActivityType = typeof(DuoLunSensor);
                    break;
                case SystemUIType.DuoLunJieDa:
                    ActivityType = typeof(DuoLunJieDa);
                    break;
                case SystemUIType.SanLian:
                    ActivityType = typeof(SanLian);
                    break;
                case SystemUIType.JingTang:
                    ActivityType = typeof(JingTang);
                    break;
                case SystemUIType.JingTangJieDa:
                    ActivityType = typeof(JingTangJieDa);
                    break;
                case SystemUIType.HuaZhong:
                    ActivityType = typeof(HuaZhong);
                    break;
                case SystemUIType.HuaZhongSmall:
                    ActivityType = typeof(HuaZhongSmall);
                    break;
                case SystemUIType.DuoLunMobilePhone:
                    ActivityType = typeof(DuoLunMobilePhone);
                    break;
                case SystemUIType.TaiPu:
                    ActivityType = typeof(TaiPu);
                    break;
                case SystemUIType.SanLianMobilePhone:
                    ActivityType = typeof(SanLianMobilePhone);
                    break;
                case SystemUIType.JingYing:
                    ActivityType = typeof(JingYing);
                    break;
                case SystemUIType.BeiKe:
                    ActivityType = typeof(BeiKe);
                    break;
                case SystemUIType.DuoLunNew:
                    ActivityType = typeof(DuoLunNew);
                    break;
                case SystemUIType.LongChuang:
                    ActivityType = typeof(LongChuang);
                    break;
                case SystemUIType.KeFeiTe:
                    ActivityType = typeof(KeiFeiTe);
                    break;
                case SystemUIType.SanLianGuangDong:
                    ActivityType = typeof(SanLianYunFu);
                    break;
                default:
                    ActivityType = typeof(DuoLunSensor);
                    break;
            }
            return ActivityType;
        }

        private void MgViewRuleModified_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(ParameterSetting));
            StartActivity(intent);
        }

        private void MgViewMap_Click(object sender, EventArgs e)
        {

            Intent intent = new Intent();
            intent.SetClass(this, typeof(Road_List));
            StartActivity(intent);
        }

        private void MgViewStartExam_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(SanLianYunFu));
            intent.PutExtra("ExamMode", "Exam");
            StartActivity(intent);
        }

        public void InitSystem()
        {
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            if (Currentversion.IsPlayBackgroundMusic)
            {
                speaker.InitLoopPlyaMediaTimer();
            }

            //TODO:这段是自己通过分辨率判断手动选择界面，建议通过layout多个分辨率实现,如果某个版本出现显示Bug可以考虑打开屏蔽
            //TODO:编写界面的时候最好是使用百分比布局。我们界面比较简单基本能够适应大多数分辨率
            // GetSystemUITypeByScreenDisplayMetrics();
            //TODO;考虑到代码的美观，可以考虑把这段代码移到Bootstrapper 中InitializeSignalAsyncUSBSerial 方法中
            if (Currentversion.masterBoxVersion == MasterControlBoxVersion.USB)
            {
                //TODO:其实简单的判断就可以了 没有必要用IsOpen和OpenSerial全局变量
                //TODO:经过实际测试，串口有问题，重复打开多次也是没有用的
                //while (!isOpen && OpenSerialCount < 1)
                //{
                //    isOpen = OpenSerial();
                //    OpenSerialCount++;
                //}
                //if (!isOpen)
                //{
                //    Toast.MakeText(this, "串口初始化失败,请就检查串口是否连接正常!", ToastLength.Long).Show();
                //}
                //TODO:这样优化后就简单多了
                if (!OpenSerial())
                {
                    Toast.MakeText(this, "串口初始化失败,请就检查串口是否连接正常!", ToastLength.Long).Show();
                }
            }
            //TODO:这个Switc Case 应该可以去掉的
            //可以考虑使用反射加依赖注入完成
            //每一个枚举上面配置对应的信号处理类

            switch (Currentversion.masterBoxVersion)
            {
                case MasterControlBoxVersion.SimulatedData:
                    Task.Run(() => { Bootstrapper.InitializeSignalAsyncTest(messager, this, DBNAME); }).ContinueWith((task) => { var examItemCreator = Singleton.GetProviderFactory; });
                    break;
                case MasterControlBoxVersion.WifiTcp:
                case MasterControlBoxVersion.WifiUdp:
                    //数据库加载后，进行提前加载灯光项目
                    Task.Run(() => { Bootstrapper.InitializeSignalUDPAsync(messager, this, DBNAME); }).ContinueWith((task) => { var examItemCreator = Singleton.GetProviderFactory; });
                    break;
                case MasterControlBoxVersion.Bluetooth:
                    Task.Run(() => { Bootstrapper.InitializeSignalAsyncBluetooth(messager, this, DBNAME); }).ContinueWith((task) => { var examItemCreator = Singleton.GetProviderFactory; });
                    break;
                case MasterControlBoxVersion.USB:
                    Task.Run(() => { Bootstrapper.InitializeSignalAsyncUSBSerial(messager, this, DBNAME); }).ContinueWith((task) => { var examItemCreator = Singleton.GetProviderFactory; });
                    break;
                case MasterControlBoxVersion.Serial:
                    Task.Run(() => { Bootstrapper.InitializeSignalAsyncSerial(messager, this, DBNAME); }).ContinueWith((task) => { var examItemCreator = Singleton.GetProviderFactory; });
                    break;
                default:
                    Task.Run(() => { Bootstrapper.InitializeSignalUDPAsync(messager, this, DBNAME); }).ContinueWith((task) => { var examItemCreator = Singleton.GetProviderFactory; });
                    break;
            }
        }

        //创建数据库
        public void CreateDataBase(string DataBaseName)
        {
            //  Java.IO.File f = new Java.IO.File(DataBaseName);
            //如果目录里面不存在就创建 数据库
            if (!File.Exists(DataBaseName))
            {
                object[] par = new object[2];
                par[0] = this;
                par[1] = Currentversion.DataBaseName;
                var dbName = Currentversion.DataBaseName.Split('-')[2].Substring(0, Currentversion.DataBaseName.Split('-')[2].Length - 3);
                var type = Type.GetType(string.Format("TwoPole.Chameleon3.Infrastructure.Services.DataBase{0},TwoPole.Chameleon3.Infrastructure", dbName));
                var db = (IDBCreate)Activator.CreateInstance(type, par);
                //TODO:使用依赖注入应该也可以。
                db.InitDataSql();
            }


        }
        /// <summary>
        /// TODO:这个函数写的比较乱 可以优化下
        /// </summary>
        /// <returns></returns>
        public bool OpenSerial()
        {
            try
            {
                var isOpen = false;
                if (!isOpen)
                {
                    MyApp.driver = new CH34xUARTDriver((UsbManager)GetSystemService(Context.UsbService), this, "cn.wch.wchusbdriver.USB_PERMISSION");
                    if (!MyApp.driver.ResumeUsbList())// ResumeUsbList方法用于枚举CH34X设备以及打开相关设备
                    {
                        //Toast.MakeText(this, "打开设备失败!", ToastLength.Long).Show();
                        MyApp.driver.CloseDevice();
                    }
                    else
                    {
                        if (!MyApp.driver.UartInit())
                        {//对串口设备进行初始化操作
                            Toast.MakeText(this, "初始化设备失败!", ToastLength.Long).Show();
                            return false;
                        }
                        #region 获取USB 相关信息 暂时不需要
                        // var device = MyApp.driver.EnumerateDevice();
                        //string DeviceInfo = device.ProductId.ToString()+":"+ device.VendorId.ToString();
                        //LogManager.WriteSystemLog(DeviceInfo);
                        #endregion
                        isOpen = true;
                    }
                    //设置57600
                    if (MyApp.driver.SetConfig(57600, 8, 0, 0, 0))
                    {
                        return true;
                    }
                    else
                    {
                        Toast.MakeText(this, "串口设置失败!", ToastLength.Long).Show();
                        return false;
                    }
                }
                else
                {
                    MyApp.driver.CloseDevice();
                    isOpen = false;
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                return false;
            }
        }
        public void InitVersionInfo()
        {
            //TODO:三亚这个比较特殊版本界面选择都将不在生效

            Currentversion = DefaultVersion;

            if (!DefaultVersion.IsForbiddenVersionChange)
            {
                string Info = ReadAuthFile("VersionInfo.ini");
                Currentversion = DataBase.GetVersionInfo(Info);
                if (Currentversion == null)
                {
                    Currentversion = DefaultVersion;
                }
                else
                {

                    Currentversion.AdsShowTime = DefaultVersion.AdsShowTime;
                }
            }
            DataBase.SaveVersionInfo(Currentversion);
            SqlDataRepertory.DB_Name = Currentversion.DataBaseName;

        }
        public string ReadAuthFile(string FileName = "Auth.ini")
        {
            try
            {
                Java.Lang.StringBuffer sb = new Java.Lang.StringBuffer();

                System.IO.Stream fis = this.OpenFileInput(FileName);

                int ch;
                while ((ch = fis.ReadByte()) != -1)
                {
                    sb.Append((char)ch);
                }
                fis.Close();
                string AuthMsg = sb.ToString();
                return AuthMsg;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}