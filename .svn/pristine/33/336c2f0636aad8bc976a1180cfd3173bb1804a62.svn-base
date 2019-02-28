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
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3
{

    //启动界面
     //[Activity(Label= "易考星", MainLauncher = true, Icon = "@drawable/iconnew")]
    public class VersionSelectActivity: Activity
    {

        GridView VersionGridView;
        //点击每一项选择对应的考试版本进入不同的考试系统
        VersionInfo DefaultVersion = new VersionInfo
        {
            //主控箱版本 USB
            masterBoxVersion = MasterControlBoxVersion.USB,
            //考试界面
            UIType = SystemUIType.JingYing,
            //数据库
            DataBaseName = DataBase.JingTang,
            //是否显示广告
            IsShowAds = true,
            //是否播放操作语音
            IsPlayActionVoice = true,
            //是否禁止版本切换
            IsForbiddenVersionChange = true,
            //是否播放背景音乐
            IsPlayBackgroundMusic = true,
            //广告显示时间
            AdsShowTime = 3000,
            //是否创建文件
            IsCreateFile = true,
            //TODO:是否使用IOC 框架 2018.3.23 鲍君
            IsUseIoc = true,
            //广告ShowTime
            WelcomeVoice = "欢迎使用易考星"
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VersionSelect);
            try
            {
                InitControl();
                InitExamRoomInfo();
                InitVersionView();
               
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog(ex.Message);
            }
           

            // Create your application here
        }

        public void InitControl()
        {

            VersionGridView= (GridView)FindViewById(Resource.Id.VersionGridView);
            VersionGridView.ItemClick += VersionGridView_ItemClick;

            ImageView imageViewSubject2 = FindViewById<ImageView>(Resource.Id.mgViewExamSubject2);
            imageViewSubject2.Click += ImageViewSubject2_Click;
        }

        private void ImageViewSubject2_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "系统正在开发中,精确期待!", ToastLength.Long).Show();
        }

        //考场
        List<KeyValuePair<int, KeyValuePair<string, string>>> listExamRoom = new List<KeyValuePair<int, KeyValuePair<string, string>>>();
        public void InitExamRoomInfo()
        {
            var Item1= new KeyValuePair<int, KeyValuePair<string, string>>(Resource.Drawable.hainanqb, new KeyValuePair<string, string>("琼北考场", DataBase.QiongBei));
            var Item2= new KeyValuePair<int, KeyValuePair<string, string>>(Resource.Drawable.hainanhaikou, new KeyValuePair<string, string>("海口考场", DataBase.HaiKou));
            var Item3 = new KeyValuePair<int, KeyValuePair<string, string>>(Resource.Drawable.hainandongfang, new KeyValuePair<string, string>("东方考场", DataBase.DongFang));
            var Item4 = new KeyValuePair<int, KeyValuePair<string, string>>(Resource.Drawable.hainansanya, new KeyValuePair<string, string>("三亚考场", DataBase.SanYa));

            listExamRoom.Add(Item1);
            listExamRoom.Add(Item2);
            listExamRoom.Add(Item3);
            listExamRoom.Add(Item4);
        }


        private string DBName = string.Empty;

        //TODO:后面可以 考虑加上语音也没有太大的必要
        private void VersionGridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            DBName = listExamRoom[e.Position].Value.Value;
          
            //打开MainActivity
             Handler StartMainActivityHandler = new Handler();
             StartMainActivityHandler.PostDelayed(StartMainActivity, 1000);
            //是不是需要PostDel
        

        }

        //跳转到主页面
        public void StartMainActivity()
        {
            try
            {
                LogManager.WriteSystemLog("DBName:" + DBName);
                Intent intent = new Intent();
                intent.SetClass(this, typeof(MainActivity));
                //通过参数把数据库名称传递过去
                intent.PutExtra("DbName", DBName);
                StartActivity(intent);
                Finish();
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog(ex.Message);
            }

        }



        public void InitVersionView()
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;

                foreach (var item in listExamRoom)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] = item.Key;
                    dataItem["itemName"] = item.Value.Key;
                    data.Add(dataItem);
                }
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.VersionGridView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });
                VersionGridView.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
           
                string msg = ex.Message;
                LogManager.WriteSystemLog("VersionSelectActivity" + msg);
            }

        }
    }
}