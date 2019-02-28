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
//古蔺没有延迟3秒
using TwoPole.Chameleon3.Business.GuLin.Modules;

namespace TwoPole.Chameleon3
{
    /// <summary>
    /// 华纵考试界面 感觉是两个线程
    /// </summary>
    [Activity(Label = "HuaZhongLuZhou")]
    public class HuaZhongLuZhou : BaseExamActivity
    {
        private RelativeLayout relativeLayout;
        private List<KeyValuePair<Int32, string>> lstKeyValueExamItems = new List<KeyValuePair<int, string>>();
        private bool StartEndStatus = true;
     
        TextView tvScore;
        TextView tvUseTime;
        TextView tvSpeed;
        TextView tvMileage;
        TextView tvAngle;
        TextView tvGear;
        Button btnStart;
        Button btnEnd;
        Button btnBack;
        Spinner MapSpinner;

        CheckBox chkRoundCar;
        CheckBox chkLightSimulation;

        TextView tvSatelliteCount;
        TextView tvArtificialEvaluation;

        ListView TipsListView;
        private Queue<string> TipsQueque = new Queue<string>();
        private int ExamDistance = 0;
        private bool IsAutoTriggerPullOver = false;
        private bool IsTriggerPullOver = false;
        private string ExamSuccessVoice = string.Empty;
        private string ExamFailVoice = string.Empty;
        private bool IsHandBreakeChange = false;


