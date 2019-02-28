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
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure;
using Android.Content.Res;
using Android.Graphics;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Infrastructure.Services;
using Android.Util;
using TwoPole.Chameleon3.Business.LuZhou.Modules;
using TwoPole.Chameleon3.Business.Modules;


namespace TwoPole.Chameleon3
{
    [Activity(Label = "LongChuang")]
    public class LongChuang : BaseExamActivity
    {
        Button btnStartExam;
        Button btnReturnHome;
        Button btnMapSelect;
        Button btnModeSelect;
        Button btnPrepareOk;
        Button btnRengong;
        Button btnCommon;


        TextView tvEngineRpm;
        TextView tvScore;
        TextView tvSpeedInfo;
        TextView tvDistanceInfo;
        TextView tvAuthInfo;
        TextView tvMapInfo;
        TextView tvSatelliteCount;
        TextView tvGear;
        TextView tvAngleInfo;
        TextView tvGpsAngleInfo;

        Spinner MapSpinner;
        Spinner ExamModeSpinner;

        #region 车辆状态


        #endregion

        ImageView ImageFinger;
        LinearLayout relativeLayout;

        protected IMessenger Messenger { get; set; }
        protected new ExamModule Module { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LongChuang);
            InitUI();

            Messenger = Singleton.GetMessager;

        }
        protected override async Task InitModuleAsync(IMapSet map)
        {
            Module = new LocalizationExamModule();
            await Module.InitAsync(new ExamInitializationContext(map)).ContinueWith(task =>
            {

            });

        }

        protected override void InitParameter()
        {
            ExamUIName = "LongChuang";
            ExamItemLineCount = 4;
            base.InitParameter();
        }
        private void InitControl()
        {
            btnStartExam = FindViewById<Button>(Resource.Id.btnStartExam);

            btnReturnHome = FindViewById<Button>(Resource.Id.btnReturnHome);

            BrokenRuleListView = (ListView)FindViewById(Resource.Id.BrokenRule_list);
            btnMapSelect = (Button)FindViewById(Resource.Id.btnMapSelect);
            btnModeSelect = (Button)FindViewById(Resource.Id.btnModeSelect);
            btnPrepareOk = (Button)FindViewById(Resource.Id.btnPrepareOk);
            btnRengong = (Button)FindViewById(Resource.Id.btnRengGong);
            btnCommon = (Button)FindViewById(Resource.Id.btnCommon);

            tvSpeedInfo = FindViewById<TextView>(Resource.Id.tvSpeedInfo);
            tvDistanceInfo = FindViewById<TextView>(Resource.Id.tvDistanceInfo);
            tvEngineRpm = FindViewById<TextView>(Resource.Id.tvEngineRpm);
            tvMapInfo = FindViewById<TextView>(Resource.Id.tvMapInfo);

            tvSatelliteCount = FindViewById<TextView>(Resource.Id.tvSatelliteCount);
            tvGear = FindViewById<TextView>(Resource.Id.tvGear);
            tvAngleInfo = FindViewById<TextView>(Resource.Id.tvAngleInfo);
            tvGpsAngleInfo = FindViewById<TextView>(Resource.Id.tvGpsAngleInfo);

            MapSpinner = new Spinner(this);
            ExamModeSpinner = new Spinner(this);

            //tvSensorInfo = FindViewById<TextView>(Resource.Id.tvSensorInfo);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
            tvAuthInfo = FindViewById<TextView>(Resource.Id.tvAuthInfo);

            btnStartExam.Enabled = true;
            btnStartExam.Click += BtnStartExam_Click;
            btnPrepareOk.Click += btnPrepareOk_Click;
            btnRengong.Click += BtnRengong_Click;
            btnCommon.Click += BtnRengong_Click;

            btnReturnHome.Click += BtnReturnHome_Click;
            //ImageFinger.Click += BtnStartExam_Click;

            btnMapSelect.Click += btnMapSelect_Click;
            btnModeSelect.Click += btnModeSelect_Click;

            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
            ExamModeSpinner.ItemSelected += ExamModeSpinner_ItemSelected;


        }




        #region  人工评判

        private void BtnRengong_Click(object sender, EventArgs e)
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



