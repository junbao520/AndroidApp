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

namespace TwoPole.Chameleon3
{
    [Activity(Label = "ItemSettingNew")]
    public class ItemSettingNew : BaseSettingActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ItemSettingNew);
            initHeader(false);
            setMyTitle(this.GetString(Resource.String.ItemSettingStr));
            InitControl();
            // Create your application here
        }
        public void InitControl()
        {
            try
            {
                var PrepareDriving = FindViewById(Resource.Id.btn_PrepareDriving);
                var LightSimulation = FindViewById(Resource.Id.btn_SimulationLights);
                var Start = FindViewById(Resource.Id.btn_VehicleStarting);
                var PullOver = FindViewById(Resource.Id.btn_PullOver);
                var ThroughPedestrianCrossing = FindViewById(Resource.Id.btn_ThroughPedestrianCrossing);
                var ThroughBusArea = FindViewById(Resource.Id.btn_ThroughBusArea);
                var ThroughSchoolArea = FindViewById(Resource.Id.btn_ThroughSchoolArea);
                var StraightThroughIntersection = FindViewById(Resource.Id.btn_StraightThroughIntersection);
                var TurnLeft = FindViewById(Resource.Id.btn_TurnLeft);
                var TurnRight = FindViewById(Resource.Id.btn_TurnRight);
                var TurnRound = FindViewById(Resource.Id.btn_TurnRound);
                var StraightDriving = FindViewById(Resource.Id.btn_StraightDriving);
                var Roundabout = FindViewById(Resource.Id.btn_Roundabout);
                var ChangeLanes = FindViewById(Resource.Id.btn_ChangeLanes);
                var Meeting = FindViewById(Resource.Id.btn_Meeting);
                var Overtake = FindViewById(Resource.Id.btn_Overtake);
                var SharpTurn = FindViewById(Resource.Id.btn_SharpTurn);
                var ModifiedGear = FindViewById(Resource.Id.btn_ModifiedGear);
                var CommonExamItem = FindViewById(Resource.Id.btn_CommonExamItem);

                PrepareDriving.Click += ExamItemMenu_Click;
                LightSimulation.Click += ExamItemMenu_Click;
                Start.Click += ExamItemMenu_Click;
                PullOver.Click += ExamItemMenu_Click;

                ThroughPedestrianCrossing.Click += ExamItemMenu_Click;
                ThroughBusArea.Click += ExamItemMenu_Click;
                ThroughSchoolArea.Click += ExamItemMenu_Click;
                StraightThroughIntersection.Click += ExamItemMenu_Click;

                TurnLeft.Click += ExamItemMenu_Click;
                TurnRight.Click += ExamItemMenu_Click;
                TurnRound.Click += ExamItemMenu_Click;
                StraightDriving.Click += ExamItemMenu_Click;


                Roundabout.Click += ExamItemMenu_Click;
                ChangeLanes.Click += ExamItemMenu_Click;
                Meeting.Click += ExamItemMenu_Click;
                Overtake.Click += ExamItemMenu_Click;


                SharpTurn.Click += ExamItemMenu_Click;
                ModifiedGear.Click += ExamItemMenu_Click;
                CommonExamItem.Click += ExamItemMenu_Click;
              

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
           
        }

        private void ExamItemMenu_Click(object sender, EventArgs e)
        {
            var Item = (View)sender;
            string PlayVoiceText = string.Empty;
            string ActivityName = string.Empty;
            switch (Item.Id)
            {
                case Resource.Id.btn_PrepareDriving:
                    PlayVoiceText = this.GetString(Resource.String.PrepareDrivingStr);
                    ActivityName = "PrepareDrivingActivity";
                    break;
                case Resource.Id.btn_SimulationLights:
                    PlayVoiceText = this.GetString(Resource.String.SimulationLightsStr);
                    ActivityName = "SimulationLightsActivity";
                    break;
                case Resource.Id.btn_VehicleStarting:
                    PlayVoiceText = this.GetString(Resource.String.VehicleStartingStr);
                    ActivityName = "VehicleStartingActivity";
                    break;
                case Resource.Id.btn_PullOver:
                    PlayVoiceText = this.GetString(Resource.String.PullOverStr);
                    ActivityName = "PullOverActivity";
                    break;
                case Resource.Id.btn_ThroughPedestrianCrossing:
                    PlayVoiceText = this.GetString(Resource.String.ThroughPedestrianCrossingStr);
                    ActivityName = "ThroughPedestrianCrossingActivity";
                    break;
                case Resource.Id.btn_ThroughBusArea:
                    PlayVoiceText = this.GetString(Resource.String.ThroughBusAreaStr);
                    ActivityName = "ThroughBusAreaActivity";
                    break;
                case Resource.Id.btn_ThroughSchoolArea:
                    PlayVoiceText = this.GetString(Resource.String.ThroughSchoolAreaStr);
                    ActivityName = "ThroughSchoolAreaActivity";
                    break;
                case Resource.Id.btn_StraightThroughIntersection:
                    PlayVoiceText = this.GetString(Resource.String.StraightThroughIntersectionStr);
                    ActivityName = "StraightThroughIntersectionActivity";
                    break;
                case Resource.Id.btn_TurnLeft:
                    PlayVoiceText = this.GetString(Resource.String.TurnLeftStr);
                    ActivityName = "TurnLeftActivity";
                    break;
                case Resource.Id.btn_TurnRight:
                    PlayVoiceText = this.GetString(Resource.String.TurnRightStr);
                    ActivityName = "TurnRightActivity";
                    break;
                case Resource.Id.btn_TurnRound:
                    PlayVoiceText = this.GetString(Resource.String.TurnRoundStr);
                    ActivityName = "TurnRoundActivity";
                    break;
                case Resource.Id.btn_StraightDriving:
                    PlayVoiceText = this.GetString(Resource.String.StraightDrivingStr);
                    ActivityName = "StraightDrivingActivity";
                    break;
                case Resource.Id.btn_Roundabout:
                    PlayVoiceText = this.GetString(Resource.String.RoundaboutStr);
                    ActivityName = "RoundaboutActivity";
                    break;
                case Resource.Id.btn_ChangeLanes:
                    PlayVoiceText = this.GetString(Resource.String.ChangeLanesStr);
                    ActivityName = "ChangeLanesActivity";
                    break;
                case Resource.Id.btn_Meeting:
                    PlayVoiceText = this.GetString(Resource.String.MeetingStr);
                    ActivityName = "MeetingActivity";
                    break;
                case Resource.Id.btn_Overtake:
                    PlayVoiceText = this.GetString(Resource.String.OvertakeStr);
                    ActivityName = "OvertakeActivity";
                    break;
                case Resource.Id.btn_SharpTurn:
                    PlayVoiceText = this.GetString(Resource.String.SharpTurnStr);
                    ActivityName = "SharpTurnActivity";
                    break;
                case Resource.Id.btn_ModifiedGear:
                    PlayVoiceText = this.GetString(Resource.String.ModifiedGearStr);
                    ActivityName = "ModifiedGearActivity";
                    break;
                case Resource.Id.btn_CommonExamItem:
                    PlayVoiceText = this.GetString(Resource.String.CommonExamItemStr);
                    ActivityName = "CommonExamItemActivity";
                    break;
                default:
                    break;
            }
            speaker.SpeakActionVoice(PlayVoiceText);
            Intent intent = new Intent();
            Type type = Type.GetType(string.Format("TwoPole.Chameleon3.{0},TwoPole.Chameleon3", ActivityName));
            intent.SetClass(this, type);
            StartActivity(intent);

        }

     
    }
}