        Authorization authorization;
        List<BrokenRuleInfo> lstRules = new List<BrokenRuleInfo>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.huazhongLuzhzou);
            authorization = new Authorization(this);
            InitParameter();
            GetIntentParameter();
            InitUI();
            InitBrokenRules();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }


        //队列来存储消息 先进先出
        protected void AddTips(string Tips)
        {
            if (TipsQueque.Count >= 5)
            {
                TipsQueque.Dequeue();
            }
            TipsQueque.Enqueue(Tips);
        }

        protected void ShowTips()
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;
                // Logger.ErrorFormat("TipsQueQue:" + TipsQueque.Count.ToString());
                foreach (var item in TipsQueque)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] = string.Empty;
                    dataItem["itemName"] = item;
                    data.Add(dataItem);
                }
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.MapListView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });
                TipsListView.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
                Logger.Error("ShowTips", ex.Message);
            }
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string msg = e.Exception.Message;
            Logger.Error(this.GetType().ToString(), msg);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                ShowConfirmDialog();
            }
            return base.OnKeyDown(keyCode, key);
        }
  

        protected override void InitUI()
        {
            InitControl();
            InitSensor();
            InitExamItemColor();
            InitExamItem();
            InitMapLines();
            InitExamContext();
            InitModuleAsync(ExamContext.Map);
            base.InitUI();
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
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            MapSpinner = (Spinner)FindViewById(Resource.Id.MapSpiner);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
            tvUseTime = FindViewById<TextView>(Resource.Id.tvUseTime);
            tvSpeed = FindViewById<TextView>(Resource.Id.tvSpeed);
            tvMileage = FindViewById<TextView>(Resource.Id.tvMileage);
            tvAngle = FindViewById<TextView>(Resource.Id.tvAngle);
            tvGear = FindViewById<TextView>(Resource.Id.tvGear);
            btnEnd = FindViewById<Button>(Resource.Id.btnEndExam);
            btnBack = FindViewById<Button>(Resource.Id.btnBack);
            btnStart = FindViewById<Button>(Resource.Id.btnStartExam);

           
            chkRoundCar= FindViewById<CheckBox>(Resource.Id.chkRoundCar);
            chkLightSimulation = FindViewById<CheckBox>(Resource.Id.chkLightSimulation);
            tvArtificialEvaluation= FindViewById<TextView>(Resource.Id.tvArtificialEvaluation);
            tvSatelliteCount = FindViewById<TextView>(Resource.Id.tvSatelliteCount);
            tvArtificialEvaluation.Click += BtnArtificialEvaluation_Click;
            btnBack.Click += BtnBack_Click;
            TipsListView = FindViewById<ListView>(Resource.Id.tips_list);
            btnStart.Click += btnStartExam_Click;
            btnEnd.Click += btnEndExam_Click;
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;


        }

   
        private void BtnArtificialEvaluation_Click(object sender, EventArgs e)
        {
            base.ArtificialEvaluation();
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            ShowConfirmDialog();
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
        protected override void RegisterMessages(object Objectmessenger)
        {
            IMessenger messenger = (IMessenger)Objectmessenger;
            messenger.Register<ModifyGearOverMessage>(this, OnModifyGearOverMessage);
            base.RegisterMessages(Objectmessenger);
        }
        protected override void OnFingerprintMessage(FingerprintMessage message)
        {
            Logger.Error("指纹消息触发开始考试,但是没有触发考试流程");
        }
        protected void OnModifyGearOverMessage(ModifyGearOverMessage message)
        {
            AddTips("加减挡完成");;
            for (var i = 0; i < AllExamItems.Count(); i++)
            {
                if (AllExamItems[i].ItemCode == message.PassedItemCode)
                {
                    lstExamItemColor[i] = ColorStateList.ValueOf(Color.Green);
                    currentExam = AllExamItems[i].ItemName;
                }
            }
            //加减挡完成更新加减挡
            RunOnUiThread(() => {
                InitExamItem();
            });
        }
        protected override void OnPullOverTrigger(PullOverTriggerMessage message)
        {
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.PullOver, null);
            }
        }
        protected override void OnExamFinishing(ExamFinishingMessage message)
        {
            RunOnUiThread(ExamFinishing);
        }
        public void ExamFinishing()
        {
            AddTips("考试结束");
            btnStart.SetBackgroundColor(Color.Rgb(77, 166, 124));
            btnStart.SetTextColor(Color.Blue);
            btnStart.Text = "请求考试";
            btnEnd.SetBackgroundColor(Color.Rgb(232, 232, 232));
            btnEnd.SetTextColor(Color.Black);
            btnStart.Enabled = true;
            btnEnd.Enabled = false;
            StartEndStatus = true;
            MapSpinner.Enabled = true;
            ExamContext.EndExamTime = DateTime.Now;
            startExamTime = null;
            EndExam();
        }
        protected override void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            try
            {
                examItemStateChangedMessage = message;
                var AllExamItems = dataService.GetExamItemsList();
                currentExam = message.ExamItem.Name;
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
                            //进入起步项目加减挡项目变颜色
                            if (message.ExamItem.ItemCode==ExamItemCodes.Start)
                            {
                                //加减挡颜色
                                for (int j = 0; j < AllExamItems.Count; j++)
                                {
                                    //加减挡
                                    if (AllExamItems[j].ItemCode==ExamItemCodes.ModifiedGear)
                                    {
                                        //加减档位颜色变化
                                        lstExamItemColor[j] = ColorStateList.ValueOf(Color.DarkRed);
                                    }
                                }
                            }
                            lstExamItemColor[i] = ColorStateList.ValueOf(Color.DarkRed);

                            AddTips(string.Format("进入[{0}]项目", AllExamItems[i].ItemName));
                        }
                        else if (ExamItemState.Finished == message.NewState)
                        {
                            lstExamItemColor[i] = ColorStateList.ValueOf(Color.Green);
                            AddTips(string.Format("完成[{0}]项目", AllExamItems[i].ItemName));
                        }
                    }
                }
                //考试状态改变
                RunOnUiThread(ExamItemChange);
            }
            catch (Exception ex)
            {
                Logger.Error(this.GetType().ToString(), ex.Message);
            }
        }

       protected override void OnBrokenRule(BrokenRuleMessage message)
        {
            try
            {
                brokenRuleMessage = message;
                AddTips(string.Format("[{0}][{1}][{2}]", message.RuleInfo.ExamItemName, message.RuleInfo.DeductedScores, message.RuleInfo.RuleName));
                RunOnUiThread(ShowBrokenRule);
            }
            catch (Exception ex)
            {
                Logger.Error(this.GetType().ToString(), ex.Message);
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
            }
            catch (Exception ex)
            {
                Logger.Error("HuaZhongSensorCarSignalReceived", ex.Message);
            }

        }

        public void btnEndExam_Click(object sender, EventArgs e)
        {
            ExamFinishing();
        }

        public void InitExamParam()
        {
            //上车准备永远需要启动
            //globalSetting.PrepareDrivingEnable = chkRoundCar.Checked;
            globalSetting.SimulationsLightOnDay = chkLightSimulation.Checked; 
            globalSetting.SimulationsLightOnNight = chkLightSimulation.Checked; 
        }
        private DateTime? startExamTime { get; set; }
        protected override void StartExam()
        {
            //if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
            //    return;
            //不在检测授权
            Logger.Error("开始考试");
            if (startExamTime.HasValue)
            {
                Logger.Error("正在考试");
                return;
            }
            //重置初始化静态判断变量
            Constants.IsTriggerStartExam = false;
            startExamTime = DateTime.Now;
            TipsQueque.Clear();
            lstBorkenRuleInfo.Clear();
            AddTips("开始考试");
            btnEnd.Enabled = true;
            btnEnd.Visibility = ViewStates.Visible;
            btnEnd.SetBackgroundColor(Color.DarkRed);
            btnEnd.SetTextColor(Color.Blue);
            StartEndStatus = false;
            //设置地图选项和白考也考选项
            MapSpinner.Enabled = false;
            IsTriggerPullOver = false;
            //初始化开始项目颜色
            InitExamItemColor();
            InitExamItem();
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            InitBrokenRules();
            InitExamParam();
            ExamContext.StartExam();
            Module.StartAsync(ExamContext).ContinueWith((task) =>
            {
            });
        }
        public void btnStartExam_Click(object sender, EventArgs e)
        {

            //1.绕车去掉 模拟灯光勾上 点请求考试 请上车准备考试 在点继续考试 直接进入模拟灯光  不点继续考试模拟灯光都不报
            //2.绕车和灯光一起去掉 点请求考试 报 请上车准备考试 在点继续考试  进入起步状态和播报 请继续完成考试
            try
            {
                var btn = (Button)sender;
              
                Logger.Error("StartExaxmClick" + btn.Text);
                if (btn.Text == "请求考试")
                {
                    StartExam();
                    //继续考试
                    btn.Text = "继续考试";
                }
                else if (btn.Text == "继续考试")
                {
                    if (chkLightSimulation.Checked == true && chkRoundCar.Checked == false)
                    {
                        messager.Send<PrepareDrivingFinishedMessage>(new PrepareDrivingFinishedMessage(false));
                    }
                    //如果绕车和模拟灯光都没有勾上
                    else if (chkRoundCar.Checked == false && chkLightSimulation.Checked == false)
                    {
                        speaker.PlayAudioAsync("请继续完成考试");
                        messager.Send<PrepareDrivingFinishedMessage>(new PrepareDrivingFinishedMessage(false));
                    }
                    else if (chkLightSimulation.Checked == true && chkRoundCar.Checked == true)
                    {
                        messager.Send<PrepareDrivingFinishedMessage>(new PrepareDrivingFinishedMessage(true));
                    }
                    btnStart.Enabled = false;
                    btnStart.SetBackgroundColor(Color.Rgb(232, 232, 232));
                    btnStart.SetTextColor(Color.Black);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(this.GetType().ToString(),ex.InnerException.Message);
            }
       
     
        }


        protected void InitMapLines()
        {
            try
            {
                var mapLines = dataService.ALLMapLines;
                MapLine mapLine = new MapLine() { Id = 0, Name = "线路选择" };
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
                Logger.Error("HuaZhongInitMap", ex.Message);
            }
        }
        protected virtual async Task InitModuleAsync(IMapSet map)
        {
            Module = new ExamModule();
            await Module.InitAsync(new ExamInitializationContext(map)).ContinueWith(task =>
            {

            });
        }

        public void EndExam()
        {
            Module.StopAsync().ContinueWith((task) =>
            {
            });
        }
        protected override void ShowBrokenRule()
        {
            try
            {
                InitBrokenRules();

                lstBorkenRuleInfo.Add(brokenRuleMessage.RuleInfo);
                for (int i = lstBorkenRuleInfo.Count - 1; i >= 0; i--)
                {
                    mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableHuaZhong, null);
                    TableTextView txt = (TableTextView)relativeLayout.FindViewById(Resource.Id.list_1_1);
                    txt.SetText(lstBorkenRuleInfo[i].ExamItemName, TextView.BufferType.Normal);
                    txt.SetTextColor(Color.Red);
                    txt.Tag = lstBorkenRuleInfo[i].Id.ToString();
                    txt.LongClick += Txt_LongClick;
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
        private void Txt_LongClick(object sender, View.LongClickEventArgs e)
        {
            try
            {
                TableTextView tv = (TableTextView)sender;
                int id = Convert.ToInt32(tv.Tag.ToString().Trim());
                var removeItem = lstBorkenRuleInfo.Where(s => s.Id == id).FirstOrDefault();
                lstBorkenRuleInfo.Remove(removeItem);
                InitBrokenRules();
                for (int i = lstBorkenRuleInfo.Count - 1; i > 0; i--)
                {
                    var item = lstBorkenRuleInfo[i];
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableHuaZhong, null);
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
                LogManager.WriteSystemLog(ex.Message + ex.Source + ex.StackTrace);
            }

        }
        private void ExamItemChange()
        {
            try
            {
                var ExamItemName = examItemStateChangedMessage.ExamItem.Name;
                InitExamItem();
            }
            catch (Exception ex)
            {
                Logger.Error("ExamItemChange", ex.Message);
            }
        }
        protected override void InitBrokenRules()
        {
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableHuaZhong, null);
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
        /// 更新车辆信号的状态
        /// </summary>
        public void UpdateCarSensorState()
        {
            try
            {
                //更新一些基本的信息
                if (ExamContext.IsExaming && !StartEndStatus)
                {
                    tvScore.Text = string.Format("{0}", ExamContext.ExamScore);
                    tvUseTime.Text = string.Format("用时:{0:00}:{1:00}:{2:00}", carSignal.UsedTime.Hours, carSignal.UsedTime.Minutes, carSignal.UsedTime.Seconds);
                    tvMileage.Text = string.Format("里程:{0:0}", carSignal.Distance);
                    ShowTips();
                }
                tvSatelliteCount.Text = "卫星:" + Convert.ToInt32(carSignal.Gps.FixedSatelliteCount).ToString();
                tvSpeed.Text = string.Format("速度:{0:0}转速:{1}", carSignal.SpeedInKmh, carSignal.Sensor.EngineRpm);
                tvGear.Text = "档位:" + Convert.ToInt32(carSignal.Sensor.Gear).ToString();
                tvAngle.Text = "角度:" + string.Format("{0:N2}°", carSignal.BearingAngle);
                UpdateCarSensor();
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
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.HuaZhongExamItemButton, null);
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
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.HuaZhongExamItemButton, null);
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
    }
}