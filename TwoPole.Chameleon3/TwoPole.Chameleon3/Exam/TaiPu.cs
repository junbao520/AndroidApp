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
    [Activity(Label = "TiaPu")]
    public class TaiPu : Activity
    {
        #region 考试流程相关变量

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
        List<ColorStateList> lstExamItemColor = new List<ColorStateList>();
        List<string> lstExamItem = new List<string>();
        private bool IsTrainning = false;

        private CarSignalReceivedMessage tempMessage { get; set; }

        ListView TipsListView;
        private Queue<string> TipsQueque = new Queue<string>();
        #endregion
        #region 变量以及控件的定义

        private EditText edtTxtLicenseType;

        private EditText edtTxtStartTime;

        private EditText edtTxtUseTime;

        private EditText edtTxtScore;

        private Spinner MapSpinner;

        private Button btnStartEndExam;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TaiPu);
            //注册消息机制
            messager = Singleton.GetMessager;
            speaker = Singleton.GetSpeaker;
            dataService = Singleton.GetDataService;
            Logger = Singleton.GetLogManager;

            GetIntentParameter();

            InitUI();
            authorization = new Authorization(this);
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);

        }
        protected void AddTips(string Tips)
        {
            if (TipsQueque.Count >= 4)
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
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            //询问用户是否需要退出考试界面
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
                       Module.StopAsync().ContinueWith((task) =>
                       {
                       });
                       Free();
                       Finish();
                   })
                   .SetNeutralButton("否", (s, e) =>
                   {
                       //不保存数据进行返回
                   })
                   .Create();       //创建alertDialog对象  
                alertDialog.Show();
            }
            catch (Exception ex)
            {
                Logger.Error("TiaPu" + ex.Message);
            }

        }
        private void InitUI()
        {
            InitControl();
            InitExamItemColor();
            InitExamItem();
            InitMapLines();
            InitExamContext();
            InitModuleAsync(ExamContext.Map);

         
            //不再注册消息 //怀疑是界面UI 更新资源抢到的

        }
        protected virtual async Task InitModuleAsync(IMapSet map)
        {
            Module = new ExamModule();
            await Module.InitAsync(new ExamInitializationContext(map)).ContinueWith(task =>
            {

            });
        }
        public void GetIntentParameter()
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
        public void InitExamItemColor()
        {
            lstExamItemColor = new List<ColorStateList>();
            lstExamItem = (from v in dataService.GetExamItemsList()
                           select v.ItemName).ToList<string>();
            for (int i = 0; i < lstExamItem.Count; i++)
            {
                lstExamItemColor.Add(ColorStateList.ValueOf(Color.Black));
            }
        }
        /// <summary>
        /// 控件的初始化
        /// </summary>
        public void InitControl()
        {
            edtTxtLicenseType = FindViewById<EditText>(Resource.Id.edtTxtLicenseType);
            edtTxtStartTime = FindViewById<EditText>(Resource.Id.edtTxtStartTime);
            edtTxtUseTime = FindViewById<EditText>(Resource.Id.edtTxtUseTime);
            edtTxtScore = FindViewById<EditText>(Resource.Id.edtTxtScore);
            TipsListView = FindViewById<ListView>(Resource.Id.tips_list);
            btnStartEndExam = FindViewById<Button>(Resource.Id.btnStartExam);
            btnStartEndExam.Click += BtnStartEndExam_Click;
            MapSpinner = (Spinner)FindViewById(Resource.Id.MapSpiner);
            MapSpinner.ItemSelected += MapSpinner_ItemSelected;
        }

        Authorization authorization;
        protected bool CheckAuth(GpsInfo gpsInfo)
        {
            bool checkAuthState = authorization.CheckPeriod(gpsInfo);
            if (!checkAuthState)
            {
                speaker.PlayAudioAsync("GPS无信号或试用已到期，请检查");
                return false;
            }

            return true;
        }

      
        private void BtnStartEndExam_Click(object sender, EventArgs e)
        {
            if (StartEndStatus)
            {
                if (tempMessage != null && !CheckAuth(tempMessage.CarSignal.Gps))
                    return;
                // speaker.PlayAudioAsync("开始考试");
                btnStartEndExam.Text = "结束考试";
                AddTips("考试结束");
                StartEndStatus = false;
                MapSpinner.Enabled = false;
                InitExamItemColor();
                InitExamItem();
                StartExam();
            }
            else
            {
                //speaker.PlayAudioAsync("考试结束");
                btnStartEndExam.Text = "开始考试";
                AddTips("开始考试");
                StartEndStatus = true;
                MapSpinner.Enabled = true;
                ExamContext.EndExamTime = DateTime.Now;
                //重新初始化
                EndExam();
                InitExamContext();
            }
        }

        #region 地图相关
        /// <summary>
        /// 地图选择改版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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
                }
            }
        }
        /// <summary>
        /// 初始化地图
        /// </summary>
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
                Logger.Error("TiaPuInitMap", ex.Message);
            }
        }

        /// <summary>
        /// 绑定地图
        /// </summary>
        /// <param name="lstMaps"></param>
        private void BindMapSpinner(List<string> lstMaps)
        {
            //MapSpiner
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstMaps);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            MapSpinner.Adapter = adapter;
            MapSpinner.Visibility = ViewStates.Visible;
        }
        #endregion

        #region 开始结束考试相关代码
        /// <summary>
        /// 结束考试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MgViewEndExam_Click(object sender, EventArgs e)
        {
            try
            {
                StartEndStatus = true;
                MapSpinner.Enabled = true;

                ExamContext.EndExamTime = DateTime.Now;

                EndExam();
                //重新初始化

            }
            catch (Exception ex)
            {
                Logger.Error("MGViewEndExam_Click" + ex.Message);
            }

        }
        /// <summary>
        /// 开始考试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MgViewStartExam_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!IsRegisterMessager)
                //{
                //    RegisterMessages(messager);
                //    IsRegisterMessager = true;
                //}
                StartEndStatus = false;
                //设置地图选项和白考也考选项
                MapSpinner.Enabled = false;

                //初始化开始项目颜色
                InitExamItemColor();
                InitExamItem();
                StartExam();
            }
            catch (Exception ex)
            {
                Logger.Error("MgViewStartExam_Click" + ex.Message);
            }

        }

        public void StartExam()
        {
            //第一步首先清楚上一次考试项目的扣分规则
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            mainLinerLayout.RemoveAllViews();
            edtTxtStartTime.Text = DateTime.Now.ToString("HH:mm:ss");
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
        #endregion

        #region 消息模式相关代码
        protected void RegisterMessages(object objectmessenger)
        {
            IMessenger messager = (IMessenger)objectmessenger;
            messager.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
            messager.Register<BrokenRuleMessage>(this, OnBrokenRule);
            messager.Register<ExamItemStateChangedMessage>(this, OnExamItemStateChanged);
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
                //
                RunOnUiThread(ExamItemChange);
            }
            catch (Exception ex)
            {
                Logger.Error("TiaPuOnExamItemStateChanged", ex.Message);
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
                Logger.Error("TiaPuBrokenRule", ex.Message);
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
                Logger.Error("TiaPuCarSignalReceived", ex.Message);
            }

        }

        public void UpdateCarSensorState()
        {
            try
            {
                //更新一些基本的信
                if (ExamContext.IsExaming && !StartEndStatus)
                {
                    edtTxtScore.Text = string.Format("{0}", ExamContext.ExamScore);
                    edtTxtUseTime.Text = string.Format("{0:00}:{1:00}:{2:00}", carSignal.UsedTime.Hours, carSignal.UsedTime.Minutes, carSignal.UsedTime.Seconds);

                }
                ShowTips();
            }
            catch (Exception ex)
            {
                Logger.Error("TiaPuCarSensorState" + ex.Message);
            }


        }
        //难道需要对资源进行加锁Lock//还有可能就是模拟json数据的频率不够太低
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

                Logger.Error("TiaPuShowBrokenRule" + ex.Message);
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
                Logger.Error("TiaPuExamItemChange", ex.Message);
            }
        }
        public void ExamItemClick(object sender, EventArgs e)
        {
            TextView tvExamItem = (TextView)sender;
            string ItemCode = dataService.GetExamItemCode(tvExamItem.Text);
            if (!string.IsNullOrEmpty(ItemCode))
            {
                if (ExamContext.IsExaming)
                {
                    Module.StartExamItemManualAsync(ExamContext, ItemCode, null);
                }
            }


        }
        protected void Free()
        {
            if (speaker != null)
            {
                speaker.CancelAllAsync();
            }
            if (messager != null)
            {
                messager.Unregister(this);
                messager = null;
            }
            if (Module != null)
            {
                ReleaseModuleAsync();
            }

        }
        private async Task ReleaseModuleAsync()
        {
            await Module.StopAsync(true);
            Module.Dispose();
            Module = null;
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
                for (int i = 0; i < ExamItemCount / 5; i++)
                {
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemTaiPu, null);
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

                    txt = (TextView)relativeLayout.FindViewById(Resource.Id.list_1_5);
                    txt.SetText(lstExamItem[i * 5 + 4], TextView.BufferType.Normal);
                    txt.SetTextColor(lstExamItemColor[i * 5 + 4]);
                    txt.Click += ExamItemClick;

                    mainLinerLayout.AddView(relativeLayout);
                }
                //4如果余数大
                int Count = ExamItemCount - (ExamItemCount / 5) * 5;
                if (Count > 0)
                {
                    int StartIndex = (ExamItemCount / 5) * 5;
                    relativeLayout = (RelativeLayout)LayoutInflater.From(this).Inflate(Resource.Layout.ExamItemTaiPu, null);
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
                Logger.Error("TiaPuInitExamItem" + ex.Message);
            }

        }
        #endregion

    }
}