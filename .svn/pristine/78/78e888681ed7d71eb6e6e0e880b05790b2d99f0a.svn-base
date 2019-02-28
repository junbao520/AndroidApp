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
using System.Net;

namespace TwoPole.Chameleon3
{
   public class WebClientHelper
    {
        public static string WebClientRequset(string postData, string url)
        {
            //< add key = "api_host" value = " http://chameleon3updater.2pole.com/" />
            int Count = 0;
            string RetrunMsg = string.Empty;
            while (Count <= 5)
            {
                RetrunMsg = Request(postData, url);
                if (!string.IsNullOrEmpty(RetrunMsg))
                {
                    return RetrunMsg;
                }
                Count++;
            }
            return "网络连接超时,请确认网络连接";

        }

        public static string Request(string postData, string url)
        {
            try
            {
                url = "http://chameleon3updater.2pole.com/" + url;
                var webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
               
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                byte[] buffer = webclient.UploadData(url, "POST", byteArray);
      
                var msg = Encoding.UTF8.GetString(buffer);
                return msg;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
    public class UpgradeProgramsModel
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public long ProcessingTime { get; set; }
        public UpgradeProgramsResponse Data { get; set; }
    }

    public class UpgradeProgramsRequest
    {
        public string Token { get; set; }
        public string Phone { get; set; }
    }

    public class UpgradeProgramsResponse
    {
        public string File { get; set; }
        public bool Result { get; set; }
    }

    public class UpdateFirImAPIModel {
        public string name { get; set; }
        public string version { get; set; }

        public string changelog { get; set; }

        public string versionShort { get; set; }

        public string updated_at { get; set; }
        public int build { get; set; }

        public string installUrl { get; set; }

        public string install_url { get; set; }

        public string direct_install_url { get; set; }

        public string update_url { get; set; }
    }

}