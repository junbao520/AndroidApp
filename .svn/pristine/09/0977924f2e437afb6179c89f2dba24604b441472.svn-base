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
using Android.Graphics;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Business.Modules;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;
using Android.Content.Res;
using System.Diagnostics;
using TwoPole.Chameleon3.Infrastructure.Services;
using Android.Util;

namespace TwoPole.Chameleon3
{
    //todo：有时间需要重新整理下通用的基类 以及考试界面的编写以及布局
    //todo: 需要形成一个规范化的流程可以减少开发时间
    [Activity(Label = "DuoLunNew")]
    public class DuoLunNew : BaseExamActivity
    {
        private LinearLayout mainLinerLayout;
        private LinearLayout relativeLayout;
        public ExamContext ExamContext { get; private set; }
        private List<KeyValuePair<Int32, string>> lstKeyValueExamItems = new List<KeyValuePair<int, string>>();
        protected ExamModule Module { get; set; }

        private bool StartEndStatus = true;

        private IDataService dataService;
        ISpeaker speaker;
        private IMessenger messager;
        private ILog Logger;
        CarSignalInfo carSignal;
        BrokenRuleMessage brokenRuleMessage;
        ExamItemStateChangedMessage examItemStateChangedMessage;


        Button btnStartExam;
        Button btnReturnHome;

        Button btnArtificialEvaluation;
        TextView tvStartTime;
        TextView tvScore;
        TextView tvUseTime;
        TextView tvSpeed;
        TextView tvEngineRatio;
        TextView tvMileage;
        TextView tvExamItem;
        TextView tvMap;


        Spinner MapSpinner;
        Spinner ExamModeSpinner;

        //考试界面信号显示
        TextView tvAngle;
        TextView tvGear;



        List<ColorStateList> lstExamItemColor = new List<ColorStateList>();

        List<string> lstExamItem = new List<string>();

