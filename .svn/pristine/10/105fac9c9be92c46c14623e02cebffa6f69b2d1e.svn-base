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
#region  开发过程中心酸和无奈
/*
 *       鲍君 2017/5/2 
 *       任务管理系统，记录了开发过程中遇到的问题。以及解决方案
 *       http://2pole.com:8400/zentao/task-view-2029.html
 *       如果不是沁恒芯片，只支持19200波特率。设置波特率无效 
 *       如下都是无关项目的吐槽
 *       //人生嘛，不多经历几次失败是不会成长的。
 *       //缘分嘛，一切都是缘分。
 *       //缘分嘛，一切都是缘分。
 *       //一切的一切都是缘分。
 *       //没有什么不可以的。
 *       //tyr catch 人生嘛，总
 *       //try catch 人生嘛，总要靠缘分才好。
 *       //我觉得没有什么不可以的。一切的一切都是靠缘分，如果有缘认识，那么一切都好，没有缘分也无所谓了。
 *       //android 项目目前已经到稳定阶段现在需要的是大规模使用，大量使用。再根据用户反馈进行修改处理。
 *       //过去的已经过去，展望未来。
 *       //系统在优化 再进行升级。
 *       //然后在做一个界面输入授权编码
 *       //做一个简单的授权机制，选择授权日期以及输入授权编码，然后把
 *       //Forever Forget
 *       //There is no hope, it is time to choose to give up
 *       //if Have is no hope,it is time to choose to give up
 *       //if Have is no hope,it is time to choose to give up
 *       //if Have is no hope,it is time to choose to give up
 *       //if I have a chance,I will leave ChongQing.
 *       //if my dream is no hope,it is time to choose to give up
 *       //Bug and Bug //try to gave up,
 *       //Forever // snower
 *       //Forever // snower
 *       //Forever // snower
 *       //Forver //  snower
 *       //Forever // snower
 *       //Forver  // snower
 *       //Forver //  snower
 *       //Forver //  snower
 *       //Forver //  snower
 *       //Forver //  snower
 *       //Forver //  snower
 *       //Forver //  snower
 *       //Forver //  snower
 *       //Forver //  snower
 *       //Is not your people, and why persistent
 *       //There is no hope,it is time 
 *       //There is no hope,it is time
 *       //There is no hope,it is time
 *       //it is no hope,it is time
 *       //it is no hope,it is time
 *       //RGWMYNPY,NMGNHWJHLKCQ
 *       //前途迷茫 一切
 *       //http://www.tuling123.com/openapi/api?key=4aa21808a3354472a4905e96c0e2e586&info=%E4%BD%AO%E5%B9%E9%BE%84
 *       //http://www.tuling123.com/openapi/api?key=4aa21808a3354472a4905e96c0e2e586&info=%E4%BD%A0%E5%B9%B4%E9%BE%84%E5%A4%9A%E5%B0%91
 *       //系统启动慢
 *       //hello world
 *       //hello world
 *       IWANTTOHAVEAGIRLFIREND
 *       //5月8号考试,希望这次考试顺利 
 *       2017/5/4
 *       mono开发过程坑：
 *       
 *       1.界面开发还是使用androidStudio 比较好，vs开发界面反应迟钝。
 *       2.界面开发
 *       1.布局等也不方便查看
 *       2.mono开发过程基本上直接寻找java代码进行翻译
 *       3.开发过程中一定要注意所有的页面异常捕获，android这个有可能不在这个界面，但是你打开过了这个界面退出的时候生命周期没有结束，引发异常可能导致闪退
 *       4.android开发过程中,部分java类库是可以支持的，可以直接采用mono里面的机制，把java类库转换过来。但是部分是不支持，那就 只有自己重写了
 *       5.mono开发最好还是先去学习android开发的基本知识,没有android基础很难开发出好的app.
 *       6.android很多方法可能会转换成为C# 里面的一个属性，所以找不到相同名字方法时候，进入类文件中寻找意思相近的属性或者方法。
 *       7.mono开发的资源比较少，需要有耐心慢慢的去寻找解决方案。
 *       8.由于mono这个机器也是把C#给翻译成为了java，这里面是可以使用java的一些基础写法，只是语法是c#.所以编写的时候尽量考虑使用java原生态的一些类型，这样对执行效率有所提高
 *       9.Android 数据库主流的都是运行时创建。或者通过另外一种方法，数据库转换成一串字符流，然后代码再通过这段字符流反向生成数据库
 *       10.中控大多数的蓝牙最好不要试图使用传输数据.这个和中控系统以及中控蓝牙有关
 *       11.用钱买时间吧,时间腾出来之后我可以做更多自己想做的事情
 *       12.it is time to change.it is time to leave chongqing.
 *       13.i think chengdu is a very good city .  
 *       14.responsibility
 *       15.Go ahead and get your driver's license as soon as possible
 *       16.Go ahead and get your driver's license as soon as possible
 *       17.It's time to change yourself
 *       18.It's time to change yourself
 *       19.It's time to change yourself 
 *       20.It's almost enough to play
 *       21.It's almost enough to play
 *       22.It's almost enough to play
 *       23.It's almost enough to play,Go ahead and get your driver's license as soon as possible 
 *       24.It's almost enough to play,Go ahead and get your driver's license as soon as possible
 *       25.It's almost enough to play,Go ahead and get your driver's license as soon as possible
 *       26.It's time to change yourself,Go ahead and get your driver's license as soon as possible
 *       27.It's time to change yourself,Go ahead and get your driver's license as soon as possible
 *       28.It's time to change yourself,Go ahead and get your driver's license as soon as possible
 *       1.授权没问题 加上一个密码。
 *       2.授权没问题 加上一个密码。
 *       3.授权没问题 加上一个密码。
 *       4.荣昌 问题,找一台比较严重的
 *       6.第一个 USB 第一次授权。 
 *       7.第一个 USB 第一次授权。 第一次退出，看看厂家Demo，沟通。
 *       8.性能速度怎么样,比较厂。 第一次退出，看看厂家Demo, 沟通。
 *       9.淡定淡定,速度做完自己要做的事情!
 *       10.Try to do something。
 *       11.Try to do something。
 *       12.Try to do something。    
 *       13.XIEHE is too pit,but patient no choice
 *      
 */
