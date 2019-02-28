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
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using Org.Json;
using System.Xml;
using Java.IO;
using Java.Text;
using TwoPole.Chameleon3.Domain;
using Java.Util;
using TwoPole.Chameleon3.Infrastructure.Services;
using Android.Content.PM;
using System.Net;

namespace TwoPole.Chameleon3
{
    /// <summary>
    /// 辅助功能界面 一键备份， 一键升级 ，一键导出 一键导入 
    /// </summary>
    [Activity(Label = "AssistanceTools")]
    public class AssistanceTools :BaseSettingActivity
    {
        TextView textViewTips;
        //主机授权编码
        EditText edtAuthCode;
        EditText edtAuthDate;
       
        EditText edtAuthCode1;
        EditText edtAuthCode2;
        EditText edtAuthCode3;
        EditText edtAuthCode4;
        
        Spinner spinnerVersion;
        Spinner spinnerUIType;
        //版本
        Spinner spinnerSensor;
        CheckBox chkAds;
        CheckBox chkbuttonVoice;

        ILog logger;
        Authorization authorization;
        private AlertDialog alertDialog = null;
        private AlertDialog.Builder builder = null;
        public string TipMessage = string.Empty;
        public string UpgradeUrl = string.Empty;
        public string FolderName = string.Empty;
        public string DictoryPath = string.Empty;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AssistanceTools);
            InitControl();
            initHeader(false);
            setMyTitle(this.GetString(Resource.String.AssistanceToolsStr));
            logger = Singleton.GetLogManager;
            authorization = new Authorization(this);
           
