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

namespace TwoPole.Chameleon3
{

    /// <summary>
    /// 灯光模拟配置界面，可以添加灯光语音以及添加灯光分组
    /// </summary>
    [Activity(Label = "LightSimulationConfig")]
    public class LightSimulationConfig :BaseSettingActivity
    {
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
            InitContorl();
            initHeader(false);
            setMyTitle("灯光模拟配置");
           
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

            LightRuleList.ItemLongClick += LightRuleList_ItemLongClick;
            LightGroupList.ItemLongClick += LightGroupList_ItemLongClick;

            LightRuleList.ItemClick += LightRuleList_ItemClick;
            InitLightGroup();
            InitLightRule();

            /*预先设置灯光模拟的考试规则*/
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯", "LowBeamLightRule")); lstItemCode.Add("41616");
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯", "LowBeamLightRule")); lstItemCode.Add("41616");
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯、左转", "LowAndLeftLightRule")); lstItemCode.Add("41601");
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯、右转", "LowAndRightLightRule")); lstItemCode.Add("41601");
            lstLightRule.Add(new KeyValuePair<string, string>("远光灯", "HighBeamLightRule")); lstItemCode.Add("41617");
            lstLightRule.Add(new KeyValuePair<string, string>("远近光交替一次", "LowAndHighBeamOnceLightRule")); lstItemCode.Add("41603");
            lstLightRule.Add(new KeyValuePair<string, string>("远近光交替二次", "LowAndHighBeamTwiceLightRule")); lstItemCode.Add("41603");
            lstLightRule.Add(new KeyValuePair<string, string>("远近光交替三次", "LowAndHighBeamThirdLightRule")); lstItemCode.Add("41603");
            lstLightRule.Add(new KeyValuePair<string, string>("雾灯、警示灯、近光、示廓灯", "FogDrivingLightRule")); lstItemCode.Add("41608");
            lstLightRule.Add(new KeyValuePair<string, string>("远光或者近光", "OpenHighLightRule")); lstItemCode.Add("41610");
            lstLightRule.Add(new KeyValuePair<string, string>("小灯、警示灯", "OutlineAndCautionLightRule")); lstItemCode.Add("41610");

            lstLightRule.Add(new KeyValuePair<string, string>("小灯、警示灯(处理左右转不同步)", "OutlineAndCautionLightRule2")); lstItemCode.Add("41610");

            lstLightRule.Add(new KeyValuePair<string, string>("小灯", "OutlineLightRule")); lstItemCode.Add("41610");
            lstLightRule.Add(new KeyValuePair<string, string>("警示灯", "CautionLightRule")); lstItemCode.Add("41610");

            lstLightRule.Add(new KeyValuePair<string, string>("小灯、近光、警示灯", "OutlineAndCautionLightRule3")); lstItemCode.Add("41601");
            lstLightRule.Add(new KeyValuePair<string, string>("小灯、近光、警示灯（处理左右转不同步）", "OutlineAndCautionLightRule4")); lstItemCode.Add("41601");

            lstLightRule.Add(new KeyValuePair<string, string>("关闭所有灯光", "CloseAllLightRule")); lstItemCode.Add("41609");
            lstLightRule.Add(new KeyValuePair<string, string>("关闭所有灯光(忽略左转)", "CloseAllLightExceptLeftRule")); lstItemCode.Add("41609");

            //爱丽舍会闪近光
            lstLightRule.Add(new KeyValuePair<string, string>("小灯、警示灯(处理近光会闪)", "OutlineAndCautionLightRuleIgnoreLowBeam")); lstItemCode.Add("41610");
            lstLightRule.Add(new KeyValuePair<string, string>("小灯(处理近光会闪)", "OutlineLightRuleIgnoreLowBeam")); lstItemCode.Add("41610");
            lstLightRule.Add(new KeyValuePair<string, string>("近光灯(处理近光会闪)", "LowBeamLightRuleIgnoreLowBeam")); lstItemCode.Add("41616");
            //lstLightRule.Add(new KeyValuePair<string, string>("三亚关闭所有灯光", "CloseAllLightRule")); lstItemCode.Add("41609");

            //TwoPole.Chameleon3.Business.Areas.HaiNan.SanYa.Rules
        }

        private void LightGroupList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            int Position = e.Position;
            var Item = dataService.AllLightExamItems[Position];
            EditLightGroup(Item);

