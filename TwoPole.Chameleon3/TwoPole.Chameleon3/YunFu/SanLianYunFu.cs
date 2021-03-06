﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Business.Modules;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Map;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "SanLianYunFun")]
    public class SanLianYunFu : BaseExamActivity
    {
        #region 信号相关变量
        TextView tv_Speed;
        TextView tv_Rev;
        TextView tv_Gear;
        TextView tv_Handbrake;
        TextView tv_Brake;
        TextView tv_Clutch;
        TextView tv_Door;
        //喇叭
        TextView tv_Hron;
        TextView tv_Safety_Belt;
        #endregion
        #region 考试按钮
        Button btn_Start;
        Button btn_Exit;
        TextView tv_RG;
        #endregion
        #region 考试状态相关
        TextView tv_Exam_Status;
        TextView tv_Student_Score;

        TextView tv_exam_mode;
        //里程
        TextView tv_Li;
        TextView tv_Start_Time;
        TextView tv_Yong;
        //线路
        TextView tv_Road;

        TextView tv_current_exam;

        Spinner MapSpinner;
        #endregion

        #region 系统相关变量
        //项目通过颜色
        private string ExamProgressingbackground = "@drawable/shapeSanLianButtonProcess";
        //项目完成颜色
        private string ExamFinishedbackground = "@drawable/shapeSanLianButtonFinish";
        //项目初始化颜色
        private string ExamInitbackground = "@drawable/ShapeSanLianButton";
        private DateTime? startExamTime { get; set; }

        private List<int> lstExamItemBackGroundDrawable = new List<int>();


        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SanLianFuYun);
            InitUI();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            LogManager.WriteSystemLog("SanLianYuuFuAndroidEnvironment_UnhandledExceptionRaise:" + e.Exception.Source + e.Exception.Message + e.Exception.StackTrace);
        }
        //LogManager.WriteSystemLog
        protected override void InitParameter()
        {
            base.InitParameter();

            //默认就不在包含学校区域 
            AllExamItems = dataService.GetExamItemsList().Where(s => s.ItemCode != ExamItemCodes.SchoolArea).ToList();

            lstExamItem = (from v in AllExamItems
                           where v.IsEnable == true
                           select v.ItemName).ToList<string>();

            // Logger.Error("lstExamItemInit:" + string.Join(",", lstExamItem));
            //初始化考试项目的Id
            lstExamItemButtonId = new List<int> { Resource.Id.list_1_1, Resource.Id.list_1_2, Resource.Id.list_1_3 };
        }
        protected override void InitExamItemColor()
        {
            lstExamItemBackGroundDrawable.Clear();
            //TODO:特殊需求 公交车站和人行横道当做一个项目模按钮
            for (int i = 0; i < lstExamItem.Count(); i++)
            {
                lstExamItemBackGroundDrawable.Add(Resource.Drawable.shapeSanLianButton);
            }
        }
        #region 初始化考试项目
        public Button GetExamItemButton(LinearLayout linearLayout, int ResourceId, List<string> lst, int Index)
        {
            Button btn = (Button)linearLayout.FindViewById<Button>(ResourceId);
            //数组索引越界
            if (Index >= lst.Count)
            {
                btn.Text = string.Empty;
                btn.Visibility = ViewStates.Invisible;
                return btn;
            }
            //广州版本特殊需求 公交车站和学校区域使用通过一个按钮
            btn.Text = lst[Index].Contains("车站") ? "公交站/学校" : lst[Index];
            btn.Click += ExamItemClick;
            //资源Id冬天替换就可以了
            Drawable initExamItemDrawable = this.BaseContext.Resources.GetDrawable(lstExamItemBackGroundDrawable[Index]);
            btn.Background = initExamItemDrawable;
            return btn;
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
                Logger.Error(ex, this.GetType().ToString());
            }


        }



        /// <summary>
        /// 初始化考试项目
        /// </summary>
        /// <param name="IsEnable"></param>
        private void InitExamItem(bool IsEnable = true)
        {
            try
            {
                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.ExamItemTable);
                mainLinerLayout.RemoveAllViews();
                int LineMaxCount = 3;
                int RowCount = (int)Math.Ceiling(lstExamItem.Count() * 1.0 / LineMaxCount);
                for (int i = 0; i < RowCount; i++)
                {
                    linearLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemSanLianYunFu, null);
                    for (int j = 0; j < LineMaxCount; j++)
                    {
                        GetExamItemButton(linearLayout, lstExamItemButtonId[j], lstExamItem, i * LineMaxCount + j);
                    }
                    mainLinerLayout.AddView(linearLayout);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType());
            }

        }
        #endregion
        private void InitControl()
        {
            BrokenRuleListView = (ListView)FindViewById(Resource.Id.lv_broken_rule);
            tv_Speed = FindViewById<TextView>(Resource.Id.tv_Speed);
            tv_Brake = FindViewById<TextView>(Resource.Id.tv_Brake);
            tv_Clutch = FindViewById<TextView>(Resource.Id.tv_Clutch);
            tv_Handbrake = FindViewById<TextView>(Resource.Id.tv_Handbrake);
            tv_Door = FindViewById<TextView>(Resource.Id.tv_Door);
            tv_Hron = FindViewById<TextView>(Resource.Id.tv_Hron);
            tv_Safety_Belt = FindViewById<TextView>(Resource.Id.tv_Safety_Belt);
            tv_Rev = FindViewById<TextView>(Resource.Id.tv_Rev);
            tv_Gear = FindViewById<TextView>(Resource.Id.tv_Gear);
            btn_Start = (Button)FindViewById(Resource.Id.btn_Start);
            btn_Exit = (Button)FindViewById(Resource.Id.btn_Exit);
            tv_RG = FindViewById<TextView>(Resource.Id.tv_RG);
            btn_Start.Click += Btn_Start_Click;
            btn_Exit.Click += Btn_Exit_Click;
            tv_RG.Click += Tv_RG_Click;
            tv_Exam_Status = FindViewById<TextView>(Resource.Id.tv_Exam_Status);
            tv_Student_Score = FindViewById<TextView>(Resource.Id.tv_Student_Score);
            tv_Li = FindViewById<TextView>(Resource.Id.tv_Li);
            tv_Start_Time = FindViewById<TextView>(Resource.Id.tv_Start_Time);
            tv_Yong = FindViewById<TextView>(Resource.Id.tv_Yong);
            tv_Road = FindViewById<TextView>(Resource.Id.tv_Road);
            tv_current_exam = FindViewById<TextView>(Resource.Id.tv_current_exam);

            tv_exam_mode = FindViewById<TextView>(Resource.Id.tv_exam_mode);
            tv_exam_mode.Click += Tv_exam_mode_Click;
            MapSpinner = new Spinner(this);
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
            tv_Road.Click += Tv_Road_Click;
            tv_RG.Enabled = false;

            if (!globalSetting.ContinueExamIfFailed)
            {
                tv_exam_mode.Text = "考试模式";
            }
            else
            {
                tv_exam_mode.Text = "训练模式";
            }



        }

        private void Tv_exam_mode_Click(object sender, EventArgs e)
        {
            var tv = (TextView)sender;
            if (tv.Text == "训练模式")
            {
                tv.Text = "考试模式";
                //直接修改内存数据值 不进行数据库保存
                ExamContext.Score.ContinueExamIfFailed = false;
                LightGroups.Clear();
            }
            else if (tv.Text == "考试模式")
            {
                tv.Text = "灯光模式";
                ExamContext.Score.ContinueExamIfFailed = true;
            }
            else if (tv.Text == "灯光模式")
            {
                tv.Text = "训练模式";
                ExamContext.Score.ContinueExamIfFailed = false;
                LightGroups.Clear();
            }
        }

        private void MapSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //Logger.Error("MapSpinner_ItemSelected:" + e.Position.ToString());
            MapSelect(e.Position);
        }

        private void Tv_Road_Click(object sender, EventArgs e)
        {
            MapSpinner.PerformClick();
            MapSecledOption();
        }


        protected void InitMapLines()
        {
            Task.Run(() =>
            {
                try
                {


                    var mapId = dataService.getDefaultMapId();
                    if (dataService.GetAllMapLines().Count >= mapId)
                    {
                        var SelectMapItem = dataService.GetAllMapLines().Where(x => x.Id == mapId).FirstOrDefault();
                        if (SelectMapItem == null)
                        {
                            MapSpinner.SetSelection(0);
                            return;
                        }
                        string name = SelectMapItem.Name;
                        MapName = SelectMapItem.Name;
                        lastMapSelectedId = mapId;
                        MapSpinner.SetSelection(mapId);
                        RunOnUiThread(() => { tv_Road.Text = name; });
                        MapSelect(mapId);
                       
                    }
                }
                catch (Exception e)
                {

                }

            });
        }




        private int lastMapSelectedId = 0;

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
                    RunOnUiThread(() => { tv_Road.Text = MapName; });
                }
                //初始化
                else if (index == 0)
                {
                    ExamContext.Map = MapSet.Empty;
                    RunOnUiThread(() => { tv_Road.Text = "无地图"; });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(this.GetType().ToString(), ex.Message);
            }
        }

        protected override void InitBrokenRules()
        {
            //清空当前初始化BrokenRules
            lstBorkenRuleInfo.Clear();
            BrokenRuleListView.Adapter = null;
            base.InitBrokenRules();
        }

        protected override void InitUI()
        {
            InitControl();
            InitAuthInfo(tv_current_exam);
            InitExamItemColor();
            InitExamItem(false);
            InitBrokenRules();
            BindExamMapLinesSpinner(MapSpinner);
            InitExamContext();
            InitModuleAsync(ExamContext.Map);

            InitMapLines();


            //初始化播放器Timer
            InitPlayTimer();

            //初始化灯光模式
            InitLightModule();
        }

       
        #region 考试按钮

        private void Tv_RG_Click(object sender, EventArgs e)
        {
            try
            {
                base.ArtificialEvaluation();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType());
            }
        }

        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            //灯光相关清空
            ReleaseLightModuleAsync();
            base.ShowConfirmDialog();
        }
        Queue<string> LightGroups = new Queue<string>();
        private void Btn_Start_Click(object sender, EventArgs e)
        {

            //开始考试按钮
            if (btn_Start.Text == "开始考试")
            {
                if (tv_exam_mode.Text == "灯光模式")
                {
                    //训练灯光
                    InitLightModule();
                    StartLightAsync(LightGroups.Dequeue());
                    return;
                }
        
                StartExam();
                if (globalSetting.ShowStudentInfo)
                {
                    ShowStudentInfoDialog();
                }
            }
            else if (btn_Start.Text == "停止考试")
            {
                ExamFinishing();
            }
        }
        AlertDialog alertStudentInfoDialog = null;
        AlertDialog.Builder StudentInfobuilder = null;
        public void ShowStudentInfoDialog()
        {

            IsPlay = true;
            //打开Timer
            timer.Change(1000, globalSetting.VoicePlayInterval * 1000);
            View view = View.Inflate(this, Resource.Layout.Dialog_StudentInfo, null);
            Button btn = view.FindViewById<Button>(Resource.Id.btn_Start);
            btn.Click += Btn_Click;
            if (StudentInfobuilder == null)
            {
                StudentInfobuilder = new AlertDialog.Builder(this, Resource.Style.DialogTheme);
            }
            alertStudentInfoDialog = StudentInfobuilder
           .SetView(view)
           .SetCancelable(false)
           .Create();       //创建alertDialog对象  
            alertStudentInfoDialog.Show();
            alertStudentInfoDialog.Window.SetLayout(800, 500);

        }
        //如果没有信号
        private void Btn_Click(object sender, EventArgs e)
        {
            alertStudentInfoDialog.Cancel();
            IsPlay = false;
            //timer 停止
            timer.Change(0, -1);
            Task.Run(() => { messager.Send<PrepareDrivingFinishedMessage>(new PrepareDrivingFinishedMessage()); });
        }
        private bool IsPlay = false;
        System.Threading.Timer timer = null;
        /// <summary>
        /// 初始化线程播放
        /// </summary>
        ///  
        public void InitPlayTimer()
        {
            if (globalSetting.VoicePlayInterval > 0)
            {
                timer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerStartPlay), null, 1000, globalSetting.VoicePlayInterval * 1000);
            }
        }
        public void TimerStartPlay(object a)
        {
            if (IsPlay)
            {
                timer.Change(1000000000, 1000000000);

                IsPlay = true;
                speaker.PlayAudioAsync("请核对学员身份后，开始考试");
            }
        }

        protected override void StartExam()
        {
            InitExamItemColor();
            InitExamItem();
            InitBrokenRules();
            //todo:为了美观可以考虑使用Dialog 的方式弹出地图
            MapSecledOption();
            ExamContext.StartExam();
            ExamContext.StartExamTime = DateTime.Now;
            tv_Exam_Status.Text = "正在考试";
            tv_RG.Enabled = true;
            tv_exam_mode.Enabled = false;
            tv_Start_Time.Text = DateTime.Now.ToString("HH:mm:ss");
            startExamTime = DateTime.Now;
            btn_Start.Text = "停止考试";
        }
        public void ExamFinishing()
        {
            btn_Start.Text = "开始考试";
            tv_Exam_Status.Text = "未在考试";
            tv_exam_mode.Enabled = true;
            tv_RG.Enabled = false;
            ExamContext.EndExamTime = DateTime.Now;
            startExamTime = null;
            InitExamItemColor();
            EndExam();
        }
        #endregion

        #region 消息

        protected override void RegisterMessages(object Objectmessenger)
        {
            base.RegisterMessages(Objectmessenger);

            IMessenger messenger = (IMessenger)Objectmessenger;
            messenger.Register<LightFinishMessage>(this, OnLightFinishMessage);
        }
        public void OnLightFinishMessage(LightFinishMessage message)
        {
            //灯光模拟考试结束消息启动下一个灯光模拟
            //灯光模拟所有分组加入队列
            //结束和自动进入
            if (LightGroups.Count()>0)
            {
                StartLightAsync(LightGroups.Dequeue());
            }
           
        }
        public void UpdateCarSensorState()
        {
            try
            {
                if (Module != null && Module.ExamManager != null)
                {
                    currentExamItem = Module.ExamManager.ExamItems.LastOrDefault(x => x.State == ExamItemState.Progressing);
                    if (currentExamItem != null)
                    {
                        tv_current_exam.Text = currentExamItem.Name;
                    }
                }

                if (startExamTime.HasValue)
                {
                    var span = DateTime.Now - startExamTime.Value;
                    var times = string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
                    tv_Student_Score.Text = ExamContext.ExamScore.ToString();
                    tv_Li.Text = string.Format("{0}m ", carSignal.Distance.ToString("N0"));
                    tv_Yong.Text = times;
                }
                tv_Speed.Text = string.Format("{0}Km/h", carSignal.SpeedInKmh.ToString("N1"));
                tv_Rev.Text = carSignal.EngineRpm.ToString();
                var gear = Convert.ToInt32(carSignal.Sensor.Gear);
                tv_Gear.Text = gear == 0 ? "空挡" : gear.ToString();
                UpdateCarSensor();
            }
            catch (Exception exp)
            {
                Logger.ErrorFormat("UpdateCarSensorState发生异常,原因:{0} 原始信号{1}", exp.Message, string.Join(",", carSignal.commands));
            }


        }
        protected void UpdateCarSensor()
        {
            var Sensor = carSignal.Sensor;
            if (Sensor == null)
                return;
            if (Sensor.Brake)
                tv_Brake.Text = "踩下";
            else
                tv_Brake.Text = "未踩下";
            if (Sensor.Clutch)
                tv_Clutch.Text = "踩下";
            else
                tv_Clutch.Text = "未踩下";
            if (Sensor.Door)
                tv_Door.Text = "开启";
            else
                tv_Door.Text = "关闭";
            if (Sensor.Handbrake)
                tv_Handbrake.Text = "拉上";
            else
                tv_Handbrake.Text = "未拉上";

            if (Sensor.Loudspeaker)
                tv_Hron.Text = "开启";
            else
                tv_Hron.Text = "关闭";

            if (Sensor.SafetyBelt)
                tv_Safety_Belt.Text = "系上";
            else
                tv_Safety_Belt.Text = "解开";
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
                    dataItem["itemCode"] = item.ExamItemCode.ToString();
                    dataItem["itemRuleName"] = item.RuleName.ToString();
                    dataItem["itemBreakTime"] = item.BreakTime.ToString("yyyy-MM-dd HH:mm:ss");
                    data.Add(dataItem);
                }
                //TODO:new的次数还是挺多的
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.SanLianFuYunBrokenRuleListView,
                    new String[] { "itemName", "itemScore", "itemCode", "itemRuleName", "itemBreakTime" },
                    new int[] { Resource.Id.tv_Item_Name, Resource.Id.tv_Item_Score, Resource.Id.tv_Item_Code, Resource.Id.tv_Item_ShuoMing, Resource.Id.tv_Item_Time });
                BrokenRuleListView.Adapter = simpleAdapter;
                base.ShowBrokenRule();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType());
            };
        }

        private string lastFinishItem = string.Empty;
        protected override void OnBrokenRule(BrokenRuleMessage message)
        {
            try
            {
                brokenRuleMessage = message;
                lstBorkenRuleInfo.Add(brokenRuleMessage.RuleInfo);
                RunOnUiThread(ShowBrokenRule);
                base.OnBrokenRule(message);

                if (message.RuleInfo.ExamItemCode != ExamItemCodes.Light)
                {
                    ////合格是一种颜色，不合格则直接结束
                    var tempRules = new List<BrokenRuleInfo>();
                    int rulecount = 6;
                    if (lstBorkenRuleInfo.Count > rulecount)
                        tempRules = lstBorkenRuleInfo.Take(rulecount).ToList();
                    else
                        tempRules = lstBorkenRuleInfo;

                    var results = tempRules.Where(x => x.ExamItemName.Equals(message.RuleInfo.ExamItemName));
                    int sumScore = 100;
                    foreach (var result in results)
                    {
                        sumScore -= result.DeductedScores;
                    }
                    if (sumScore < 90 && lastFinishItem != message.RuleInfo.ExamItemName)
                    {
                        lastFinishItem = message.RuleInfo.ExamItemName;
                        messager.Send(new MapItemFinishedMessage(message.RuleInfo.ExamItemCode));
                    }
                }
            }
            catch (Exception ex)
            {

                Logger.Error(ex, this.GetType());
            }


        }

        protected override void OnVehicleStartingMessage(VehicleStartingMessage message)
        {
            //触发起步但是起步语音要变
            try
            {
                Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.Start, null, "请起步,继续完成考试");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }

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
            if (message.CarSignal == null || message.CarSignal.Gps == null)
                return;

            //正常考试应该不会进去
            //点击开始考试LightExamItem
            //多测试多观察
            if (LightExamItem != null && LightExamItem.State == ExamItemState.Progressing)
            {
                LightExamItem.Execute(message.CarSignal);
            }


            tempMessage = message;
            carSignal = message.CarSignal;

            //更新界面地图更改名字
            MapSecledOption();

            RunOnUiThread(() => { UpdateCarSensorState(); });
        }
        protected override void OnExamFinishing(ExamFinishingMessage message)
        {
            Logger.Info("收到考试结束消息");
            RunOnUiThread(ExamFinishing);
            base.OnExamFinishing(message);
        }
        private void ExamItemChange()
        {
            try
            {
                var ExamItemName = examItemStateChangedMessage.ExamItem.Name;
                currentExam = ExamItemName;
                //显示当前考试项目
                tv_current_exam.Text = ExamItemName;
                InitExamItem();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType());
            }
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
                //公交车站和学校区域当做一个项目
                string itemcode = message.ExamItem.ItemCode == ExamItemCodes.SchoolArea ? ExamItemCodes.BusArea : message.ExamItem.ItemCode;

                for (var i = 0; i < lstExamItem.Count; i++)
                {
                    //顺序不一致
                    if (AllExamItems[i].ItemCode == itemcode)
                    {
                        if (ExamItemState.Progressing == message.NewState)
                        {
                            lstExamItemBackGroundDrawable[i] = Resource.Drawable.shapeSanLianButtonProcess;
                        }
                        else if (ExamItemState.Finished == message.NewState)
                        {
                            //lstExamItemBackGroundDrawable[i] = Resource.Drawable.shapeSanLianButtonFinish;
                            //合格是一种颜色，不合格则为灰色
                            var tempRules = new List<BrokenRuleInfo>();
                            int rulecount = 6;
                            if (lstBorkenRuleInfo.Count > rulecount)
                                tempRules = lstBorkenRuleInfo.Take(rulecount).ToList();
                            else
                                tempRules = lstBorkenRuleInfo;

                            var results = tempRules.Where(x => x.ExamItemName.Equals(message.ExamItem.Name));
                            int sumScore = 100;
                            foreach (var result in results)
                            {
                                sumScore -= result.DeductedScores;
                            }
                            if (sumScore >= 90)
                                lstExamItemBackGroundDrawable[i] = Resource.Drawable.shapeSanLianButtonFinish;
                            else
                                lstExamItemBackGroundDrawable[i] = Resource.Drawable.shapeSanLianButtonFinish2;
                        }
                    }
                }
                RunOnUiThread(ExamItemChange);
                base.OnExamItemStateChanged(message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
            }
            base.OnExamItemStateChanged(message);
        }


        #region 灯光模拟相关

        #endregion
        protected LightModule LightModule { get; set; }
        protected ILightExamItem LightExamItem { get; set; }

        //按下一次初始化一次
        protected void InitLightModule()
        {
            LightModule = new LightModule();
            LightExamItem = LightModule.LightExamItem;
            LightModule.InitAsync(new ExamInitializationContext(MapSet.Empty)).Wait();

            var lightGroup = dataService.AllLightExamItems;
            LightGroups = new Queue<string>();
            foreach (var item in lightGroup)
            {
                //考试完成后在继续考试
                LightGroups.Enqueue(item.GroupName);
            }
        }
        private async Task ReleaseLightModuleAsync()
        {
            await LightModule.StopAsync();
            LightModule.Dispose();
            LightModule = null;
        }
        /// <summary>
        /// 开启模拟灯光
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private async Task StartLightAsync(string group)
        {
            if (LightExamItem != null && LightExamItem.State == ExamItemState.Progressing)
                await LightModule.StopAsync();
            await LightModule.StartAsync(new ExamContext { ExamGroup = group });
        }
        #endregion

    }
}