            RunOnUiThread(InitData);

        }
        public void InitData()
        {
            try
            {
                string activeInfo = authorization.GetAuthorizationInfo();
                if (!string.IsNullOrEmpty(activeInfo))
                {
                    textViewTips.Text = activeInfo;
                }
                BindSpinner(DataBase.lstVersionInfo.Select(c => c.VersionName).ToList(), spinnerVersion);
                BindSpinner(DataBase.lstUIInfo.Select(c => c.Key).ToList(), spinnerUIType);
                BindSpinner(DataBase.lstSensorInfo.Select(c => c.Key).ToList(), spinnerSensor);

                chkAds.Checked = DataBase.Currentversion.IsShowAds;
                chkbuttonVoice.Checked = DataBase.Currentversion.IsPlayActionVoice;
                int VersionNameIndex = 0;
                int UIIndex = 0;
                int SensorIndex = 0;
                for (int i = 0; i < DataBase.lstVersionInfo.Count; i++)
                {
                    if (DataBase.Currentversion.DataBaseName.Contains(DataBase.lstVersionInfo[i].DataBaseName))
                    {
                        VersionNameIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < DataBase.lstUIInfo.Count; i++)
                {
                    if ((SystemUIType)Enum.Parse(typeof(SystemUIType), DataBase.lstUIInfo[i].Value) == DataBase.Currentversion.UIType)
                    {
                        UIIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < DataBase.lstSensorInfo.Count; i++)
                {
                    if ((MasterControlBoxVersion)Enum.Parse(typeof(MasterControlBoxVersion), DataBase.lstSensorInfo[i].Value) == DataBase.Currentversion.masterBoxVersion)
                    {
                        SensorIndex = i;
                        break;
                    }
                }
                spinnerVersion.SetSelection(VersionNameIndex);
                spinnerSensor.SetSelection(SensorIndex);
                spinnerUIType.SetSelection(UIIndex);
                DictoryPath = Android.OS.Environment.ExternalStorageDirectory.ToString() + "/" + DataBase.GetFolderName(DataBase.Currentversion.DataBaseName);
                FolderName = DataBase.Currentversion.FolderName;
            }
            catch (Exception ex)
            {

                logger.Error("AssistanceTools InitData", ex.Message);
            }
          
          //  int VersionNameIndex=DataBase.lstVersionInfo.Where(s=>s.VersionName==DataBase.Currentversion.VersionName).
            //获取索引
        }
        private void BindSpinner(List<string> lstDataSource, Spinner spinner)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstDataSource);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.Visibility = ViewStates.Visible;
        }
        public void InitControl()
        {
            var btnBackUp = FindViewById<Button>(Resource.Id.btnBackUp);
            var btnUpdate = FindViewById<Button>(Resource.Id.btnUpdate);

            var btnNewRulesUpdate= FindViewById<Button>(Resource.Id.btnNewRulesUpdate);
           

            var btnExport=FindViewById<Button>(Resource.Id.btnExport);
            var btnImport = FindViewById<Button>(Resource.Id.btnImport);

            var btnMapExport=FindViewById<Button>(Resource.Id.btnMapExport);
            var btnWindowsMapExport= FindViewById<Button>(Resource.Id.btnWindowsMapExport);
            var btnMapImport = FindViewById<Button>(Resource.Id.btnMapImport);

            var btnLightExport = FindViewById<Button>(Resource.Id.btnLightExport);
            var btnLightImport = FindViewById<Button>(Resource.Id.btnLightImport);

            var btnSettingExport = FindViewById<Button>(Resource.Id.btnSettingExport);
            var btnSettingImport = FindViewById<Button>(Resource.Id.btnSettingImport);



            var btnGrantAuth = FindViewById<Button>(Resource.Id.btnGrantAuth);
            var btnActivation = FindViewById<Button>(Resource.Id.btnActivation);
            var btnSaveVersion = FindViewById<Button>(Resource.Id.btnSaveSoftInfo);

           var btnSoftwareUpgrade= FindViewById<Button>(Resource.Id.btnSoftwareUpgrade);

            chkAds = FindViewById<CheckBox>(Resource.Id.chkAds);
            chkbuttonVoice = FindViewById<CheckBox>(Resource.Id.chkbuttonVoice);
            spinnerVersion = (Spinner)FindViewById(Resource.Id.spinnerVersion);
            spinnerUIType = (Spinner)FindViewById(Resource.Id.spinnerUIType);
            spinnerSensor = (Spinner)FindViewById(Resource.Id.spinnerSensor);

            textViewTips = FindViewById<TextView>(Resource.Id.textviewTips);
            edtAuthDate = FindViewById<EditText>(Resource.Id.edtAuthDate);
            edtAuthCode =  FindViewById<EditText>(Resource.Id.edtAuthCode);
            edtAuthCode1 = FindViewById<EditText>(Resource.Id.edtAuthCode1);
            edtAuthCode2 = FindViewById<EditText>(Resource.Id.edtAuthCode2);
            edtAuthCode3 = FindViewById<EditText>(Resource.Id.edtAuthCode3);
            edtAuthCode4 = FindViewById<EditText>(Resource.Id.edtAuthCode4);

            btnBackUp.Click += BtnBackUp_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnExport.Click += BtnExport_Click;
            btnImport.Click += BtnImport_Click;
            btnSaveVersion.Click += BtnSaveVersion_Click;
            btnMapExport.Click += BtnMapExport_Click;
            btnMapImport.Click += BtnMapImport_Click;
            btnWindowsMapExport.Click += BtnWindowsMapExport_Click;
            btnLightExport.Click += BtnLightExport_Click;
            btnLightImport.Click += BtnLightImport_Click;
            btnNewRulesUpdate.Click += BtnNewRulesUpdate_Click;
            btnSettingExport.Click += BtnSettingExport_Click;
            btnSettingImport.Click += BtnSettingImport_Click;
            btnGrantAuth.Click += BtnGrantAuth_Click;
            btnActivation.Click += BtnActivation_Click;
            btnSoftwareUpgrade.Click += BtnSoftwareUpgrade_Click;



        }

        private void BtnSoftwareUpgrade_Click(object sender, EventArgs e)
        {
            GetApi();
        }

        public void GetApi()
        {
            try
            {
                string response = HttpGet(DataBase.GetUpdateApiUrl());
                LogManager.WriteSystemLog(response);
                var result = response.FromJson<UpdateFirImAPIModel>();
                if (result == null)
                {
                    LogManager.WriteSystemLog("result is null");
                }
               // LogManager.WriteSystemLog(result.build);
                int VersionCode = getVersionCode();
               // LogManager.WriteSystemLog(VersionCode.ToString());

                if (VersionCode==result.build)
                {

                    textViewTips.Text = "已经是最新版本无需升级";
                }
                else
                {
                    string msg = result.changelog;
                   // SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
                //    UpgradeUrl = result.update_url;
                   // string date = sdf.Format(new Java.Util.Date(result.updated_at));
                    ShowQuestionDialog("软件升级",result.changelog,Upgrade,Cancel, "确定", "取消");
               
                }
 
            }
            catch (Exception ex)
            {

                LogManager.WriteSystemLog(ex.Message);
            }

        }
        //http://blog.csdn.net/wwj_748/article/details/8195565
        //进行apk安装
        private void installApk(string filePath)
        {
            var context = this;
            if (context == null)
                return;
            // 通过Intent安装APK文件
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Android.Net.Uri.Parse("file://" + filePath), "application/vnd.android.package-archive");
            intent.SetFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }
     
        public void Upgrade()
        {
            Intent intent = new Intent();
            intent.SetAction("android.intent.action.VIEW");
            Android.Net.Uri url = Android.Net.Uri.Parse(UpgradeUrl);
            intent.SetData(url);
            StartActivity(intent);
        }
        public void Cancel()
        {
        }
        private int getVersionCode()
        {
            // 获取packagemanager的实例
            PackageManager packageManager = this.PackageManager;
            // getPackageName()是你当前类的包名，0代表是获取版本信息
            PackageInfo packInfo = packageManager.GetPackageInfo(this.PackageName, 0);
            return packInfo.VersionCode;

        }
        public string HttpGet(string Url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            request.Method = "GET";

            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            System.IO.Stream myResponseStream = response.GetResponseStream();

            System.IO.StreamReader myStreamReader = new System.IO.StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();

            myResponseStream.Close();



            return retString;

        }

      

        private void BtnWindowsMapExport_Click(object sender, EventArgs e)
        {
            string Map = dataService.GetAllMapLines().ToJson();
            string MapName = DataBase.Currentversion.VersionName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".map";
          
            Java.IO.File file = new Java.IO.File(DictoryPath, MapName);
            BufferedWriter bw = new BufferedWriter(new FileWriter(file, false));
            bw.Write(Map);
            bw.Flush();
            textViewTips.Text = "地图导出成功";

        }

        private void BtnNewRulesUpdate_Click(object sender, EventArgs e)
        {
            var dataService = Singleton.GetDataService;
            bool Result=  dataService.UpdateNewRules();
            if (Result)
            {
                textViewTips.Text = "升级成功,请重新启动软件";
            }
            else
            {
                textViewTips.Text = "升级失败,请联系管理员进行手动升级";
            }
        }

        private void InputPasswordDialog()
        {
          
            View view = View.Inflate(this, Resource.Layout.Dialog_Input_Password, null);
            EditText editText = (EditText)view.FindViewById(Resource.Id.dialog_input_value);
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("请输入密码")
            .SetView(view)
            .SetNegativeButton("取消", (s, e) =>
            {

            })
            .SetPositiveButton("确定", (s, e) =>
            {
                //保存地图记录地图
                string password= editText.Text;
                if (!string.IsNullOrEmpty(password))
                {
                    if (password!= "Yuan@twopole0720")
                    {
                        TipMessage = "密码不正确";
                        RunOnUiThread(ShowTips);
                    }
                    //进行授权 1495 5248 5634 9504
                    else
                    {
                        GrantAuth();
                    }

                }
                else
                {

                    TipMessage = "请输入密码";
                    RunOnUiThread(ShowTips);
                }
             
            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
        }
        public void ShowTips()
        {
            textViewTips.Text = TipMessage;
        }

        private void BtnSaveVersion_Click(object sender, EventArgs e)
        {
            try
            {
                string DataBaseName = DataBase.lstVersionInfo[spinnerVersion.SelectedItemPosition].DataBaseName;
                string UIType = DataBase.lstUIInfo[spinnerUIType.SelectedItemPosition].Value;
                bool IsShowAds = chkAds.Checked;
                string SensorInfo = DataBase.lstSensorInfo[spinnerSensor.SelectedItemPosition].Value;
                string Info = string.Format("{0};{1};{2};{3};{4}", DataBaseName, UIType, IsShowAds,SensorInfo,chkbuttonVoice.Checked);

                authorization.WriteAuthFile(Info, "VersionInfo.ini");
                textViewTips.Text = "保存版本信息成功,请退出软件重新打开";
            }
            catch (Exception ex)
            {
                textViewTips.Text = "保存版本信息失败,请重试"+ex.Message;
            }    
        }

        private void BtnSettingImport_Click(object sender, EventArgs e)
        {
            textViewTips.Text = "参数开始一键导入,请等待";
            File file = new File(DictoryPath, "Setting.config");
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
            string date = sdf.Format(new Java.Util.Date(file.LastModified()));
            ShowQuestionDialog("导入参数", string.Format("您确定需要导入{0}导出的参数", date), SettingCoverExport, SettingNotCoverExport, "确定", "取消");
           
        }

        private void BtnSettingExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Setting = dataService.AllSettings.ToJson();
                File file = new File(DictoryPath, "Setting.config");
                BufferedWriter bw = new BufferedWriter(new FileWriter(file, false));
                bw.Write(Setting);
                bw.Flush();
                textViewTips.Text = "参数配置导出成功";
            }
            catch (Exception ex)
            {
                textViewTips.Text = "参数配置导出失败" + ex.Message;
            }
        }

        public void SettingCoverExport()
        {
            try
            {
                File file = new File(DictoryPath, "Setting.config");
                SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
                string date = sdf.Format(new Java.Util.Date(file.LastModified()));

                BufferedReader br = new BufferedReader(new FileReader(file));
                var SettingJson = br.ReadLine();

                var settings = SettingJson.FromJson<IList<Setting>>();

                dataService.SaveAddSettings(settings, true);
                textViewTips.Text = "参数设置导入成功";
            }
            catch (Exception ex)
            {
                textViewTips.Text = "参数设置导入失败" + ex.Message;
            }

        }
        public void SettingNotCoverExport()
        {


        }
        private void BtnLightImport_Click(object sender, EventArgs e)
        {
            textViewTips.Text = "灯光开始一键导入,请等待";
            File file = new File(DictoryPath, "Light.config");
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
            string date = sdf.Format(new Java.Util.Date(file.LastModified()));
            ShowQuestionDialog("导入灯光", string.Format("您确定需要导入{0}导出的灯光", date), LightCoverExport, LightNotCoverExport, "确定", "取消");
        }

        private void BtnLightExport_Click(object sender, EventArgs e)
        {
         

            try
            {
                string LightExamGroup = dataService.AllLightExamItems.ToJson();
                File file = new File(DictoryPath, "Light.config");
                BufferedWriter bw = new BufferedWriter(new FileWriter(file, false));
                bw.Write(LightExamGroup);
                bw.Flush();
                textViewTips.Text = "灯光配置导出成功";
            }
            catch (Exception ex)
            {
                textViewTips.Text = "灯光配置导出失败" + ex.Message;
            }
        }
        public void LightCoverExport()
        {
            try
            {
                File file = new File(DictoryPath, "Light.config");
                SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
                string date = sdf.Format(new Java.Util.Date(file.LastModified()));

                BufferedReader br = new BufferedReader(new FileReader(file));
                var LightGroupJson = br.ReadLine();

                var LightGroups = LightGroupJson.FromJson<IList<LightExamItem>>();

                dataService.SaveLightExamItems(LightGroups, false);
                textViewTips.Text = "灯光导入成功";
            }
            catch (Exception ex)
            {

                textViewTips.Text = "灯光导入失败"+ex.Message;
            }
           
        }
        public void LightNotCoverExport()
        {
          
          
        }

        //地图特殊处理一下
        private void BtnMapImport_Click(object sender, EventArgs e)
        {
            textViewTips.Text = "地图开始一键导入,请等待";
            System.IO.DirectoryInfo theFolder = new System.IO.DirectoryInfo(DictoryPath + "/" + "map");
            ShowQuestionDialog("导入地图", string.Format("您确定需要导入地图?"), MapCoverExport, MapNotCoverExport, "确定", "取消");
            //ShowQuestionDialog("导入地图", string.Format("您确定需要导入{0}导出的地图", date), MapCoverExport, MapNotCoverExport, "确定", "取消");
        }

        private void BtnMapExport_Click(object sender, EventArgs e)
        {
            try
            {
                //string Map = dataService.GetAllMapLines().ToJson();
                var maps = dataService.GetAllMapLines();

                if (!System.IO.Directory.Exists(DictoryPath + "/" + "map"))
                {
                    System.IO.Directory.CreateDirectory(DictoryPath + "/" + "map");
                }
                else
                {
                    System.IO.DirectoryInfo theFolder = new System.IO.DirectoryInfo(DictoryPath + "/" + "map");
                    foreach (var item in theFolder.GetFiles())
                    {
                        //每次导出我会清空整个文件夹
                        System.IO.File.Delete(item.FullName);
                    }
                }
                foreach (var item in maps)
                {
                    File file = new File(DictoryPath + "/" + "map", item.Name+".map");
                    BufferedWriter bw = new BufferedWriter(new FileWriter(file, false));
                    bw.Write(item.ToJson());
                    bw.Flush();
                }
                textViewTips.Text = "地图导出成功";

            }
            catch (Exception ex)
            {
                textViewTips.Text = "地图导出失败" + ex.Message;
            }
        }
        public void MapCoverExport()
        {
            try
            {

                if (!System.IO.Directory.Exists(DictoryPath + "/" + "map"))
                {
                    textViewTips.Text = "未导出地图";
                    return;
                }

                System.IO.DirectoryInfo theFolder = new System.IO.DirectoryInfo(DictoryPath + "/" + "map");
                List<MapLine> lstMaps = new List<MapLine>();
                foreach (var item in theFolder.GetFiles())
                {
                    File file = new File(item.FullName);
                    LogManager.WriteSystemLog(item.FullName);
                    BufferedReader br = new BufferedReader(new FileReader(file));
                    var MapJson = br.ReadLine();
                    var Map= MapJson.FromJson<MapLine>();
                    lstMaps.Add(Map);

                }

                dataService.SaveAddMap(lstMaps, true);
                textViewTips.Text = "导入地图成功";

            }
            catch (Exception ex)
            {

                textViewTips.Text = "地图导入失败"+ex.Message;
            }
          
        }
        public void MapNotCoverExport()
        {
         
           
        }

        

        private void BtnActivation_Click(object sender, EventArgs e)
        {
            try
            {
                string inputString = edtAuthCode1.Text.Trim() + edtAuthCode2.Text.Trim() + edtAuthCode3.Text.Trim() + edtAuthCode4.Text.Trim();
                string msg = string.Empty;
                if (!authorization.ISCheckValid(inputString, ref msg))
                {
                    textViewTips.Text = "激活失败";
                }
                else
                {
                    textViewTips.Text = msg;
                }
            }
            catch (Exception ex)
            {
                m_setting_head_title.Text = "辅助功能" + "  " + "激活失败:" + ex.Message;

            }

        }
        private string transferLongToDate(string dateFormat,long millSec)
        {
            SimpleDateFormat sdf = new SimpleDateFormat(dateFormat);
            Date date = new Date(millSec);
            return sdf.Format(date);
        }
        //授权把数据写入文件

        public void GrantAuth()
        {
            try
            {
                string AuthCode = edtAuthCode.Text.Trim() ;
                //string Date = transferLongToDate("yyyy/MM/dd", calendarViewAuthDate.Date);
                string Date = edtAuthDate.Text.Trim();
                try
                {
                   var Temp= Convert.ToDateTime(Date);
                }
                catch (Exception ex)
                {
                    textViewTips.Text = "试用写入失败,请检查日期输入是否正确"+ex.Message;
                    return;
                }
                authorization.WriteAuthFile(Date + "@" + AuthCode);
                //logger.WriteAuthFile(AuthCode + "@" + Date,this);
            }
            catch (System.Exception ex)
            {
                textViewTips.Text = "试用写入失败,请重新写入"+ex.Message;
                return;
            }
            textViewTips.Text = "试用写入成功";
        }
        private void BtnGrantAuth_Click(object sender, EventArgs e)
        {
            InputPasswordDialog();
        }
        public void ImportSuccess()
        {
            try
            {
                //最简单的都是加入浅醉
                File file = new File(DictoryPath, "config.config");
                SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
                string date = sdf.Format(new Java.Util.Date(file.LastModified()));

                BufferedReader br = new BufferedReader(new FileReader(file));
                var Config = br.ReadLine();
                var Data = Config.Split('@');
                string SettingJson = Config.Split('@')[0];
                string LightGroupJson = Config.Split('@')[1];
                string MapJson = Config.Split('@')[2];

                var settings = SettingJson.FromJson<IList<Setting>>();
                var LightGroups = LightGroupJson.FromJson<IList<LightExamItem>>();
                var Maps = MapJson.FromJson<List<MapLine>>();

                dataService.SaveAddSettings(settings);
                dataService.SaveLightExamItems(LightGroups);
                dataService.SaveAddMap(Maps);

                textViewTips.Text = "一键导入成功";
            }
            catch (Exception ex)
            {
                textViewTips.Text = "一键导入失败";
                Logger.Error("一键导入", ex.Message);
            }
            
        }
       
        public void ImportCancel()
        {
            textViewTips.Text = "开始一键导入,请等待";
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            //读取文件
            textViewTips.Text = "开始一键导入,请等待";
            File file = new File(DictoryPath, "config.config");
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
            string date = sdf.Format(new Java.Util.Date(file.LastModified()));
            ShowQuestionDialog("导入配置",string.Format("您确定需要恢复{0}导出的配置",date),ImportSuccess, ImportCancel,"确定","取消");
     
         

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            try
            {
                textViewTips.Text = "正在进行配置导出,请等待";
                JSONStringer js = new JSONStringer();
                var dataService = Singleton.GetDataService;
                string Setting = dataService.AllSettings.ToJson();
                string LightExamGroup = dataService.AllLightExamItems.ToJson();
                string Map = dataService.GetAllMapLines().ToJson();
                string config = Setting + "@" + LightExamGroup + "@" + Map;
                File file = new File(DictoryPath, "config.config");
                BufferedWriter bw = new BufferedWriter(new FileWriter(file,false));
                bw.Write(config);
                bw.Flush();
                textViewTips.Text = "配置导出成功";
            }
            catch (Exception ex)
            {
                textViewTips.Text = "一键导出失败";
                Logger.Error("一键导出",ex.Message);
            }
            //串口服务器//自己买一个呗//好简单的东西哟
            //一键导出把配置，灯光分组，以及地图都导出
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            //升级，直接下载文件之后写入覆盖
            InputTelphoneNumber(false);
        }

        private void BtnBackUp_Click(object sender, EventArgs e)
        {
            InputTelphoneNumber(true);
        }

        /// <summary>
        /// 输入客户手机号码
        /// </summary>
        private void InputTelphoneNumber(bool IsBack)
        {
            AlertDialog alertDialog = null;
            AlertDialog.Builder builder = null;
            View view = View.Inflate(this, Resource.Layout.Dialog_Input, null);
            EditText editText = (EditText)view.FindViewById(Resource.Id.dialog_input_value);
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("请输入手机号码")
            .SetView(view)
            .SetNegativeButton("取消", (s, e) =>
            {
                
            })
            .SetPositiveButton("确定", (s, e) =>
            {
                //保存地图记录地图
                //保存
                //保存地图记录地图
                string telphone = editText.Text;
                string PhoneRegx = @"[0-9]\d{10}$";
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(PhoneRegx);
                if (!regex.IsMatch(telphone))
                {
                    textViewTips.Text = "电话号码不正确,请检查";
                    return;
                }
                if (string.IsNullOrEmpty(telphone))
                {
                    return;
                }
                else
                {
                    //如果是需要一键备份
                    if (IsBack)
                    {
                        BackUpData(telphone);
                    }
                    else
                    {
                        UpdateData(telphone);
          
                    }
                    //进行BackUp
                }
            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
        }

        

        public void BackUpData(string Phone)
        {
            try
            {
                var token = RSAHelper.Encrypt("ceshi").Replace("+", "%2B");
                //  var filePath = Path.Combine(Application.StartupPath, BackUpFileName);
                System.IO.FileStream fs = new System.IO.FileStream(SqlDataRepertory.DB_Name, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                Byte[] byteFile = new byte[fs.Length];
                fs.Read(byteFile, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                var postData = string.Format("Token={0}&Phone={1} &UploadFile={2}&&FileName={3}", token, Phone, Convert.ToBase64String(byteFile).Replace("+", "%2B"), SqlDataRepertory.DB_Name.Split('/')[SqlDataRepertory.DB_Name.Split('/').Length - 1]);
                var msg = WebClientHelper.WebClientRequset(postData, "api/AndroidUpdater/BackupPrograms");
                var obj = msg.FromJson<UpgradeProgramsModel>();
                //地图
                if (obj.StatusCode == 200)
                {
                    Toast.MakeText(this, "备份成功", ToastLength.Long);
                    textViewTips.Text = "备份成功";
                }
                else
                {
                    textViewTips.Text = "备份失败,请检查网络连接";
                }
            }
            catch (Exception ex)
            {
                textViewTips.Text = "备份失败"+ex.Message;
            }
          

        }

        public void UpdateData(string Phone)
        {
            try
            {
                var token = RSAHelper.Encrypt("ceshi").Replace("+", "%2B");
                var postData = string.Format("Token={0}&Phone={1}", token, Phone);

                var msg = WebClientHelper.WebClientRequset(postData, "api/Updater/UpgradePrograms");
                var obj = msg.FromJson<UpgradeProgramsModel>();

                if (obj.StatusCode == 200)
                {

                    //升级下来的名字
                    //  string filename = System.IO.Path.Combine(Application.StartupPath, "易考星升级包.zip");
                    //下载下来的是一个压缩包数据流//
                    System.IO.FileStream fs = new System.IO.FileStream(SqlDataRepertory.DB_Name, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    fs.Write(Convert.FromBase64String(obj.Data.File), 0, Convert.FromBase64String(obj.Data.File).Length);
                    fs.Flush();
                    fs.Close();
                    textViewTips.Text = "升级成功";
                }
                else
                {
                    textViewTips.Text = "升级失败,请检查网络连接";
                }
            }
            catch (Exception ex)
            {

                textViewTips.Text = "升级失败"+ex.Message;
            }
        
        }
    }
}