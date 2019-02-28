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
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;
using Android.Content.Res;
namespace TwoPole.Chameleon3
{
    [Activity(Label = "北京科技大学 VR实验室界面")]
    public class BeiKe : BaseExamActivity
    {

        private List<KeyValuePair<Int32, string>> lstKeyValueExamItems = new List<KeyValuePair<int, string>>();


        private bool IsExam = false;


        //扣分的序号
        //
        //扣分的序号
        private int BreakRuleIndex = 0;

        Button btnStartExam;
        Button btnEndExam;
        Button btnStartPullOver;
        Button btnPrepareDrivingFinish;
        TextView tvInfo;
        TextView tvExit;
        TextView tvScore;
        Spinner MapSpinner;
        Spinner ExamItemSpinner;
        RadioButton radMap;
        RadioButton radExamItem;
        CheckBox chkClosePrepareDriving;
        CheckBox chkCloseSimulationLights;
        CheckBox chkExamFailEnd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BeiKe);

            InitUI();

            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

        }
        protected override void InitUI()
        {
            InitControl();

            InitBrokenRules();

            InitAuthInfo(tvInfo);

            InitExamItemColor();

            InitExamItem(false);

            BindExamMapLinesSpinner(MapSpinner);

            BindExamItemSpinner(lstExamItem);
            //BindExamModeSpinner(ExamModeSpinner);

            InitExamContext();


            InitModuleAsync(ExamContext.Map);



            base.InitUI();
        }
        protected override void InitParameter()
        {
            base.InitParameter();
            ExamUIName = "BeiKe";
            ExamItemLineCount = 6;

            //TODO:这个不应该在这写死吧 不太好
            for (int i = 0; i < lstExamItem.Count; i++)
            {
                if (lstExamItem[i] == "公共汽车站")
                {
                    lstExamItem[i] = "公交汽车";
                }
            }


        }
        protected override void InitExamStatusColor()
        {
            ExamInitColor = Color.White;
            ExamProgressingColor = Color.Blue;
            ExamFinishedColor = Color.Yellow;
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
        protected override void RegisterMessages(object Objectmessenger)
        {
            base.RegisterMessages(Objectmessenger);
        }

       //TODO:这种写法不科学，可以考虑用成熟控件代替
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
                    linearLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemBeiKe, null);

                    for (int j = 0; j < ExamItemLineCount; j++)
                    {
                        ListID = GetExamItemId(j);
                        btn = (Button)linearLayout.FindViewById<Button>(ListID);
                        btn.Text = lstExamItem[i * ExamItemLineCount + j];
                        btn.SetTextColor(lstExamItemColor[i * ExamItemLineCount + j]);
                        btn.Enabled = IsEnable;
                        btn.Click += ExamItemClick;
                    }

                    mainLinerLayout.AddView(linearLayout);
                }
                //4如果余数大
                int Count = ExamItemCount - (ExamItemCount / ExamItemLineCount) * ExamItemLineCount;
                if (Count > 0)
                {
                    int StartIndex = (ExamItemCount / ExamItemLineCount) * ExamItemLineCount;
                    linearLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemBeiKe, null);
                    for (int i = 0; i < ExamItemLineCount; i++)
                    {
                        ListID = GetExamItemId(i);
                        if (i >= Count)
                        {
                            btn = (Button)linearLayout.FindViewById<Button>(ListID);
                            btn.Visibility = ViewStates.Invisible;
                        }
                        else
                        {
                            btn = (Button)linearLayout.FindViewById<Button>(ListID);
                            btn.SetTextColor(lstExamItemColor[StartIndex + i]);
                            btn.Text = lstExamItem[StartIndex + i];
                            btn.Click += ExamItemClick;
                            btn.Enabled = IsEnable;
                        }


                    }
                    mainLinerLayout.AddView(linearLayout);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ExamUIName + "InitExamItem", ex.Message);
            }

        }

        public void ExamItemClick(object sender, EventArgs e)
        {
            try
            {
                Button btnExamItem = (Button)sender;
                var btnText = btnExamItem.Text;
                if (btnText == "公交汽车")
                {
                    btnText = "公共汽车站";
                }
                string ItemCode = dataService.GetExamItemCode(btnText);
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
        public override void GetIntentParameter()
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



        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            MapSpinner = (Spinner)FindViewById(Resource.Id.MapSpiner);
            ExamItemSpinner = (Spinner)FindViewById(Resource.Id.ExamItemSpiner);
            tvInfo = FindViewById<TextView>(Resource.Id.tvInfo);
            tvExit = FindViewById<TextView>(Resource.Id.tvExit);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
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
            tvExit.Click += TvExit_Click;

            chkClosePrepareDriving.Checked = !globalSetting.PrepareDrivingEnable;
            chkExamFailEnd.Checked = !globalSetting.ContinueExamIfFailed;
            chkCloseSimulationLights.Checked = !globalSetting.SimulationsLightOnDay;
            chkCloseSimulationLights.Checked = !globalSetting.SimulationsLightOnNight;


        }

        private void TvExit_Click(object sender, EventArgs e)
        {
            ShowConfirmDialog();
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
            OnPullOverTrigger(new PullOverTriggerMessage());
        }
        protected override void OnPullOverTrigger(PullOverTriggerMessage message)
        {
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.PullOver, null);
            }
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
            EndExam();
        }

        private void BtnStartExam_Click(object sender, EventArgs e)
        {
            try
            {
                if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                    return;

                MapSpinner.Enabled = false;


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
                chkClosePrepareDriving.Enabled = false;
                chkCloseSimulationLights.Enabled = false;
                chkExamFailEnd.Enabled = false;
                IsExam = true;
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

            if (!IsExam)
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


        //靠边停车触发考试结束
        protected override void OnExamFinishing(ExamFinishingMessage message)
        {
            RunOnUiThread(EndExam);
            base.OnExamFinishing(message);
        }
        protected override void OnExamItemStateChanged(ExamItemStateChangedMessage message)
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
                Logger.Error("BeiKeOnExamItemStateChanged", ex.Message);
            }
        }
        private void ExamItemChange()
        {
            InitExamItem(true);
        }

        protected override void OnBrokenRule(BrokenRuleMessage message)
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
        protected override void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            try
            {
                carSignal = message.CarSignal;
                RunOnUiThread(UpdateCarSensorState);
            }
            catch (Exception ex)
            {
                Logger.Error("BeiKeCarSignalReceived", ex.Message);
            }

        }


        protected override void StartExam()
        {
        
            BreakRuleIndex = 0;
            //第一步首先清楚上一次考试项目的扣分规则
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            mainLinerLayout.RemoveAllViews();

            InitBrokenRules();

            InitExamItem();
            //TODO:这点
            //             
            try
            {
                //可能
                List<Setting> lstSetting = new List<Setting> { };
                var setting = new Setting { Key = "PrepareDrivingEnable", Value = (!chkClosePrepareDriving.Checked).ToString(), GroupName = "GlobalSettings" };
                lstSetting.Add(setting);
                setting = new Setting { Key = "ContinueExamIfFailed", Value = (!chkExamFailEnd.Checked).ToString(), GroupName = "GlobalSettings" };
                lstSetting.Add(setting);
                setting = new Setting { Key = "SimulationsLightOnDay", Value = (!chkCloseSimulationLights.Checked).ToString(), GroupName = "GlobalSettings" };
                lstSetting.Add(setting);
                setting = new Setting { Key = "SimulationsLightOnNight", Value = (!chkCloseSimulationLights.Checked).ToString(), GroupName = "GlobalSettings" };
                lstSetting.Add(setting);

                globalSetting.PrepareDrivingEnable = !chkClosePrepareDriving.Checked;
                globalSetting.ContinueExamIfFailed = !chkExamFailEnd.Checked;
                globalSetting.SimulationsLightOnDay = !chkCloseSimulationLights.Checked;
                globalSetting.SimulationsLightOnNight = !chkCloseSimulationLights.Checked;

                //UpdateSettings(lstSetting);
            }
            catch (Exception ex)
            {
                Logger.Error(ExamUIName, "startExamUpdateSettingError:" + ex.Message);
            }
            ExamContext.StartExam();
            Module.StartAsync(ExamContext).ContinueWith((task) =>
            {
            });
        }
        public virtual void UpdateSettings(IList<Setting> listSetting)
        {
            //如果是考试项目就进行更新项目的开始和结束语音
            dataService.SaveUpdateSettings(listSetting);

        }
        protected override void EndExam()
        {
            IsExam = false;
            MapSpinner.Enabled = true;
            ExamItemSpinner.Enabled = true;
            btnEndExam.Enabled = false;
            btnStartExam.Enabled = true;
            chkClosePrepareDriving.Enabled = true;
            chkCloseSimulationLights.Enabled = true;
            chkExamFailEnd.Enabled = true;
            ExamContext.EndExamTime = DateTime.Now;
            Module.StopAsync().ContinueWith((task) =>
            {

            });
        }
        protected override void InitBrokenRules()
        {
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            LinearLayout linearLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableBeiKe, null);

            TableTextView txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_1);
            txt.SetText("序号", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_2);
            txt.SetText("错误信息", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_3);
            txt.SetText("扣分", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_4);
            txt.SetText("时间", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_5);
            txt.SetText("代码", TextView.BufferType.Normal);
            txt.SetTextColor(Color.Black);
            txt.SetBackgroundColor(Color.White);

            mainLinerLayout.AddView(linearLayout);
        }
        protected override void ShowBrokenRule()
        {
            try
            {
                BreakRuleIndex += 1;
                BrokenRuleInfo RuleInfo = brokenRuleMessage.RuleInfo;
                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
                LinearLayout linearLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableBeiKe, null);
                TableTextView txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_1);
                txt.SetText(BreakRuleIndex.ToString(), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_2);
                txt.SetText(RuleInfo.RuleName.ToString(), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_3);
                txt.SetText(RuleInfo.DeductedScores.ToString(), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_4);
                txt.SetText(RuleInfo.BreakTime.ToString("HH:mm:ss"), TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                txt = (TableTextView)linearLayout.FindViewById(Resource.Id.list_1_5);
                txt.SetText(RuleInfo.RuleCode, TextView.BufferType.Normal);
                txt.SetTextColor(Color.Red);

                mainLinerLayout.AddView(linearLayout);
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
                if (ExamContext.IsExaming && IsExam)
                {
                    string UseTime = string.Format("{0:00}:{1:00}:{2:00}", carSignal.UsedTime.Hours, carSignal.UsedTime.Minutes, carSignal.UsedTime.Seconds);
                    tvInfo.Text = string.Format("用时:{0}里程:{1:0}速度:{2}转速:{3}方向:{4:#0.00}°档位:{5}", UseTime, carSignal.Distance, carSignal.SpeedInKmh, carSignal.EngineRpm, carSignal.BearingAngle, Convert.ToInt32(carSignal.Sensor.Gear).ToString());
                    tvScore.Text = string.Format("{0}分", ExamContext.ExamScore);
                }

            }
            catch (Exception ex)
            {
                Logger.Error("UpdateCarSensorState", ex.Message);
            }


        }
    }
}