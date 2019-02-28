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
using Android.Util;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Infrastructure;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Messages;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Foundation.Spatial;
using Twopole.Chameleon3.Domain;

namespace TwoPole.Chameleon3
{

    /// <summary>
    /// 灯光模拟配置界面，可以添加灯光语音以及添加灯光分组
    /// </summary>
    [Activity(Label = "LightSimulationConfig")]
    public class LightSimulationConfigNew : Activity
    {
        private IDataService dataService;
        private ILog Logger;
        ListView LightRuleList;
        ListView LightGroupList;
        Button btnLightRuleAdd;
        Button btnLightGroupAdd;
        private AlertDialog alertDialog = null;
        private AlertDialog.Builder builder = null;
        List<KeyValuePair<string, string>> lstLightRule = new List<KeyValuePair<string, string>>();
        List<string> lstItemCode = new List<string>();
        private string LightRuleNamespace = "TwoPole.Chameleon3.Business.Rules.{0},TwoPole.Chameleon3.Business";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LightSimulationConfig);
            dataService = Singleton.GetDataService;
            Logger = Singleton.GetLogManager;
            InitContorl();
        }
        public void InitContorl()
        {
            LightGroupList=(ListView)FindViewById(Resource.Id.lightgroup_list);
            LightRuleList = (ListView)FindViewById(Resource.Id.lightrule_list);
            btnLightGroupAdd = FindViewById<Button>(Resource.Id.btnLightGroupAdd);
            btnLightRuleAdd = FindViewById<Button>(Resource.Id.btnLightRuleAdd);
            btnLightGroupAdd.Click += BtnLightGroupAdd_Click;
            btnLightRuleAdd.Click += BtnLightRuleAdd_Click;
            LightGroupList.ItemClick += LightGroupList_ItemClick;

            LightRuleList.ItemLongClick += LightGroupList_ItemLongClick;

            LightRuleList.ItemClick += LightRuleList_ItemClick;
            InitLightGroup();
            InitLightRule();
            //TODO：这个类没有用
            /*预先设置灯光模拟的考试规则*/
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯、示廓灯", "LowBeamLightRule")); lstItemCode.Add("41616");
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯、示廓灯", "LowBeamLightRule")); lstItemCode.Add("41616");
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯、示廓灯、左转", "LowAndLeftLightRule")); lstItemCode.Add("41601");
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯、示廓灯、右转", "LowAndRightLightRule")); lstItemCode.Add("41601");
            lstLightRule.Add(new KeyValuePair<string, string>("远光、近光灯、示廓灯", "HighBeamLightRule"));       lstItemCode.Add("41617");
            lstLightRule.Add(new KeyValuePair<string, string>("远近光交替一次", "LowAndHighBeamOnceLightRule"));  lstItemCode.Add("41603");
            lstLightRule.Add(new KeyValuePair<string, string>("远近光交替二次", "LowAndHighBeamTwiceLightRule")); lstItemCode.Add("41603");
            lstLightRule.Add(new KeyValuePair<string, string>("远近光交替三次", "LowAndHighBeamThirdLightRule")); lstItemCode.Add("41603");
            lstLightRule.Add(new KeyValuePair<string, string>("雾灯、警示灯、近光、示廓灯", "FogDrivingLightRule"));lstItemCode.Add("41608");
            lstLightRule.Add(new KeyValuePair<string, string>("远光或者近光", "OpenHighLightRule")); lstItemCode.Add("41610");
            lstLightRule.Add(new KeyValuePair<string, string>("小灯、警示灯", "OutlineAndCautionLightRule")); lstItemCode.Add("41610");
            lstLightRule.Add(new KeyValuePair<string, string>("小灯", "OutlineLightRule")); lstItemCode.Add("41610");
            lstLightRule.Add(new KeyValuePair<string, string>("警示灯", "CautionLightRule")); lstItemCode.Add("41610");
            lstLightRule.Add(new KeyValuePair<string, string>("关闭所有灯光", "CloseAllLightRule")); lstItemCode.Add("41609");
        }

        private void LightGroupList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            //长按 
            int Position = e.Position;
            var Item = dataService.AllLightRules[Position];
           EditLightRule(Item);
            
        }

        private void LightRuleList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int Position = e.Position;
            var Item = dataService.AllLightRules[Position];
            AlertDeleteLightRuleDialog(Item);
        }

        private void LightGroupList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int Position = e.Position;
            var Item = dataService.AllLightExamItems[Position];
            AlertDeleteDialog(Item);
        }

        private void BtnLightRuleAdd_Click(object sender, EventArgs e)
        {
            AddLightRule();
        }

        private void BtnLightGroupAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddLightGroup();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            
        }
        private void BindSpinner(List<string> lstDataSource, Spinner spinner)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lstDataSource);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.Visibility = ViewStates.Visible;

        }
        private void EditLightRule(LightRule lightRule)
        {
            View view = View.Inflate(this, Resource.Layout.Dialog_Input_editlightrule, null);
            EditText editText = (EditText)view.FindViewById(Resource.Id.dialog_input_value);
            //进行一个数据绑定
            editText.Text = lightRule.VoiceFile;

            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("编辑灯光模拟项目")
            .SetView(view)
            .SetNegativeButton("取消", (s, e) =>
            {

            })
            .SetPositiveButton("确定", (s, e) =>
            {
                //保存地图记录地图
                string LightRuleName = editText.Text;
                if (!string.IsNullOrEmpty(LightRuleName))
                {
                    lightRule.VoiceText = LightRuleName;
                    lightRule.VoiceFile = LightRuleName;
                    dataService.EditLightRule(lightRule);
                    InitLightRule();

                }
                else
                {
                    //开始


                }
            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
        }
        private void AddLightRule()
        {
            View view = View.Inflate(this, Resource.Layout.Dialog_Input_lightrule, null);
            EditText editText = (EditText)view.FindViewById(Resource.Id.dialog_input_value);
            CheckBox chkEnableCustom = view.FindViewById<CheckBox>(Resource.Id.chkEnableCustom);
            #region 允许灯光
            CheckBox chkAllowLowBeam = view.FindViewById<CheckBox>(Resource.Id.chkAllowLowBeamLight);
            CheckBox chkAllowHighBeam = view.FindViewById<CheckBox>(Resource.Id.chkAllowHighBeamLight);
            CheckBox chkAllowOutlineLight = view.FindViewById<CheckBox>(Resource.Id.chkAllowOutlineLight);
            CheckBox chkAllowFogLight = view.FindViewById<CheckBox>(Resource.Id.chkAllowFogLight);
            CheckBox chkAllowTurnLeftLight = view.FindViewById<CheckBox>(Resource.Id.chkAllowTurnLeftLight);
            CheckBox chkAllowTurnRightLight = view.FindViewById<CheckBox>(Resource.Id.chkAllowTurnRightLight);
            CheckBox chkAllowCautionLight = view.FindViewById<CheckBox>(Resource.Id.chkAllowCautionLight);
            #endregion
            #region 禁止灯光
            CheckBox chkForbidLowBeam = view.FindViewById<CheckBox>(Resource.Id.chkForbidLowBeamLight);
            CheckBox chkForbidHighBeam = view.FindViewById<CheckBox>(Resource.Id.chkForbidHighBeamLight);
            CheckBox chkForbidOutlineLight = view.FindViewById<CheckBox>(Resource.Id.chkForbidOutlineLight);
            CheckBox chkForbidFogLight = view.FindViewById<CheckBox>(Resource.Id.chkForbidFogLight);
            CheckBox chkForbidTurnLeftLight = view.FindViewById<CheckBox>(Resource.Id.chkForbidTurnLeftLight);
            CheckBox chkForbidTurnRightLight = view.FindViewById<CheckBox>(Resource.Id.chkForbidTurnRightLight);
            CheckBox chkForbidCautionLight = view.FindViewById<CheckBox>(Resource.Id.chkForbidCautionLight);
            #endregion
            #region 交替灯光
            CheckBox chkAlternateLightOne = view.FindViewById<CheckBox>(Resource.Id.chkAlternateLightOne);
            CheckBox chkAlternateLightTwo= view.FindViewById<CheckBox>(Resource.Id.chkAlternateLightTwo);
            CheckBox chkAlternateLightThree = view.FindViewById<CheckBox>(Resource.Id.chkAlternateLightThree);
            CheckBox chkAlternateLightFour = view.FindViewById<CheckBox>(Resource.Id.chkAlternateLightFour);
            #endregion

            //进行一个数据绑定
            Spinner lightruleSpinner = (Spinner)view.FindViewById(Resource.Id.LightRulespinner);
            BindSpinner(lstLightRule.Select(s => s.Key).ToList<string>(), lightruleSpinner);
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("新增灯光模拟项目")
            .SetView(view)
            .SetNegativeButton("取消", (s, e) =>
            {

            })
            .SetPositiveButton("确定", (s, e) =>
            {
                //保存地图记录地图
                string LightRuleName = editText.Text;
                if (!string.IsNullOrEmpty(LightRuleName))
                {

                    if (!chkEnableCustom.Checked)
                    {
                        int position = lightruleSpinner.SelectedItemPosition;
                        LightRule lightRule = new LightRule();
                        lightRule.ItemCode = lstItemCode[position];
                        lightRule.ItemName = LightRuleName;
                        lightRule.LightRuleType = string.Format("TwoPole.Chameleon3.Business.Rules.{0},TwoPole.Chameleon3.Business", lstLightRule[position].Value);
                        lightRule.OperDes = lstLightRule[position].Key;
                        lightRule.VoiceText = LightRuleName;
                        lightRule.VoiceFile = LightRuleName;

                        dataService.SaveLightRule(lightRule);

                        InitLightRule();

                    }
                    else //表示的是自选组合
                    {

                        CustomLight customLight = new CustomLight();

                        if (chkAllowLowBeam.Checked)
                        {
                            customLight.Allowlights += "LowBeam,";
                        }
                        if (chkAllowHighBeam.Checked)
                        {
                            customLight.Allowlights += "HighBeam,";
                        }
                        if (chkAllowFogLight.Checked)
                        {
                            customLight.Allowlights += "FogLight,";
                        }
                        if (chkAllowCautionLight.Checked)
                        {
                            customLight.Allowlights += "CautionLight,";
                        }
                        if (chkAllowOutlineLight.Checked)
                        {
                            customLight.Allowlights += "OutlineLight,";
                        }
                        if (chkAllowTurnLeftLight.Checked)
                        {
                            customLight.Allowlights += "LeftIndicatorLight,";
                        }
                        if (chkAllowTurnRightLight.Checked)
                        {
                            customLight.Allowlights += "RightIndicatorLight,";
                        }
                        if (string.IsNullOrEmpty(customLight.Allowlights))
                        {
                            return;
                        }
                        customLight.Allowlights = customLight.Allowlights.Substring(0, customLight.Allowlights.Length - 1);

                        if (chkForbidLowBeam.Checked)
                        {
                            customLight.Fobbidlights += "LowBeam,";
                        }
                        if (chkForbidHighBeam.Checked)
                        {
                            customLight.Fobbidlights += "HighBeam,";
                        }
                        if (chkForbidFogLight.Checked)
                        {
                            customLight.Fobbidlights += "FogLight,";
                        }
                        if (chkForbidCautionLight.Checked)
                        {
                            customLight.Fobbidlights += "CautionLight,";
                        }
                        if (chkForbidOutlineLight.Checked)
                        {
                            customLight.Fobbidlights += "OutlineLight,";
                        }
                        if (chkForbidTurnLeftLight.Checked)
                        {
                            customLight.Fobbidlights += "LeftIndicatorLight,";
                        }
                        if (chkForbidTurnRightLight.Checked)
                        {
                            customLight.Fobbidlights += "RightIndicatorLight,";
                        }
                        if (string.IsNullOrEmpty(customLight.Fobbidlights))
                        {
                            return;
                        }
                        customLight.Fobbidlights = customLight.Fobbidlights.Substring(0, customLight.Fobbidlights.Length - 1);

                        if (chkAlternateLightOne.Checked)
                        {
                            customLight.AlternateCount = 1;
                        }
                        if (chkAlternateLightTwo.Checked)
                        {
                            customLight.AlternateCount = 2;
                        }
                        if (chkAlternateLightThree.Checked)
                        {
                            customLight.AlternateCount = 3;
                        }
                        if (chkAlternateLightFour.Checked)
                        {
                            customLight.AlternateCount = 4;
                        }
                        LightRule lightRule = new LightRule();
                        lightRule.ItemCode ="41601";
                        lightRule.ItemName = LightRuleName;
                        lightRule.LightRuleType = string.Format("TwoPole.Chameleon3.Business.Rules.CustomLightRule,TwoPole.Chameleon3.Business");
                        lightRule.OperDes = customLight.ToJson();
                        lightRule.VoiceText = LightRuleName;
                        lightRule.VoiceFile = LightRuleName;
                    }
             

                }
                else
                {
                   //开始
                   
               
                }
            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
        }
        public string GetAllowFobbidLights(bool light,string LightName)
        {
            return string.Empty;
        }
        public void AlertDeleteLightRuleDialog(LightRule item)
        {
            try
            {
                builder = new AlertDialog.Builder(this);
                alertDialog = builder
                .SetTitle("温馨提示")
                .SetMessage("请确认是否需要删除灯光模拟项目：" + item.ItemName + "?")
                .SetNegativeButton("取消", (s, e) =>
                {
                })
                .SetPositiveButton("删除灯光模拟项目", (s, e) =>
                {
                    dataService.DeleteLightRule(item);
                    InitLightRule();
                })

                .Create();       //创建alertDialog对象  
                alertDialog.Show();

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "系统异常" + ex.Message, ToastLength.Short).Show();
                Logger.Error("Map", ex.Message);
            }

        }
        public void AlertDeleteDialog(LightExamItem item)
        {
            try
            {
                builder = new AlertDialog.Builder(this);
                alertDialog = builder
                .SetTitle("提示")
                .SetMessage("请确认是否需要删除灯光模拟分组：" +item.GroupName+ "?")
                .SetNegativeButton("取消", (s, e) =>
                {
                })
                .SetPositiveButton("删除灯光分组", (s, e) =>
                {
                    dataService.DeleteLightExamItem(item);
                    InitLightGroup();
                })
           
                .Create();       //创建alertDialog对象  
                alertDialog.Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "系统异常" + ex.Message, ToastLength.Short).Show();
                Logger.Error("Map", ex.Message);
            }

        }
       
        private void AddLightGroup()
        {
            View view = View.Inflate(this, Resource.Layout.Dialog_LightRuleGroup_Input, null);
            EditText edtGroupName = (EditText)view.FindViewById(Resource.Id.InputValueGroupName);
            EditText edt1=(EditText)view.FindViewById(Resource.Id.InputValue1);
            EditText edt2 = (EditText)view.FindViewById(Resource.Id.InputValue2);
            EditText edt3 = (EditText)view.FindViewById(Resource.Id.InputValue3);
            EditText edt4 = (EditText)view.FindViewById(Resource.Id.InputValue4);
            EditText edt5 = (EditText)view.FindViewById(Resource.Id.InputValue5);
            EditText edt6 = (EditText)view.FindViewById(Resource.Id.InputValue6);
            EditText edt7 = (EditText)view.FindViewById(Resource.Id.InputValue7);
            EditText edt8 = (EditText)view.FindViewById(Resource.Id.InputValue8);
            EditText edt9 = (EditText)view.FindViewById(Resource.Id.InputValue9);
            EditText edt10 = (EditText)view.FindViewById(Resource.Id.InputValue10);
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("新增灯光模拟分组")
            .SetView(view)
            .SetNegativeButton("取消", (s, e) =>
            {

            })
            .SetPositiveButton("确定", (s, e) =>
            {
                //保存地图记录地图
                string LightRuleGroupName = edtGroupName.Text;
                if (string.IsNullOrEmpty(LightRuleGroupName))
                {
                    return;
                }
                else
                {
                    //一组,1,2,3,4,5
                    LightExamItem Item = new LightExamItem();
                    Item.GroupName = LightRuleGroupName;
                    string LightRules = string.Empty;
                    if (!string.IsNullOrEmpty(edt1.Text))
                    {
                        LightRules = edt1.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt2.Text))
                    {
                        LightRules = LightRules + edt2.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt3.Text))
                    {
                        LightRules = LightRules + edt3.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt4.Text))
                    {
                        LightRules = LightRules + edt4.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt5.Text))
                    {
                        LightRules = LightRules + edt5.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt6.Text))
                    {
                        LightRules = LightRules + edt6.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt7.Text))
                    {
                        LightRules = LightRules + edt7.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt8.Text))
                    {
                        LightRules = LightRules + edt8.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt9.Text))
                    {
                        LightRules = LightRules + edt9.Text + ",";
                    }
                    if (!string.IsNullOrEmpty(edt10.Text))
                    {
                        LightRules = LightRules + edt10.Text + ",";
                    }
                    Logger.Error(LightRules);
                    LightRules = LightRules.Substring(0, LightRules.Length - 1);

                    Item.LightRules = LightRules;
                    dataService.SaveLightExamItem(Item);
                    InitLightGroup();
                }
            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
        } 

        public void InitLightRule()
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;

                foreach (var item in dataService.AllLightRules)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] ="";
                    dataItem["itemName"] = item.Id+","+item.ItemName+",("+item.OperDes+")";
                    data.Add(dataItem);
                }
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.MapListView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });


                LightRuleList.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Logger.Error("LightRuleInit", ex.Message);
            }

        }
        public void InitLightGroup()
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;

                foreach (var item in dataService.AllLightExamItems)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] = string.Empty;
                    dataItem["itemName"] = item.GroupName+","+item.LightRules;
                    data.Add(dataItem);
                }
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.MapListView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });


                LightGroupList.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Logger.Error("LightRuleInit", ex.Message);
            }
        }

    }
}