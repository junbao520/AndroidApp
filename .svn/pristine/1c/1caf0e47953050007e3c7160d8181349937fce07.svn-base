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
    [Activity(Label = "DeductionRuleUpdate.cs")]
    public class DeductionRuleUpdate : BaseSettingActivity
    {
        ListView examItemList;
        ListView deductionruleList;
        private AlertDialog alertDialog = null;
        private AlertDialog.Builder builder = null;
        List<Domain.DeductionRule> lstDeductionRule = null;
        List<KeyValuePair<string, string>> lstLightRule = new List<KeyValuePair<string, string>>();
        List<string> lstItemCode = new List<string>();
        string ItemCode = string.Empty;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DeductionRuleUpdate);
            InitContorl();
            InitExamItem();
            initHeader(true);
            setMyTitle("扣分规则修改");
        }
        public void InitContorl()
        {
            examItemList = (ListView)FindViewById(Resource.Id.examItem_list);
            deductionruleList = (ListView)FindViewById(Resource.Id.deductionrule_list);
            examItemList.ItemClick += examItemList_ItemClick;
            deductionruleList.ItemClick += deductionruleList_ItemClick;
        }


        private void deductionruleList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //获取当前扣分规则
            var item = lstDeductionRule[e.Position];
            EditDeductionRule(item);
        }
        private void EditDeductionRule(Domain.DeductionRule deductionRule)
        {
            View view = View.Inflate(this, Resource.Layout.Dialog_DeductionRuleUpdate, null);
            EditText edtDeductionContent = (EditText)view.FindViewById(Resource.Id.edtDeductionContent);
            EditText edtDeductionScore = (EditText)view.FindViewById(Resource.Id.edtDeductionScore);
            CheckBox chkIsRequired=(CheckBox)view.FindViewById(Resource.Id.chkIsDeduction);
            //进行一个数据绑定

            edtDeductionContent.Text = deductionRule.DeductedReason;
            edtDeductionScore.Text = deductionRule.DeductedScores.ToString();
            chkIsRequired.Checked = deductionRule.IsRequired;
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("编辑扣分规则")
            .SetView(view)
            .SetNegativeButton("取消", (s, e) =>
            {

            })
            .SetPositiveButton("确定", (s, e) =>
            {
                //保存地图记录地图
                deductionRule.DeductedReason = edtDeductionContent.Text.Trim();
                deductionRule.DeductedScores = Convert.ToInt32(edtDeductionScore.Text);
                deductionRule.IsRequired = chkIsRequired.Checked;
                
                dataService.UpdateDeductionRules(deductionRule);


                InitDeductionRule(ItemCode);
                //取消对话框显示
                alertDialog.Cancel();
            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
            DisplayMetrics dm = new DisplayMetrics();
            base.WindowManager.DefaultDisplay.GetMetrics(dm);
            alertDialog.Window.SetLayout(dm.WidthPixels / 4 * 3, dm.HeightPixels / 4 * 3);
        }


        private void examItemList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int Position = e.Position;
            var Item = dataService.AllExamItems[Position];
            ItemCode = Item.ItemCode;
            InitDeductionRule(Item.ItemCode);
        }

        public void InitExamItem()
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;
          
                foreach (var item in dataService.AllExamItems)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] ="";
                    dataItem["itemName"] = item.ItemName;
                    data.Add(dataItem);
                }
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.MapListView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });
                examItemList.SetAdapter(simpleAdapter);
       
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Logger.Error("LightRuleInit", ex.Message);
            }

        }
        public void InitDeductionRule(string examItemCodes)
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;

               lstDeductionRule= dataService.GetDeductionRuleByExamItem(new List<string> { examItemCodes });

                foreach (var item in lstDeductionRule)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] = string.Empty;
                    dataItem["itemName"] = item.RuleName + "(" + item.DeductedScores + ")";
                    data.Add(dataItem);
                }
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.MapListView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });
                deductionruleList.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Logger.Error("deductionruleList", ex.Message);
            }
        }

    }
}