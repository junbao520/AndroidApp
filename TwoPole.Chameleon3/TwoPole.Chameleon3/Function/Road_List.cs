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
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "Road_List")]
    public class Road_List:BaseSettingActivity
    {
        protected ListView line_list;
        protected Button btnNewMap;
        private AlertDialog alertDialog = null;
        private AlertDialog.Builder builder = null;
        private string TipsMsg = string.Empty;
        protected override void OnCreate(Bundle savedInstanceState)
        {
           //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
          //this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Road_List);
            InitControl();
            initHeader(false);
            setMyTitle("线路规划");
            InitRoadMap();

            // Create your application here
        }
        public void InitControl()
        {
            line_list = FindViewById<ListView>(Resource.Id.line_list);
            btnNewMap = FindViewById<Button>(Resource.Id.btn_roadmap_new);
            btnNewMap.Click += RoadMapNew_Click;
            line_list.ItemClick += Line_list_ItemClick;
            line_list.ItemLongClick += Line_list_ItemLongClick;
        }

        //长按进行删除操作
        private void Line_list_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs args)
        {
            //弹出消息提示框提示用户确认是否进行删除
            var Map = dataService.GetAllMapLines()[args.Position];
            try
            {
                builder = new AlertDialog.Builder(this);
                alertDialog = builder
                .SetTitle("提示")
                .SetMessage("请确认是否需要删除"+Map.Name+"?")
                .SetNegativeButton("取消", (s, e) =>
                {
                   
                })
                .SetPositiveButton("删除地图", (s, e) =>
                {  
                    //进行删除操作
                    dataService.DeleteMap(Map);
                    InitRoadMap();
                    Toast.MakeText(this, "删除成功", ToastLength.Short).Show();
                })
                .Create();       //创建alertDialog对象  
                alertDialog.Show();

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "系统异常" + ex.Message, ToastLength.Short).Show();
            }


        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
            {
                Finish();
            }
            return base.OnKeyDown(keyCode, key);
        }
        private void Line_list_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //单机某一项直接进入对应的丢他
            var Map = dataService.GetAllMapLines()[e.Position];
            EditMapDetail(Map.Name, Map.Id);
        }

        public void RoadMapNew_Click(object sender, EventArgs e)
        {
            AddNewMap();
        }


        private void InitRoadMap()
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;
                foreach (var item in dataService.GetAllMapLines())
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemName"]=string.Format("{0}   {1}",item.Name,item.CreateOn.ToString());
                    data.Add(dataItem);
                }
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.RoadNameListView, new String[] { "itemName" }, new int[] { Resource.Id.itemName });
                line_list.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
                Log.Error("GridViewInitMap", ex.Message);
            }
        }

        public void ShowTips()
        {
            Toast.MakeText(this, TipsMsg, ToastLength.Long);
        }
      
        private void AddNewMap()
        {
            View view = View.Inflate(this,Resource.Layout.Dialog_Input, null);
            EditText editText = (EditText)view.FindViewById(Resource.Id.dialog_input_value);
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("新增路线")
            .SetView(view)
            .SetNegativeButton("取消", (s, e) =>
            {
               
            })
            .SetPositiveButton("确定", (s, e) =>
            {
                //保存地图记录地图
                string mapName = editText.Text;
                if (string.IsNullOrEmpty(mapName))
                {
                    TipsMsg = "请输入地图名称";
                    RunOnUiThread(ShowTips);
                    //Toast.MakeText(this, "请输入地图名称", ToastLength.Long);
                }
                else
                {
                    if (dataService.GetAllMapLines().Where(m=>m.Name==mapName).Count()>=1)
                    {
                        TipsMsg = "地图名称已经存在";
                        RunOnUiThread(ShowTips);
                        //地图名称不可以重复
                        return;
                    }
                    AddMapDetail(mapName);
                    //Toast.MakeText(this, mapName, ToastLength.Long);
                }
            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
        }
        /// <summary>
        /// 添加地图
        /// </summary>
        /// <param name="MapName"></param>
        private void AddMapDetail(string  MapName)
        {
            //打开地图界面
            Intent intent = new Intent();
            intent.SetClass(this, typeof(Map));
            intent.PutExtra("MapName", MapName);
            intent.PutExtra("MapType", "Add");
            StartActivity(intent);
        }
        private void EditMapDetail(string MapName,int MapID)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(Map));
            intent.PutExtra("MapName", MapName);
            intent.PutExtra("MapID", MapID);
            intent.PutExtra("MapType", "Edit");
            StartActivity(intent);
        }
    }
}