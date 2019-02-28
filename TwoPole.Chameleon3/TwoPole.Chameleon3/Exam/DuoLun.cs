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
    [Activity(Label = "DuoLun")]
    public class DuoLun : Activity
    {
        private LinearLayout   mainLinerLayout;
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
        ImageView mgViewStart;
        TextView tvStartTime;
        TextView tvScore;
        TextView tvUseTime;
        TextView tvSpeed;
        TextView tvEngineRatio;
        TextView tvMileage;
        TextView tvExamItem;
        TextView tvGear;
        Spinner MapSpinner;
        Spinner ExamModeSpinner;
        List<ColorStateList> lstExamItemColor = new List<ColorStateList>();

        List<string> lstExamItem = new List<string>();

        private bool IsTrainning = false;

        //if 1==1 you know //你清楚么？你知道么？why try ca
        //if 1==1 you know //你清楚么？你知道么？why try ca 
        //if 1==1 you know //你清楚么？你知道么？why try ca
        //三公里自动触发靠边停车
        //三公
        protected override void OnCreate(Bundle savedInstanceState)
        {
           //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DuoLun);
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            dataService = Singleton.GetDataService;
            Logger = Singleton.GetLogManager;
            GetIntentParameter();
            InitBrokenRules();
            InitUI();
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
            //开始考试
            //原来考试
            //开始记录
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

        }
        public void GetIntentParameter()
        {
            IsTrainning = Intent.GetStringExtra("ExamMode") == "Train";
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string msg = e.Exception.Message;
            Logger.Error("Environement",msg);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                ShowConfirmDialog();
            }
            return base.OnKeyDown(keyCode, key);
        }
        protected  void Free()
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
            if (speaker!=null)
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
            ExamContext.ExamMode =IsTrainning?ExamMode.Training: ExamMode.Examming;
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
            mgViewLowBeamLight = FindViewById<ImageView>(Resource.Id.mgViewLowBeamLight);
            mgViewHighBeamLight = FindViewById<ImageView>(Resource.Id.mgViewHightBeamLight);
            mgViewOutLineLight = FindViewById<ImageView>(Resource.Id.mgViewOutlineLight); 
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

            MapSpinner = (Spinner)FindViewById(Resource.Id.MapSpiner);
            ExamModeSpinner = (Spinner)FindViewById(Resource.Id.ExamModeSpinner);
            tvStartTime = FindViewById<TextView>(Resource.Id.tvStartTime);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
            tvUseTime = FindViewById<TextView>(Resource.Id.tvUseTime);
            tvSpeed = FindViewById<TextView>(Resource.Id.tvSpeed);
            tvEngineRatio = FindViewById<TextView>(Resource.Id.tvEngineRatio);
            tvMileage = FindViewById<TextView>(Resource.Id.tvMileage);
            tvExamItem = FindViewById<TextView>(Resource.Id.tvExamItem);
            tvGear = FindViewById<TextView>(Resource.Id.tvGear);
            mgViewStart = FindViewById<ImageView>(Resource.Id.mgViewStartExam);
            mgViewStart.Click += btnStartExam_Click;
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
            ExamModeSpinner.ItemSelected += ExamModeSpinner_ItemSelected;

        }

        //设置考试模式是白考还是夜间考试
        private void ExamModeSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ExamContext.ExamTimeMode = (ExamTimeMode)(e.Position + 1);
        }

        private void MapSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var SelectItemPosition = e.Position;
            
            if (SelectItemPosition>0&&ExamContext!=null)
            {
                var SelectMapItem = dataService.GetAllMapLines()[SelectItemPosition - 1];
                if (SelectMapItem.Points!=null)
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
            messager.Register<VehicleStartingMessage>(this, OnVehicleStartTrigger);
        }
        private void OnVehicleStartTrigger(VehicleStartingMessage message)
        {
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.Start, null);
            }
        }
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
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            try
            {
                carSignal = message.CarSignal;
                RunOnUiThread(UpdateCarSensorState);
            }
            catch (Exception ex)
            {
                Logger.Error("DuoLunCarSignalReceived", ex.Message);
            }
  
        }
        public void btnStartExam_Click(object sender, EventArgs e)
        {
            //开始考试点击之后 需要进行图片切换
            if (StartEndStatus)
            {
                mgViewStart.SetImageResource(Resource.Drawable.jsks);
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
                mgViewStart.SetImageResource(Resource.Drawable.ks);
                StartEndStatus = true;
                MapSpinner.Enabled = true;
                ExamModeSpinner.Enabled = true;
                ExamContext.EndExamTime = DateTime.Now;
                EndExam();
            }

        }

        
        protected void InitMapLines()
        {
            Task.Run(() =>
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
                    //var mapId = dataService.getDefaultMapId();
                    //进行地图绑定
                    //if (mapId > 0)
                    //{
                    //    var temp = mapLines.FirstOrDefault(x => x.Id == mapId);
                    //    if (temp == null)
                    //        return;
                    //    ExamContext.Map = new MapSet(temp.Points.ToMapPoints());
                    //}
                }
                catch (Exception ex)
                {
                    Logger.Error("DuoLunInitMap", ex.Message);
                }

             

            });
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
                Logger.Error("ShowBorkenRule",ex.Message);
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
                Logger.Error("ExamItemChange",ex.Message);
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
            title.SetText("原因", TextView.BufferType.Normal);
            title.SetTextColor(Color.Black);
            mainLinerLayout.AddView(relativeLayout);
        }

        public void ExamItemClick(object sender, EventArgs e)
        {
            try
            {
                TextView tvExamItem = (TextView)sender;
                int id = tvExamItem.Id;
                 //
                //点击之后设置颜色 
               //徒手抓四郎
               //tvExamItem.SetTextColor(ColorStateList.ValueOf(Color.Red));
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

                Logger.Error("ExamItemClick",ex.Message);
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
          
                //所有的考试项目但是不包含综合评判的
                int ExamItemCount = lstExamItem.Count;
                for (int i = 0; i < ExamItemCount / 4; i++)
                {
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItem, null);
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

                    //txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_5);
                    //txt.SetText(lstExamItem[i * 5 + 4], TextView.BufferType.Normal);
                    //txt.SetTextColor(lstExamItemColor[i * 5 + 4]);
                    //txt.Click += ExamItemClick;

                    mainLinerLayout.AddView(relativeLayout);
                }
                //4如果余数大
                int Count = ExamItemCount - (ExamItemCount / 4) * 4;
                if (Count> 0)
                {
                    int StartIndex = (ExamItemCount / 4) * 4;
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItem, null);
                    for (int i = 0; i <4; i++)
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

                        if (i>=Count)
                        {
                            txt = (TextView)relativeLayout.FindViewById(ListID);
                            txt.SetText("", TextView.BufferType.Normal);
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
                Logger.Error("InitExamItem",ex.Message);
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
            List<string> Maps = new List<string> {"白天","夜间" };
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
                    tvScore.Text = string.Format("{0}", ExamContext.ExamScore);
                    tvUseTime.Text = string.Format("{0:00}:{1:00}:{2:00}", carSignal.UsedTime.Hours, carSignal.UsedTime.Minutes, carSignal.UsedTime.Seconds);
                    tvMileage.Text = string.Format("{0:0}米", carSignal.Distance);
                    tvSpeed.Text = string.Format("速度{0:0}", carSignal.SpeedInKmh);
                    tvEngineRatio.Text = string.Format("转速{0}", carSignal.Sensor.EngineRpm);
                    tvGear.Text = Convert.ToInt32(carSignal.Sensor.Gear).ToString();
                }

                //下面部分是更新车辆状态图标
                if (carSignal.Sensor.LowBeam)
                    mgViewLowBeamLight.SetImageResource(Resource.Drawable.low_beam_on);
                else
                    mgViewLowBeamLight.SetImageResource(Resource.Drawable.low_beam_off);
                if (carSignal.Sensor.HighBeam)
                    mgViewHighBeamLight.SetImageResource(Resource.Drawable.high_beam_on);
                else
                    mgViewHighBeamLight.SetImageResource(Resource.Drawable.high_beam_off);

                if (carSignal.Sensor.LeftIndicatorLight)
                    mgViewTurnLeftLight.SetImageResource(Resource.Drawable.left_light_on);
                else
                    mgViewTurnLeftLight.SetImageResource(Resource.Drawable.left_light_off);

                if (carSignal.Sensor.RightIndicatorLight)
                    mgViewTurnRightLight.SetImageResource(Resource.Drawable.right_light_on);
                else
                    mgViewTurnRightLight.SetImageResource(Resource.Drawable.right_light_off);

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
            catch (Exception ex)
            {
                Logger.Error("UpdateCarSensorState",ex.Message);
            }

          
        }
    }
}