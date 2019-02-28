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
using TwoPole.Chameleon3.Business.JingTang.Modules;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;
using Android.Content.Res;

namespace TwoPole.Chameleon3
{
    /// <summary>
    /// 成都金堂考场
    /// </summary>
    [Activity(Label = "JingTang")]
    public class JingTangJieDa : Activity
    {
        private LinearLayout mainLinerLayout;
        private RelativeLayout relativeLayout;
        public ExamContext ExamContext { get; private set; }
        private List<KeyValuePair<Int32, string>> lstKeyValueExamItems = new List<KeyValuePair<int, string>>();
        protected ExamModule Module { get; set; }

        private bool IsExame = false;

        private IDataService dataService;
        ISpeaker speaker;
        private IMessenger messager;
        private ILog Logger;
        //扣分的序号
        //扣分的序号
        //
        //扣分的序号
        private int BreakRuleIndex = 0;
        CarSignalInfo carSignal;
        BrokenRuleMessage brokenRuleMessage;
        ExamItemStateChangedMessage examItemStateChangedMessage;
        Button btnStartExam;
        Button btnEndExam;
        Button btnStartPullOver;
        Button btnPrepareDrivingFinish;
        TextView tvStartTime;
        TextView tvUseTime;
        TextView tvMileage;
        Spinner MapSpinner;
        Spinner ExamItemSpinner;
        RadioButton radMap;
        RadioButton radExamItem;
        CheckBox chkClosePrepareDriving;
        CheckBox chkCloseSimulationLights;
        CheckBox chkExamFailEnd;
        List<ColorStateList> lstExamItemColor = new List<ColorStateList>();

        List<string> lstExamItem = new List<string>();

