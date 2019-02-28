using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Twopole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3
{
    /// <summary>
    /// 科菲特考试界面 基类重构争取把考试界面代码缩减到200行左右
    [Activity(Label = "陕西科菲特")]
    public class KeiFeiTe : BaseExamActivity
    {
        #region 考试项目相关
        protected TextView tvExamItem { get; set; }
        protected ImageView imgExamStatus { get; set; }

        protected TextView tvExamInfo { get; set; }
        protected TextView tvExamCode { get; set; }
        protected TextView tvRuleContent { get; set; }
        protected TextView tvRuleScore { get; set; }
        protected int imgExamSuccessResId { get; set; }
        protected int imgExamFailResId { get; set; }
        protected int imgExamProcessResId { get; set; }
        #endregion
        #region 初始化界面控件
        protected Button btnStartExam { get; set; }
        protected Button btnPullOver { get; set; }
        protected Button btnLightSimulation { get; set; }
        protected TextView tvGps { get; set; }
        protected TextView tvSpeed { get; set; }
        protected TextView tvMileage { get; set; }
        protected TextView tvMapInfo { get; set; }
        protected ImageView ImageUser { get; set; }
        protected Spinner MapSpinner { get; set; }
        #endregion
        protected ExamProcess examProcess { get; set; }
        protected ExamItemProcess examItemProcess { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.KeFeiTe);
            InitUI();

            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

        }
        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            LogManager.WriteSystemLog("AndroidEnvironment_UnhandledExceptionRaiser" + e.Exception.Message);
            Logger.Error(e.Exception, this.GetType().ToString());
        }
        //TODO:命名一定要规范 2018 09 09 鲍君
        protected void InitControl()
        {
            btnStartExam = (Button)FindViewById(Resource.Id.btnStartExam);
            btnPullOver = (Button)FindViewById(Resource.Id.btnPullOver);
            btnLightSimulation = (Button)FindViewById(Resource.Id.btnLightingSimulation);
            tvGps = FindViewById<TextView>(Resource.Id.tvGps);
            tvMileage = FindViewById<TextView>(Resource.Id.tvMileage);
            tvMapInfo = FindViewById<TextView>(Resource.Id.tvMapInfo);
            tvSpeed = FindViewById<TextView>(Resource.Id.tvSpeed);
            MapSpinner = (Spinner)FindViewById(Resource.Id.MapSpiner);
            ImageUser = FindViewById<ImageView>(Resource.Id.ImageUser);
            btnStartExam.Click += BtnStartExam_Click;
            btnPullOver.Click += BtnPullOver_Click;
            btnLightSimulation.Click += BtnLightSimulation_Click;
            ImageUser.Click += ImageUser_Click;
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
        }
        private void ImageUser_Click(object sender, EventArgs e)
        {
            base.ArtificialEvaluation();
        }
        private void MapSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            MapSelect(e.Position);
        }
        private void BtnLightSimulation_Click(object sender, EventArgs e)
        {
            base.StartExamItem(ExamItemCodes.Light, true);
        }
        private void BtnPullOver_Click(object sender, EventArgs e)
        {
            base.StartExamItem(ExamItemCodes.PullOver, true);
        }
        private void BtnStartExam_Click(object sender, EventArgs e)
        {
            try
            {
                if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                    return;
                if (btnStartExam.Text == "开始考试")
                {
                    MapSpinner.Visibility = ViewStates.Invisible;
                    btnStartExam.Text = "结束考试";
                    examProcess = new ExamProcess();
                    MapSpinner.Enabled = false;
                    tvMapInfo.Text = "线路:" + MapName;
                    InitExamItem();
                    StartExam();
                }
                else
                {
                    ExamFinishing();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }
        }
        public void ExamFinishing()
        {
            btnStartExam.Text = "开始考试";
            tvMapInfo.Text = "线路:";
            MapSpinner.Visibility = ViewStates.Visible;
            MapSpinner.Enabled = true;
            //初始化人工评判不可以使用
            ExamContext.EndExamTime = DateTime.Now;
            EndExam();
        }
        protected override void StartExam()
        {
            InitExamItem();
            ExamContext.StartExam();
            Module.StartAsync(ExamContext).ContinueWith((task) =>
            {
            });
        }
        protected override void EndExam()
        {
            Module.StopAsync().ContinueWith((task) =>
            {
            });
        }
        protected override void InitUI()
        {
            try
            {
                InitControl();
                InitExamItem();
                InitAuthInfo(tvGps);
                BindExamMapLinesSpinner(MapSpinner);
                InitExamContext();
                InitModuleAsync(ExamContext.Map);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }

        }

        /// <summary>
        /// 明白了这个重写了没有调用基类的 所以为空
        /// </summary>
        protected override void InitParameter()
        {
            base.InitParameter();
            lstExamItem = (from v in dataService.GetExamItemsList(true)
                           where v.IsEnable == true
                           select v.ItemName).ToList<string>();
            imgExamFailResId = Resource.Drawable.exam_error;
            imgExamSuccessResId = Resource.Drawable.exam_success;
            imgExamProcessResId = Resource.Drawable.exam_process;
        }
        //初始化考试项目
        protected void InitExamItem()
        {
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.ExamItemTable);
            mainLinerLayout.RemoveAllViews();
            foreach (var item in lstExamItem)
            {
                linearLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableKeFeiTe, null);
                tvExamItem = (TextView)linearLayout.FindViewById(Resource.Id.tvExamItem);
                tvExamItem.Text = item;
                tvExamItem.Click += TvExamItem_Click;
                mainLinerLayout.AddView(linearLayout);
                //添加分割线
                linearLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableKeFeiTesplitBar, null);
                mainLinerLayout.AddView(linearLayout);
            }
        }
        //todo：是否可以移动到基类里面去?
        public override void GetIntentParameter()
        {
            IsTrainning = Intent.GetStringExtra("ExamMode") == "Train";
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            return base.OnKeyDown(keyCode, key);
        }
        #region 考试消息接收

        protected override void OnBrokenRule(BrokenRuleMessage message)
        {
            brokenRuleMessage = message;
            RunOnUiThread(ShowBrokenRule);
        }
        protected override void ShowBrokenRule()
        {
            try
            {
                BrokenRuleInfo RuleInfo = brokenRuleMessage.RuleInfo;
                var index = lstExamItem.ToList().IndexOf(RuleInfo.ExamItemName);
                var CurrentExamItemProcess = examProcess.GetExamProcess(RuleInfo.ExamItemCode);
                if (CurrentExamItemProcess == null)
                {
                    CurrentExamItemProcess = new ExamItemProcess();
                }
                CurrentExamItemProcess.IsSuccess = false;
                examProcess.AddExamItem(CurrentExamItemProcess);
                index = index > 0 ? index * 2 : index;

                mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.ExamItemTable);
                //通过索引获取子View
                var examItemView = (LinearLayout)mainLinerLayout.GetChildAt(index);
                imgExamStatus = examItemView.FindViewById<ImageView>(Resource.Id.imgExamStatus);
                tvExamInfo = examItemView.FindViewById<TextView>(Resource.Id.tvExamInfo);
                imgExamStatus.SetImageResource(imgExamFailResId);
                tvExamInfo.Text = GetExamInfo(RuleInfo.DeductedScores);
                //添加扣分规则
                linearLayout = (LinearLayout)LayoutInflater.From(this).Inflate(Resource.Layout.tableKeFeiTeBreakeRule, null);
                tvExamCode = linearLayout.FindViewById<TextView>(Resource.Id.tvExamCode);
                tvRuleScore = linearLayout.FindViewById<TextView>(Resource.Id.tvRuleScore);
                tvRuleContent = linearLayout.FindViewById<TextView>(Resource.Id.tvRuleContent);
                tvExamCode.Text = RuleInfo.ExamItemCode;
                tvRuleScore.Text = RuleInfo.DeductedScores.ToString();
                tvRuleContent.Text = RuleInfo.RuleName;
                examItemView.AddView(linearLayout);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }

        }
        protected override void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            try
            {
                if (message.CarSignal == null || message.CarSignal.Gps == null)
                    return;
                RunOnUiThread(() =>
                {
                    if (message.CarSignal.Gps.FixedSatelliteCount >= 3)
                    {
                        tvGps.Text = "GPS:正常";
                    }
                    else
                    {
                        tvGps.Text = "GPS:异常";
                    }
                    tvMileage.Text = string.Format("里程:{0:0}米", message.CarSignal.Distance);
                    tvSpeed.Text = tvSpeed.Text = string.Format("速度:{0:0}Km/h", message.CarSignal.SpeedInKmh);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }
        }
        protected override void OnExamFinishing(ExamFinishingMessage message)
        {
            RunOnUiThread(ExamFinishing);
            base.OnExamFinishing(message);
        }
        protected override void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            try
            {
                //特殊 这个版本综合评判状态也要加入
                examItemStateChangedMessage = message;
                var AllExamItems = dataService.GetExamItemsList(true);
                Logger.Error(message.ExamItem.ItemCode);
                examItemProcess = examProcess.GetExamProcess(message.ExamItem.ItemCode);
                if (examItemProcess == null)
                {
                    examItemProcess = new ExamItemProcess();
                }
                examItemProcess.ExamItemCodes = message.ExamItem.ItemCode;
                examItemProcess.ExamItemName = message.ExamItem.Name;

                for (var i = 0; i < AllExamItems.Count(); i++)
                {
                    if (AllExamItems[i].ItemCode == message.ExamItem.ItemCode)
                    {
                        //项目开始
                        if (ExamItemState.Progressing == message.NewState)
                        {
                            examItemProcess.BeginTime = DateTime.Now;
                            examItemProcess.Status = ExamItemSatatus.Process;
                        }
                        //项目结束
                        else if (ExamItemState.Finished == message.NewState)
                        {
                            examItemProcess.EndTime = DateTime.Now;
                            examItemProcess.Status = ExamItemSatatus.Finish;
                        }
                        examProcess.AddExamItem(examItemProcess);
                    }
                }
                base.OnExamItemStateChanged(message);
                //考试状态改变
                RunOnUiThread(ExamItemChange);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }
            // base.OnExamItemStateChanged(message);
        }
        private void ExamItemChange()
        {
            var index = lstExamItem.ToList().IndexOf(examItemProcess.ExamItemName);
            index = index > 0 ? index * 2 : index;
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.ExamItemTable);
            var examItemView = (LinearLayout)mainLinerLayout.GetChildAt(index);
            imgExamStatus = examItemView.FindViewById<ImageView>(Resource.Id.imgExamStatus);
            tvExamInfo = examItemView.FindViewById<TextView>(Resource.Id.tvExamInfo);
            //首先更新项目
            if (examItemProcess.Status == ExamItemSatatus.Process)
            {
                imgExamStatus.SetImageResource(imgExamProcessResId);
                tvExamInfo.Text = string.Format("{0}", GetDateTimeToString(DateTime.Now));
            }
            else if (examItemProcess.Status == ExamItemSatatus.Finish)
            {
                if (examItemProcess.IsSuccess)
                {
                    imgExamStatus.SetImageResource(imgExamSuccessResId);
                    tvExamInfo.Text = string.Format("{0}-{1}", GetDateTimeToString(examItemProcess.BeginTime), GetDateTimeToString(examItemProcess.EndTime));
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取考试过程显示信息
        /// </summary>
        /// <param name="status"></param>
        /// <param name="IsError"></param>
        /// <returns></returns>
        public string GetExamInfo(int DeductionScore = 0)
        {
            string result = string.Empty;
            string beginTime = examItemProcess.BeginTime.HasValue ? examItemProcess.BeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            var endTime = examItemProcess.EndTime.HasValue ? examItemProcess.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            if (examItemProcess.Status == ExamItemSatatus.Process)
            {
                result = beginTime;
            }
            if (examItemProcess.Status == ExamItemSatatus.Finish)
            {

                if (string.IsNullOrEmpty(beginTime))
                {
                    result = endTime;
                }
                else
                {
                    result = beginTime + "->" + endTime;
                }
            }
            if (DeductionScore > 0)
            {
                result = string.Format("扣分:{0} {1}->{2}", DeductionScore, beginTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return result;
        }

        private void TvExamItem_Click(object sender, EventArgs e)
        {
            TextView tv = (TextView)sender;
            if (!tv.Text.Contains("综合"))
            {
                base.StartExamItem(tv.Text);
            }
        }
    }
}