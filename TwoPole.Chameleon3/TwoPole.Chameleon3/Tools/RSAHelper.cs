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
            param.KeyContainerName = "2PoleChameleon3";//�ܳ����������ƣ����ּ��ܽ���һ�²��ܽ��ܳɹ�
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(key);//��Ҫ���ܵ��ַ���ת��Ϊ�ֽ�����
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//�����ܺ���ֽ�����ת��Ϊ�µļ����ֽ�����
                var encrypt = Convert.ToBase64String(encryptdata);//�����ܺ���ֽ�����ת��Ϊ�ַ���
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