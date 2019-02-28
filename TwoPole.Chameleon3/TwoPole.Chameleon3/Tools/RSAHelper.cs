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
using System.Security.Cryptography;

namespace TwoPole.Chameleon3
{
   public class RSAHelper
    {
        public static string Encrypt(string key)
        {
            var param = new CspParameters();
            param.KeyContainerName = "2PoleChameleon3";//密匙容器的名称，保持加密解密一致才能解密成功
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(key);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                var encrypt = Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
                return encrypt;
            }
        }

        public static string Decrypt(string encryptString)
        {
            try
            {
                var param = new CspParameters();
                param.KeyContainerName = "2PoleChameleon3";
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
                {
                    byte[] encryptdata = Convert.FromBase64String(encryptString);
                    byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                    var decrypt = Encoding.Default.GetString(decryptdata);
                    return decrypt;
                }
            }
            catch (Exception ex)
            {

                return ex.StackTrace;
            }


        }
    }
}