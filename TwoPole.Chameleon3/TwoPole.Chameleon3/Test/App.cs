//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//namespace TwoPole.Chameleon3
//{
//    /// <summary>
//    /// ����App�����쳣  //��������ôһЩ�뷨����һ�������������ģ���Ȼ�Ǻܾ�û�������Ү������Ҳ����ν�����һ��������Ҳ���Ǻ��ѵ������������ǱȽϵ�����˼
       //����뷨�Ѿ�ʵ���ˣ�����ͻȻ��һ���뷨��һ������������������������Ե�����׼���ɡ�
       //û��ʲô�ǲ����Խ����//
//    ///��һ��������
//    /// </summary>
//    ///����App�����쳣��ҵ��   
//   public class App:Application
//    {
//        public override void OnCreate()
//        {
//            base.OnCreate();
//            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; 
//        }
//        ������ʦ��
//        ������ʦ��
//        if it is time
//        no money,no money
//       
//        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
//        {
//
//        }

//        protected override void Dispose(bool disposing)
//        {
//            AppDomain.CurrentDomain.UnhandledException-= CurrentDomain_UnhandledException;
//            base.Dispose(disposing);
//        }
//    }
//}