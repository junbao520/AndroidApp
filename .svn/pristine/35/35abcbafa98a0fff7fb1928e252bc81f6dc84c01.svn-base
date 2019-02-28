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
using System.Reflection;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "ParameterSetting")]
    public class ParameterSetting :BaseSettingActivity
    {
        //try catch do something

        TextView tvMessage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
                //try catch do something 
                //if i want to do something hello do you wang to do something
                // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.ParameterSettings);
                initHeader(false);
                setMyTitle("参数设置");

                var GlobalSetting = FindViewById(Resource.Id.btn_Global_setting);
                GlobalSetting.Click += GlobalSetting_Click;

                //GlobalSetting.Id
                var ItemSetting = FindViewById(Resource.Id.btn_Item_setting);
                ItemSetting.Click += ItemSetting_Click;

                var SensorSetting = FindViewById(Resource.Id.btn_Sensor_setting);
                SensorSetting.Click += SensorSetting_Click;
                

                var LightSetting = FindViewById(Resource.Id.btn_LightSimulation_Setting);
                LightSetting.Click += LightSetting_Click;

                var DeductionRuleSetting= FindViewById(Resource.Id.btn_DeductionRule_Setting);
                DeductionRuleSetting.Click += DeductionRuleSetting_Click;

                var InitSetting = FindViewById(Resource.Id.btn_Init_Setting);
                InitSetting.Click += InitSetting_Click;


                var AssistanceTools = FindViewById(Resource.Id.btn_Assistance_Tools);
                AssistanceTools.Click += AssistanceTools_Click;

                tvMessage = FindViewById<TextView>(Resource.Id.tvMessage);

                //if I have
                var SensorLineSetting = FindViewById(Resource.Id.btn_SensorLine_Setting);
                SensorLineSetting.Click += SensorLineSetting_Click;
  
        }

        private void DeductionRuleSetting_Click(object sender, EventArgs e)
        {
            PlayActionText(this.GetString(Resource.String.DeductionRuleSettingStr));
            Intent intent = new Intent();
            intent.SetClass(this, typeof(DeductionRuleUpdate));
            StartActivity(intent);
        }

        private void AssistanceTools_Click(object sender, EventArgs e)
        {

            //辅助工具
            Intent intent = new Intent();
            intent.SetClass(this, typeof(AssistanceTools));
            StartActivity(intent);
        }

        //进行配置初始化，但是我需要得到DB名称
        private void InitSetting_Click(object sender, EventArgs e)
        {
            //开始
            //询问是否确认删除
            AlertDeleteUpdateDialog();
        }
        public void AlertDeleteUpdateDialog()
        {
            try
            {
                AlertDialog alertDialog = null;
                AlertDialog.Builder builder = null;
                builder = new AlertDialog.Builder(this);
                alertDialog = builder
               .SetTitle("警告")
               .SetMessage("恢复初始数据会造成所有数据以及系统设置丢失!!!")
               .SetNegativeButton("取消", (s, e) =>
               {
               })
               .SetPositiveButton("确定", (s, e) =>
               {
                   //进行数据库创建

                   LogManager.WriteSystemLog("Init:"+SqlDataRepertory.DB_Name);
                   Java.IO.File f = new Java.IO.File(SqlDataRepertory.DB_Name);
                   if (f.Exists())
                   {
                       f.Delete();
                       CreateDataBase(SqlDataRepertory.DB_Name);
                   }
                   else
                   {
                       CreateDataBase(SqlDataRepertory.DB_Name);
                   }
                   tvMessage.Text = "恢复出厂设置成功,请退出软件后重新打开";
               })
               .Create();       //创建alertDialog对象  
                alertDialog.Show();

            }
            catch (Exception ex)
            {
                tvMessage.Text = "恢复出厂设置失败"+ex.Message;
                LogManager.WriteSystemLog("数据初始化" + ex.Message);
            }

        }
        public void CreateDataBase(string DBNAME)
        {
            object[] par = new object[2];
            par[0] = this;
            par[1] = DBNAME;
            var dbName = DBNAME.Split('-')[2].Substring(0, DBNAME.Split('-')[2].Length - 3);
            var type = Type.GetType(string.Format("TwoPole.Chameleon3.Infrastructure.Services.DataBase{0},TwoPole.Chameleon3.Infrastructure", dbName));
            var db = (IDBCreate)Activator.CreateInstance(type, par);
            db.InitDataSql();
        }
        private void SensorLineSetting_Click(object sender, EventArgs e)
        {
            PlayActionText(this.GetString(Resource.String.SensorLineSettingStr));
            Intent intent = new Intent();
            intent.SetClass(this, typeof(SensorLineSetting));
            StartActivity(intent);
        }

        private void AboutUs_Click(object sender, EventArgs e)
        {

        }

        private void LightSetting_Click(object sender, EventArgs e)
        {
            PlayActionText(this.GetString(Resource.String.LightSimulationSettingStr));
            Intent intent = new Intent();
            intent.SetClass(this, typeof(LightSimulationConfig));
            StartActivity(intent);
        }

        private void SensorSetting_Click(object sender, EventArgs e)
        {
            PlayActionText(this.GetString(Resource.String.SensorSettingStr));
            Intent intent = new Intent();
            intent.SetClass(this, typeof(SensorSetting));
            StartActivity(intent);
        }

        private void ItemSetting_Click(object sender, EventArgs e)
        {
            PlayActionText(this.GetString(Resource.String.ItemSettingStr));
            Intent intent = new Intent();
            intent.SetClass(this, typeof(ItemSettingNew));
            StartActivity(intent);
        }

        private void GlobalSetting_Click(object sender, EventArgs e)
        {
            PlayActionText(this.GetString(Resource.String.GlobalSettingStr));
            Intent intent = new Intent();
            intent.SetClass(this, typeof(GlobalSetting));
            StartActivity(intent);
        }

        private void PlayActionText(string ActionText)
        {
            speaker.SpeakActionVoice(ActionText);
        }
    }
}