#endregion
namespace TwoPole.Chameleon3
{
    //TODO MainLauncher 这个属性控制了 那个是启动界面 如果是True 就表示是启动界面  
    //TODO:如果不修改启动界面了？这样好不好实现？
    //TODO:TeamView 第一次使用
    //TODO:新功能列表 1.扣分规则修改功能 预计天  2.人工评判功能 天 3.新的多伦界面 天 4.扣分删除功能 小时  5.自动升级功能 天  这些功能 这个比较有意义
    //TODO:自适应界面还是有一些问题，有时间最好系统的学习编写Demo。系统找不到对应分辨路文件会往下找

    //adb connect 127.0.0.1:5555
    //[Activity(Label = "领航e驾", MainLauncher = true, Icon = "@drawable/hzx_logo")]
    public class MainActivityHZX : Activity
    {
        #region 测试使用 可以考虑删除掉
        int count = 1;
        int BthExceptionCount = 0;
        TextView textViewShow;
        CarSignalInfo carSingal;
        DateTime? FirstDateTime = null;
        double TimespanMS = 0;
        public bool IsTestMode = false;
        public MasterControlBoxVersion Version = MasterControlBoxVersion.USB;
        public SystemUIType SystemType = SystemUIType.DuoLunSensor;
        static string DBNAME = Android.OS.Environment.ExternalStorageDirectory + "/" + DataBase.RongChang;
        private bool IsShowAds = true;
        public bool isOpen = false;
        public int OpenSerialCount = 0;
        #endregion //设备销售额 
        //TODO:把文字常量都尽量放到string.xml中
        //TODO:现在使用的是SpeakManagerBak
        //TODO:界面状态保留
        //TODO:灯光模拟待测试
        //TODO:辅助功能界面 版本切换
        //TODO:地图选择后面两个点看不见
        //TODO:开机启动
        //TODO:有空尝试对类文件进行文件归类整理，命名空间不变应该没得问题
        //TODO:报超过全程速度限制，下周让客户配合打日志回来。
        //TODO:信号滤波还未启用
        //TODO:中控上面本地时间有问题,重启之后会恢复初始化1970年。
        //TODO:z
        //TODO:中控上面接入Gps会自动更新时间。
        //TODO:需要一个清空系统缓存的功能
        //TODO:
        //1.进入项目黄色
        //1.进入项目黄色 
        //TODO:借的中控坏了返修了 170815
        //初始化版本相关的所有信息不合格
        //项目语音优先 Clear 语音
        //不影响使用 Try Catch 
        //不影响使用 Try Catch
        //界面状态保留不在加在
        //集成自动升级功能 TODO:这个可以考虑下
        //集成自动升级功能 
        //http://www.51mono.com/article/category/1/2.html  mono 开发者联盟11523
        #region 开光
        //-------------------佛祖保佑-------------------
        //                   _ooOoo_                  
        //                  o8888888o                 
        //                  88"". ""88                
        //                  (| -_- |)                 
        //                  O\  =  /O                 
        //               ____/`---'\____              
        //             .'  \\|     |//  `.            
        //            /  \\|||  :  |||//  \           
        //           /  _||||| -:- |||||-  \          
        //           |   | \\\  -  /// |   |          
        //           | \_|  ''\---/''  |   |          
        //           \  .-\__  `-`  ___/-. /          
        //         ___`. .'  /--.--\  `. . __         
        //      ."" '<  `.___\_<|>_/___.'  >'"".      
        //     | | :  `- \`.;`\ _ /`;.`/ - ` : | |    
        //     \  \ `-.   \_ __\ /__ _/   .-` /  /    
        //======`-.____`-.___\_____/___.-`____.-'======
        //                   `=---='                  
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        //            佛祖保佑       永无BUG            
        //      本模块已经经过开光处理，绝无可能再产生bug    
        //=============================================
        #endregion
        #region need to do
        //TODO:日志清理功能，客户日志满了之后会影响使用
        //TODO:在线升级功能，还有一些Bug
        //TODO:扣分规则修改功能,手动扣分功能
        #endregion
        IMessenger messager;
        ISpeaker speaker;
        ImageView ImageAds;
        Handler InitSystemHandler;
        VersionInfo Currentversion;

