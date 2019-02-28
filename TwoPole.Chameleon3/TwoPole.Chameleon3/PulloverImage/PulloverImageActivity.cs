using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;

using Android.Provider;

using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

using System.Linq;
using System.Text;


using Android.Views;

using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Domain;
using Android.Graphics.Drawables;

namespace TwoPole.Chameleon3
{
   

    [Activity(Label = "PulloverImage")]
    public class PulloverImageActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.PulloverImage);

            InitControl();
        }
        Button btnOpenCamera;
        Button btnCapture;
        Button btnCaculation;
        ImageView imageVResult;
        ImageView imageVCamera;
        TextView txtResult;

        private int Imagewidth = 240;
        private int Imageheight = 320;

        protected void InitControl()
        {
            btnOpenCamera = FindViewById<Button>(Resource.Id.btnOpenCamera);
            btnCapture = FindViewById<Button>(Resource.Id.btnCapture);
            btnCaculation = FindViewById<Button>(Resource.Id.btnCaculation);

            imageVResult = FindViewById<ImageView>(Resource.Id.imageVResult);

            imageVCamera = FindViewById<ImageView>(Resource.Id.imageVCamera);

            txtResult = FindViewById<TextView>(Resource.Id.txtResult);

            btnOpenCamera.Click += btnOpenCamera_Click;

            btnCapture.Click += btnCapture_Click;

            btnCaculation.Click += btnCaculation_Click;

        }

        protected void btnOpenCamera_Click(object sender,EventArgs e)
        { }
        protected void btnCapture_Click(object sender, EventArgs e)
        { }

        private string photodir= Android.OS.Environment.ExternalStorageDirectory + "/"+ "road01.jpg";
        File _file;
        protected void btnCaculation_Click(object sender, EventArgs e)
        {
             try
            {
                ////取SD卡根目录
                //_file = new File(Android.OS.Environment.ExternalStorageDirectory, String.Format("{0}.jpg", "road01"));       //保存路径
                //Bitmap bitmap = _file.Path.LoadAndResizeBitmap(Imagewidth, Imagewidth);
                ////没取到或不存在
                //if (bitmap == null)
                //    return;

                Bitmap bitmap = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.road01, null);
                bitmap = Bitmap.CreateScaledBitmap(bitmap, Imagewidth, Imageheight, true);

                //把不可编辑的图片重新加载为可编辑的
                Bitmap image = Bitmap.CreateBitmap(Imagewidth,Imageheight,Bitmap.Config.Argb8888);
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        int pix = bitmap.GetPixel(i,j);
                        Color mcolor = new Color(pix);
                        image.SetPixel(i,j, mcolor);
                    }

                }


                //Bitmap img = Bitmap.CreateScaledBitmap(image, Imagewidth,Imageheight,true);

            Bitmap img = image;
            RoadLineProcess processer = new RoadLineProcess();

             

            img = processer.ProcessBefore(img);
            img = processer.GrayImage(img);
            img = processer.DrawOutLine(img);
            img = processer.ProcessImage2Value(img);
            string msg = "";
                int result = 0;
            Bitmap imgDraw = Bitmap.CreateScaledBitmap(image, Imagewidth, Imageheight, true);
             img = processer.DrawLine4(img, imgDraw, ref msg, ref result);
                imageVResult.SetImageBitmap(img);


               
            txtResult.Text = string.Format("识别结果：{0}", msg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



    }


   
}