            InitLightGroup();
        }

        private void LightGroupList_ItemLongClick1(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            int Position = e.Position;
            var Item = dataService.AllLightExamItems[Position];
        }

        private void LightRuleList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            //长按 
            int Position = e.Position;
            var Item = dataService.AllLightRules[Position];

           EditLightRuleNew(Item);
            
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

        //todo:根据李松林要求添加灯光模拟编辑功能

        private void EditLightRuleNew(LightRule lightRule)
        {
            View view = View.Inflate(this, Resource.Layout.Dialog_Input_lightrule, null);
            EditText editText = (EditText)view.FindViewById(Resource.Id.dialog_input_value);
            //进行一个数据绑定
            Spinner lightruleSpinner = (Spinner)view.FindViewById(Resource.Id.LightRulespinner);
            BindSpinner(lstLightRule.Select(s => s.Key).ToList<string>(), lightruleSpinner);
            editText.Text = lightRule.ItemName;
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
                    //spinnerHighBeamAddress.SelectedItemPosition
                    int position = lightruleSpinner.SelectedItemPosition;
     
                    lightRule.ItemCode = lstItemCode[position];
                    lightRule.ItemName = LightRuleName;
                    lightRule.LightRuleType = string.Format("TwoPole.Chameleon3.Business.Rules.{0},TwoPole.Chameleon3.Business", lstLightRule[position].Value);
                    lightRule.OperDes = lstLightRule[position].Key;
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
                    //spinnerHighBeamAddress.SelectedItemPosition
                    int position = lightruleSpinner.SelectedItemPosition;
                    LightRule lightRule = new LightRule();
                    lightRule.ItemCode = lstItemCode[position];
                    lightRule.ItemName = LightRuleName;
                    lightRule.LightRuleType =string.Format("TwoPole.Chameleon3.Business.Rules.{0},TwoPole.Chameleon3.Business",lstLightRule[position].Value);
                    lightRule.OperDes = lstLightRule[position].Key;
                    lightRule.VoiceText = LightRuleName;
                    lightRule.VoiceFile = LightRuleName;

                    dataService.SaveLightRule(lightRule);
                   
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

        //2张
        public void SetLightGroupText(string[] Lights,int Index,EditText edt)
        {
            if (Index>Lights.Count()-1)
            {
                return;
            }
            else
            {
                //设置值
                edt.Text = Lights[Index];
            }
        }
       
        /// <summary>
        /// 编辑灯光模拟分组
        /// </summary>
        private void EditLightGroup(LightExamItem Item)
        {
            View view = View.Inflate(this, Resource.Layout.Dialog_LightRuleGroup_Input, null);
            EditText edtGroupName = (EditText)view.FindViewById(Resource.Id.InputValueGroupName);
            EditText edt1 = (EditText)view.FindViewById(Resource.Id.InputValue1);
            EditText edt2 = (EditText)view.FindViewById(Resource.Id.InputValue2);
            EditText edt3 = (EditText)view.FindViewById(Resource.Id.InputValue3);
            EditText edt4 = (EditText)view.FindViewById(Resource.Id.InputValue4);
            EditText edt5 = (EditText)view.FindViewById(Resource.Id.InputValue5);
            EditText edt6 = (EditText)view.FindViewById(Resource.Id.InputValue6);
            EditText edt7 = (EditText)view.FindViewById(Resource.Id.InputValue7);
            EditText edt8 = (EditText)view.FindViewById(Resource.Id.InputValue8);
            EditText edt9 = (EditText)view.FindViewById(Resource.Id.InputValue9);
            EditText edt10 = (EditText)view.FindViewById(Resource.Id.InputValue10);
            edtGroupName.Text = Item.GroupName;
            edtGroupName.Enabled = false;

            var Lights = Item.LightRules.Split(',');

            SetLightGroupText(Lights, 0, edt1);
            SetLightGroupText(Lights, 1, edt2);
            SetLightGroupText(Lights, 2, edt3);
            SetLightGroupText(Lights, 3, edt4);
            SetLightGroupText(Lights, 4, edt5);
            SetLightGroupText(Lights, 5, edt6);
            SetLightGroupText(Lights, 6, edt7);
            SetLightGroupText(Lights, 7, edt8);
            SetLightGroupText(Lights, 8, edt9);
            SetLightGroupText(Lights, 9, edt10);
            //然后进行赋值



            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("编辑灯光模拟分组")
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
                    LightRules = LightRules.Substring(0, LightRules.Length - 1);

                    Item.LightRules = LightRules;
                    dataService.UpdateLightExamItem(Item);
                    InitLightGroup();
                }
            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
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