        //表示是海口版本
        public static bool IsHaiKouSpecial = false;
        //表示是成都版本
        public static bool IsChengDuSpecial = false;


        VersionInfo DefaultVersion = new VersionInfo
        {
            //主控箱版本 USB
            masterBoxVersion = MasterControlBoxVersion.WifiUdp,
            //考试界面 TODO:感觉UI问题太多 可以考虑完全重构下UI，1.不能很好的自适应 2.重复性代码比较多 3.比较杂乱 
            //todo:客户需求一台车装多个软件 软件机制要稍微修改下 所有的数据文件或者日志文件都存放在自己的目录文件中不在散放在根目录中
            //todo:有空时可以研究下使用Aop进行日志拦截以及性能拦截 
            UIType = SystemUIType.DuoLun,

            DataBaseName = DataBase.LongQuan,
            //是否显示广告
            IsShowAds = true,
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
            WelcomeVoice = "欢迎使用领航e驾科目三驾考系统"


        };


        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.HomeHZX);
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

                //区分海振兴和易考星
                DataBase.ProgramType = 1;

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
        }


        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        public string GetIntentParameterDBName()
        {
            var DBName = Intent.GetStringExtra("DbName");
            return DBName;
        }
        public SystemUIType GetIntentParameterUIType()
        {
            var UIType = Intent.GetStringExtra("UIName");
            return (SystemUIType)Enum.Parse(typeof(SystemUIType), UIType);
        }

        /// <summary>
        /// 初始化当前版本信息
        /// </summary>
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
            //TODO;其实这点应该不用的，直接有数据就用没有数据就不管
            if (IsHaiKouSpecial)
            {
                //LogManager.WriteSystemLog("IsHaiKouSpecial");
                //海口版本太特殊,会根据用户界面选择进行重置数据库
                Currentversion.DataBaseName = GetIntentParameterDBName();

                LogManager.WriteSystemLog("ReadDBName" + Currentversion.DataBaseName);
            }
            if (IsChengDuSpecial)
            {
                Currentversion.DataBaseName = GetIntentParameterDBName();
                Currentversion.UIType = GetIntentParameterUIType();
            }
            DataBase.SaveVersionInfo(Currentversion);


            if (Currentversion.IsShowAds)
            {
                InitSystemHandler = new Handler();
                InitSystemHandler.PostDelayed(HanderRun, Currentversion.AdsShowTime);
            }
            else
            {
                ImageAds.Visibility = ViewStates.Gone;
            }

            SqlDataRepertory.DB_Name = Currentversion.DataBaseName;

        }

        /// <summary>
        /// TODO:这种授权方法太简单了，可以考虑处理下
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
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

        public void InitSystemNew()
        {
            //播放背景音乐
            if (Currentversion.IsPlayBackgroundMusic)
            {
                Singleton.GetSpeaker.InitLoopPlyaMediaTimer();
            }
            //打开串口
            if (Currentversion.masterBoxVersion == MasterControlBoxVersion.USB)
            {
                if (!OpenSerial())
                {
                    Toast.MakeText(this, "串口初始化失败,请就检查串口是否连接正常!", ToastLength.Long).Show();
                }
            }
            Task.Run(() => { Bootstrapper.InititalizeSignalAsyc(Currentversion.masterBoxVersion); }).ContinueWith((task) => { var examItemCreator = Singleton.GetProviderFactory; });
        }
        /// <summary>
        /// 初始化系统
        /// </summary>
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
        public void HanderRun()
        {
            ImageAds.SetImageResource(Resource.Drawable.hzx_bgAd);
            ImageAds.Visibility = ViewStates.Gone;
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

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            LogManager.WriteSystemLog("MainActivity AndroidEnvironment_UnhandledExceptionRaise:" + e.Exception.Source + e.Exception.Message + e.Exception.StackTrace);
            // Toast.MakeText(this, e.Exception.Message, ToastLength.Long);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                ExitSystem();
            }
            return base.OnKeyDown(keyCode, key);
        }

        public void InitControl()
        {
            ImageAds = (ImageView)FindViewById(Resource.Id.ImageAdsHZX);
            Button btnNetworkTest = FindViewById<Button>(Resource.Id.btnNetworkTestHZX);
            Button btnSingleTraining = FindViewById<Button>(Resource.Id.btnSingleTrainingHZX);
            Button btnSystemSetting = FindViewById<Button>(Resource.Id.btnSystemsettingsHZX);
            Button btnExitSystem = FindViewById<Button>(Resource.Id.btnExitSystemHZX);
            btnNetworkTest.Click += BtnNetworkTest_Click;
            btnSingleTraining.Click += BtnSingleTraining_Click;
            btnSystemSetting.Click += BtnSystemSetting_Click;
            btnExitSystem.Click += BtnExitSystem_Click;
        }

        private void ExitSystem()
        {
            try
            {
                speaker.onStop();
                var messanger = Singleton.GetMessager;
                messager.Unregister(this);
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                Finish();
            }
            catch (Exception)
            {
                Finish();
            }
        }
        private void BtnExitSystem_Click(object sender, EventArgs e)
        {
            ExitSystem();
        }

        private void BtnSystemSetting_Click(object sender, EventArgs e)
        {
            PlayActionText(sender);
            Intent intent = new Intent();
            intent.SetClass(this, typeof(SystemSettings));
            //todo;可能存在Bug
            //intent.PutExtra("IsShowAds", Currentversion.IsShowAds);
            StartActivity(intent);
            //Type type=new typ
        }

        private void GetSystemUITypeByScreenDisplayMetrics()
        {
            DisplayMetrics dm = new DisplayMetrics();
            base.WindowManager.DefaultDisplay.GetMetrics(dm);
            //BUG 不能 
            SystemType = Currentversion.UIType;
            if (SystemType == SystemUIType.JingTang)
            {
                //600*1024 是大一点的捷达车中控
                if (dm.WidthPixels == 1024 && dm.HeightPixels == 600)
                {
                    SystemType = SystemUIType.JingTang;
                }
                else
                {
                    SystemType = SystemUIType.JingTangJieDa;
                }
            }

            else if (SystemType == SystemUIType.DuoLun || SystemType == SystemUIType.DuoLunSensor)
            {
                if (dm.WidthPixels == 1024 && dm.HeightPixels == 600)
                {

                }
                else if (dm.WidthPixels == 800 && dm.HeightPixels == 480)
                {
                    //TODO:多伦的将根据屏幕进行自适应,公司没有这种分辨率机型。有问题可以及时解除屏蔽解决。
                    SystemType = SystemUIType.DuoLunJieDa;
                }
                else
                {
                    SystemType = SystemUIType.DuoLunJieDa;
                }
            }

            else if (SystemType == SystemUIType.HuaZhong || SystemType == SystemUIType.HuaZhongSmall)
            {
                if (dm.WidthPixels == 1024 && dm.HeightPixels == 600)
                {
                    SystemType = SystemUIType.HuaZhong;
                }
                else if (dm.WidthPixels == 800 && dm.HeightPixels == 480)
                {
                    SystemType = SystemUIType.HuaZhongSmall;
                }
                else
                {
                    SystemType = SystemUIType.HuaZhongSmall;
                }
            }
            else
            {

            }
            Currentversion.UIType = SystemType;

        }
        private void BtnSingleTraining_Click(object sender, EventArgs e)
        {
            //单机训练开始进行Loop
            PlayActionText(sender);
            Intent intent = new Intent();
            intent.SetClass(this, GetActivityType());
            intent.PutExtra("ExamMode", "Train");
            StartActivity(intent);
        }

        /// <summary>
        /// TODO可以考虑使用反射
        /// </summary>
        /// <returns></returns>
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
        private void BtnNetworkTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DataBase.CanUse())
                {
                    //TODO:这个可能有Bug，应该没有吧
                    //if (!Tools.IsNetworkConnected(this))
                    //{

                    //    speaker.PlayAudioAsync("请检查网络是否打开");
                    //    return;
                    //}
                    //if (!Tools.Ping())
                    //{
                    //    speaker.PlayAudioAsync("请检查网络是否打开");
                    //    return;
                    //}
                    CreateDialog();
                }
                else
                {
                    PlayActionText(sender);
                    Intent intent = new Intent();
                    intent.SetClass(this, GetActivityType());
                    intent.PutExtra("ExamMode", "Exam");
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {

                LogManager.WriteSystemLog("NetWorkTest Click:" + ex.Message);
            }


        }
        private void PlayActionText(object sender)
        {
            string ActionText = ((Button)sender).Text;
            speaker.SpeakActionVoice(ActionText);
        }

        #region 租赁方案相关代码
        private AlertDialog alertDialog = null;
        private AlertDialog.Builder builder = null;
        private bool IsStopCheck = false;
        private string url = "https://qr.api.cli.im/qr?data=cs&level=H&transparent=false&bgcolor=%23ffffff&forecolor=%23000000&blockpixel=12&marginblock=1&logourl=&size=280&kid=cliim&key=dbf0b9f9bfcda98b665f7aa4674e46a9";
        private void CreateDialog()
        {
            LogManager.WriteSystemLog("OpenDialog");
            IsStopCheck = false;
            View view = View.Inflate(this, Resource.Layout.WeiXinCode, null);
            //ImageView editText = (EditText)view.FindViewById(Resource.Id.dialog_input_value);
            ImageView imageview = (ImageView)view.FindViewById(Resource.Id.imageViewQRCode);
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("模拟训练购买")
            .SetView(view)
            .SetNegativeButton("我已购买", (s, e) =>
            {
                //则手动去请求
                IsStopCheck = true;
                CheckByHours();
            })
            .SetNeutralButton("取消", (s, e) =>
            {

            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
            //设备序列号 请根据Url的要求进行提交上去
            //string EquipmentNumber = DataBase.EquipmentNumber;


            //Drawable drawable = loadImageFromNetwork(url);
            //imageview.SetImageDrawable(drawable);
            Bitmap bitmap = GetBitmapByUrl(url);
            imageview.SetImageBitmap(bitmap);

            //多线程来进行检测
            Task.Run(() =>
            {
                CheckByHours();

            });

        }



        public Bitmap GetBitmapByUrl(string url)

        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = "GET";
            req.KeepAlive = true;
            req.ContentType = "image/*";
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            System.IO.Stream stream = null;
            try
            {
                stream = rsp.GetResponseStream();
                Bitmap bitmap = BitmapFactory.DecodeStream(stream);
                return bitmap;
            }
            catch (Exception e)
            {

                LogManager.WriteSystemLog("DownLoadImage" + e.Message);
            }
            finally
            {
                if (stream != null) stream.Close();

                if (rsp != null) rsp.Close();
            }
            return null;

        }


        private Drawable loadImageFromNetwork(string imageUrl)
        {
            Drawable drawable = null;
            try
            {
                // 可以在这里通过文件名来判断，是否本地有此图片  
                drawable = Drawable.CreateFromStream(
                        new URL(imageUrl).OpenStream(), "image.jpg");
            }
            catch (Exception e)
            {

            }
            if (drawable == null)
            {

            }
            else
            {

            }

            return drawable;
        }


        /// <summary>
        /// TODO:这个方法 总是会报异常 原因不明确 ErrorException of type 'Java.Lang.RuntimeException' was thrown.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Bitmap getBitmap(string path)
        {
            try
            {
                Java.Net.URL url = new Java.Net.URL(path);
                LogManager.WriteSystemLog("Java.Net.URL url = new Java.Net.URL(path)");
                Java.Net.HttpURLConnection conn = (Java.Net.HttpURLConnection)url.OpenConnection();
                LogManager.WriteSystemLog("Java.Net.HttpURLConnection conn = (Java.Net.HttpURLConnection)url.OpenConnection();");
                conn.ConnectTimeout = 4000;
                conn.RequestMethod = "GET";
                LogManager.WriteSystemLog(" conn.RequestMethod = GET;");
                if (conn.ResponseCode == Java.Net.HttpStatus.Ok)
                {
                    LogManager.WriteSystemLog("status ok");
                    Bitmap bitmap = BitmapFactory.DecodeStream(conn.InputStream);
                    LogManager.WriteSystemLog(" conn.RequestMethod = GET;");
                    return bitmap;
                }
                else
                {
                    var re = conn.ResponseCode;
                    LogManager.WriteSystemLog("status not ok");
                    // LogManager.WriteSystemLog("ReCode:" + re.ToString());


                }
            }
            catch (Exception ex)
            {
                // LogManager.WriteSystemLog("getBitmap:Error" + ex.stat);
            }
            return null;
        }
        public void CheckByHours()
        {
            //循环检测购买的小时数
            bool isBuy = false;

            while (!IsStopCheck)
            {
                if (!DataBase.CanUse())
                {
                    //延迟30毫秒
                    Thread.Sleep(60 * 1000);
                    bool IsBuy = Tools.BuyMinute() > 0;
                    if (IsBuy)
                    {
                        DataBase.ByMinute = Tools.BuyMinute();
                        DataBase.ByTimes = DateTime.Now;
                        speaker.SpeakAsync("目前还剩余{0}" + DataBase.ByMinute.ToString() + "分钟");
                        break;
                    }
                }
            }
            alertDialog.Dismiss();
            if (DataBase.CanUse())
            {
                Intent intent = new Intent();
                intent.SetClass(this, GetActivityType());
                intent.PutExtra("ExamMode", "Exam");
                StartActivity(intent);
                IsStopCheck = true;
            }

            //如果当前设备不可用


        }
        public void CreateFile(string FileName, string FileContent)
        {
            //
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
        #endregion
        #region 测试使用 
        //TODO:通过XML 反向生成需要的资源文件
        public void DbCreate()
        {
            try
            {
                //res/drawable/block_5.xml
                var s = ApplicationContext.GetDir("res\\xml", FileCreationMode.WorldReadable);

                if (Directory.Exists(s.CanonicalPath))
                {
                    Toast.MakeText(this, "存在文件夹：" + s.ToString(), ToastLength.Long).Show();
                    var afile = Directory.GetFiles(s.AbsolutePath);
                    if (System.IO.File.Exists(s.CanonicalPath + "\\device_filter.xml"))
                    {
                        Toast.MakeText(this, "检测到xml", ToastLength.Long).Show();
                    }
                }
                //Toast.MakeText(this, "获取路径3："+s.ToString(), ToastLength.Long).Show();
                //存在数据库，则不用重新解压
                if (System.IO.File.Exists(DBNAME))
                {
                    Toast.MakeText(this, "有数据库，不用解压", ToastLength.Long).Show();
                    return;
                }
                string data = SqlDataRepertory.DataFile_fuling;
                //转换为byte[]
                var databytes = Convert.FromBase64String(data);
                System.IO.File.WriteAllBytes(DBNAME, databytes);
                Toast.MakeText(this, "无数据库，解压完成", ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

                Toast.MakeText(this, "初始化数据库出错：" + ex.Message, ToastLength.Long).Show();

            }
        }

        //TODO:USB断线重连机制一直都是一样一个问题，可以研究下。串口可以断线重连
        public bool ReOpenSerial()
        {
            try
            {
                //如果没有连接
                if (!MyApp.driver.IsConnected)
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
                        {
                            Toast.MakeText(this, "初始化设备失败!", ToastLength.Long).Show();
                            return false;
                        }
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
        public void Init()
        {
            Singleton.InitSpeaker(this);
            Button btnBluetoothDataReceive = FindViewById<Button>(Resource.Id.btnBluetoothDataReceive);
            btnBluetoothDataReceive.Click += BluetoothClick;
            textViewShow = FindViewById<TextView>(Resource.Id.txtViewShow);
            Button btnSensor = FindViewById<Button>(Resource.Id.btnCarSensor);
            btnSensor.Click += CarSensorClick;

            Button btnConnectBluetooth = FindViewById<Button>(Resource.Id.btnConnectBluetooth);
            btnConnectBluetooth.Click += ConnectBluetooth;

            Button btnDuolun = FindViewById<Button>(Resource.Id.btnDuolun);
            btnDuolun.Click += DuoLunClick;

            Button btnUdpConnect = FindViewById<Button>(Resource.Id.btnUDPSocket);
            btnUdpConnect.Click += UdpConnect;

            Button btnMap = FindViewById<Button>(Resource.Id.btnMap);
            btnMap.Click += Map;


            messager = Singleton.GetMessager;
            RegisterMessages(messager);
        }
        public void Map(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(Road_List));
            StartActivity(intent);
        }
        public void UdpConnect(object sender, EventArgs e)
        {
            //Bootstrapper.InitializeSignalUDPMessageAsync(messager, this);

        }
        public void ConnectBluetooth(object sender, EventArgs e)
        {
            //  Bootstrapper.InitializeSignalAsyncTest(messager, this);
            //BluetoothConnect//目前用作测试数据库
            //IDataService dataservice = Singleton.GetDataService;
            //var examItems = dataservice.GetSettings();
            //var LightRules = dataservice.AllLightRules;
            //var DeductionRules = dataservice.AllDeductionRules;
            //var LightGroup = dataservice.AllLightExamItems;
            //var Maps = dataservice.GetAllMapLines();
            //BluetoothCarSignalSeed.pair(BluetoothCarSignalSeed.DefaultBluetoothAddress, BluetoothCarSignalSeed.DefaultBluetoothPassword);
        }
        public void DuoLunClick(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(DuoLunZhongKong));
            StartActivity(intent);
        }
        public void CarSensorClick(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(CarSensor));
            StartActivity(intent);
        }
        public void BluetoothClick(object sender, EventArgs e)
        {
            isOpen = OpenSerial();
            if (isOpen)
            {

                // Bootstrapper.InitializeSignalAsyncUSBSerial(messager, this);
            }

        }
        protected void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
            messager.Register<BrokenRuleMessage>(this, OnBthExceptionReceive);
            messager.Register<USBConnectMessage>(this, OnUsbConnectMessageReceive);
        }

        private void OnUsbConnectMessageReceive(USBConnectMessage message)
        {
            LogManager.WriteSystemLog("ReConnect");
            ReOpenSerial();

        }

        private void OnBthExceptionReceive(BrokenRuleMessage message)
        {
            BthExceptionCount++;
        }

        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {

            if (!FirstDateTime.HasValue)
            {
                FirstDateTime = DateTime.Now;
            }
            if ((DateTime.Now - FirstDateTime.Value).TotalMilliseconds >= TimespanMS)
            {
                TimespanMS = (DateTime.Now - FirstDateTime.Value).TotalMilliseconds;
            }
            carSingal = message.CarSignal;
            FirstDateTime = DateTime.Now;
            RunOnUiThread(TestUpdateUi);
        }
        public void TestUpdateUi()
        {
            try
            {
                textViewShow.Text = carSingal.Sensor.ToString() + "\r\n" + "ExceptionCount:" + BthExceptionCount.ToString() + "\r\n" + "TimeSpan(ms):" + TimespanMS.ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }
        #endregion

    }


}

