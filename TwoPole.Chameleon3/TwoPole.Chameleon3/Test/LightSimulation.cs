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
using TwoPole.Chameleon3.Infrastructure;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Business.Modules;
using TwoPole.Chameleon3.Business.ExamItems;
using TwoPole.Chameleon3.Infrastructure.Messages;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure.Map;
using Android.Graphics;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "LightSimulation")]
    public class LightSimulation : Activity
    {
        protected IDataService dataService;
        protected ISpeaker speaker;
        protected IMessenger messager;
        protected GlobalSettings Settings;
        private ILog Logger;
        private CarSignalInfo carSignal;
        private BrokenRuleMessage brokenRuleMessage;
        private LinearLayout mainLinerLayout;
        private RelativeLayout relativeLayout;
        public IExamScore ExamScore { get; private set; }
        protected LightModule LightModule { get; set; }
        protected ILightExamItem LightExamItem { get; set; }

        ImageView mgViewLowBeamLight;
        ImageView mgViewHighBeamLight;
        ImageView mgViewTurnRightLight;
        ImageView mgViewTurnLeftLight;
        ImageView mgViewOutLineLight;
        ImageView mgViewFogLight;
        Button btnRandom;
        TextView tvScore;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.lightsimulation);
            InitControl();
          
            messager = Singleton.GetMessager;
            Logger = Singleton.GetLogManager;
            ExamScore = Singleton.GetExamScore;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                //询问用户是否需要退出考试界面
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
                   .SetMessage("您是否需要退出灯光模拟项目?")
                   .SetPositiveButton("是", (s, e) =>
                   {
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
                Logger.Error("LightSimulation" + ex.Message);
            }

        }
        public void InitControl()
        {
            mgViewLowBeamLight = FindViewById<ImageView>(Resource.Id.mgViewLowBeamLight);
            mgViewHighBeamLight = FindViewById<ImageView>(Resource.Id.mgViewHightBeamLight);
            mgViewOutLineLight = FindViewById<ImageView>(Resource.Id.mgViewOutlineLight);
            mgViewTurnLeftLight = FindViewById<ImageView>(Resource.Id.mgViewTurnLeftLight);
            mgViewTurnRightLight = FindViewById<ImageView>(Resource.Id.mgViewTurnRightLight);
            mgViewFogLight = FindViewById<ImageView>(Resource.Id.mgViewFogLight);
            btnRandom = FindViewById<Button>(Resource.Id.btnRandom);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
            btnRandom.Click += BtnRandom_Click;
        }
        protected void RegisterMessages(object objectmessenger)
        {
            InitModule();
            IMessenger messenger =(IMessenger)objectmessenger;
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
            messenger.Register<BrokenRuleMessage>(this, OnBrokenRule);
        }
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            carSignal = message.CarSignal;
            RunOnUiThread(UpdateCarSensorState);
            //更新界面UI 需要在UI线程操作
            if (LightExamItem != null && LightExamItem.State == ExamItemState.Progressing)
            {
                LightExamItem.Execute(message.CarSignal);
            }
        }

        public void UpdateCarSensorState()
        {
            try
            {
                //下面部分是更新车辆状态图标
                if (carSignal.Sensor.LowBeam)
                    mgViewLowBeamLight.SetImageResource(Resource.Drawable.low_beam_on);
                else
                    mgViewLowBeamLight.SetImageResource(Resource.Drawable.low_beam_off);
                if (carSignal.Sensor.HighBeam)
                    mgViewHighBeamLight.SetImageResource(Resource.Drawable.high_beam_on);
                else
                    mgViewHighBeamLight.SetImageResource(Resource.Drawable.high_beam_off);
                if (carSignal.Sensor.FogLight)
                    mgViewFogLight.SetImageResource(Resource.Drawable.fog_light_on);
                else
                    mgViewFogLight.SetImageResource(Resource.Drawable.fog_light_off);

                //处理左右转
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
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }


        }

        private async Task StartLightAsync(string group)
        {
            if (LightExamItem != null && LightExamItem.State == ExamItemState.Progressing)
                await LightModule.StopAsync();
            await LightModule.StartAsync(new ExamContext { ExamGroup =string.Empty });
        }
        private void OnBrokenRule(BrokenRuleMessage message)
        {
            brokenRuleMessage = message;
     
            RunOnUiThread(ShowBrokenRule);
        }
        private void ShowBrokenRule()
        {
            try
            {
                UpdateExamScore();
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
                Logger.Error(ex.Message);
            }
        }
    

        public void UpdateExamScore()
        {
            tvScore.Text = string.Format("成绩:{0}分", ExamScore.Score);
        }
        private void BtnRandom_Click(object sender, EventArgs e)
        {
            //会进行重置
            ExamScore.Reset();
            mainLinerLayout = (LinearLayout)this.FindViewById(Resource.Id.RuleBreakTable);
            mainLinerLayout.RemoveAllViews();
            tvScore.Text = string.Format("成绩:{0}分",ExamScore.Score);
            StartLightAsync(string.Empty);
        }

        protected void InitModule()
        {
            LightModule = new LightModule();
            LightExamItem = LightModule.LightExamItem;
            LightModule.InitAsync(new ExamInitializationContext(MapSet.Empty)).Wait();
        }
        protected virtual void Free()
        {
            
            if (messager != null)
            {
                messager.Unregister(this);
                messager = null;
            }
            if (LightModule != null)
            {
                ReleaseModuleAsync();
            }
        }
        private async Task ReleaseModuleAsync()
        {
            await LightModule.StopAsync();
            LightModule.Dispose();
            LightModule = null;
        }

    }
}