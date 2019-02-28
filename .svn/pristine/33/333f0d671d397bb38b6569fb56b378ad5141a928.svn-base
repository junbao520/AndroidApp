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
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Business.Modules;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure.Messages;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Map;
using Android.Graphics;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "LightSimulation")]
    public class LightSimulationNew :BaseSettingActivity
    {
        protected IDataService dataService;
        protected ISpeaker speaker;
        protected IMessenger messager;
        protected GlobalSettings Settings;
        private ILog Logger;
        private CarSignalInfo carSignal;
        private BrokenRuleMessage brokenRuleMessage;
        private LinearLayout mainLinerLayout;
        private RelativeLayout relativeLayout;
        List<BrokenRuleInfo> lstBorkenRuleInfo = new List<BrokenRuleInfo>();
        public IExamScore ExamScore { get; private set; }
        protected LightModule LightModule { get; set; }
        protected ILightExamItem LightExamItem { get; set; }

        //开始考试
        GridView mGridView;
        TextView tvScore;
        KeyValuePair<int, string> mapKeyValue = new KeyValuePair<int, string>();
        LightExamItem[] LightGroup;
        protected CarSignalReceivedMessage tempMessage { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LightSimulationNew);
            InitControl();
            initHeader(false);
            setMyTitle("灯光模拟");
          
            messager = Singleton.GetMessager;
            Logger = Singleton.GetLogManager;
            ExamScore = Singleton.GetExamScore;
            dataService = Singleton.GetDataService;
          
            InitLightGroup();
            InitBrokenRules();
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                //询问用户是否需要退出考试界面
                ShowConfirmDialog();
            }
            return base.OnKeyDown(keyCode, key);
        }
        public void ShowConfirmDialog()
        {
            try
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                AlertDialog alertDialog = builder
                   .SetTitle("提示")
                   .SetMessage("您是否需要退出灯光模拟项目?")
                   .SetPositiveButton("是",(s, e) =>
                   {
                       Free();
                       Finish();
                   })
                   .SetNeutralButton("否", (s, e) =>
                   {
                       //不保存数据进行返回
                   })
                   .Create();       //创建alertDialog对象  
                alertDialog.Show();
            }
            catch (Exception ex)
            {
                Logger.Error("LightSimulation" + ex.Message);
            }

        }
        public void InitControl()
        {
            mGridView = (GridView)FindViewById(Resource.Id.MapGridView);
            mGridView.ItemClick += MGridView_ItemClick;
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
        }
        protected bool CheckAuth(GpsInfo gpsInfo)
        {
            Authorization authorization = new Authorization(this);
            bool checkAuthState = authorization.CheckPeriod(gpsInfo);
            if (!checkAuthState)
            {
                string AuthCode = authorization.AuthorizationCode;
                string message = string.Format("设备识别码:{0},请联系销售人员激活软件。", AuthCode);
                speaker.PlayAudioAsync(message);
                return false;
            }

            return true;
        }
        private void MGridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                if (LightModule==null)
                {
                    Toast.MakeText(this, "灯光模拟正在加载中，请耐心等待", ToastLength.Long);
                    return;
                }

                if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                    return;
                //加上授权 //

                InitBrokenRules();
                lstBorkenRuleInfo.Clear();
                var Position = e.Position;

                if (Position >=LightGroup.Length)
                {
                    ExamScore.Reset();

                    StartLightAsync(string.Empty);
                }
                else
                {
                    string GroupName = LightGroup[Position].GroupName;
                    ExamScore.Reset();
                    StartLightAsync(GroupName);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            //获取点击的
        }
        private void InitBrokenRules()
        {

            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.table, null);
            mainLinerLayout.RemoveAllViews();

            TableTextView title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
            title.SetText("扣分项目", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);
            title.SetBackgroundColor(Color.Yellow);

            title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
            title.SetText("扣分", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);
            title.SetBackgroundColor(Color.Yellow);
            title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
            title.SetText("扣分原因", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);
            title.SetBackgroundColor(Color.Yellow);
            mainLinerLayout.AddView(relativeLayout);
        }
        private void ShowBrokenRule()
        {
            try
            {
                InitBrokenRules();
                lstBorkenRuleInfo.Add(brokenRuleMessage.RuleInfo);

                for (int i = lstBorkenRuleInfo.Count - 1; i >= 0; i--)
                {
                    mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.table, null);
                    TableTextView txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                    txt.SetText(lstBorkenRuleInfo[i].ExamItemName, TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);

                    txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
                    txt.SetText(lstBorkenRuleInfo[i].DeductedScores.ToString(), TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);

                    txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
                    txt.SetText(lstBorkenRuleInfo[i].RuleName, TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);
                    mainLinerLayout.AddView(relativeLayout);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ShowBorkenRule", ex.Message);
            }
        }
        protected void RegisterMessages(object objectmessenger)
        {
            InitModule();
            IMessenger messenger =(IMessenger)objectmessenger;
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
            messenger.Register<BrokenRuleMessage>(this, OnBrokenRule);

        }
        private void OnBrokenRule(BrokenRuleMessage message)
        {
            try
            {
                brokenRuleMessage = message;
                RunOnUiThread(ShowBrokenRule);
            }
            catch (Exception ex)
            {
                Logger.Error("HuaZhongBrokenRule", ex.Message);
            }
        }
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            if (message.CarSignal == null || message.CarSignal.Gps == null)
                return;
            carSignal = message.CarSignal;

            tempMessage = message;
            //更新界面UI 需要在UI线程操作
            if (LightExamItem != null && LightExamItem.State == ExamItemState.Progressing)
            {
                LightExamItem.Execute(message.CarSignal);
            }
            RunOnUiThread(UpdateCarSensorState);
        }
        public void UpdateCarSensorState()
        {
            tvScore.Text = string.Format("成绩:{0}分",ExamScore.Score);
        }



        private async Task StartLightAsync(string group)
        {
            if (LightExamItem != null && LightExamItem.State == ExamItemState.Progressing)
                await LightModule.StopAsync();
            await LightModule.StartAsync(new ExamContext { ExamGroup =group });
        }
        private void BtnRandom_Click(object sender, EventArgs e)
        {
            //会进行重置
            ExamScore.Reset();
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            mainLinerLayout.RemoveAllViews();
            StartLightAsync(string.Empty);
        }

        protected void InitModule()
        {
            LightModule = new LightModule();
            LightExamItem = LightModule.LightExamItem;
            LightModule.InitAsync(new ExamInitializationContext(MapSet.Empty)).Wait();
        }
        protected virtual void Free()
        {
            
            if (messager != null)
            {
                messager.Unregister(this);
                messager = null;
            }
            if (LightModule != null)
            {
                ReleaseModuleAsync();
            }
        }
        private async Task ReleaseModuleAsync()
        {
            await LightModule.StopAsync();
            LightModule.Dispose();
            LightModule = null;
        }
        /// <summary>
        /// 初始化灯光模拟分组
        /// </summary>
        public void InitLightGroup()
        {

            //一共19个考试项目
            try
            {
                LightGroup = dataService.AllLightExamItems;
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;

                foreach (var item in LightGroup)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] =Resource.Drawable.nav_turn_via_1;
                    dataItem["itemName"] = item.GroupName;
                    data.Add(dataItem);
                }
                dataItem = new JavaDictionary<string, object>();
                dataItem["itemImage"] = Resource.Drawable.nav_turn_via_1;
                dataItem["itemName"] = "随机";
                data.Add(dataItem);
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.LightGridView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });
                mGridView.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
                Logger.Error("Map", ex.Message);
            }
        }

    }
}