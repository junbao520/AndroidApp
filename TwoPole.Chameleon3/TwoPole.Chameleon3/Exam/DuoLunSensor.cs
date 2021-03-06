﻿using System;
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

namespace TwoPole.Chameleon3
{
    [Activity(Label = "DuoLunSensor")]
    public class DuoLunSensor : Activity
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
        ImageView mgViewReturnHome;
        //ImageView mg btnArtificialJudgment
        TextView tvStartTime;
        TextView tvScore;
        TextView tvUseTime;
        TextView tvSpeed;
        TextView tvEngineRatio;
        TextView tvMileage;
        TextView tvExamItem;
        TextView tvShowMessage;

        Spinner MapSpinner;
        Spinner ExamModeSpinner;

        //考试界面信号显示
        TextView tvAngle;
        TextView tvGear;



        ImageView mgViewLowBeamLight;
        ImageView mgViewHighBeamLight;
        ImageView mgViewTurnRightLight;
        ImageView mgViewTurnLeftLight;
        ImageView mgViewOutLineLight;
        ImageView mgViewFogLight;
        ImageView mgViewEngine;
        ImageView mgViewDoor;
        ImageView mgViewHandBreak;
        ImageView mgViewSafetyBelt;
        ImageView mgViewBreak;
        ImageView mgViewClutch;
        ImageView mgViewLoudSpeaker;

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
            SetContentView(Resource.Layout.DuoLunSensor);
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
            if (savedInstanceState != null)
            {
                StartEndStatus = savedInstanceState.GetBoolean("StartEndStatus");
                if (!StartEndStatus)
                {
                    MapSpinner.Enabled = true;
                    ExamModeSpinner.Enabled = true;
                    ExamContext.EndExamTime = DateTime.Now;
                    mgViewStart.SetImageResource(Resource.Drawable.jsks);
                }
            }
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

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
        }
        public void GetIntentParameter()
        {
            IsTrainning = Intent.GetStringExtra("ExamMode") == "Train";
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string msg = e.Exception.Message;
            Logger.Error("DuoLunSensor AndroidEnvironment_UnhandledExceptionRaiser", msg);
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
                           //Logger.InfoFormat("ExamContextIsExaming");
                           Module.StopAsync().ContinueWith((task) =>
                           {
                               // Logger.InfoFormat("Module.StopAsync()");
                           });
                       }
                       Free();
                       //Logger.InfoFormat(" Free();");
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
            BindExamModeSpinner();
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
                lstExamItemColor.Add(ColorStateList.ValueOf(Color.Blue));
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
                        tvShowMessage.SetTextColor(Color.Red);
                        tvShowMessage.Text = string.Format("项目列表:  {0}", authorization.GetAuthorizationInfo());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("InitAuthInfo", ex.Message);
                tvShowMessage.Text = string.Format("授权异常",ex.Message);
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
            tvUseTime = FindViewById<TextView>(Resource.Id.tvUseTime);
            tvSpeed = FindViewById<TextView>(Resource.Id.tvSpeed);
            tvEngineRatio = FindViewById<TextView>(Resource.Id.tvEngineRatio);
            tvMileage = FindViewById<TextView>(Resource.Id.tvMileage);
            tvExamItem = FindViewById<TextView>(Resource.Id.tvExamItem);
            tvShowMessage = FindViewById<TextView>(Resource.Id.tvShowMessage);
            mgViewStart = FindViewById<ImageView>(Resource.Id.mgViewStartExam);
            mgViewReturnHome = FindViewById<ImageView>(Resource.Id.mgViewReturnHome);
            mgViewReturnHome.Click += MgViewReturnHome_Click;
            mgViewStart.Click += btnStartExam_Click;
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
            ExamModeSpinner.ItemSelected += ExamModeSpinner_ItemSelected;

            tvAngle = FindViewById<TextView>(Resource.Id.tvAngle);
            tvGear = FindViewById<TextView>(Resource.Id.tvGear);

            mgViewLowBeamLight = FindViewById<ImageView>(Resource.Id.mgViewLowBeamLight);
            mgViewHighBeamLight = FindViewById<ImageView>(Resource.Id.mgViewHightBeamLight);
            mgViewOutLineLight = FindViewById<ImageView>(Resource.Id.mgViewOutLineLight);
            mgViewTurnLeftLight = FindViewById<ImageView>(Resource.Id.mgViewTurnLeftLight);
            mgViewTurnRightLight = FindViewById<ImageView>(Resource.Id.mgViewTurnRightLight);
            mgViewFogLight = FindViewById<ImageView>(Resource.Id.mgViewFogLight);
            mgViewEngine = FindViewById<ImageView>(Resource.Id.mgViewEngine);
            mgViewDoor = FindViewById<ImageView>(Resource.Id.mgViewDoor);
            mgViewHandBreak = FindViewById<ImageView>(Resource.Id.mgViewHandBreak);
            mgViewSafetyBelt = FindViewById<ImageView>(Resource.Id.mgViewSafetyBelt);
            mgViewBreak = FindViewById<ImageView>(Resource.Id.mgViewBreak);
            mgViewClutch = FindViewById<ImageView>(Resource.Id.mgViewClutch);
            mgViewLoudSpeaker = FindViewById<ImageView>(Resource.Id.mgViewLoudSpeaker);
        }

        private void MgViewReturnHome_Click(object sender, EventArgs e)
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
            Logger.Info("接收到起步指令");
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
            mgViewStart.SetImageResource(Resource.Drawable.ks);
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
                Logger.Error("DuoLunOnExamItemStateChanged", ex.Message);
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
        protected bool CheckAuth(GpsInfo gpsInfo)
        {
            bool checkAuthState = authorization.CheckPeriod(gpsInfo);
            if (!checkAuthState)
            {
                string AuthCode = authorization.GetAuthorizationCode();
                string message = string.Format("设备识别码:{0},请联系销售人员激活软件。",AuthCode);
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

                if (DataBase.IsLeaseScheme && !DataBase.CanUse())
                {
                    speaker.PlayAudioAsync("请购买模拟训练学时");
                    //结束考试
                    RunOnUiThread(ExamFinishing);
                    return;
                }
                tempMessage = message;

                carSignal = message.CarSignal;


                //if (carSignal.Distance >= ExamDistance && IsAutoTriggerPullOver && IsTriggerPullOver == false)
                //{
                //    //启动靠边停车

                //    Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.PullOver, null);
                //    IsTriggerPullOver = true;
                //    return;
                //}
                RunOnUiThread(UpdateCarSensorState);
            }
            catch (Exception ex)
            {
                Logger.Error("DuoLunSensorCarSignalReceived", ex.Message);
            }

        }
        public void btnStartExam_Click(object sender, EventArgs e)
        {
            if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                return;

            if (DataBase.IsLeaseScheme&&!DataBase.CanUse())
            {
                speaker.PlayAudioAsync("请购买模拟训练学时");
                return;
            }
            //开始考试点击之后 需要进行图片切换
            if (StartEndStatus)
            {
                mgViewStart.SetImageResource(Resource.Drawable.jsks);
                StartEndStatus = false;
                //设置地图选项和白考也考选项
                MapSpinner.Enabled = false;
                ExamModeSpinner.Enabled = false;
                IsTriggerPullOver = false;
                //初始化开始项目颜色
                InitExamItemColor();
                InitExamItem();
                StartExam();
            }
            else
            {
                ExamFinishing();
                //mgViewStart.SetImageResource(Resource.Drawable.ks);
                //StartEndStatus = true;
                //MapSpinner.Enabled = true;
                //ExamModeSpinner.Enabled = true;
                //ExamContext.EndExamTime = DateTime.Now;
                //EndExam();
                //Logger.Debug("EndExam");
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
                Logger.Error("DuoLunSensorInitMap", ex.Message);
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
            lstRules.Clear();
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
        private void Txt_LongClick(object sender, View.LongClickEventArgs e)
        {
            try
            {
                TableTextView tv = (TableTextView)sender;
                int index = Convert.ToInt32(tv.Tag.ToString().Trim());

                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                mainLinerLayout.RemoveViewAt(index);
                lstRules.RemoveAt(index);
            }
            catch (Exception ex)
            {
                LogManager.WriteSystemLog(ex.Message + ex.Source + ex.StackTrace);
            }

        }
        private void ShowBrokenRule()
        {
            try
            {
                BrokenRuleInfo RuleInfo = brokenRuleMessage.RuleInfo;
                lstRules.Add(RuleInfo);
                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.table, null);
                TableTextView txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                txt.SetText(RuleInfo.ExamItemName, TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);
                txt.Tag = (lstRules.Count()-1).ToString();
                txt.LongClick += Txt_LongClick;



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
                tvExamItem.Text = ExamItemName;
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
            relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.table, null);

            TableTextView title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
            title.SetText("扣分项目", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);

            title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
            title.SetText("扣分", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);

            title = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
            title.SetText("扣分原因", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);
            mainLinerLayout.AddView(relativeLayout);
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
            if (carSignal.Sensor.LowBeam)
                mgViewLowBeamLight.SetImageResource(Resource.Drawable.low_beam_on);
            else
                mgViewLowBeamLight.SetImageResource(Resource.Drawable.low_beam_off);
            if (carSignal.Sensor.HighBeam)
                mgViewHighBeamLight.SetImageResource(Resource.Drawable.high_beam_on);
            else
                mgViewHighBeamLight.SetImageResource(Resource.Drawable.high_beam_off);
            //报警灯亮起时左右转向亮
            if (carSignal.Sensor.CautionLight)
            {
                mgViewTurnLeftLight.SetImageResource(Resource.Drawable.left_light_on);
                mgViewTurnRightLight.SetImageResource(Resource.Drawable.right_light_on);
            }
            else
            {
                if (carSignal.Sensor.LeftIndicatorLight)
                    mgViewTurnLeftLight.SetImageResource(Resource.Drawable.left_light_on);
                else
                    mgViewTurnLeftLight.SetImageResource(Resource.Drawable.left_light_off);
                if (carSignal.Sensor.RightIndicatorLight)
                    mgViewTurnRightLight.SetImageResource(Resource.Drawable.right_light_on);
                else
                    mgViewTurnRightLight.SetImageResource(Resource.Drawable.right_light_off);
            }
            if (carSignal.Sensor.FogLight)
                mgViewFogLight.SetImageResource(Resource.Drawable.fog_light_on);
            else
                mgViewFogLight.SetImageResource(Resource.Drawable.fog_light_off);

            if (carSignal.Sensor.Engine)
                mgViewEngine.SetImageResource(Resource.Drawable.engine_on);
            else
                mgViewEngine.SetImageResource(Resource.Drawable.engine_off);

            if (carSignal.Sensor.Door)
                mgViewDoor.SetImageResource(Resource.Drawable.door_on);
            else
                mgViewDoor.SetImageResource(Resource.Drawable.door_off);

            if (carSignal.Sensor.Handbrake)
                mgViewHandBreak.SetImageResource(Resource.Drawable.hand_break_on);
            else
                mgViewHandBreak.SetImageResource(Resource.Drawable.hand_break_off);

            if (carSignal.Sensor.SafetyBelt)
                mgViewSafetyBelt.SetImageResource(Resource.Drawable.safety_belt_off);
            else
                mgViewSafetyBelt.SetImageResource(Resource.Drawable.safety_belt_on);

            if (carSignal.Sensor.Brake)
                mgViewBreak.SetImageResource(Resource.Drawable.brake_on);
            else
                mgViewBreak.SetImageResource(Resource.Drawable.brake_off);

            if (carSignal.Sensor.Clutch)
                mgViewClutch.SetImageResource(Resource.Drawable.clutch_on);
            else
                mgViewClutch.SetImageResource(Resource.Drawable.clutch_off);

            if (carSignal.Sensor.Loudspeaker)
                mgViewLoudSpeaker.SetImageResource(Resource.Drawable.loudspeaker_on);
            else
                mgViewLoudSpeaker.SetImageResource(Resource.Drawable.loudspeaker_off);

            if (carSignal.Sensor.OutlineLight)
                mgViewOutLineLight.SetImageResource(Resource.Drawable.outline_light_on);
            else
                mgViewOutLineLight.SetImageResource(Resource.Drawable.outline_light_off);
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
                // Logger.Error("UpdateCarSensorState", ex.Message);
            }


        }

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
                for (int i = 0; i < ExamItemCount / 4; i++)
                {
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItem, null);
                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                    txt.SetText(lstExamItem[i * 4], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 4]);
                    txt.Click += ExamItemClick;
                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_2);
                    txt.SetText(lstExamItem[i * 4 + 1], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 4 + 1]);
                    txt.Click += ExamItemClick;

                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_3);
                    txt.SetText(lstExamItem[i * 4 + 2], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 4 + 2]);
                    txt.Click += ExamItemClick;

                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_4);
                    txt.SetText(lstExamItem[i * 4 + 3], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 4 + 3]);
                    txt.Click += ExamItemClick;

                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_5);
                    txt.Visibility = ViewStates.Invisible;

                    mainLinerLayout.AddView(relativeLayout);
                }
                //4如果余数大
                int Count = ExamItemCount - (ExamItemCount / 4) * 4;
                if (Count > 0)
                {
                    int StartIndex = (ExamItemCount / 4) * 4;
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItem, null);
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
                Logger.Error("InitExamItem", ex.Message);
            }

        }

        public void ExamItemClick(object sender, EventArgs e)
        {
            try
            {
                TextView tvExamItem = (TextView)sender;
                int id = tvExamItem.Id;

                var ExamItemText = tvExamItem.Text;
          
                if (ExamItemText == "公交汽车")
                {
                    ExamItemText = "公共汽车站";
                }
                string ItemCode = dataService.GetExamItemCode(ExamItemText);
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

    }
}