        private bool IsTrainning = false;
        private bool IsTriggerPullOver = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.JingTangJieDa);
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            dataService = Singleton.GetDataService;
            Logger = Singleton.GetLogManager;
            GetIntentParamete();
            InitBrokenRules();
            InitUI();
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);

            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

        }
        public void GetIntentParamete()
        {
            IsTrainning = Intent.GetStringExtra("ExamMode") == "Train";
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string msg = e.Exception.Message;
            Logger.Error("Environement", msg);
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
                Logger.Error("DuoLunShowConfirmDialog" + ex.Message);
            }

        }
        private void InitUI()
        {
            InitControl();
            InitExamItem();
            InitMapLines();
            InitExamContext();
            ShowBrokenRule();
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
            lstExamItem = (from v in dataService.GetExamItemsList()
                           select v.ItemName).ToList<string>();
            for (int i = 0; i < lstExamItem.Count; i++)
            {
                lstExamItemColor.Add(ColorStateList.ValueOf(Color.Blue));
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {


            MapSpinner = (Spinner)FindViewById(Resource.Id.MapSpiner);
            ExamItemSpinner = (Spinner)FindViewById(Resource.Id.ExamItemSpiner);
            tvStartTime = FindViewById<TextView>(Resource.Id.tvStartTime);
            tvUseTime = FindViewById<TextView>(Resource.Id.tvUseTime);
            tvMileage = FindViewById<TextView>(Resource.Id.tvMileage);
            btnStartExam = FindViewById<Button>(Resource.Id.btnStartExam);
            btnEndExam = FindViewById<Button>(Resource.Id.btnEndExam);
            btnPrepareDrivingFinish = FindViewById<Button>(Resource.Id.btnFinishPrepareDriving);
            btnStartPullOver = FindViewById<Button>(Resource.Id.btnPullOver);

             radMap = FindViewById<RadioButton>(Resource.Id.radMap);
             radExamItem = FindViewById<RadioButton>(Resource.Id.radExamItem);
             chkClosePrepareDriving = FindViewById<CheckBox>(Resource.Id.chkClosePrepareDriving);
             chkCloseSimulationLights = FindViewById<CheckBox>(Resource.Id.chkCloseSimulationLights);
             chkExamFailEnd = FindViewById<CheckBox>(Resource.Id.chkExamFailEnd);
            radMap.Click += RadMap_Click;
            radExamItem.Click += RadExamItem_Click;
            btnStartExam.Click += BtnStartExam_Click;
            btnEndExam.Click += BtnEndExam_Click;
            btnPrepareDrivingFinish.Click += BtnPrepareDrivingFinish_Click;
            btnStartPullOver.Click += BtnStartPullOver_Click;
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
            ExamItemSpinner.ItemSelected += ExamItemSpinner_ItemSelected;

        }

        private void RadExamItem_Click(object sender, EventArgs e)
        {
            if (radExamItem.Checked)
            {
                radMap.Checked = false;
            }
        }

        private void RadMap_Click(object sender, EventArgs e)
        {
            if (radMap.Checked)
            {
                radExamItem.Checked = false;
            }
        }

        private void BtnStartPullOver_Click(object sender, EventArgs e)
        {
            btnStartPullOver.Enabled = true;
            ExamItemClick("靠边停车");
        }

        private void BtnPrepareDrivingFinish_Click(object sender, EventArgs e)
        {
            btnPrepareDrivingFinish.Enabled = false;
            //发送上车准备完成的消息
            messager.Send<PrepareDrivingFinishedMessage>(new PrepareDrivingFinishedMessage());
        }

        private void BtnEndExam_Click(object sender, EventArgs e)
        {
            btnPrepareDrivingFinish.Enabled = true;
            btnStartPullOver.Enabled = true;
      
            btnStartExam.Enabled = true;
            btnEndExam.Enabled = false;
            MapSpinner.Enabled = true;
            ExamItemSpinner.Enabled = true;
            btnPrepareDrivingFinish.Enabled = false;
            btnStartPullOver.Enabled = false;
            ExamFinishing();
        }

        private void BtnStartExam_Click(object sender, EventArgs e)
        {
            try
            {
               
                MapSpinner.Enabled = false;
                IsExame = true;
                IsTriggerPullOver = false;
                if (!radExamItem.Checked)
                {
                    ExamItemSpinner.Enabled = false;
                }
                else
                {
                    ExamItemSpinner.Enabled = true;
                }
                btnStartExam.Enabled = false;
                btnEndExam.Enabled = true;
                btnPrepareDrivingFinish.Enabled = true;
                btnStartPullOver.Enabled = true;
                StartExam();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message + ":" + ex.StackTrace);
            }
           
        }

        private void ExamItemSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
           var SelectItemPosition = e.Position;

            //判断处理一下，主要是初始化就会进入这个函数

            if (!IsExame)
            {
                return;
            }
         ExamItemClick(lstExamItem[SelectItemPosition]);
        }

        //设置考试模式是白考还是夜间考试

        private void MapSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var SelectItemPosition = e.Position;

            if (SelectItemPosition > 0 && ExamContext != null)
            {
                var SelectMapItem = dataService.GetAllMapLines()[SelectItemPosition - 1];
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
            messager.Register<ExamFinishingMessage>(this, OnExamFinishingMessage);
        }

        /// <summary>
        /// 自动触发靠边停车
        /// </summary>
        /// <param name="message"></param>
        private void OnPullOverTrigger(PullOverTriggerMessage message)
        {
            if (ExamContext.IsExaming)
            {
                // Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.PullOver, null);
            }
        }
        //靠边停车触发考试结束
        private void OnExamFinishingMessage(ExamFinishingMessage message)
        {
            RunOnUiThread(ExamFinishing);
        }
        public void ExamFinishing()
        {
            //  mgViewStart.SetImageResource(Resource.Drawable.ks);
            IsExame = false;
            MapSpinner.Enabled = true;
            ExamItemSpinner.Enabled = true;
            btnEndExam.Enabled = false;
            btnStartExam.Enabled = true;
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
                    return;
                }
                for (var i = 0; i < AllExamItems.Count(); i++)
                {
                    if (AllExamItems[i].ItemCode == message.ExamItem.ItemCode)
                    {
                        if (ExamItemState.Progressing == message.NewState)
                        {
                            lstExamItemColor[i] = ColorStateList.ValueOf(Color.DarkBlue);
                        }
                        else if (ExamItemState.Finished == message.NewState)
                        {
                            lstExamItemColor[i] = ColorStateList.ValueOf(Color.Green);
                        }
                    }
                }
                //考试状态改变
                //RunOnUiThread(ExamItemChange);
            }
            catch (Exception ex)
            {
                Logger.Error("JingTangOnExamItemStateChanged", ex.Message);
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
                Logger.Error("DuoLunBrokenRule", ex.Message);
            }
        }
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            try
            {
                carSignal = message.CarSignal;

                //if (carSignal.Distance>=3000&&IsTriggerPullOver==false)
                //{
                //    ExamItemClick("靠边停车");
                //    IsTriggerPullOver = true;
                //    btnStartPullOver.Enabled = false;
                //    return;
                //}

                RunOnUiThread(UpdateCarSensorState);
            }
            catch (Exception ex)
            {
                Logger.Error("JingTangCarSignalReceived", ex.Message);
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
                    Logger.Error("JingTangInitMap", ex.Message);
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
        
            BreakRuleIndex = 0;
            //第一步首先清楚上一次考试项目的扣分规则
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            mainLinerLayout.RemoveAllViews();

            InitBrokenRules();

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
        private void InitBrokenRules()
        {
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableJingTangJieDa, null);

            TableTextView txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
            txt.SetText("序号", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
            txt.SetText("错误信息", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
            txt.SetText("扣分", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_4);
            txt.SetText("错误时间", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_5);
            txt.SetText("错误代码", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            mainLinerLayout.AddView(relativeLayout);
        }
        private void ShowBrokenRule()
        {
            try
            {
                BreakRuleIndex += 1;
                BrokenRuleInfo RuleInfo = brokenRuleMessage.RuleInfo;
                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableJingTangJieDa, null);
                TableTextView txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                txt.SetText(BreakRuleIndex.ToString(), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);
            
                txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
                txt.SetText(RuleInfo.RuleName.ToString(), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
                txt.SetText(RuleInfo.DeductedScores.ToString(), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_4);
                txt.SetText(RuleInfo.BreakTime.ToString("HH:mm:ss"), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_5);
                txt.SetText(RuleInfo.RuleCode, TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                mainLinerLayout.AddView(relativeLayout);
            }
            catch (Exception ex)
            {
                Logger.Error("ShowBorkenRule", ex.Message);
            }
        }

 

        public void ExamItemClick(string ExamItemName)
        {
            try
            {
                string ItemCode = dataService.GetExamItemCode(ExamItemName);
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
                Logger.Error("ExamItemClick", ex.Message);
            }


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

        /// <summary>
        /// 绑定考试列表下拉列表框
        /// </summary>
        /// <param name="lstExamItem"></param>
        private void BindExamItemSpinner(List<string> lstExamItem)
        {
            //MapSpiner
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstExamItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ExamItemSpinner.Adapter = adapter;
            ExamItemSpinner.Visibility = ViewStates.Visible;
        }


        /// <summary>
        /// 更新车辆信号的状态
        /// </summary>
        public void UpdateCarSensorState()
        {
            try
            {
                //更新一些基本的信
                if (ExamContext.IsExaming && IsExame)
                {
                  //tvScore.Text = string.Format("{0}", ExamContext.ExamScore);
                    tvUseTime.Text = string.Format("考试时长：{0:00}:{1:00}:{2:00}", carSignal.UsedTime.Hours, carSignal.UsedTime.Minutes, carSignal.UsedTime.Seconds);
                    tvMileage.Text = string.Format("考试里程：{0:0}米", carSignal.Distance);
                  //  tvSpeed.Text = string.Format("速度{0:0}", carSignal.SpeedInKmh);
                   // tvEngineRatio.Text = string.Format("转速{0}", carSignal.Sensor.EngineRpm);
                    //tvGear.Text = Convert.ToInt32(carSignal.Sensor.Gear).ToString();
                }

            }
            catch (Exception ex)
            {
                Logger.Error("UpdateCarSensorState", ex.Message);
            }


        }

        private void InitExamItem()
        {
            lstExamItem = (from v in dataService.GetExamItemsList()
                           select v.ItemName).ToList<string>();

            BindExamItemSpinner(lstExamItem);
        }
    }
}