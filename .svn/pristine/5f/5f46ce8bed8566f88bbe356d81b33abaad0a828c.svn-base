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

namespace TwoPole.Chameleon3
{
    [Activity(Label = "SanLian")]
    public class SanLianMobilePhone : Activity
    {

        private LinearLayout mainLinerLayout;
        private RelativeLayout relativeLayout;
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
        ImageView mgViewStart;
        TextView tvStartTime;
        TextView tvScore;
        TextView tvUseTime;
        TextView tvSpeed;
        TextView tvEngineRatio;
        TextView tvMileage;
        TextView tvExamItem;
        TextView tvGear;
        TextView tvAngle;
        TextView textViewCarType;
        TextView tvTips;
        Spinner MapSpinner;
        Spinner ExamModeSpinner;
        Authorization authorization;
        List<ColorStateList> lstExamItemColor = new List<ColorStateList>();
        private CarSignalReceivedMessage tempMessage { get; set; }
        List<string> lstExamItem = new List<string>();

        private bool IsTrainning = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //if:1+1=2 elif 2+2=4
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SanLianMobilePhone);
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            dataService = Singleton.GetDataService;
            Logger = Singleton.GetLogManager;
            authorization = new Authorization(this);
            GetIntentParameter();
            InitBrokenRules();
            InitUI();
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

        }
        public void GetIntentParameter()
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
                Logger.Error("SanLianShowConfirmDialog" + ex.Message);
            }

        }
        private void InitUI()
        {
            InitControl();
            InitMapLines();
            BindExamModeSpinner();

            InitExamItemColor();
            InitExamItem();


            InitExamContext();
            InitModuleAsync(ExamContext.Map);
            string ActiveInfo = authorization.getActiveInfo();
            //角度变化
            //角度变化
            //微积分
            if (!string.IsNullOrEmpty(ActiveInfo))
            {
                tvTips.Text = string.Format("试用到期时间:{0}", ActiveInfo.Split('@')[0]);
            }
        }

        protected bool CheckAuth(GpsInfo gpsInfo)
        {
            bool checkAuthState = authorization.CheckPeriod(gpsInfo);
            if (!checkAuthState)
            {
                // Toast.MakeText(this,"GPS无信号或试用已到期，请检查",ToastLength.Long);
                speaker.PlayAudioAsync("GPS无信号或试用已到期，请检查");
                return false;
            }

            return true;
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
            ExamModeSpinner = (Spinner)FindViewById(Resource.Id.ExamModeSpinner);
            tvStartTime = FindViewById<TextView>(Resource.Id.tvStartTime);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
            tvGear = FindViewById<TextView>(Resource.Id.tvGear);
            tvUseTime = FindViewById<TextView>(Resource.Id.tvUseTime);
            tvSpeed = FindViewById<TextView>(Resource.Id.tvSpeed);
            tvEngineRatio = FindViewById<TextView>(Resource.Id.tvEngineRatio);
            tvMileage = FindViewById<TextView>(Resource.Id.tvMileage);
            tvExamItem = FindViewById<TextView>(Resource.Id.tvExamItem);
            tvGear = FindViewById<TextView>(Resource.Id.tvGear);
            tvAngle = FindViewById<TextView>(Resource.Id.tvAngle);
            mgViewStart = FindViewById<ImageView>(Resource.Id.mgViewStartExam);
            textViewCarType = FindViewById<TextView>(Resource.Id.textViewCarType);
            tvTips = FindViewById<TextView>(Resource.Id.tvTips);
            mgViewStart.Click += btnStartExam_Click;
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
            ExamModeSpinner.ItemSelected += ExamModeSpinner_ItemSelected;
            if (dataService.GetSettings().LicenseC2)
            {
                textViewCarType.Text = "车型：C2";
            }

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
        }
        //靠边停车触发考试结束
        private void OnExamFinishingMessage(ExamFinishingMessage message)
        {
            RunOnUiThread(ExamFinishing);
        }
        public void ExamFinishing()
        {
            mgViewStart.SetImageResource(Resource.Drawable.StartExam);
            StartEndStatus = true;
            MapSpinner.Enabled = true;
            ExamModeSpinner.Enabled = true;
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
                Logger.Error("SanLianOnExamItemStateChanged", ex.Message);
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
                Logger.Error("SanLianBrokenRule", ex.Message);
            }
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
                Logger.Error("SanLianCarSignalReceived", ex.Message);
            }

        }
        public void btnStartExam_Click(object sender, EventArgs e)
        {
            //开始考试点击之后 需要进行图片切换

            if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                return;

            if (StartEndStatus)
            {
                mgViewStart.SetImageResource(Resource.Drawable.EndExam);
                StartEndStatus = false;
                //设置地图选项和白考也考选项
                MapSpinner.Enabled = false;
                ExamModeSpinner.Enabled = false;
                //初始化开始项目颜色
                InitExamItemColor();
                InitExamItem();
                StartExam();
            }
            else
            {
                mgViewStart.SetImageResource(Resource.Drawable.StartExam);
                StartEndStatus = true;
                MapSpinner.Enabled = true;
                ExamModeSpinner.Enabled = true;
                ExamContext.EndExamTime = DateTime.Now;
                EndExam();
            }

        }


        protected void InitMapLines()
        {
            try
            {
                var mapLines = dataService.GetAllMapLines();
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
                Logger.Error("SanLianInitMap", ex.Message+";"+ex.StackTrace);
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
            InitBrokenRules();
            tvStartTime.Text = DateTime.Now.ToString("开始时间:HH:mm:ss");
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
        private void ShowBrokenRule()
        {
            try
            {
                BrokenRuleInfo RuleInfo = brokenRuleMessage.RuleInfo;
                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.table, null);
                TableTextView txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                txt.SetText(RuleInfo.ExamItemName, TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
                txt.SetText(RuleInfo.DeductedScores.ToString(), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
                txt.SetText(RuleInfo.RuleName, TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);
                mainLinerLayout.AddView(relativeLayout);
            }
            catch (Exception ex)
            {
                Logger.Error("ShowBorkenRule", ex.Message);
            }
        }

        private void ExamItemChange()
        {
            try
            {
                var ExamItemName = examItemStateChangedMessage.ExamItem.Name;
                tvExamItem.Text = "当前项目:" + ExamItemName;
                InitExamItem();
            }
            catch (Exception ex)
            {
                Logger.Error("ExamItemChange", ex.Message);
            }
        }
        private void InitBrokenRules()
        {
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableMobilePhone, null);

            TableTextView title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
            title.SetText("扣分项目", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);

            title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
            title.SetText("扣分", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);

            title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
            title.SetText("原因", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);
            mainLinerLayout.AddView(relativeLayout);
        }

        public void ExamItemClick(object sender, EventArgs e)
        {
            try
            {
                Button buttonExamItem = (Button)sender;
                string ItemCode = dataService.GetExamItemCode(buttonExamItem.Text);
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

        private void BindExamModeSpinner()
        {
            List<string> Maps = new List<string> { "白天", "夜间" };
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, Maps);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ExamModeSpinner.Adapter = adapter;
            ExamModeSpinner.Visibility = ViewStates.Visible;

        }

        /// <summary>
        /// 更新车辆信号的状态
        /// </summary>
        public void UpdateCarSensorState()
        {
            try
            {
                //更新一些基本的信
                if (ExamContext.IsExaming && !StartEndStatus)
                {
                    tvScore.Text = string.Format("分数:{0}", ExamContext.ExamScore);
                    tvUseTime.Text = string.Format("用时时间:{0:00}:{1:00}:{2:00}", carSignal.UsedTime.Hours, carSignal.UsedTime.Minutes, carSignal.UsedTime.Seconds);
                    tvMileage.Text = string.Format("里程:{0:0}米", carSignal.Distance);
                    tvSpeed.Text = string.Format("速度:{0:0}", carSignal.SpeedInKmh);
                    tvEngineRatio.Text = string.Format("转速:{0}", carSignal.Sensor.EngineRpm);
                    tvGear.Text = "档位:" + Convert.ToInt32(carSignal.Sensor.Gear).ToString();
                    tvAngle.Text = "陀螺仪:" + carSignal.BearingAngle.ToString();
                }

            }
            catch (Exception ex)
            {
                Logger.Error("UpdateCarSensorState", ex.Message);
            }


        }
        private void InitExamItem()
        {
            try
            {
                LinearLayout mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.ExamItemTable);
                mainLinerLayout.RemoveAllViews();
                Button button;
                int ListID = 0;

                //所有的考试
                //所有的考试项目但是不包含综合评判的
                int ExamItemCount = lstExamItem.Count;
                for (int i = 0; i < ExamItemCount / 4; i++)
                {
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.SanLianExamItemButton, null);
                    button = relativeLayout.FindViewById<Button>(Resource.Id.list_1_1);
                    button.Text = lstExamItem[i * 4];
                    button.SetTextColor(lstExamItemColor[i * 4]);
                    button.Click += ExamItemClick;

                    button = relativeLayout.FindViewById<Button>(Resource.Id.list_1_2);
                    button.Text = lstExamItem[i * 4 + 1];
                    button.SetTextColor(lstExamItemColor[i * 4 + 1]);
                    button.Click += ExamItemClick;

                    button = relativeLayout.FindViewById<Button>(Resource.Id.list_1_3);
                    button.Text = lstExamItem[i * 4 + 2];
                    button.SetTextColor(lstExamItemColor[i * 4 + 2]);
                    button.Click += ExamItemClick;

                    button = relativeLayout.FindViewById<Button>(Resource.Id.list_1_4);
                    button.Text = lstExamItem[i * 4 + 3];
                    button.SetTextColor(lstExamItemColor[i * 4 + 3]);
                    button.Click += ExamItemClick;


                    mainLinerLayout.AddView(relativeLayout);
                }
                //4如果余数大
                int Count = ExamItemCount - (ExamItemCount / 4) * 4;
                if (Count > 0)
                {
                    int StartIndex = (ExamItemCount / 4) * 4;
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.SanLianExamItemButton, null);
                    for (int i = 0; i < 4; i++)
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
                            default:
                                break;
                        }

                        if (i >= Count)
                        {
                            button = relativeLayout.FindViewById<Button>(ListID);
                            button.SetWidth(0);
                            button.SetHeight(0);
                            button.Text = "";
                            button.Visibility = ViewStates.Invisible;
                            //relativeLayout.RemoveViewAt(i);
                        }
                        else
                        {
                            button = relativeLayout.FindViewById<Button>(ListID);
                            button.SetTextColor(lstExamItemColor[StartIndex + i]);
                            button.Text = lstExamItem[StartIndex + i];
                            button.Click += ExamItemClick;
                        }

                    }
                    mainLinerLayout.AddView(relativeLayout);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("InitExamItem", ex.Message);
            }

        }


    }
}