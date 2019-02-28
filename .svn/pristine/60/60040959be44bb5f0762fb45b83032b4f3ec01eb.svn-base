using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System;
using Java.Net;
using Android.Graphics.Drawables;

namespace App1Test
{

    //测试专用
    [Activity(Label = "App1Test", MainLauncher = true)]



    public class MainActivity : Activity
    {
        private string url = "https://qr.api.cli.im/qr?data=cs&level=H&transparent=false&bgcolor=%23ffffff&forecolor=%23000000&blockpixel=12&marginblock=1&logourl=&size=280&kid=cliim&key=dbf0b9f9bfcda98b665f7aa4674e46a9";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ImageView imageview = FindViewById<ImageView>(Resource.Id.imageViewQRCode);

       
            //Drawable drawable = loadImageFromNetwork(IMAGE_URL);
            //mImageView.setImageDrawable(drawable);

            Bitmap bm = getBitmap(url);

            imageview.SetImageBitmap(bm);
        }

        public Bitmap getBitmap(string path)
        {
            try
            {
                Java.Net.URL url = new Java.Net.URL(path);
                Java.Net.HttpURLConnection conn = (Java.Net.HttpURLConnection)url.OpenConnection();
                conn.ConnectTimeout = 3000;
                conn.RequestMethod = "GET";
                if (conn.ResponseCode == Java.Net.HttpStatus.Ok)
                {
                    Bitmap bitmap = BitmapFactory.DecodeStream(conn.InputStream);
                    return bitmap;
                }
                else
                {
                    var re = conn.ResponseCode;
                   //算求来
                }
            }
            catch (Exception ex)
            {
                //LogManager.WriteSpeechErrorLog("getBitmap:Error" + ex.Message);
                Toast.MakeText(this, ex.Message, ToastLength.Long);
            }
            return null;
        }
    }
}

