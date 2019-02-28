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
using Android.Util;

namespace TwoPole.Chameleon3
{
    //TODO:可以考虑通过注解的行驶初始化控件
    //TODO:有时间可以考虑把整个基类重写一次 写规范一些 写的更好更全
    [Activity(Label = "BaseExamActivity")]
    public class BaseExamActivity : Activity
    {
        protected LinearLayout mainLinerLayout;
        protected LinearLayout linearLayout;
        protected List<string> lstExamItem = new List<string>();
        protected List<string> lstAllExamItem = new List<string>();
        protected List<string> lstArtificialEvaluationExamItem = new List<string>();
        protected List<ColorStateList> lstExamItemColor = new List<ColorStateList>();
        protected List<Color> lstExamItemBackGroundColor = new List<Color>();

        //人工评判项目Id
        protected List<int> lstRGExamItemButtonId = new List<int>();
        //考试项目的ButtonId
        protected List<int> lstExamItemButtonId = new List<int>();

        protected List<KeyValuePair<int, MapLine>> lstKeyValueMaps = new List<KeyValuePair<int, MapLine>>();



        protected int ExamItemLineCount = 2;

        /// <summary>
        /// 扣分规则ListView
        /// </summary>
        protected ListView BrokenRuleListView;
        /// <summary>
        /// 数据库服务类
        /// </summary>
        protected IDataService dataService;
        /// <summary>
        /// 语音
        /// </summary>
        protected ISpeaker speaker;
        /// <summary>
        /// 消息
        /// </summary>
        protected IMessenger messager;
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog Logger;
        /// <summary>
        /// 当前车载信号
        /// </summary>
        protected CarSignalInfo carSignal;

        public ExamContext ExamContext { get; private set; }
        protected ExamModule Module { get; set; }
        protected CarSignalReceivedMessage tempMessage { get; set; }
        protected ExamItemStateChangedMessage examItemStateChangedMessage { get; set; }
        protected BrokenRuleMessage brokenRuleMessage { get; set; }

        protected List<BrokenRuleInfo> lstBorkenRuleInfo = new List<BrokenRuleInfo>();
        protected GlobalSettings globalSetting { get; set; }
        Authorization authorization;

        protected bool IsTrainning = false;

        /// <summary>
        /// 考试界面名称
        /// </summary>
        protected string ExamUIName = "";
        protected string MapName = "无地图";
        //人工评判选择的考试项目
        protected string RGSelectExamItem = "";
        protected string DefaultStringFormat = "yyyy-MM-dd HH:mm:ss";
        protected Color ExamProgressingColor = Color.DarkRed;
        protected Color ExamFinishedColor = Color.Green;
        protected Color ExamInitColor = Color.Blue;
        protected string currentExam = string.Empty;
        protected IExamItem currentExamItem = null;
        protected List<ExamItem> AllExamItems = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            dataService = Singleton.GetDataService;
            Logger = Singleton.GetLogManager;
            globalSetting = dataService.GetSettings();
            authorization = new Authorization(this);
            GetIntentParameter();
            InitExamStatusColor();
            InitParameter();
            //新添加基类就进行初始化
            //InitUI();
            //TODO:可以考虑使用Task.Run 来写
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
        }

        protected virtual void InitParameter()
        {
            AllExamItems = dataService.GetExamItemsList();
            lstExamItem = (from v in AllExamItems
                           where v.IsEnable == true
                           select v.ItemName).ToList<string>();
            lstArtificialEvaluationExamItem = (from v in dataService.GetExamItemsList(true)
                                               where v.IsEnable == true
                                               select v.ItemName).ToList<string>();
            lstRGExamItemButtonId = new List<int> { Resource.Id.btnExamItem1, Resource.Id.btnExamItem2, Resource.Id.btnExamItem3, Resource.Id.btnExamItem4, Resource.Id.btnExamItem5, Resource.Id.btnExamItem6 };
        }
        /// <summary>
        /// 初始化TextView 控件
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        public TextView InitTextView(int ResourceId)
        {
            return FindViewById<TextView>(ResourceId);
        }
        /// <summary>
        /// 初始化Button控件
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        public Button InitButton(int ResourceId)
        {
            return FindViewById<Button>(ResourceId);
        }
        protected virtual void InitExamStatusColor()
        {
            ExamProgressingColor = Color.DarkRed;
            ExamFinishedColor = Color.Green;
            ExamInitColor = Color.Blue;
        }
        protected virtual void InitExamItemColor()
        {
            lstExamItemColor = new List<ColorStateList>();
            //可以去掉有一些项目 比如 上车准备 减速让行 
            //TODO:这里面不包含综合评判
            lstExamItem = (from v in dataService.GetExamItemsList()
                           where v.IsEnable == true
                           select v.ItemName).ToList<string>();
            for (int i = 0; i < lstExamItem.Count; i++)
            {
                lstExamItemColor.Add(ColorStateList.ValueOf(ExamInitColor));
                //lstExamItemBackGroundColor.Add()
            }
            lstAllExamItem = (from v in dataService.GetExamItemsList()
                              where v.IsEnable == true
                              select v.ItemName).ToList<string>();
        }
        protected virtual void InitUI()
        {

        }
        protected virtual void InitBrokenRules()
        {

        }
        protected virtual void StartExam()
        {

            //进行授权判断
            Module.StartAsync(ExamContext).ContinueWith((task) =>
            {
            });
        }

