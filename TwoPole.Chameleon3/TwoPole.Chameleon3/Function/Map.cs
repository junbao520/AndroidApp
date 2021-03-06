﻿using System;
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
    [Activity(Label = "Map")]
    public class Map :BaseSettingActivity
    {
        LinearLayout mbtn_header_right;
        TextView tvCarSignal;
       // TextView mRoad_title;
        GridView mGridView;
        ListView RoadMapListView;
        ImageView micon_gps_blue;
        EditText edtRoadName;
        CarSignalInfo carSignal;
        //如果这个信号和上一个信号差距过大，打点图的时候将被抛弃。
        private AlertDialog alertDialog = null;
        private AlertDialog.Builder builder = null;
        List<KeyValuePair<int, MapLinePoint>> lstExamItem = new List<KeyValuePair<int, MapLinePoint>>();
        List<KeyValuePair<int, MapLinePoint>> lstRoadMapExamItem = new List<KeyValuePair<int, MapLinePoint>>();
        MapLinePoint mapLinePoint = null;
        KeyValuePair<int, MapLinePoint> mapKeyValue = new KeyValuePair<int, MapLinePoint>();
        int MapID;
        string MapName;
        bool IsAddMap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //  this.SetTheme(Android.Resource.Style.ThemeNoTitleBarFullScreen);//全屏并且无标题栏，必须在OnCreate前面设置。
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Map);
            InitContorl();
            initHeader(true);
            setMyTitle("线路采集");
            GetIntentParameter();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegisterMessages));
            t.Start(messager);
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            Logger.Error("AndroidEnvironmentMap", e.Exception.Message);
        }

        public override void ShowConfirmDialog()
        {
            Back();
        }
        public void Back()
        {
            builder = new AlertDialog.Builder(this);
            alertDialog = builder
            .SetTitle("提示")
            .SetMessage("请确认是否需要更新或者保存地图" + edtRoadName.Text + "?")
            .SetNegativeButton("取消", (s, e) =>
            {
                Finish();
            })
            .SetPositiveButton("保存地图", (s, e) =>
            {
                if (IsAddMap)
                {
                    SaveMap();//MapType
                        }
                else
                {
                    UpdateMap();
                }

            })
            .Create();       //创建alertDialog对象  

            alertDialog.Show();
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent key)
        {
            try
            {
                if (keyCode == Keycode.Back && key.Action == KeyEventActions.Down)
                {
                    Back();
                }
                return base.OnKeyDown(keyCode, key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return base.OnKeyDown(keyCode, key);
            }

        }
        public void SaveMap()
        {
            try
            {
                MapLine mapLine = new MapLine();
                IList<MapLinePoint> Points = new List<MapLinePoint>();

                foreach (var item in lstRoadMapExamItem)
                {
                    Points.Add(item.Value);
                }
                mapLine.Id = 0;
                mapLine.Points = Points;
                mapLine.Name = edtRoadName.Text;
                mapLine.Distance = Points.CalculateDistances();
                mapLine.CreateOn = DateTime.Now;
                mapLine.Remark = mapLine.Points.ToJson();
                bool Result = dataService.SaveMap(mapLine);
                if (Result)
                {
                    Toast.MakeText(this, "保存成功", ToastLength.Short).Show();
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "保存失败", ToastLength.Short).Show();
                }

            }
            catch (Exception ex)
            {
                Log.Error("SaveMap", ex.Message);
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }


        }

        public void UpdateMap()
        {
            try
            {
                MapLine mapLine = dataService.GetAllMapLines().Where(s => s.Id == MapID).FirstOrDefault();
                IList<MapLinePoint> Points = new List<MapLinePoint>();

                foreach (var item in lstRoadMapExamItem)
                {
                    Points.Add(item.Value);
                }
                mapLine.Points = Points;
                mapLine.Name = edtRoadName.Text;
                mapLine.Distance = Points.CalculateDistances();
                mapLine.CreateOn = DateTime.Now;
                mapLine.Remark = mapLine.Points.ToJson();
                bool Result = dataService.UpdateMap(mapLine);
                if (Result)
                {
                    Toast.MakeText(this, "更新地图成功", ToastLength.Short).Show();
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "更新地图失败", ToastLength.Short).Show();
                }

            }
            catch (Exception ex)
            {
                Log.Error("SaveMap", ex.Message);
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }


        }
        public void GetIntentParameter()
        {
            MapID = Intent.GetIntExtra("MapID", 0);
            MapName = Intent.GetStringExtra("MapName");
            IsAddMap = Intent.GetStringExtra("MapType") == "Add";

            //初始化地图名称
           // mRoad_title.Text = MapName;
            edtRoadName.Text = MapName;

            //这个需要初始化绑定数据
            if (IsAddMap == false)
            {
                //lstRoadMapExamItem
                var Map = dataService.GetAllMapLines().Where(s => s.Id == MapID).FirstOrDefault();

                //在所有的点位里面
                foreach (var item in Map.Points)
                {
                    if (lstExamItem.Where(s => s.Value.Name == item.Name).Count() >= 1)
                    {
                        var Item = lstExamItem.Where(s => s.Value.Name == item.Name).FirstOrDefault();
                        mapKeyValue = new KeyValuePair<int, MapLinePoint>(Item.Key, item);
                    }
                    else
                    {
                        mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.icon_add_blue, item);
                    }
                    lstRoadMapExamItem.Add(mapKeyValue);

                }
                InitRoadMap();
            }


        }
        public void InitContorl()
        {
            mbtn_header_right = (LinearLayout)FindViewById(Resource.Id.btn_header_right);
            tvCarSignal = (TextView)FindViewById(Resource.Id.tvCarSignal);
           // mRoad_title = (TextView)FindViewById(Resource.Id.road_title);
            micon_gps_blue = (ImageView)FindViewById(Resource.Id.icon_gps_blue);
            edtRoadName = FindViewById<EditText>(Resource.Id.edtTxtRoadName);
            mGridView = (GridView)FindViewById(Resource.Id.MapGridView);
            RoadMapListView = (ListView)FindViewById(Resource.Id.roadmap_list);

            mGridView.ItemClick += MGridView_ItemClick;
            RoadMapListView.ItemClick += RoadMapListView_ItemClick;


            InitExamItemMap();
        }

        protected void RegisterMessages(object Objectmessenger)
        {
            IMessenger messenger = (IMessenger)Objectmessenger;
            messenger.Register<CarSignalReceivedMessage>(this, OnCarSignalReceived);
        }
        //Todo:Gps会闪0，但是不是特别频繁故暂时不予处理。无效的Gps点位只要不记录在数据库中就可以了。
        private void OnCarSignalReceived(CarSignalReceivedMessage message)
        {
            try
            {
                //如果Gps无效则不需要进行记录//
                if (message == null || message.CarSignal == null||!message.CarSignal.IsGpsValid)
                {
                    return;
                }
                carSignal = message.CarSignal;
                RunOnUiThread(UpadteCarSignal);
            }
            catch (Exception ex)
            {
                Logger.Error("Map", ex.Message);
            }

        }
        private void UpadteCarSignal()
        {

            try
            {
                if (carSignal == null || carSignal.Gps == null || carSignal.Sensor == null)
                {
                    return;
                }
                //很有可能是Gps信号异常造成的更新UI爆发未处理异常
                string Lat = string.Format("{0:#0.00000}°", carSignal.Gps.LatitudeDegrees);
                string Lon = string.Format("{0:#0.00000}°", carSignal.Gps.LongitudeDegrees);
                string Angle = string.Format("{0,-5}°", carSignal.BearingAngle);
                string SatelliteCount = string.Format("{0}/{1}", carSignal.Gps.TrackedSatelliteCount, carSignal.Gps.FixedSatelliteCount);
                string Speed = string.Format("{0}Km/h", carSignal.Sensor.SpeedInKmh);
                tvCarSignal.Text = string.Format("纬度:{0}经度:{1}卫星颗数:{2}方向:{3}速度：{4}", Lat, Lon, SatelliteCount, Angle, Speed);
            }
            catch (Exception ex)
            {
                Logger.Error("UpadteCarSignal", ex.Message);
            }
        }

        //单机询问用户是否需要删除
        private void RoadMapListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                int Position = e.Position;
                string ItemName = lstRoadMapExamItem[Position].Value.Name;
                AlertDeleteUpdateDialog(ItemName, Position);
            }
            catch (Exception ex)
            {
                Logger.Error("Map", ex.Message);
            }
        }
        public void AlertDeleteUpdateDialog(string ItemName, int Position)
        {
            try
            {
                builder = new AlertDialog.Builder(this);
                alertDialog = builder
                .SetTitle("提示")
                .SetMessage("请确认是否需要删除或者更新地图" + ItemName + "?")
                .SetNegativeButton("取消", (s, e) =>
                {
                })
                .SetPositiveButton("删除地图", (s, e) =>
                {
                    lstRoadMapExamItem.RemoveAt(Position);
                    InitRoadMap();
                    Toast.MakeText(this, "删除成功", ToastLength.Short).Show();
                })
                .SetNeutralButton("更新地图", (s, e) =>
                {
                    var Point = lstRoadMapExamItem[Position];
                    lstRoadMapExamItem.RemoveAt(Position);
                    Point.Value.Latitude = carSignal.Gps.LatitudeDegrees;
                    Point.Value.Longitude = carSignal.Gps.LongitudeDegrees;
                    Point.Value.Angle = carSignal.Gps.AngleDegrees;
                    Point.Value.Altitude = carSignal.Gps.AltitudeMeters;
                    lstRoadMapExamItem.Add(Point);
                    Toast.MakeText(this, "更新成功", ToastLength.Short).Show();
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



        private void MGridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                var mapLinePoint = new MapLinePoint();
                var Point = lstExamItem[e.Position];
                speaker.PlayAudioAsync(Point.Value.Name);
                mapLinePoint.Name = Point.Value.Name;
                mapLinePoint.Latitude = carSignal.Gps.LatitudeDegrees;
                mapLinePoint.Longitude = carSignal.Gps.LongitudeDegrees;
                mapLinePoint.Angle = carSignal.Gps.AngleDegrees;
                mapLinePoint.Altitude = carSignal.Gps.AltitudeMeters;
                mapLinePoint.PointType = Point.Value.PointType;
                mapLinePoint.SequenceNumber = lstRoadMapExamItem.Count > 0 ? lstRoadMapExamItem.Max(x => x.Value.SequenceNumber) + 1 : 1;
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Point.Key, mapLinePoint);
                lstRoadMapExamItem.Add(mapKeyValue);
                InitRoadMap();
            }
            catch (Exception ex)
            {
                Logger.Error("MapItemClick", ex.Message);
            }


        }
        public void InitRoadMap()
        {
            try
            {
                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;

                foreach (var item in lstRoadMapExamItem)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] = item.Key;
                    dataItem["itemName"] = item.Value.Name;
                    data.Add(dataItem);
                }
                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.MapListView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });
                RoadMapListView.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Logger.Error("Map", ex.Message);
            }

        }


        //初始化考试项目地图
        public void InitExamItemMap()
        {
            //一共19个考试项目

            try
            {
                mapLinePoint = new MapLinePoint() { Name = "公交汽车", PointType = MapPointType.BusArea, SequenceNumber = 1 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.bus_station, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "学校区域", PointType = MapPointType.SchoolArea, SequenceNumber = 2 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.school, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "人行横道", PointType = MapPointType.PedestrianCrossing, SequenceNumber = 3 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.zebra, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "减速慢行", PointType = MapPointType.SlowSpeed, SequenceNumber = 4 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.slowspeed, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "路口左转", PointType = MapPointType.TurnLeft, SequenceNumber = 5 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.turn_left, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "路口右转", PointType = MapPointType.TurnRight, SequenceNumber = 6 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.turn_right, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "路口直行", PointType = MapPointType.StraightThrough, SequenceNumber = 7 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.straight_driving, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "直线行驶", PointType = MapPointType.StraightDriving, SequenceNumber = 8 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.straight_driving, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "加减档", PointType = MapPointType.ModifiedGear, SequenceNumber = 9 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.modified_gear, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "掉头", PointType = MapPointType.TurnRound, SequenceNumber = 10 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.turn_round, mapLinePoint);
                lstExamItem.Add(mapKeyValue);



                mapLinePoint = new MapLinePoint() { Name = "靠边停车", PointType = MapPointType.PullOver, SequenceNumber = 11 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.stop, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "会车", PointType = MapPointType.Meeting, SequenceNumber = 12 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.meeting, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "超车", PointType = MapPointType.Overtaking, SequenceNumber = 13 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.overtaking, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "变更车道", PointType = MapPointType.ChangeLines, SequenceNumber = 14 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.change_lines, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                //mapLinePoint = new MapLinePoint() { Name = "下一个项", PointType = MapPointType.NextItem,SequenceNumber=15 };
                //mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.next, mapLinePoint);
                //lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "环岛", PointType = MapPointType.Roundabout, SequenceNumber = 16 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.roundabout, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "限速", PointType = MapPointType.StartSpeedLimit, SequenceNumber = 17 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.speed_limit_30, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "急弯泼路", PointType = MapPointType.SharpTurn, SequenceNumber = 18 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.sharp_turn, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "掉头点", PointType = MapPointType.TurnRoundPlease, SequenceNumber = 19 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.turn_round, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "通过拱桥", PointType = MapPointType.ArchBridge, SequenceNumber = 20 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.location, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "临时停车", PointType = MapPointType.TempPark, SequenceNumber = 21 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.location, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                //mapLinePoint = new MapLinePoint() { Name = "起伏弯道", PointType = MapPointType.WavedCurve, SequenceNumber = 22 };
                //mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.location, mapLinePoint);
                //lstExamItem.Add(mapKeyValue);

                mapLinePoint = new MapLinePoint() { Name = "考试结束", PointType = MapPointType.ExamEnd, SequenceNumber = 22 };
                mapKeyValue = new KeyValuePair<int, MapLinePoint>(Resource.Drawable.location, mapLinePoint);
                lstExamItem.Add(mapKeyValue);

                var data = new List<IDictionary<string, object>>();
                IDictionary<string, object> dataItem;

                foreach (var item in lstExamItem)
                {
                    dataItem = new JavaDictionary<string, object>();
                    dataItem["itemImage"] = item.Key;
                    dataItem["itemName"] = item.Value.Name;
                    data.Add(dataItem);
                }

                SimpleAdapter simpleAdapter = new SimpleAdapter(this, data, Resource.Layout.MapGridView, new String[] { "itemImage", "itemName" }, new int[] { Resource.Id.itemImage, Resource.Id.itemName });
                mGridView.SetAdapter(simpleAdapter);
            }
            catch (Exception ex)
            {
                Logger.Error("Map", ex.Message);
            }
        }


    }
}