        #endregion
        private void BtnEndExam_Click(object sender, EventArgs e)
        {
            FinishExam();
        }
        public void FinishExam()
        {
            btnRengong.Enabled = false;
            btnCommon.Enabled = false;
            btnPrepareOk.Enabled = true;
            btnStartExam.Enabled = true;
            btnPrepareOk.Text = "准备就绪";
            btnStartExam.Text = "开始考试";

            startExamTime = DateTime.Now;
            btnMapSelect.Enabled = false;
            btnModeSelect.Enabled = false;
            MapSpinner.Enabled = false;
            ExamModeSpinner.Enabled = false;
            //初始化考试项目颜色
            EndExam();
            //结束考试后初始化项目颜色
            InitExamItemColor();
            InitExamItem(true);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                ShowConfirmDialog();
            }
            return base.OnKeyDown(keyCode, key);
        }


        private void BtnReturnHome_Click(object sender, EventArgs e)
        {
            ShowConfirmDialog();
        }

        private void btnMapSelect_Click(object sender, EventArgs e)
        {
            MapSpinner.PerformClick();

        }

        private void btnModeSelect_Click(object sender, EventArgs e)
        {
           ExamModeSpinner.PerformClick();
        }

        private int lastMapSelectedId = 0;
    
        #region 开始考试

        private void btnPrepareOk_Click(object sender, EventArgs e)
        {
            Logger.Error("btnPrepareOk_Click");
            btnStartExam.Enabled = false;
            if (btnPrepareOk.Text.Contains("准备"))
            {
                btnCommon.Enabled = true;
                btnRengong.Enabled = true;
                btnPrepareOk.Text = "继续考试";
                StartPrepareExam();
            }
            else if (btnPrepareOk.Text.Contains("继续"))
            {
                Messenger.Send(new PrepareDrivingFinishedMessage());
                btnPrepareOk.Text = "结束考试";
            }
            else if (btnPrepareOk.Text.Contains("结束"))
            {
                btnPrepareOk.Text = "准备就绪";
                BtnEndExam_Click(sender, e);
            }

        }
        private void BtnStartExam_Click(object sender, EventArgs e)
        {
            Logger.Error("StartExam Click");
            btnPrepareOk.Enabled = false;
            if (btnStartExam.Text.Contains("开始"))
            {
                btnRengong.Enabled = true;
                btnCommon.Enabled = true;
                btnMapSelect.Text = MapName;
                btnStartExam.Text = "继续考试";
                StartExam();
            }
            else if (btnStartExam.Text.Contains("继续"))
            {
                btnStartExam.Text = "结束考试";
                Messenger.Send(new PrepareDrivingFinishedMessage());
            }
            else if (btnStartExam.Text.Contains("结束"))
            {
                btnStartExam.Text = "开始考试";
                btnMapSelect.Text = "线路选择";
                BtnEndExam_Click(sender, e);
            }
        }

        private DateTime? startExamTime { get; set; }
        protected override void StartExam()
        {
            try
            {
                if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                    return;

                if (startExamTime.HasValue)
                {
                    Logger.Error("开始考试");
                    return;
                }
                MapSecledOption();

                //记录下选择的地图名称
                var mapName = btnMapSelect.Text;
                //再次初始化地图
                if (mapName!="地图选择")
                {
                    var SelectMapItem = lstMapLine.Where(s => s.Name == mapName).FirstOrDefault();
                    //
                    var mapSet = new MapSet(SelectMapItem.Points.ToMapPoints());
                    //把地图选择信息保存起来后续需要可以直接取
                    MapName = SelectMapItem.Name;
                    Logger.Info("开始考试地图:"+mapName);
                    ExamContext.Map = mapSet;
                    dataService.SaveDefaultMapId(SelectMapItem.Id);
                }
                else
                {
                    ExamContext.Map = MapSet.Empty;
                    dataService.SaveDefaultMapId(0);
                }
                //开始考试之前加载一次地图
                tempItem = "准备考试";
                tvScore.Text = string.Format("准备考试   100   00:00:00");
                startExamTime = DateTime.Now;
                btnMapSelect.Enabled = false;
                btnModeSelect.Enabled = false;
                MapSpinner.Enabled = false;
                ExamModeSpinner.Enabled = false;
                InitExamItemColor();
                InitExamItem(true);

                InitBrokenRules();

                ExamContext.EndExamTime = null;
                btnStartExam.Text = "继续考试";

                Module.StartAsync(ExamContext).ContinueWith((task) =>
                {
                });
            }
            catch (Exception ex)
            {
                Logger.Error("StartExam", ex.Message);
            }

        }
        protected void StartPrepareExam()
        {

            try
            {
                if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                    return;
                if (startExamTime.HasValue)
                {
                    Logger.Error("正在开始考试");
                    return;
                }
                tempItem = "准备考试";
                tvScore.Text = string.Format("准备考试   100   00:00:00");
                startExamTime = DateTime.Now;
                btnMapSelect.Enabled = false;
                btnModeSelect.Enabled = false;
                MapSpinner.Enabled = false;
                ExamModeSpinner.Enabled = false;
                InitExamItemColor();
                InitExamItem(true);
                InitBrokenRules();
                ExamContext.EndExamTime = null;
                btnPrepareOk.Text = "继续考试";
                Module.ReadyStartExamAsync(ExamContext).ContinueWith((task) =>
                {
                });
            }
            catch (Exception ex)
            {
                Logger.Error("StartExam", ex.Message);
            }

        }
        protected override void EndExam()
        {
            startExamTime = null;
            btnStartExam.Enabled = true;
            btnMapSelect.Enabled = true;
            btnModeSelect.Enabled = true;
            MapSpinner.Enabled = true;
            ExamModeSpinner.Enabled = true;
            ExamContext.EndExamTime = DateTime.Now;
            Module.StopAsync().ContinueWith((task) =>
            {
            });
        }
        #endregion
        public override void GetIntentParameter()
        {
            IsTrainning = Intent.GetStringExtra("ExamMode") == "Train";
        }
        protected override void InitBrokenRules()
        {
            lstBorkenRuleInfo.Clear();
            BrokenRuleListView.Adapter = null;
            base.InitBrokenRules();
        }

        protected void UI_Load()
        {
            LoadSensorControl();
        }

        protected override void InitUI()
        {

            InitControl();

            InitSensor();

            InitBrokenRules();

            InitAuthInfo(tvScore);

            InitExamItemColor();

            InitExamItem(false);

            BindExamMapLinesSpinner(MapSpinner);


            BindExamModeSpinner(ExamModeSpinner);

            InitExamContext();

            InitModuleAsync(ExamContext.Map);

            base.InitUI();
        }

 

        string tempItem = "准备考试";
        public void UpdateCarSensorState()
        {
            try
            {
                //开始考试按钮状态不可以点击 表示已经开始考试


                if (Module != null && Module.ExamManager != null)
                    currentExamItem = Module.ExamManager.ExamItems.LastOrDefault(x => x.State == ExamItemState.Progressing);
                if (currentExamItem != null)
                {
                    tempItem = currentExamItem.Name;
                    if (currentExamItem.ItemCode != ExamItemCodes.PrepareDriving)
                    {
                        //tvSensorInfo.Text = string.Format("正在训练“{0}”里程:{1:0}米 速度:{2}KM/h 转速:{3}转 方向:{4:#0.00}°档位:{5}", currentExamItem.Name, carSignal.Distance, carSignal.SpeedInKmh, carSignal.EngineRpm, carSignal.BearingAngle, Convert.ToInt32(carSignal.Sensor.Gear).ToString());
                    }
                }
                else
                {
                    //tvSensorInfo.Text = string.Format("里程:{0:0}米 速度:{1}KM/h 转速:{2}转 方向:{3:#0.00}°档位:{4}", carSignal.Distance, carSignal.SpeedInKmh, carSignal.EngineRpm, carSignal.BearingAngle, Convert.ToInt32(carSignal.Sensor.Gear).ToString());
                }
                if (startExamTime.HasValue)
                {
                    var span = DateTime.Now - startExamTime.Value;
                    var times = string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
                    tvScore.Text = string.Format("{0}   {1}             {2}", tempItem, ExamContext.ExamScore, times);

                    tvSpeedInfo.Text = string.Format("速度：{0}Km/h", carSignal.SpeedInKmh.ToString("N1"));
                    tvDistanceInfo.Text = string.Format("里程：{0}m ", carSignal.Distance.ToString("N0"));
                    tvAngleInfo.Text = string.Format("陀螺仪：{0}度", carSignal.AngleZ.ToString("N1"));
                    tvGpsAngleInfo.Text = string.Format("Gps方向：{0}度", carSignal.Gps.AngleDegrees.ToString("N1"));
                }
                if (carSignal.Sensor != null)
                {
                    tvSatelliteCount.Text = string.Format("卫星：{0}", carSignal.Gps.TrackedSatelliteCount);
                    tvEngineRpm.Text = string.Format("转速：{0}", carSignal.EngineRpm);
                    tvGear.Text = string.Format("档位：{0}", Convert.ToInt32(carSignal.Sensor.Gear));
                }
                UpdateCarSensor();
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("UpdateCarSensorState发生异常,原因:{0} 原始信号{1}", exp.Message, string.Join(",", carSignal.commands));
            }


        }
        protected override void ShowBrokenRule()
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;

                foreach (var item in lstBorkenRuleInfo.OrderByDescending(s => s.BreakTime))
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemScore"] = item.DeductedScores.ToString();
                    dataItem["itemName"] = item.ExamItemName.ToString();
                    dataItem["itemRuleName"] = item.RuleName.ToString();
                    data.Add(dataItem);
                }
                //TODO:new的次数还是挺多的
                //TODO:new的次数还是挺多的
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.LongChuangRuleListView, new String[] { "itemName", "itemScore","itemRuleName" }, new int[] { Resource.Id.itemName, Resource.Id.itemScore,Resource.Id.itemRuleName});
                BrokenRuleListView.Adapter = simpleAdapter;
                base.ShowBrokenRule();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }

        }

        #region 地图和考试模式
        private void ExamModeSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ExamModeSelect(e.Position);
        }
        //用于选择地图（spinner被button触发后，不会触发Selected事件，不知为啥，手动完成功能）
        private void MapSecledOption()
        {
            try
            {
                int index = MapSpinner.SelectedItemPosition;
                //不能使用索引
                if (lastMapSelectedId != index && index > 0)
                {
                    MapSelect(index);
                    lastMapSelectedId = index;
                    RunOnUiThread(() => { btnMapSelect.Text = MapName; });
                }
                //初始化
                else if(index==0)
                {
                    ExamContext.Map = MapSet.Empty;
                    RunOnUiThread(() => { btnMapSelect.Text = "地图选择"; });
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
        private void MapSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            MapSelect(e.Position);
        }
        #endregion


        #region 消息接收

        protected override void RegisterMessages(object Objectmessenger)
        {
            IMessenger messenger = (IMessenger)Objectmessenger;
            messenger.Register<ExamItemStateChangedMessage>(this, OnExamItemStateChanged);
            messenger.Register<ModifyGearOverMessage>(this, OnModifyGearOverMessage);
            base.RegisterMessages(Objectmessenger);
        }
        protected override void OnFingerprintMessage(FingerprintMessage message)
        {
            Logger.Error("指纹消息触发开始考试,但是没有触发考试流程");
        }
        protected override void OnBrokenRule(BrokenRuleMessage message)
        {
            brokenRuleMessage = message;
            lstBorkenRuleInfo.Add(brokenRuleMessage.RuleInfo);
            RunOnUiThread(ShowBrokenRule);
            base.OnBrokenRule(message);
        }

        protected override void OnPullOverTrigger(PullOverTriggerMessage message)
        {
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.PullOver, null);
            }
        }
        protected override void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            try
            {
                if (message.CarSignal == null || message.CarSignal.Gps == null)
                    return;
                tempMessage = message;
                carSignal = message.CarSignal;
                RunOnUiThread(UpdateCarSensorState);
                base.OnCarSignalReceived(message);
                //地图加载
                 MapSecledOption();
                //地图加载
                if (message.CarSignal.Sensor.Brake)
                {
                    if (btnStartExam.Text.Contains("继续"))
                    {
                        RunOnUiThread(() =>
                        {
                            btnStartExam.Text = "结束考试";
                        });
                    }
                    else if (btnPrepareOk.Text.Contains("继续"))
                    {
                        RunOnUiThread(() =>
                        {
                            btnPrepareOk.Text = "结束考试";
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ExamUIName + "SensorCarSignalReceived", ex.Message);
            }

        }
        protected override void OnExamFinishing(ExamFinishingMessage message)
        {
            RunOnUiThread(FinishExam);
            base.OnExamFinishing(message);
        }
        protected override void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            try
            {
                examItemStateChangedMessage = message;
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
                            //lstExamItemColor[i] = ColorStateList.ValueOf(ExamProgressingColor);
                            lstExamItemBackGroundColor[i] = Color.ParseColor(processingColor);
                        }
                        else if (ExamItemState.Finished == message.NewState)
                        {
                            // lstExamItemColor[i] = ColorStateList.ValueOf(ExamFinishedColor);
                            lstExamItemBackGroundColor[i] = Color.ParseColor(finishedColor);
                        }
                    }
                }
                RunOnUiThread(ExamItemChange);
                base.OnExamItemStateChanged(message);
            }
            catch (Exception ex)
            {
                Logger.Error("DuoLunOnExamItemStateChanged", ex.Message);
            }
            base.OnExamItemStateChanged(message);
        }

        //加减挡完成
        protected  void OnModifyGearOverMessage(ModifyGearOverMessage message)
        {
            for (var i = 0; i < AllExamItems.Count(); i++)
            {
                if (AllExamItems[i].ItemCode == message.PassedItemCode)
                {
                   lstExamItemBackGroundColor[i] = Color.ParseColor(finishedColor);
                    currentExam = AllExamItems[i].ItemName;
                }
            }
            //加减挡完成更新加减挡
            RunOnUiThread(() => {
                InitExamItem();
            });

           
        }
        #endregion

        #region 考试项目UI更新相关
        private void ExamItemChange()
        {
            try
            {
                var ExamItemName = examItemStateChangedMessage.ExamItem.Name;

                currentExam = ExamItemName;
                InitExamItem();
            }
            catch (Exception ex)
            {
                Logger.Error("ExamItemChange", ex.Message);
            }
        }
        protected override void InitExamStatusColor()
        {
            ExamInitColor = Color.ParseColor(btnbackColor);
            ExamProgressingColor = Color.Blue;
            ExamFinishedColor = Color.Red;
        }
        protected override void InitExamItemColor()
        {
            lstExamItemColor.Clear();
            lstExamItemBackGroundColor.Clear();
            //可以去掉有一些项目 比如 上车准备 减速让行 #DCDDDD
            Android.Graphics.Color backgroundcolor = new Color(220, 221, 221);
            foreach (var item in AllExamItems)
            {
                lstExamItemColor.Add(ColorStateList.ValueOf(Color.Black));
                lstExamItemBackGroundColor.Add(ExamInitColor);
            }



        }


        #region  信号控件条
        ImageView imgEngine;
        ImageView imgSafety;

        ImageView imgHandbreak;
        ImageView imgDoor;
        ImageView imgClutch;
        ImageView imgBrake;
        ImageView imgLoudspeaker;
        ImageView imgLeftlight;

        ImageView imgRightlight;
        ImageView imgOutline;
        ImageView imgLowBeam;
        ImageView imgHighBeam;
        ImageView imgFoglight;
        private void LoadSensorControl()
        {
            InitSensor();
        }

        protected void InitSensor()
        {
            imgEngine = FindViewById<ImageView>(Resource.Id.imgEngine);
            imgSafety = FindViewById<ImageView>(Resource.Id.imgSafety);
            imgHandbreak = FindViewById<ImageView>(Resource.Id.imgHandbreak);
            imgDoor = FindViewById<ImageView>(Resource.Id.imgDoor);
            imgClutch = FindViewById<ImageView>(Resource.Id.imgClutch);
            imgBrake = FindViewById<ImageView>(Resource.Id.imgBrake);
            imgLoudspeaker = FindViewById<ImageView>(Resource.Id.imgLoudspeaker);
            imgLeftlight = FindViewById<ImageView>(Resource.Id.imgLeftlight);
            imgRightlight = FindViewById<ImageView>(Resource.Id.imgRightlight);
            imgOutline = FindViewById<ImageView>(Resource.Id.imgOutline);
            imgLowBeam = FindViewById<ImageView>(Resource.Id.imgLowBeam);
            imgHighBeam = FindViewById<ImageView>(Resource.Id.imgHighBeam);
            imgFoglight = FindViewById<ImageView>(Resource.Id.imgFoglight);
        }

        protected void UpdateCarSensor()
        {

            var Sensor = carSignal.Sensor;
            if (Sensor == null)
                return;

            if (Sensor.Brake)
                imgBrake.SetImageResource(Resource.Drawable.luozhou_brake_on);
            else
                imgBrake.SetImageResource(Resource.Drawable.luozhou_brake_off);

            if (Sensor.CautionLight)
            {
                imgLeftlight.SetImageResource(Resource.Drawable.luozhou_left_light_on);
                imgRightlight.SetImageResource(Resource.Drawable.luozhou_right_light_on);
            }
            else
            {
                imgLeftlight.SetImageResource(Resource.Drawable.luozhou_left_light_off);
                imgRightlight.SetImageResource(Resource.Drawable.luozhou_right_light_off);
            }

            if (Sensor.Clutch)
                imgClutch.SetImageResource(Resource.Drawable.luozhou_clutch_on);
            else
                imgClutch.SetImageResource(Resource.Drawable.luozhou_clutch_off);

            if (Sensor.Door)
                imgDoor.SetImageResource(Resource.Drawable.luozhou_door_on);
            else
                imgDoor.SetImageResource(Resource.Drawable.luozhou_door_off);

            if (Sensor.Engine)
                imgEngine.SetImageResource(Resource.Drawable.luozhou_engine_on);
            else
                imgEngine.SetImageResource(Resource.Drawable.luozhou_engine_off);

            if (Sensor.FogLight)
                imgFoglight.SetImageResource(Resource.Drawable.luozhou_fog_light_on);
            else
                imgFoglight.SetImageResource(Resource.Drawable.luozhou_fog_light_off);

            if (Sensor.Handbrake)
                imgHandbreak.SetImageResource(Resource.Drawable.luozhou_hand_break_on);
            else
                imgHandbreak.SetImageResource(Resource.Drawable.luozhou_hand_break_off);

            if (Sensor.HighBeam)
                imgHighBeam.SetImageResource(Resource.Drawable.luozhou_high_beam_on);
            else
                imgHighBeam.SetImageResource(Resource.Drawable.luozhou_high_beam_off);

            if (Sensor.LeftIndicatorLight)
                imgLeftlight.SetImageResource(Resource.Drawable.luozhou_left_light_on);
            else
                imgLeftlight.SetImageResource(Resource.Drawable.luozhou_left_light_off);

            if (Sensor.Loudspeaker)
                imgLoudspeaker.SetImageResource(Resource.Drawable.luozhou_loudspeaker_on);
            else
                imgLoudspeaker.SetImageResource(Resource.Drawable.luozhou_loudspeaker_off);

            if (Sensor.LowBeam)
                imgLowBeam.SetImageResource(Resource.Drawable.luozhou_low_beam_on);
            else
                imgLowBeam.SetImageResource(Resource.Drawable.luozhou_low_beam_off);

            if (Sensor.OutlineLight)
                imgOutline.SetImageResource(Resource.Drawable.luozhou_outline_light_on);
            else
                imgOutline.SetImageResource(Resource.Drawable.luozhou_outline_light_off);

            if (Sensor.RightIndicatorLight)
                imgRightlight.SetImageResource(Resource.Drawable.luozhou_right_light_on);
            else
                imgRightlight.SetImageResource(Resource.Drawable.luozhou_right_light_off);

            //泸州这个正确?
            if (Sensor.SafetyBelt)
                imgSafety.SetImageResource(Resource.Drawable.luozhou_safety_belt_on);
            else
                imgSafety.SetImageResource(Resource.Drawable.luozhou_safety_belt_off);
        }
        #endregion




        string btnbackColor = "#EEDC82";
        string processingColor = "#ADD8E6";
        string finishedColor = "#F4A460";


        //todo:可以考虑使用更新的方式下 为何要用移除这种方法了？这也是值得优化的
        private void InitExamItem(bool IsEnable = true)
        {
            try
            {
                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.ExamItemTable);
                mainLinerLayout.RemoveAllViews();
                Button btn;
                int ListID = 0;


                //所有的考试
                //所有的考试项目但是不包含综合评判的
                int ExamItemCount = lstExamItem.Count;
                for (int i = 0; i < ExamItemCount / ExamItemLineCount; i++)
                {
                    relativeLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemLongChuang, null);
                    btn = (Button)relativeLayout.FindViewById<Button>(Resource.Id.list_1_1);

                    btn.Text = lstExamItem[i * ExamItemLineCount];
                    btn.SetTextColor(lstExamItemColor[i * ExamItemLineCount]);
                    btn.SetBackgroundColor(lstExamItemBackGroundColor[i * ExamItemLineCount]);
                 
                    //btn.SetBackgroundColor(Color.ParseColor(btnbackColor));
                    btn.Enabled = IsEnable;
                    btn.Click += ExamItemClick;

                    btn = (Button)relativeLayout.FindViewById<Button>(Resource.Id.list_1_2);
                    btn.Text = lstExamItem[i * ExamItemLineCount + 1];
                    btn.SetTextColor(lstExamItemColor[i * ExamItemLineCount + 1]);
                    btn.SetBackgroundColor(lstExamItemBackGroundColor[i * ExamItemLineCount + 1]);
                    btn.Click += ExamItemClick;
                    btn.Enabled = IsEnable;
                    //btn.SetBackgroundColor(Color.ParseColor(btnbackColor));

                    btn = (Button)relativeLayout.FindViewById<Button>(Resource.Id.list_1_3);
                    btn.Text = lstExamItem[i * ExamItemLineCount + 2];
                    btn.SetTextColor(lstExamItemColor[i * ExamItemLineCount + 2]);
                    btn.SetBackgroundColor(lstExamItemBackGroundColor[i * ExamItemLineCount + 2]);
                    btn.Click += ExamItemClick;
                    btn.Enabled = IsEnable;
                    //btn.SetBackgroundColor(Color.ParseColor(btnbackColor));

                    btn = (Button)relativeLayout.FindViewById<Button>(Resource.Id.list_1_4);
                    btn.Text = lstExamItem[i * ExamItemLineCount + 3];
                    btn.SetTextColor(lstExamItemColor[i * ExamItemLineCount + 3]);
                    btn.SetBackgroundColor(lstExamItemBackGroundColor[i * ExamItemLineCount + 3]);
                    btn.Click += ExamItemClick;
                    btn.Enabled = IsEnable;
                    //btn.SetBackgroundColor(Color.ParseColor(btnbackColor));

                    mainLinerLayout.AddView(relativeLayout);
                }
                //4如果余数大
                int Count = ExamItemCount - (ExamItemCount / ExamItemLineCount) * ExamItemLineCount;
                if (Count > 0)
                {
                    int StartIndex = (ExamItemCount / ExamItemLineCount) * ExamItemLineCount;
                    relativeLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemJingYing, null);
                    for (int i = 0; i < ExamItemLineCount; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                ListID = Resource.Id.list_1_1;
                                break;
                            case 1:
                                ListID = Resource.Id.list_1_2;
                                break;
                        }

                        if (i >= Count)
                        {
                            btn = (Button)relativeLayout.FindViewById<Button>(ListID);
                            btn.Visibility = ViewStates.Invisible;
                        }
                        else
                        {
                            btn = (Button)relativeLayout.FindViewById<Button>(ListID);
                            btn.SetTextColor(lstExamItemColor[StartIndex + i]);
                            btn.Text = lstExamItem[StartIndex + i];
                            //btn.SetBackgroundColor(lstExamItemBackGroundColor[StartIndex + i]);
                            btn.Click += ExamItemClick;
                            btn.Enabled = IsEnable;
                        }


                    }
                    mainLinerLayout.AddView(relativeLayout);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ExamUIName + "InitExamItem", ex.Message);
            }

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
                       EndExam();
                       //if (ExamContext.IsExaming)
                       //{
                       //    //调用考试流程进行结束考试
                       //    //Logger.InfoFormat("ExamContextIsExaming");
                       //    Module.StopAsync().ContinueWith((task) =>
                       //    {
                       //        // Logger.InfoFormat("Module.StopAsync()");
                       //    });
                       //}
                       
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
        protected override async Task ReleaseModuleAsync()
        {
            await Module.StopAsync(true);
            Module.Dispose();
            Module = null;
        }
        public void ExamItemClick(object sender, EventArgs e)
        {
            try
            {
                Button btnExamItem = (Button)sender;
                string ItemCode = dataService.GetExamItemCode(btnExamItem.Text);
                if (!string.IsNullOrEmpty(ItemCode))
                {
                    Module.StartExamItemManualAsync(ExamContext, ItemCode, null);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ExamUIName + "ExamItemClick", ex.Message);
            }


        }
        #endregion
    }
}