        private bool IsTrainning = false;
        private int ExamDistance = 0;
        private bool IsAutoTriggerPullOver = false;
        private bool IsTriggerPullOver = false;
        private string ExamSuccessVoice = string.Empty;
        private string ExamFailVoice = string.Empty;
        private CarSignalReceivedMessage tempMessage { get; set; }
        private GlobalSettings globalSetting { get; set; }
        Authorization authorization;
        List<BrokenRuleInfo> lstRules = new List<BrokenRuleInfo>();

   
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DuoLunNew);
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            dataService = Singleton.GetDataService;
            Logger = Singleton.GetLogManager;
            globalSetting = dataService.GetSettings();
            authorization = new Authorization(this);
            InitParameter();
            GetIntentParameter();
            InitBrokenRules();
            InitUI();
            //TODO:考试界面状态恢复中控上面用不到
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

        }
        public List<TwoPole.Chameleon3.Domain.DeductionRule> GetDeductionRule()
        {
            string currentExamItemCode = dataService.GetExamItemCode(currentExam);
            List<string> lstExamCode = new List<string> { currentExamItemCode, ExamItemCodes.CommonExamItem };
            return dataService.GetDeductionRuleByExamItem(lstExamCode);
        }
        AlertDialog alertDialog = null;
        AlertDialog.Builder builder = null;
        public void ArtificialEvaluation()
        {

            View view = View.Inflate(this, Resource.Layout.Dialog_ArtificialEvaluation, null);
            TableLayout tableLayout = (TableLayout)view.FindViewById(Resource.Id.DeductionRuleTable);
            tableLayout.Click += TableLayout_Click;
            var result = GetDeductionRule();


            foreach (var item in result)
            {
                TableRow tableRow = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.tableArtificialEvaluation, null);
                TextView tvDeductionReason = (TextView)tableRow.FindViewById(Resource.Id.tvDeductionReason);
                TextView tvDeductionScore = (TextView)tableRow.FindViewById(Resource.Id.tvDeductionScore);

                Button btnDeduction = (Button)tableRow.FindViewById(Resource.Id.btnDeduction);
                tvDeductionReason.Text = item.DeductedReason;
                tvDeductionScore.Text = item.DeductedScores.ToString();
                btnDeduction.Text = "扣分";
                btnDeduction.Tag = item.RuleCode + "," + item.SubRuleCode;
                btnDeduction.Click += BtnDeduction_Click;
                tableLayout.AddView(tableRow);
            }

            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("人工评判")
             .SetNeutralButton("返回", (s, e) =>
              {
                  //不保存数据进行返回
              })
            .SetView(view)
            .Create();       //创建alertDialog对象  

            alertDialog.Show();

            //根据屏幕分辨路不同
            DisplayMetrics dm = new DisplayMetrics();
            base.WindowManager.DefaultDisplay.GetMetrics(dm);
            //宽高都是3/2 
            alertDialog.Window.SetLayout(dm.WidthPixels / 3 * 2, dm.HeightPixels / 3 * 2);

        }

        private void BtnDeduction_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var Tag = btn.Tag.ToString();
            var ruleCode = Tag.Split(',')[0];
            var subRuleCode = Tag.Split(',')[1];

            if (string.IsNullOrEmpty(subRuleCode))
            {
                subRuleCode = null;
            }

            var examScore = Singleton.GetExamScore;
            var examItemCode = dataService.GetExamItemCode(currentExam);
            examScore.BreakRule(examItemCode, currentExam, ruleCode, subRuleCode);

            //取消对话框
            alertDialog.Cancel();

        }

        private void TableLayout_Click(object sender, EventArgs e)
        {

        }

        //关闭对话框
        private void BtnReturn_Click(object sender, EventArgs e)
        {
            alertDialog.Cancel();
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            //保存界面的考试状态
            outState.PutBoolean("StartEndStatus", StartEndStatus);

        }
        public void InitParameter()
        {
            ExamDistance = globalSetting.ExamDistance;
            ExamSuccessVoice = globalSetting.CommonExamItemExamSuccessVoice;
            ExamFailVoice = globalSetting.CommonExamItemExamFailVoice;
            IsAutoTriggerPullOver = globalSetting.EndExamByDistance;
            currentExam = "综合评判";
        }
        public void GetIntentParameter()
        {
            IsTrainning = Intent.GetStringExtra("ExamMode") == "Train";
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string msg = e.Exception.Message + e.Exception.StackTrace;
            LogManager.WriteSystemLog(msg);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                ShowConfirmDialog();
            }
            return base.OnKeyDown(keyCode, key);
        }
        protected void Free()
        {
            if (messager != null)
            {
                messager.Unregister(this);
                messager = null;
            }
            if (Module != null)
            {
                ReleaseModuleAsync();
            }
            if (speaker != null)
            {
                speaker.CancelAllAsync();
            }
        }
        private async Task ReleaseModuleAsync()
        {
            await Module.StopAsync(true);
            Module.Dispose();
            Module = null;
        }
        public void ShowConfirmDialog()
        {
            try
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                AlertDialog alertDialog = builder
                   .SetTitle("提示")
                   .SetMessage("您是否需要退出考试界面?")
                   .SetPositiveButton("是", (s, e) =>
                   {
                       //退出考试界面的时候直接当结束考试
                       //直接取消所有的语音播报
                       speaker.StopAllSpeak();
                       if (ExamContext.IsExaming)
                       {
                           //调用考试流程进行结束考试
                           Module.StopAsync().ContinueWith((task) =>
                           {
                           });
                       }
                       Free();
                       Finish();
                   })
                   .SetNeutralButton("否", (s, e) =>
                   {
                       //不保存数据进行返回
                   })
                   .Create();//创建alertDialog对象  
                alertDialog.Show();
            }
            catch (Exception ex)
            {
                Logger.Error("DuoLunSensorShowConfirmDialog" + ex.Message);
            }

        }
        private void InitUI()
        {
            InitControl();
            //TODO:初始化显示授权信息,最后三天会以红字提醒
            InitAuthInfo();
            InitExamItemColor();
            InitExamItem();
            InitMapLines();
            InitExamContext();
            InitModuleAsync(ExamContext.Map);
            // RegisterMessages(messager);
        }
        public void InitExamContext()
        {
            ExamContext = new ExamContext();
            ExamContext.ExamMode = IsTrainning ? ExamMode.Training : ExamMode.Examming;
            ExamContext.ExamTimeMode = ExamTimeMode.Day;
            ExamContext.Map = MapSet.Empty;
        }
        public void InitExamItemColor()
        {
            lstExamItemColor = new List<ColorStateList>();
            //可以去掉有一些项目 比如 上车准备 减速让行 
            lstExamItem = (from v in dataService.GetExamItemsList()
                           select v.ItemName).ToList<string>();

            for (int i = 0; i < lstExamItem.Count; i++)
            {
                lstExamItemColor.Add(ColorStateList.ValueOf(Color.Black));
            }
            for (int i = 0; i < lstExamItem.Count; i++)
            {
                if (lstExamItem[i] == "公共汽车站")
                {
                    lstExamItem[i] = "公交汽车";
                }
            }
        }

        public void InitAuthInfo()
        {
            try
            {
                DateTime? dtAuthTime = authorization.GetAuthorizationTime();
                if (dtAuthTime != null)
                {
                    if ((dtAuthTime.Value - DateTime.Now).TotalDays <= 3)
                    {

                        tvMileage.SetTextColor(Color.Red);
                        tvMileage.Text = string.Format("项目列表:  {0}", authorization.GetAuthorizationInfo());
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.Error("InitAuthInfo", ex.Message);
                Logger.Error(ex, GetType().ToString());
                tvMileage.Text = string.Format("授权异常", ex.Message);
            }

        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            MapSpinner = (Spinner)FindViewById(Resource.Id.MapSpiner);
            tvStartTime = FindViewById<TextView>(Resource.Id.tvStartTime);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
            tvUseTime = FindViewById<TextView>(Resource.Id.tvUseTime);
            tvSpeed = FindViewById<TextView>(Resource.Id.tvSpeed);
            tvEngineRatio = FindViewById<TextView>(Resource.Id.tvEngineRatio);
            tvMileage = FindViewById<TextView>(Resource.Id.tvMileage);
            tvExamItem = FindViewById<TextView>(Resource.Id.tvExamItem);
            tvMap = FindViewById<TextView>(Resource.Id.tvMap);
            btnStartExam = FindViewById<Button>(Resource.Id.btnStartExam);
            btnReturnHome = FindViewById<Button>(Resource.Id.btnReturnHome);
            btnArtificialEvaluation = FindViewById<Button>(Resource.Id.btnArtificialEvaluation);
            tvAngle = FindViewById<TextView>(Resource.Id.tvAngle);
            tvGear = FindViewById<TextView>(Resource.Id.tvGear);
            btnReturnHome.Click += BtnReturnHome_Click;
            btnStartExam.Click += btnStartExam_Click;
            btnArtificialEvaluation.Click += BtnArtificialEvaluation_Click;
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;

            Button btnStraightDriving = FindViewById<Button>(Resource.Id.btnStraightDriving);
            Button btnMeeting = FindViewById<Button>(Resource.Id.btnMeeting);
            Button btnChangeLanes = FindViewById<Button>(Resource.Id.btnChangeLanes);
            Button btnOvertake = FindViewById<Button>(Resource.Id.btnOvertake);
            Button btnTurnRound = FindViewById<Button>(Resource.Id.btnTurnRound);
            Button btnPullOver = FindViewById<Button>(Resource.Id.btnPullOver);

            btnStraightDriving.Click += ExamButton;
            btnMeeting.Click += ExamButton;
            btnChangeLanes.Click += ExamButton;
            btnOvertake.Click += ExamButton;
            btnTurnRound.Click += ExamButton;
            btnPullOver.Click += ExamButton;



        }

        private void ExamButton(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var ItemCode = string.Empty;
            switch (btn.Text)
            {
                case "直线":
                    ItemCode = ExamItemCodes.StraightDriving;
                    break;
                case "会车":
                    ItemCode = ExamItemCodes.Meeting;
                    break;
                case "变道":
                    ItemCode = ExamItemCodes.ChangeLanes;
                    break;
                case "超车":
                    ItemCode = ExamItemCodes.ChangeLanes;
                    break;
                case "掉头":
                    ItemCode = ExamItemCodes.TurnRound;
                    break;
                case "停车":
                    ItemCode = ExamItemCodes.PullOver;
                    break;
            }
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ItemCode, null);
            }
        }

        private void BtnArtificialEvaluation_Click(object sender, EventArgs e)
        {
            try
            {
                base.ArtificialEvaluation();
            }
            catch (Exception ex)
            {
                Logger.Error("ArtificialEvaluation" + ex.Message + ex.Source + ex.StackTrace);
            }

        }

        private void BtnReturnHome_Click(object sender, EventArgs e)
        {
            ShowConfirmDialog();
        }


        //设置考试模式是白考还是夜间考试
        private void ExamModeSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ExamContext.ExamTimeMode = (ExamTimeMode)(e.Position + 1);
        }

        private void MapSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var SelectItemPosition = e.Position;

            if (SelectItemPosition > 0 && ExamContext != null)
            {
                var SelectMapItem = dataService.GetAllMapLines()[SelectItemPosition - 1];
                tvMap.Text = SelectMapItem.Name;
                if (SelectMapItem.Points != null)
                {
                    var mapSet = new MapSet(SelectMapItem.Points.ToMapPoints());
                    ExamContext.Map = mapSet;

                    dataService.SaveDefaultMapId(SelectMapItem.Id);
                }
            }

        }

        protected void RegisterMessages(object Objectmessenger)
        {
            IMessenger messenger = (IMessenger)Objectmessenger;
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
            messenger.Register<BrokenRuleMessage>(this, OnBrokenRule);
            messenger.Register<ExamItemStateChangedMessage>(this, OnExamItemStateChanged);
            messager.Register<ExamFinishingMessage>(this, OnExamFinishingMessage);
            messager.Register<PullOverTriggerMessage>(this, OnPullOverTrigger);
            messager.Register<VehicleStartingMessage>(this, OnVehicleStartTrigger);
        }
        private void OnVehicleStartTrigger(VehicleStartingMessage message)
        {
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.Start, null);
            }
        }
        /// <summary>
        /// 自动触发靠边停车
        /// </summary>
        /// <param name="message"></param>
        private void OnPullOverTrigger(PullOverTriggerMessage message)
        {
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.PullOver, null);
            }
        }
        //private void OnStartExamTrigger(start)
        //靠边停车触发考试结束
        private void OnExamFinishingMessage(ExamFinishingMessage message)
        {
            RunOnUiThread(ExamFinishing);
        }
        public void ExamFinishing()
        {
            btnStartExam.Text = "开始考试";
            StartEndStatus = true;
            tvMap.Visibility = ViewStates.Invisible;
            MapSpinner.Visibility = ViewStates.Visible;
            MapSpinner.Enabled = true;
            //初始化人工评判不可以使用
            btnArtificialEvaluation.Enabled = false;
            ExamContext.EndExamTime = DateTime.Now;
            EndExam();
        }
        private void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            try
            {
                examItemStateChangedMessage = message;
                var AllExamItems = dataService.GetExamItemsList();
                if (message.ExamItem.Name == "综合评判")
                {
                    //if else if else if
                    return;
                }
                for (var i = 0; i < AllExamItems.Count(); i++)
                {
                    if (AllExamItems[i].ItemCode == message.ExamItem.ItemCode)
                    {
                        if (ExamItemState.Progressing == message.NewState)
                        {
                            lstExamItemColor[i] = ColorStateList.ValueOf(Color.DarkRed);
                        }
                        else if (ExamItemState.Finished == message.NewState)
                        {
                            lstExamItemColor[i] = ColorStateList.ValueOf(Color.Green);
                        }
                    }
                }
                //考试状态改变
                RunOnUiThread(ExamItemChange);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, GetType().ToString());
                // Logger.Error("DuoLunOnExamItemStateChanged", ex.Message);
            }
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
                Logger.Error(ex, GetType().ToString());
            }
        }
        protected bool CheckAuth(GpsInfo gpsInfo)
        {
            bool checkAuthState = authorization.CheckPeriod(gpsInfo);
            if (!checkAuthState)
            {
                string AuthCode = authorization.GetAuthorizationCode();
                string message = string.Format("设备识别码:{0},请联系销售人员激活软件。", AuthCode);
                speaker.PlayAudioAsync(message);
                return false;
            }

            return true;
        }
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            try
            {
                if (message.CarSignal == null || message.CarSignal.Gps == null)
                    return;

                tempMessage = message;

                carSignal = message.CarSignal;

                RunOnUiThread(UpdateCarSensorState);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, GetType().ToString());
            }

        }
        public void btnStartExam_Click(object sender, EventArgs e)
        {
            if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                return;
            //开始考试点击之后 需要进行图片切换
            if (StartEndStatus)
            {
                btnStartExam.Text = "结束考试";
                StartEndStatus = false;
                btnArtificialEvaluation.Enabled = true;
                tvMap.Visibility = ViewStates.Visible;
                MapSpinner.Visibility = ViewStates.Invisible;
                //设置地图选项和白考也考选项
                MapSpinner.Enabled = false;
                IsTriggerPullOver = false;
                //初始化开始项目颜色
                InitExamItemColor();
                InitExamItem();
                StartExam();
            }
            else
            {
                btnArtificialEvaluation.Enabled = false;
                ExamFinishing();
            }

        }


        protected void InitMapLines()
        {
            try
            {
                var mapLines = dataService.ALLMapLines;
                MapLine mapLine = new MapLine() { Id = 0, Name = "无地图" };
                List<MapLine> lstMapLine = new List<MapLine>();
                lstMapLine.Add(mapLine);
                lstMapLine.AddRange(mapLines);
                List<string> lstMaps = new List<string>();
                foreach (var item in lstMapLine)
                {
                    lstMaps.Add(item.Name);
                }
                BindMapSpinner(lstMaps);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, GetType().ToString());
            }
        }
        protected virtual async Task InitModuleAsync(IMapSet map)
        {
            Module = new ExamModule();
            await Module.InitAsync(new ExamInitializationContext(map)).ContinueWith(task =>
            {

            });
        }
        public void StartExam()
        {
            //第一步首先清楚上一次考试项目的扣分规则
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            mainLinerLayout.RemoveAllViews();
            lstRules.Clear();
            InitBrokenRules();
            tvStartTime.Text = DateTime.Now.ToString("HH:mm:ss");
            ExamContext.StartExam();
            Module.StartAsync(ExamContext).ContinueWith((task) =>
            {
            });
        }

        public void EndExam()
        {
            Module.StopAsync().ContinueWith((task) =>
            {

            });
        }


        //Remove之后进行刷新重新显示
        private void ShowBrokenRule()
        {
            try
            {
                //我需要获取一个索引
                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                mainLinerLayout.RemoveAllViews();
                BrokenRuleInfo RuleInfo = brokenRuleMessage.RuleInfo;
                lstRules.Add(RuleInfo);
                //lstRules.Reverse();
                //todo:为什么要-1 ？
                for (int i = lstRules.Count; i > 0; i--)
                {
                    //数组索引从0开始
                    var item = lstRules[i - 1];
                    relativeLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableNew, null);
                    TableTextView txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                    txt.SetText(item.ExamItemName, TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);
                    txt.Tag = item.Id.ToString();
                    txt.LongClick += Txt_LongClick;

                    //txt.SetOnClickListener = (new View:O);

                    txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
                    txt.SetText(item.DeductedScores.ToString(), TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);

                    txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
                    txt.SetText(item.RuleName, TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);

                    mainLinerLayout.AddView(relativeLayout);
                }

            }
            catch (Exception ex)
            {
                Logger.Error("ShowBorkenRule", ex.Message);
            }
        }
        //这样是可以的但是没有办法定位到索引
        private void Txt_LongClick(object sender, View.LongClickEventArgs e)
        {
            try
            {
                TableTextView tv = (TableTextView)sender;
                int id = Convert.ToInt32(tv.Tag.ToString().Trim());
                // Toast.MakeText(this, id.ToString(), ToastLength.Long);

                var examContentRuleItem = ExamContext.Rules.Where(s => s.Id == id).FirstOrDefault();
                ExamContext.Rules.Remove(examContentRuleItem);
                //把分数加回去
                ExamContext.Score.AddScore(examContentRuleItem.DeductedScores);
                //更新分数
                tvScore.Text = string.Format("{0}", ExamContext.ExamScore);

                //扣分之后需要把分加回去

                var removeItem = lstRules.Where(s => s.Id == id).FirstOrDefault();
                lstRules.Remove(removeItem);

                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                mainLinerLayout.RemoveAllViews();
                //不可以只移除界面显示需要移除内存里面的东西
                for (int i = lstRules.Count; i > 0; i--)
                {
                    var item = lstRules[i - 1];
                    relativeLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableNew, null);
                    TableTextView txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                    txt.SetText(item.ExamItemName, TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);

                    txt.Tag = item.Id.ToString();
                    txt.LongClick += Txt_LongClick;


                    txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
                    txt.SetText(item.DeductedScores.ToString(), TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);

                    txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
                    txt.SetText(item.RuleName, TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);

                    mainLinerLayout.AddView(relativeLayout);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, GetType().ToString());
            }

        }

        private void ExamItemChange()
        {
            try
            {
                var ExamItemName = examItemStateChangedMessage.ExamItem.Name;
                tvExamItem.Text = ExamItemName;
                currentExam = ExamItemName;
                InitExamItem();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, GetType().ToString());
            }
        }
        private void InitBrokenRules()
        {
            //todo:界面上已经画好了 初始化的标题栏
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            relativeLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableNew, null);
            // relativeLayout.Click += RelativeLayout_Click;
        }

        /// <summary>
        /// 绑定地图控件下拉列表
        /// </summary>
        private void BindMapSpinner(List<string> lstMaps)
        {
            //MapSpiner
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstMaps);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            MapSpinner.Adapter = adapter;
            MapSpinner.Visibility = ViewStates.Visible;
        }

        private void BindExamModeSpinner()
        {
            List<string> Maps = new List<string> { "白天", "夜间" };
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, Maps);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ExamModeSpinner.Adapter = adapter;
            ExamModeSpinner.Visibility = ViewStates.Visible;

        }
        public void UpdateCarSensor()
        {
            //todo：没有需要更新的图标
        }
        /// <summary>
        /// 更新车辆信号的状态
        /// </summary>
        public void UpdateCarSensorState()
        {
            try
            {
                //更新一些基本的信
                if (!StartEndStatus)
                {
                    tvScore.Text = string.Format("{0}", ExamContext.ExamScore);
                    tvUseTime.Text = string.Format("{0:00}:{1:00}:{2:00}", carSignal.UsedTime.Hours, carSignal.UsedTime.Minutes, carSignal.UsedTime.Seconds);
                    tvMileage.Text = string.Format("里程:{0:0}米", carSignal.Distance);
                }
                tvSpeed.Text = string.Format("速度:{0:0}", carSignal.SpeedInKmh);
                tvEngineRatio.Text = string.Format("转速:{0}", carSignal.Sensor.EngineRpm);
                tvGear.Text = "档位:" + Convert.ToInt32(carSignal.Sensor.Gear).ToString();

                tvAngle.Text = "角度:" + string.Format("{0:N2}°", carSignal.BearingAngle);
                UpdateCarSensor();
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("UpdateCarSensorState发生异常,原因:{0} 原始信号{1}", exp.Message, string.Join(",", carSignal.commands));
            }


        }
        //TODO:应该把这个改成TableView
        //可以参考扣分规则 基类改成双层循环 减少重复性代码
        private void InitExamItem()
        {
            try
            {
                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.ExamItemTable);
                mainLinerLayout.RemoveAllViews();
                TextView txt;
                int ListID = 0;

                //所有的考试
                //所有的考试项目但是不包含综合评判的
                int ExamItemCount = lstExamItem.Count;
                for (int i = 0; i < ExamItemCount / 5; i++)
                {
                    relativeLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemNew, null);
                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                    txt.SetText(lstExamItem[i * 5], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 5]);
                    txt.Click += ExamItemClick;
                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
                    txt.SetText(lstExamItem[i * 5 + 1], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 5 + 1]);
                    txt.Click += ExamItemClick;

                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
                    txt.SetText(lstExamItem[i * 5 + 2], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 5 + 2]);
                    txt.Click += ExamItemClick;

                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_4);
                    txt.SetText(lstExamItem[i * 5 + 3], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 5 + 3]);
                    txt.Click += ExamItemClick;

                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_5);
                    txt.SetText(lstExamItem[i * 5 + 4], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 5 + 4]);
                    txt.Click += ExamItemClick;

                    mainLinerLayout.AddView(relativeLayout);
                }
                //4如果余数大
                int Count = ExamItemCount - (ExamItemCount / 5) * 5;
                if (Count > 0)
                {
                    int StartIndex = (ExamItemCount / 5) * 5;
                    relativeLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemNew, null);
                    for (int i = 0; i < 5; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                ListID = Resource.Id.list_1_1;
                                break;
                            case 1:
                                ListID = Resource.Id.list_1_2;
                                break;
                            case 2:
                                ListID = Resource.Id.list_1_3;
                                break;
                            case 3:
                                ListID = Resource.Id.list_1_4;
                                break;
                            case 4:
                                ListID = Resource.Id.list_1_5;
                                break;
                            default:
                                break;
                        }

                        if (i >= Count)
                        {
                            txt = (TextView)relativeLayout.FindViewById(ListID);
                            txt.SetText("", TextView.BufferType.Normal);
                            txt.Visibility = ViewStates.Invisible;
                        }
                        else
                        {
                            txt = (TextView)relativeLayout.FindViewById(ListID);
                            txt.SetTextColor(lstExamItemColor[StartIndex + i]);
                            txt.SetText(lstExamItem[StartIndex + i], TextView.BufferType.Normal);
                            txt.Click += ExamItemClick;
                        }


                    }
                    mainLinerLayout.AddView(relativeLayout);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, GetType().ToString());
            }

        }

        public void ExamBtnClick(View view)
        {
            var ItemCode = string.Empty;
            switch (view.Id)
            {
                case Resource.Id.btnStraightDriving:
                    ItemCode = ExamItemCodes.StraightDriving;
                    break;
                case Resource.Id.btnMeeting:
                    ItemCode = ExamItemCodes.Meeting;
                    break;
                case Resource.Id.btnChangeLanes:
                    ItemCode = ExamItemCodes.ChangeLanes;
                    break;
                case Resource.Id.btnOvertake:
                    ItemCode = ExamItemCodes.ChangeLanes;
                    break;
                case Resource.Id.btnTurnRound:
                    ItemCode = ExamItemCodes.ChangeLanes;
                    break;
                case Resource.Id.btnPullOver:
                    ItemCode = ExamItemCodes.PullOver;
                    break;
            }
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ItemCode, null);
            }
        }
        public void ExamItemClick(object sender, EventArgs e)
        {
            try
            {
                TextView tvExamItem = (TextView)sender;
                int id = tvExamItem.Id;

                string ItemCode = dataService.GetExamItemCode(tvExamItem.Text);
                if (!string.IsNullOrEmpty(ItemCode))
                {
                    if (ExamContext.IsExaming)
                    {
                        Module.StartExamItemManualAsync(ExamContext, ItemCode, null);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, GetType().ToString());
            }


        }



    }
}