        protected virtual void EndExam()
        {
            try
            {


                ExamContext.EndExamTime = DateTime.Now;
                if (Module != null)
                {
                    Module.StopAsync().ContinueWith((task) => { });
                }
            }
            catch (Exception e)
            {

            }
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                ShowConfirmDialog();
            }
            return base.OnKeyDown(keyCode, key);
        }
        /// <summary>
        /// TODO:这个弹出来时黑框 字体又大 很难看 可以考虑优化一下
        /// </summary>
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
                       speaker.StopAllSpeak();
                       EndExam();
                       Free();
                       Finish();
                   })
                   .SetNeutralButton("否", (s, e) =>
                   {
                       //不保存数据进行返回
                       //
                   })
                   .Create();//创建alertDialog对象  
                alertDialog.Show();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }

        }

        public void InitAuthInfo(TextView tvAuthMessage)
        {
            try
            {
                DateTime? dtAuthTime = authorization.AuthorizationTime;
                if (dtAuthTime != null)
                {
                    if ((dtAuthTime.Value - DateTime.Now).TotalDays <= 3)
                    {
                        tvAuthMessage.SetTextColor(Color.Red);
                        tvAuthMessage.Text = string.Format("{0}", authorization.GetAuthorizationInfo());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, GetType().ToString());
                tvAuthMessage.Text = string.Format("授权异常", ex.Message);
            }

        }
        /// <summary>
        /// 是否授权到期
        /// </summary>
        public bool IsAuthorizationExpires
        {
            get
            {
                if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                    return true;
                return false;
            }
        }
        protected bool CheckAuth(GpsInfo gpsInfo)
        {
            bool checkAuthState = authorization.CheckPeriod(gpsInfo);
            if (!checkAuthState)
            {
                string AuthCode = authorization.AuthorizationCode;

                string message = string.Format("GPS无信号或试用已到期，请检查");
                //string message = string.Format("设备识别码:{0},请联系销售人员激活软件。", AuthCode);
                speaker.PlayAudioAsync(message);
                return false;
            }

            return true;
        }

        public virtual void GetIntentParameter()
        {
            IsTrainning = Intent.GetStringExtra("ExamMode") == "Train";
        }
        public void InitExamContext()
        {
            ExamContext = new ExamContext();
            ExamContext.ExamMode = IsTrainning ? ExamMode.Training : ExamMode.Examming;
            ExamContext.ExamTimeMode = ExamTimeMode.Day;
            ExamContext.Map = MapSet.Empty;
        }
        public void ExamModeSelect(int SelectItemPosition)
        {
            ExamContext.ExamTimeMode = (ExamTimeMode)(SelectItemPosition + 1);
        }

        public void MapSelect(string name)
        {
                //通过对应的KeyValue 里面进行取值
                var SelectMapItem = lstKeyValueMaps.Where(x => x.Value.Name==MapName).FirstOrDefault().Value;
                if (SelectMapItem.Points != null)
                {
                    var mapSet = new MapSet(SelectMapItem.Points.ToMapPoints());
                    //把地图选择信息保存起来后续需要可以直接取
                    MapName = SelectMapItem.Name;
                    ExamContext.Map = mapSet;
                    dataService.SaveDefaultMapId(SelectMapItem.Id);
                    Logger.InfoFormat("##保存地图取出序号：{0}，{1}", SelectMapItem.Id, MapName);
                }
       }
        public void MapSelect(int SelectItemPosition)
        {
            if (SelectItemPosition >= 0 && ExamContext != null)
            {

                //通过对应的KeyValue 里面进行取值

                var SelectMapItem = lstMapLine[SelectItemPosition];
                //var SelectMapItem = dataService.GetAllMapLines().Where(x=>x.Id== SelectItemPosition).FirstOrDefault();
                if (SelectItemPosition == 0)
                {
                    dataService.SaveDefaultMapId(0);
                    return;
                }
                if (SelectMapItem.Points != null)
                {
                    var mapSet = new MapSet(SelectMapItem.Points.ToMapPoints());
                    //把地图选择信息保存起来后续需要可以直接取
                    MapName = SelectMapItem.Name;
                    ExamContext.Map = mapSet;
                    dataService.SaveDefaultMapId(SelectMapItem.Id);
                    Logger.InfoFormat("##保存地图取出序号：{0}，{1}", SelectMapItem.Id, MapName);
                }
            }
        }
        protected virtual void RegisterMessages(object Objectmessenger)
        {
            IMessenger messenger = (IMessenger)Objectmessenger;
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
            messenger.Register<BrokenRuleMessage>(this, OnBrokenRule);
            messenger.Register<ExamItemStateChangedMessage>(this, OnExamItemStateChanged);
            messager.Register<ExamFinishingMessage>(this, OnExamFinishing);
            messager.Register<PullOverTriggerMessage>(this, OnPullOverTrigger);
            messager.Register<FingerprintMessage>(this, OnFingerprintMessage);
            messager.Register<VehicleStartingMessage>(this, OnVehicleStartingMessage);
            
        }
        /// <summary>
        /// 靠边停车之后触发起步
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnVehicleStartingMessage(VehicleStartingMessage message)
        {

        }
        protected virtual void OnFingerprintMessage(FingerprintMessage message)
        {

        }
        protected virtual void OnPullOverTrigger(PullOverTriggerMessage message)
        {
            if (ExamContext.IsExaming)
            {
                Module.StartExamItemManualAsync(ExamContext, ExamItemCodes.PullOver, null);
            }
        }
        protected virtual void OnExamFinishing(ExamFinishingMessage message)
        {

        }
        protected virtual void OnCarSignalReceived(CarSignalReceivedMessage message)
        {

        }
        protected virtual void OnBrokenRule(BrokenRuleMessage message)
        {


        }
        protected virtual void ShowBrokenRule()
        {

        }
        public string GetDateTimeToString(DateTime dt, string format = "")
        {
            if (string.IsNullOrEmpty(format))
            {
                format = DefaultStringFormat;
            }
            if (dt == null)
            {
                return string.Empty;
            }
            return dt.ToString(format);
        }
        public string GetDateTimeToString(DateTime? dt, string format = "")
        {
            //感觉后面一个是多余的
            if (dt.HasValue && dt != null)
            {
                return GetDateTimeToString(dt.Value, format);
            }
            else
            {
                return string.Empty;
            }

        }
        protected virtual void OnExamItemStateChanged(ExamItemStateChangedMessage message)
        {
            currentExam = message.ExamItem.Name;
        }
        protected void StartExamItem(string ExamItem, bool IsExamItemCode = false)
        {
            try
            {
                string ItemCode = IsExamItemCode ? ExamItem : dataService.GetExamItemCode(ExamItem);
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
                Logger.Error(ex, GetType().ToString());
            }
        }
        public int GetExamItemId(int Index)
        {
            int Id = 0;
            switch (Index)
            {
                case 0:
                    Id = Resource.Id.list_1_1;
                    break;
                case 1:
                    Id = Resource.Id.list_1_2;
                    break;
                case 2:
                    Id = Resource.Id.list_1_3;
                    break;
                case 3:
                    Id = Resource.Id.list_1_4;
                    break;
                case 4:
                    Id = Resource.Id.list_1_5;
                    break;
                case 5:
                    Id = Resource.Id.list_1_6;
                    break;
            }
            return Id;
        }
        private void BindSpinner(List<string> lstDataSource, Spinner spinner)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstDataSource);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.Visibility = ViewStates.Visible;
        }
        /// <summary>
        /// 考试地图绑定
        /// </summary>
        /// <param name="MapSpinner"></param>
        /// 
        public List<MapLine> lstMapLine;
        protected void BindExamMapLinesSpinner(Spinner MapSpinner)
        {
            try
            {
                var mapLines = dataService.ALLMapLines;
                MapLine mapLine = new MapLine() { Id = 0, Name = "无地图" };
                lstMapLine = new List<MapLine>();
                lstMapLine.Add(mapLine);
                lstMapLine.AddRange(mapLines);
                List<string> lstMaps = new List<string>();

               string strMapInfo = string.Empty;
                for (int i = 0; i < lstMapLine.Count(); i++)
                {
                    //lstKeyValueMaps.Add(new KeyValuePair<int, MapLine>(i, lstMapLine[i]));
                    lstMaps.Add(lstMapLine[i].Name);
                }
                //加载出来
                //最简单的嘛通过索引的地图名称再去反响查找
                BindSpinner(lstMaps, MapSpinner);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }
        }

        /// <summary>
        /// 考试模式绑定
        /// </summary>
        /// <param name="ExamModeSpinner"></param>
        protected void BindExamModeSpinner(Spinner ExamModeSpinner)
        {
            List<string> ExamModes = new List<string> { "白天", "夜间" };
            BindSpinner(ExamModes, ExamModeSpinner);
        }
        protected virtual async Task InitModuleAsync(IMapSet map)
        {
            Module = new ExamModule();
            await Module.InitAsync(new ExamInitializationContext(map)).ContinueWith(task =>
            {

            });
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

        protected virtual async Task ReleaseModuleAsync()
        {
            await Module.StopAsync(true);
            Module.Dispose();
            Module = null;
        }

        public List<TwoPole.Chameleon3.Domain.DeductionRule> GetDeductionRule()
        {
            //string currentExamItemCode = dataService.GetExamItemCode(currentExam);
            //List<string> lstExamCode = new List<string> { currentExamItemCode};
            //return dataService.GetDeductionRuleByExamItem(lstExamCode);
            return GetDeductionRule(currentExam);

        }
        public List<TwoPole.Chameleon3.Domain.DeductionRule> GetDeductionRule(string ExamItemName)
        {
            //TODO:临时解决 底层有Bug   
            string currentExamItemCode = string.Empty;

            if (ExamItemName.Contains("车站"))
            {
                currentExamItemCode = ExamItemCodes.BusArea;
            }
            if (ExamItemName.Contains("综合") || string.IsNullOrEmpty(ExamItemName))
            {
                currentExamItemCode = ExamItemCodes.CommonExamItem;
            }
            if (string.IsNullOrEmpty(currentExamItemCode))
            {
                currentExamItemCode = dataService.GetExamItemCode(ExamItemName);
            }
            List<string> lstExamCode = new List<string> { currentExamItemCode };
            return dataService.GetDeductionRuleByExamItem(lstExamCode);
        }
        AlertDialog alertDialog = null;
        AlertDialog.Builder builder = null;



        public Button GetTableRowButton(TableRow tableRow, int ResourceId, List<string> lst, int Index)
        {
            Button btn = (Button)tableRow.FindViewById<Button>(ResourceId);
            //数组索引越界
            if (Index >= lst.Count)
            {
                btn.Text = string.Empty;
                btn.Visibility = ViewStates.Invisible;
                return btn;
            }
            btn.Text = lst[Index];
            btn.Click += BtnRGExamItem_Click;
            return btn;
        }

        private void BtnRGExamItem_Click(object sender, EventArgs e)
        {
            //人工评判
            var text = ((Button)sender).Text;
            var result = GetDeductionRule(text);
            RGSelectExamItem = text;
            // View view = View.Inflate(this, Resource.Layout.Dialog_ArtificialEvaluation, null);
            TableLayout tableLayout = (TableLayout)alertDialog.FindViewById(Resource.Id.DeductionRuleTable);
            TableLayout tableLayoutExamItem = (TableLayout)alertDialog.FindViewById(Resource.Id.ExamItemBtnTable);
            //清空所有的 扣分规则
            tableLayout.RemoveAllViews();
            foreach (var item in result)
            {
                TableRow tableRow = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.tableArtificialEvaluation, null);
                TextView tvDeductionReason = (TextView)tableRow.FindViewById(Resource.Id.tvDeductionReason);
                TextView tvDeductionScore = (TextView)tableRow.FindViewById(Resource.Id.tvDeductionScore);
                Button btnDeduction = (Button)tableRow.FindViewById(Resource.Id.btnDeduction);
                tvDeductionReason.Text = item.RuleName;
                tvDeductionScore.Text = item.DeductedScores.ToString();
                btnDeduction.Text = "扣分";
                btnDeduction.Tag = item.RuleCode + "," + item.SubRuleCode;
                btnDeduction.Click += BtnDeduction_Click;
                tableLayout.AddView(tableRow);
            }

        }


        public void ArtificialEvaluation()
        {
            try
            {
                View view = View.Inflate(this, Resource.Layout.Dialog_ArtificialEvaluation, null);
                TableLayout tableLayout = (TableLayout)view.FindViewById(Resource.Id.DeductionRuleTable);
                //考试项目ExamItem
                TableLayout tableLayoutExamItem = (TableLayout)view.FindViewById(Resource.Id.ExamItemBtnTable);

                var result = GetDeductionRule();
                //Logger.Error("lstArtificialEvaluationExamItem:" + string.Join(",", lstArtificialEvaluationExamItem));
                //还差一个综合评判
                int LineMaxCount = 6;
                //向上取整
                int RowCount = (int)Math.Ceiling(lstArtificialEvaluationExamItem.Count * 1.0 / LineMaxCount);

                for (int i = 0; i < RowCount; i++)
                {
                    TableRow tableRow = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.tableArtificialEvaluationExamItem, null);
                    for (int j = 0; j < LineMaxCount; j++)
                    {
                        GetTableRowButton(tableRow, lstRGExamItemButtonId[j], lstArtificialEvaluationExamItem, i * LineMaxCount + j);
                    }
                    tableLayoutExamItem.AddView(tableRow);
                }

                foreach (var item in result)
                {
                    TableRow tableRow = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.tableArtificialEvaluation, null);
                    TextView tvDeductionReason = (TextView)tableRow.FindViewById(Resource.Id.tvDeductionReason);
                    TextView tvDeductionScore = (TextView)tableRow.FindViewById(Resource.Id.tvDeductionScore);

                    Button btnDeduction = (Button)tableRow.FindViewById(Resource.Id.btnDeduction);
                    tvDeductionReason.Text = item.DeductedReason;
                    tvDeductionScore.Text = item.DeductedScores.ToString();
                    btnDeduction.Text = "扣分";
                    btnDeduction.Tag = item.RuleCode + "," + item.SubRuleCode;
                    btnDeduction.Click += BtnDeduction_Click;
                    tableLayout.AddView(tableRow);
                }

                if (builder == null)
                {
                    //减少重复New如果有问题可以打开还原
                    builder = new AlertDialog.Builder(this);
                }
                alertDialog = builder
               .SetTitle("人工评判")
                .SetNeutralButton("返回", (s, e) =>
                {
                    //不保存数据进行返回
                })
               .SetView(view)
               .Create();       //创建alertDialog对象  

                alertDialog.Show();

                //根据屏幕分辨路不同
                DisplayMetrics dm = new DisplayMetrics();
                base.WindowManager.DefaultDisplay.GetMetrics(dm);
                //宽高都是3/2 
                alertDialog.Window.SetLayout(dm.WidthPixels - 100, dm.HeightPixels);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, this.GetType().ToString());
            }


        }



        private TableRow GetDeductionRuleTableRow(TwoPole.Chameleon3.Domain.DeductionRule item)
        {

            TableRow tableRow = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.tableArtificialEvaluation, null);
            TextView tvDeductionReason = (TextView)tableRow.FindViewById(Resource.Id.tvDeductionReason);
            TextView tvDeductionScore = (TextView)tableRow.FindViewById(Resource.Id.tvDeductionScore);
            tvDeductionReason.Text = item.DeductedReason;
            tvDeductionScore.Text = item.DeductedScores.ToString();

            return tableRow;
        }

        private void BtnDeduction_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var Tag = btn.Tag.ToString();
            var ruleCode = Tag.Split(',')[0];
            var subRuleCode = Tag.Split(',')[1];

            if (string.IsNullOrEmpty(subRuleCode))
            {
                subRuleCode = null;
            }
            var examScore = Singleton.GetExamScore;

            var examItemCode = dataService.GetExamItemCode(currentExam);

            if (!string.IsNullOrEmpty(RGSelectExamItem))
            {
                examItemCode = dataService.GetExamItemCode(RGSelectExamItem);
                examScore.BreakRule(examItemCode, RGSelectExamItem, ruleCode, subRuleCode);
            }
            else
            {
                examScore.BreakRule(examItemCode, currentExam, ruleCode, subRuleCode);
            }
            //需要根据选择的ExamItemCode 进行扣分

            //取消对话框
            alertDialog.Cancel();

        }
    }
}