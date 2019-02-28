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
using TwoPole.Chameleon3.Business.HaiNan.SanYa.Modules;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Infrastructure.Services;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "JingYing")]
    public class JingYing : BaseExamActivity
    {
        Button btnStartExam;
        Button btnEndExam;
        Button btnReturnHome;
        TextView tvScore;
        TextView tvSensorInfo;
        TextView tvAuthInfo;

        #region 车辆状态
        Button btnDoor;
        Button btnEngine;
        Button btnHandBreak;
        Button btnBreake;
        Button btnCluth;
        Button btnSafeBelt;
        Button btnSpeaker;
        Button btnTurnLeftLight;
        Button btnTurnRightLight;
        Button btnOutLineLight;
        Button btnHighBeamLight;
        Button btnLowBeamLight;
        Button btnFogLight;
        Button btnNeturl;

        #endregion
        Spinner MapSpinner;
        Spinner ExamModeSpinner;

        ListView BrokenRuleListView;
        ImageView ImageFinger;
        LinearLayout relativeLayout;
        protected new ExamModule Module { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.JingYing);
            InitUI();
        }
        protected override async Task InitModuleAsync(IMapSet map)
        {
            Module = new ExamModule();
            await Module.InitAsync(new ExamInitializationContext(map)).ContinueWith(task =>
            {

            });

        }

        protected override void InitParameter()
        {
            ExamUIName = "JingYing";
            ExamItemLineCount = 2;
            base.InitParameter();
        }
        private void InitControl()
        {
            btnStartExam = FindViewById<Button>(Resource.Id.btnStartExam);
            btnEndExam = FindViewById<Button>(Resource.Id.btnEndExam);
            btnReturnHome = FindViewById<Button>(Resource.Id.btnReturnHome);
            ImageFinger = FindViewById<ImageView>(Resource.Id.ImageFinger);
            BrokenRuleListView = (ListView)FindViewById(Resource.Id.BrokenRule_list);
            MapSpinner = (Spinner)FindViewById(Resource.Id.MapSpiner);
            ExamModeSpinner = (Spinner)FindViewById(Resource.Id.ExamModeSpinner);
            tvSensorInfo = FindViewById<TextView>(Resource.Id.tvSensorInfo);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
            tvAuthInfo = FindViewById<TextView>(Resource.Id.tvAuthInfo);
            btnDoor = FindViewById<Button>(Resource.Id.btnDoor);
            btnEngine = FindViewById<Button>(Resource.Id.btnEngine);
             btnHandBreak = FindViewById<Button>(Resource.Id.btnHandBreak);
             btnBreake = FindViewById<Button>(Resource.Id.btnBreake);
             btnCluth = FindViewById<Button>(Resource.Id.btnCluth);
             btnSafeBelt = FindViewById<Button>(Resource.Id.btnSafeBelt);
             btnSpeaker = FindViewById<Button>(Resource.Id.btnSpeaker);
             btnTurnLeftLight = FindViewById<Button>(Resource.Id.btnTurnLeftLight);
             btnTurnRightLight = FindViewById<Button>(Resource.Id.btnTurnRightLight);
             btnOutLineLight = FindViewById<Button>(Resource.Id.btnOutLineLight);
             btnHighBeamLight = FindViewById<Button>(Resource.Id.btnHighBeamLight);
             btnLowBeamLight = FindViewById<Button>(Resource.Id.btnLowBeamLight);
             btnFogLight = FindViewById<Button>(Resource.Id.btnFogLight);
             btnNeturl = FindViewById<Button>(Resource.Id.btnNeturl);
            btnStartExam.Click += BtnStartExam_Click;
            btnEndExam.Click += BtnEndExam_Click;
            btnReturnHome.Click += BtnReturnHome_Click;
            ImageFinger.Click += BtnStartExam_Click;
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
            ExamModeSpinner.ItemSelected += ExamModeSpinner_ItemSelected;

        }

        private void BtnEndExam_Click(object sender, EventArgs e)
        {
            EndExam();
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
        #region 开始考试
        private void ImageFinger_Click(object sender, EventArgs e)
        {
            StartExam();
        }
        private void BtnStartExam_Click(object sender, EventArgs e)
        {
            StartExam();
        }

        protected override void StartExam()
        {
            try
            {
                if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                    return;
                //判断一下当前是否正在考试如果正在考试

                //TODO:有可能存在Bug!
                //if (btnStartExam.Enabled==false)
                //{
                //    return;
                //}
                //指纹验证成功,开始考试，请在三分钟之内完成上车准备
                //三亚特殊需要先熄火在录入指纹

                //泸县使用原来的美女头像，不要指纹图片，不需要喜欢
                //海口版本不要先熄火
                if (!DataBase.VersionNumber.Contains("泸县"))
                {
                    if (tempMessage != null && tempMessage.CarSignal.Sensor.Engine&& !DataBase.VersionNumber.Contains("泸县"))
                    {
                        //客户觉得每次都要熄火太麻烦
                        //speaker.PlayAudioAsync("请先熄火再录指纹");
                        //return;
                    }
                    ImageFinger.SetImageResource(Resource.Drawable.Fingerprintsuccess);
                }

                tvScore.Text = string.Format("分数:{0}",100);
                
           
                  
                btnStartExam.Enabled = false;
                btnEndExam.Enabled = true;
                ImageFinger.Enabled = false;
                MapSpinner.Enabled = false;
                ExamModeSpinner.Enabled = false;
                InitExamItemColor();
                InitExamItem(true);
                InitBrokenRules();
                ExamContext.EndExamTime = null;
                Module.StartAsync(ExamContext).ContinueWith((task) =>
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
            //泸县使用原来的美女头像，不要指纹图片
            if (!DataBase.VersionNumber.Contains("泸县"))
                ImageFinger.SetImageResource(Resource.Drawable.Fingerprint);
            btnStartExam.Enabled = true;
            btnEndExam.Enabled = false;
            ImageFinger.Enabled = true;
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
            //清空当前初始化BrokenRules
            lstBorkenRuleInfo.Clear();
            BrokenRuleListView.Adapter = null;
            base.InitBrokenRules();
        }

        protected void UI_Load()
        {
            btnEndExam.Enabled = false;
            //泸县使用原来的美女头像，不要指纹图片
            if (!DataBase.VersionNumber.Contains("泸县"))
               ImageFinger.SetImageResource(Resource.Drawable.Fingerprint);

        }
        protected override void InitUI()
        {

            InitControl();

            UI_Load();

            InitBrokenRules();

            InitAuthInfo(tvAuthInfo);

            InitExamItemColor();

            InitExamItem(false);

            BindExamMapLinesSpinner(MapSpinner);

            BindExamModeSpinner(ExamModeSpinner);

            InitExamContext();

            InitModuleAsync(ExamContext.Map);

            base.InitUI();
        }

        protected void SetButtonColor(Button btn, bool Status)
        {
            //Logger.Error("SetButtonColor"+Status.ToString());
            if (Status)
            {
                btn.SetTextColor(ColorStateList.ValueOf(Color.Red));
            }
            else
            {
                btn.SetTextColor(ColorStateList.ValueOf(Color.Black));
            }
        }
        protected void UpdateCarSensor()
        {
            SetButtonColor(btnBreake, carSignal.Sensor.Brake);
            SetButtonColor(btnCluth, carSignal.Sensor.Clutch);
            SetButtonColor(btnDoor, carSignal.Sensor.Door);
            SetButtonColor(btnEngine, carSignal.Sensor.Engine);
            SetButtonColor(btnFogLight, carSignal.Sensor.FogLight);
            SetButtonColor(btnHandBreak, carSignal.Sensor.Handbrake);
            SetButtonColor(btnHighBeamLight, carSignal.Sensor.HighBeam);
            SetButtonColor(btnLowBeamLight, carSignal.Sensor.LowBeam);
            SetButtonColor(btnNeturl, carSignal.Sensor.IsNeutral);
            SetButtonColor(btnOutLineLight, carSignal.Sensor.OutlineLight);
            SetButtonColor(btnSafeBelt, carSignal.Sensor.SafetyBelt);
            SetButtonColor(btnSpeaker, carSignal.Sensor.Loudspeaker);
            SetButtonColor(btnTurnLeftLight, carSignal.Sensor.LeftIndicatorLight);
            SetButtonColor(btnTurnRightLight, carSignal.Sensor.RightIndicatorLight);
            if (carSignal.Sensor.CautionLight)
            {
                SetButtonColor(btnTurnLeftLight, true);
                SetButtonColor(btnTurnRightLight, true);
            }
        }

        public void UpdateCarSensorState()
        {
            try
            {
                //开始考试按钮状态不可以点击 表示已经开始考试


                if (Module != null && Module.ExamManager != null)
                    currentExamItem = Module.ExamManager.ExamItems.LastOrDefault(x => x.State == ExamItemState.Progressing);
                if (currentExamItem != null)
                {
                    if (currentExamItem.ItemCode != ExamItemCodes.PrepareDriving)
                    {
                        tvSensorInfo.Text = string.Format("正在训练“{0}”里程:{1:0}米 速度:{2}KM/h 转速:{3}转 方向:{4:#0.00}°档位:{5}", currentExamItem.Name, carSignal.Distance, carSignal.SpeedInKmh, carSignal.EngineRpm, carSignal.BearingAngle, Convert.ToInt32(carSignal.Sensor.Gear).ToString());
                    }
                    else
                        tvSensorInfo.Text = string.Format("里程:{0:0}米 速度:{1}KM/h 转速:{2}转 方向:{3:#0.00}°档位:{4}", carSignal.Distance, carSignal.SpeedInKmh, carSignal.EngineRpm, carSignal.BearingAngle, Convert.ToInt32(carSignal.Sensor.Gear).ToString());
                }
                else
                {
                    tvSensorInfo.Text = string.Format("里程:{0:0}米 速度:{1}KM/h 转速:{2}转 方向:{3:#0.00}°档位:{4}", carSignal.Distance, carSignal.SpeedInKmh, carSignal.EngineRpm, carSignal.BearingAngle, Convert.ToInt32(carSignal.Sensor.Gear).ToString());
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

               // var TempBrokenRules = lstBorkenRuleInfo.OrderByDescending(s => s.BreakTime);

                foreach (var item in lstBorkenRuleInfo.OrderByDescending(s => s.BreakTime))
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] = "";
                    dataItem["itemName"] = string.Format("{0} -{1} {2}", item.ExamItemName, item.DeductedScores, item.RuleName);
                    data.Add(dataItem);
                }
                //TODO:new的次数还是挺多的
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.MapListView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });
                BrokenRuleListView.Adapter = simpleAdapter;

                tvScore.Text = string.Format("分数:{0}", ExamContext.ExamScore);
                base.ShowBrokenRule();
            }
            catch (Exception ex)
            {
                Logger.Error(ExamUIName + "ShowBrokenRule", ex.Message);
            }

        }

        #region 地图和考试模式
        private void ExamModeSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ExamModeSelect(e.Position);
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
            messenger.Register<ExamStatusMessage>(this, OnExamStatusMessage);
           // messager.Register<EngineChangedMessage>(this, OnEngineChanged);
            base.RegisterMessages(Objectmessenger); 
        }
        private void OnEngineChanged(EngineChangedMessage message)
        {
            //var carSignalSet = Singleton.GetCarSignalSet;
            //if (!carSignalSet.Current.Sensor.SafetyBelt)
            //{
            //    //再界面通过消息机制传递数据
            //    messager.Send(new SafetyBeltMessage());
            //}
        }
        protected void OnExamStatusMessage(ExamStatusMessage message)
        {
            RunOnUiThread(()=> {tvSensorInfo.Text= string.Format("上车准备:{0}车头:{1}车尾:{2}外后视镜:{3}内后视镜:{4}座椅:{5}",
                       message.Message,
                       message.SensorInfo.ArrivedHeadstock ? "是" : "否",
                       message.SensorInfo.ArrivedTailstock ? "是" : "否",
                       message.SensorInfo.ExteriorMirror ? "是" : "否",
                       message.SensorInfo.InnerMirror ? "是" : "否",
                       message.SensorInfo.Seats ? "是" : "否");
            });
        }
        protected override void OnFingerprintMessage(FingerprintMessage message)
        {
            RunOnUiThread(StartExam);
           // base.OnFingerprintMessage(message);
        }
        protected override void OnBrokenRule(BrokenRuleMessage message)
        {
            brokenRuleMessage = message;
            lstBorkenRuleInfo.Add(brokenRuleMessage.RuleInfo);
            RunOnUiThread(ShowBrokenRule);
            base.OnBrokenRule(message);
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
            }
            catch (Exception ex)
            {
                Logger.Error(ExamUIName + "SensorCarSignalReceived", ex.Message);
            }

        }
        protected override void OnExamFinishing(ExamFinishingMessage message)
        {
            RunOnUiThread(EndExam);
            base.OnExamFinishing(message);
        }
        protected override void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            try
            {
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
                            lstExamItemColor[i] = ColorStateList.ValueOf(ExamProgressingColor);
                            lstExamItemBackGroundColor[i] = Color.Blue;
                        }
                        else if (ExamItemState.Finished == message.NewState)
                        {
                            lstExamItemColor[i] = ColorStateList.ValueOf(ExamFinishedColor);
                            lstExamItemBackGroundColor[i] = Color.Red;
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
        #endregion

        #region 考试项目UI更新相关
        private void ExamItemChange()
        {
            InitExamItem();
        }
        protected override void InitExamStatusColor()
        {
            ExamInitColor = Color.Black;
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
                lstExamItemColor.Add(ColorStateList.ValueOf(ExamInitColor));
                lstExamItemBackGroundColor.Add(backgroundcolor);
            }

        }
        private void InitExamItem(bool IsEnable=true)
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
                 relativeLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemJingYing, null);
                    btn = (Button)relativeLayout.FindViewById<Button>(Resource.Id.list_1_1);

                    btn.Text = lstExamItem[i * ExamItemLineCount];
                    btn.SetTextColor(lstExamItemColor[i * ExamItemLineCount]);
                    //btn.SetBackgroundColor(lstExamItemBackGroundColor[i * ExamItemLineCount]);
                    //btn.SetBackgroundColor()
                    btn.Enabled = IsEnable;
                    btn.Click += ExamItemClick;

                    btn = (Button)relativeLayout.FindViewById<Button>(Resource.Id.list_1_2);
                    btn.Text = lstExamItem[i * ExamItemLineCount + 1];
                    btn.SetTextColor(lstExamItemColor[i * ExamItemLineCount + 1]);
                   // btn.SetBackgroundColor(lstExamItemBackGroundColor[i * ExamItemLineCount + 1]);
                    btn.Click += ExamItemClick;
                    btn.Enabled = IsEnable;

                    mainLinerLayout.AddView(relativeLayout);
                }
                //4如果余数大
                int Count = ExamItemCount - (ExamItemCount / ExamItemLineCount) * ExamItemLineCount;
                if (Count > 0)
                {
                    int StartIndex =(ExamItemCount / ExamItemLineCount)*ExamItemLineCount;
                    relativeLayout =(LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemJingYing, null);
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