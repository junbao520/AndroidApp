using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Infrastructure.Instance;
using Color = Android.Graphics.Color;
using Point = Android.Graphics.Point;

namespace TwoPole.Chameleon3
{
   public  class RoadLineProcess
    {
        //强制图片缩放
        private int Imagewidth = 240;
        private int Imageheight = 320;

       //R,G,B都在100以下则设为黑色
        private int whileLimit = 100;

       //标准15cm车道线对应的像素值40
        public  int RoadWidth = 40;

        public double pix = 2.6;

        private int pullOverRoadW = 15;
        private GlobalSettings settings { get; set; }
        protected IDataService dataService;
        public RoadLineProcess()
        {
            dataService = Singleton.GetDataService;

            settings = dataService.GetSettings();

           
            if (pullOverRoadW == 10)
            {
                RoadWidth = 27;
            }
        }
        /// <summary>
        /// R,G,B都在100以下则设为黑色
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public Bitmap ProcessBefore(Bitmap img)
       {
            //不做任何处理，原来写的预处理有些问题
           ////for (int i = 0; i < Imagewidth * 2 / 3; i++)
           ////{
           ////    for (int j = 0; j < Imageheight; j++)
           ////    {
           ////        Color pixColor = img.GetPixel(i, j);
           ////        if (pixColor.R > whileLimit && pixColor.G > whileLimit && pixColor.B > whileLimit)
           ////        {

           ////        }
           ////        else
           ////        {
           ////            img.SetPixel(i, j, Color.Black);
           ////        }
           ////    }
           ////}
           
           return img;
       }


       /// <summary>
       /// 灰度化
       /// </summary>
       /// <param name="img"></param>
       /// <returns></returns>
        public Bitmap GrayImage(Bitmap img)
        {
            //Bitmap curBitmap = img;
            Bitmap curBitmap = Bitmap.CreateBitmap(img);

            if (curBitmap != null)
            {
         
                int curColor;
                int ret;
                //二维图像数组循环  
                for (int i = 0; i < curBitmap.Width; i++)
                {
                    for (int j = 0; j < curBitmap.Height; j++)
                    {
                        try
                        {
                            //读取当前像素的RGB颜色值
                            curColor = curBitmap.GetPixel(i, j);

                            Android.Graphics.Color mColor = new Color(curColor);


                            //利用公式计算灰度值（加权平均法）
                            ret = (int)(mColor.R * 0.299 + mColor.G * 0.587 + mColor.B * 0.114);
                            //设置该点像素的灰度值，R=G=B=ret
                            curBitmap.SetPixel(i, j, Color.Rgb(ret, ret, ret));
                        }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }
                    }
                }

               
              //return curBitmap;
              
            }

            Filter filter = new Filter();
            //var img1 = new Bitmap(curBitmap, Imagewidth, Imageheight);
            var img2 = filter.ColorfulBitmapMedianFilterFunction(curBitmap, 3, false);

           return img2;
        }

       /// <summary>
       /// 画轮廓
       /// </summary>
       /// <param name="img"></param>
       /// <returns></returns>
        public Bitmap DrawOutLine(Bitmap img)
        {
            string str = "";

            //Color c1 = new Color(); //     Represents an Rgb (alpha, red, green, blue) color.
            //Color c2 = new Color();
            //Color c3 = new Color();
            //Color c4 = new Color();
            int mc1 ; //     Represents an Rgb (alpha, red, green, blue) color.
            int mc2 ;
            int mc3 ;
            int mc4 ;
            int rr, gg, bb, r1, r2, r3, r4, fxr, fyr;
            int g1, g2, g3, g4, fxg, fyg, b1, b2, b3, b4, fxb, fyb;
            Bitmap box1 = img;
            //把图片框中的图片给一个bitmap类型//     Encapsulates a GDI+ bitmap, which consists of the pixel data for a graphics
            //     image and its attributes.
            for (int i = 0; i < Imagewidth - 1; i++)
            {
                for (int j = 0; j < Imageheight - 1; j++)
                {
                    mc1 = box1.GetPixel(i, j);
                    //   GetPixel(i, j)  Gets the color of the specified pixel in this System.Drawing.Bitmap.
                    mc2 = box1.GetPixel(i + 1, j + 1);
                    mc3 = box1.GetPixel(i + 1, j);
                    mc4 = box1.GetPixel(i, j + 1);
                    Color c1 = new Color(mc1);
                    Color c2 = new Color(mc2);
                    Color c3 = new Color(mc3);
                    Color c4 = new Color(mc4);
                    r1 = c1.R;
                    r2 = c2.R;
                    r3 = c3.R;
                    r4 = c4.R;
                    fxr = r1 - r2;
                    fyr = r3 - r4;
                    rr = Math.Abs(fxr) + Math.Abs(fyr);
                    //  Math.Abs(fyr)   Returns the absolute value of a 32-bit signed integer.
                    if (rr < 0) rr = 0;
                    if (rr > 255) rr = 255;
                    g1 = c1.G;
                    g2 = c2.G;
                    g3 = c3.G;
                    g4 = c4.G;
                    fxg = g1 - g2;
                    fyg = g3 - g4;
                    gg = Math.Abs(fxg) + Math.Abs(fyg);
                    if (gg < 0) gg = 0;
                    if (gg > 255) gg = 255;
                    b1 = c1.B;
                    b2 = c2.B;
                    b3 = c3.B;
                    b4 = c4.B;
                    fxb = b1 - b2;
                    fyb = b3 - b4;
                    bb = Math.Abs(fxb) + Math.Abs(fyb);
                    if (bb < 0) bb = 0;
                    if (bb > 255) bb = 255;
                    Color cc = Color.Rgb(rr, gg, bb); //用FromRgb由颜色分量值创建Color结构
                    //str += string.Format("({0},{1},{2})", rr, gg, bb)+"\r\n";
                    //Color cc = Color.FromRgb(rr, rr, rr);//用FromRgb由颜色分量值创建Color结构.获得单色图像
                    //    Color.FromRgb(rr, gg, bb)  Creates a System.Drawing.Color structure from the specified 8-bit color values
                    //     (red, green, and blue). The alpha value is implicitly 255 (fully opaque).
                    //     Although this method allows a 32-bit value to be passed for each color component,
                    //     the value of each component is limited to 8 bits.
                    box1.SetPixel(i, j, cc);
                    //  box1.SetPixel(i, j, cc)   Sets the color of the specified pixel in this System.Drawing.Bitmap.
                    //pictureBox6.Image = box1;

                }
    

            }
            return box1;

        }

        //todo:阈值要可调
        int pulloverImageLimitValue = 11;
        /// <summary>
        /// 二值化
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public Bitmap ProcessImage2Value(Bitmap img)
       {
          
           var img1 = img;
           //设置阈值为20（自定义）
           int threshold = 15;
            //获取设置的阈值，建议：白线30，黄线15
            threshold = Convert.ToInt32(pulloverImageLimitValue);
           string str = "";
           //计算二值化
           for (int i = 0; i < img1.Width; i++)
           {

               for (int j = 0; j < img1.Height; j++)
               {
                   int pixelColor = img1.GetPixel(i, j);
                    Color mpixel = new Color(pixelColor);
                   //str += string.Format("{0},{1},{2},{3}", pixelColor.A, pixelColor.R, pixelColor.G, pixelColor.B)+"\r\n";
                   if (mpixel.R > threshold)
                       img1.SetPixel(i, j, Color.Rgb(255, 255, 255));
                   else
                       img1.SetPixel(i, j, Color.Rgb( 0, 0, 0));

               }
           }
            return img1;
       }




        //记录竖着的所有直线
        public List<Line> AllLineSet = new List<Line>();

        public List<Line> ValidLineSet = new List<Line>();
        //记录4条线的8个点
        public List<Point> linePoints = new List<Point>();
        //连续10个点
        private int validPointCount = 40;
        //连续10个点,依次相减不超过3
        private int pixRange = 4;
        //上一行和下一有白点行超过4 ，则重新记录
        private int rowRange = 4;
        //先计算4条线的斜率K，再计算与图片的上下边框的焦点，作为线段的两点
        List<Point> newLinePoints = new List<Point>();
        //记录画线的边界点
        List<Point> SpecifiedPointS = new List<Point>();

       /// <summary>
       /// 
       /// </summary>
       /// <param name="img">处理过后的图片</param>
       /// <param name="OrialImg">原图（线画在原图上）</param>
       /// <param name="msgInfo"></param>
       /// <returns></returns>
       public Bitmap DrawLine4(Bitmap map, Bitmap OrialImg, ref string msgInfo, ref int value)
       {
           try
           {


               value = 0;
               AllLineSet = new List<Line>();
               ValidLineSet = new List<Line>();
               //记录整张图，每一行白色点坐标，一半张图的进行记录
               List<List<Point>> Locations = new List<List<Point>>();

               List<List<Point>> LinesPoints = new List<List<Point>>();
               List<Point> OnelinePoint = new List<Point>();
               //记录向下遍历的白点位置
               List<int> RecordLocate = new List<int>();

               newLinePoints = new List<Point>();
                
                //var img2 = new Bitmap(map, Imagewidth, Imageheight);
                var img2 = Bitmap.CreateScaledBitmap(map, Imagewidth, Imageheight, true);
                //竖着遍历，一共4条,中间2条之间的距离，则是
                //左边两条线
                for (int i = 0; i < img2.Height; i++)
               {
                   double whitePixs = 0;
                   List<Point> points = new List<Point>();
                   for (int j = 0; j < img2.Width*2/3; j++)
                   {
                       var pix = img2.GetPixel(j, i);
                        Color tem = new Color(pix);
                       //白色
                       if (tem.A == 255 && tem.R == 255)
                       {
                           whitePixs++;
                           points.Add(new Point(j, i));
                       }
                   }
                   if (points.Count >= 1)
                       Locations.Add(points);
               }
               //未检测到车道
               if (Locations.Count < img2.Height/2)
               {
                   value = 999;
                   msgInfo = "未检测到车道";
                   return null;
               }

               int lastValue = -1;
               int invalidCount = 0;
               int OnelineMinX = 0;
               //逐行比对，连续10行相差不超过3，则取其中一个点

               #region 画第一条线

               int mCount = 0;
               int mStartCount = 0;
               for (int i = 0; i < Locations.Count - 1; i++)
               {

                   var temp = Locations[i];
                   var next = Locations[i + 1];

                   lastValue = lastValue < 0 ? next[0].X : lastValue;

                   if (Math.Abs(temp[0].X - lastValue) <= pixRange)
                   {
                       invalidCount = 0;
                       lastValue = temp[0].X;
                       mStartCount++;
                       RecordLocate.Add(i);
                       OnelinePoint.Add(temp[0]);
                   }
                   else
                   {
                       invalidCount++;
                   }

                   mCount = mStartCount > mCount ? mStartCount : mCount;


                   if (invalidCount >= rowRange)
                   {
                       OnelinePoint.Clear();
                       invalidCount = 0;
                       lastValue = -1;
                   }
                   if (RecordLocate.Count > 1 && Math.Abs(i - RecordLocate[RecordLocate.Count - 2]) > rowRange)
                   {
                       OnelinePoint.Clear();
                       invalidCount = 0;
                       lastValue = -1;
                       mStartCount = 0;
                       i = i - rowRange + 2;
                       i = i < 0 ? i + rowRange : i;
                       RecordLocate.Clear();
                   }
                   if (mStartCount >= validPointCount)
                   {

                       CalStraightLine(OnelinePoint);

                       OnelinePoint.Clear();
                       lastValue = -1;
                       RecordLocate.Clear();
                       linePoints.Add(temp[0]);
                       break;
                   }

               }



               #endregion

               #region 画第2条线(新)

               int increment = 15;
                //根据配置来
                //if (settings.PullOverRoadW == PullOverRoadWidth.Pullover10CM)
                //    increment = 10;
               if (SpecifiedPointS.Count > 0)
               {
                   Locations.Clear();
                    //去除第一条已经画过的线，重新计算图片
                    int a = img2.GetPixel(1,1);
                    Color mC = new Color(a);
                   for (int i = 0; i < Imageheight; i++)
                   {
                       for (int j = 0; j < SpecifiedPointS[i].X + increment; j++)
                       {
                           img2.SetPixel(j, i, Color.Rgb(0, 0, 0));
                       }
                   }
                   // this.pictureBox2.Image = img2;

                   for (int i = 0; i < img2.Height; i++)
                   {
                       double whitePixs = 0;
                       List<Point> points = new List<Point>();
                       for (int j = 0; j < img2.Width*2/3; j++)
                       {
                           var pix = img2.GetPixel(j, i);
                            Color tem = new Color(pix);
                           //白色
                           if (tem.A == 255 && tem.R == 255)
                           {
                               whitePixs++;
                               points.Add(new Point(j, i));
                           }
                       }
                       if (points.Count >= 1)
                           Locations.Add(points);
                   }
               }

               mStartCount = 0;
               for (int i = 0; i < Locations.Count - 1; i++)
               {

                   var temp = SpecifiedPointS.Count > 0
                       ? Locations[i].ToArray()
                       : Locations[i].ToArray().Reverse().ToArray();
                   var next = SpecifiedPointS.Count > 0
                       ? Locations[i + 1].ToArray()
                       : Locations[i + 1].ToArray().Reverse().ToArray();

                   lastValue = lastValue < 0 ? next[0].X : lastValue;

                   if (Math.Abs(temp[0].X - lastValue) <= pixRange)
                   {
                       invalidCount = 0;
                       lastValue = temp[0].X;
                       mStartCount++;
                       RecordLocate.Add(i);
                       OnelinePoint.Add(temp[0]);
                   }
                   else
                   {
                       invalidCount++;
                   }

                   mCount = mStartCount > mCount ? mStartCount : mCount;


                   if (invalidCount >= rowRange)
                   {
                       OnelinePoint.Clear();
                       invalidCount = 0;
                       lastValue = -1;
                   }
                   if (RecordLocate.Count > 1 && Math.Abs(i - RecordLocate[RecordLocate.Count - 2]) > rowRange)
                   {
                       OnelinePoint.Clear();
                       invalidCount = 0;
                       lastValue = -1;
                       mStartCount = 0;
                       i = i - rowRange + 2;
                       i = i < 0 ? i + rowRange : i;
                   }
                   if (mStartCount >= validPointCount)
                   {
                       CalStraightLine(OnelinePoint);
                       OnelinePoint.Clear();
                       lastValue = -1;
                       RecordLocate.Clear();
                       linePoints.Add(temp[0]);
                       break;
                   }

               }

               if (SpecifiedPointS.Count < 1)
               {
                   msgInfo = "未检测到车道01";
                   return null;
               }

               #endregion




               #region 画右边部分车轮廓线

               Locations.Clear();
               //左边两条线
               for (int i = 0; i < img2.Height; i++)
               {
                   double whitePixs = 0;
                   List<Point> points = new List<Point>();
                   for (int j = img2.Width*2/3; j < img2.Width; j++)
                   {
                       var pix = img2.GetPixel(j, i);
                        Color tem = new Color(pix);
                       //白色
                       if (tem.A == 255 && tem.R == 255)
                       {
                           whitePixs++;
                           points.Add(new Point(j, i));
                       }
                   }
                   if (points.Count >= 1)
                       Locations.Add(points);
               }



               mStartCount = 0;
               for (int i = 0; i < Locations.Count - 1; i++)
               {

                   var temp = Locations[i];
                   var next = Locations[i + 1];

                   lastValue = lastValue < 0 ? next[0].X : lastValue;

                   if (Math.Abs(temp[0].X - lastValue) <= pixRange)
                   {
                       invalidCount = 0;
                       lastValue = temp[0].X;
                       mStartCount++;
                       RecordLocate.Add(i);
                       OnelinePoint.Add(temp[0]);
                   }
                   else
                   {
                       invalidCount++;
                   }




                   if (invalidCount >= rowRange)
                   {
                       OnelinePoint.Clear();
                       invalidCount = 0;
                       lastValue = -1;
                   }
                   if (RecordLocate.Count > 1 && Math.Abs(i - RecordLocate[RecordLocate.Count - 2]) > rowRange)
                   {
                       OnelinePoint.Clear();
                       invalidCount = 0;
                       lastValue = -1;
                       mStartCount = 0;
                       i = i - rowRange + 2;
                       i = i < 0 ? i + rowRange : i;
                   }
                   if (mStartCount >= validPointCount)
                   {
                       CalStraightLine(OnelinePoint);
                       OnelinePoint.Clear();
                       lastValue = -1;
                       RecordLocate.Clear();
                       linePoints.Add(temp[0]);
                       break;
                   }

               }


               #endregion

               //去除车裙边阴影
               //3条线时，1-2条离太远，2-3条离太近
               if (newLinePoints.Count > 5)
               {
                   if (Math.Abs(newLinePoints[0].X - newLinePoints[2].X) > 70 &&
                       Math.Abs(newLinePoints[2].X - newLinePoints[4].X) < 30)
                   {
                       //去除第2条线
                       newLinePoints.RemoveAt(3);
                       newLinePoints.RemoveAt(2);
                   }
               }



               for (int i = 0; i < newLinePoints.Count - 1; i = i + 2)
               {
                   ValidLineSet.Add(new Line(newLinePoints[i].X, newLinePoints[i].Y, newLinePoints[i + 1].X,
                       newLinePoints[i + 1].Y));
               }


               //using (Graphics g = Graphics.FromImage(OrialImg))
               //{

               //    foreach (var line in ValidLineSet)
               //    {
               //        g.DrawLine(new Pen(Color.Red, 3), line.X1, line.Y1, line.X2, line.Y2);
               //    }

               //}


               if (ValidLineSet.Count < 2)
               {
                   msgInfo = "检测到少于两条线，不进行距离计算";
                   return null;
               }

               //计算车道到车边的距离
               var uplist = newLinePoints.Where(x => x.Y == 0).ToArray();
               var downlist = newLinePoints.Where(x => x.Y == img2.Height).ToArray();
               //计算车道像素点（15cm）,上下两端取平均值
               int width = Convert.ToInt32(((uplist[1].X - uplist[0].X) + (downlist[1].X - downlist[0].X))/2);
                ///车道的距离上下都大于50或者都小于10，则计算后的距离缩进一个车道
                int offsetDistance = 0;
                //必须检测到3条线后
                if (uplist.Length > 2 && downlist.Length > 2)
                {
                    if ((Math.Abs(uplist[1].X - uplist[0].X) > 50 && Math.Abs(downlist[1].X - downlist[0].X) > 50) ||
                        (Math.Abs(uplist[1].X - uplist[0].X) < 10 || Math.Abs(downlist[1].X - downlist[0].X) < 10))
                    {
                        //缩小10厘米
                        offsetDistance = 10;
                    }
                }
                
                //左边只有一根线的时候，设个默认值（由识别的标准车道获得40）
                int lastwidth = width;
               if (width < RoadWidth - 3 || width > RoadWidth + 3)
               {
                   lastwidth = width;
                   width = RoadWidth;
               }


               Canvas canvas=new Canvas();
                Paint paint = new Paint();

                paint.StrokeWidth=3;
                //paint.TextSize=100;
                Color mm = new Color(Color.Red);
                paint.SetARGB(255,mm.R,mm.G,mm.B);



               
                                     

                //bitmap = Bitmap.createBitmap(800, 480, Bitmap.Config.Rgb_8888); //设置位图的宽高,bitmap为透明
                //canvas = new Canvas(bitmap);
                //canvas.drawColor(Color.TRANSPARENT, PorterDuff.Mode.CLEAR);//设置为透明，画布也是透明


                //canvas.DrawLine(0, 20, 750, 200, paint);

                ////在画布上贴张小图
                //Bitmap bm = BitmapFactory.decodeResource(getResources(), R.drawable.ic_launcher);
                //canvas.drawBitmap(bm, 0, 0, paint);

               





                //画线
                if (offsetDistance > 0)
                {
                    canvas = new Canvas(OrialImg);
                    //平移10cm画线
                   
                        int temp = 0;
                        int offdis = 0;

                        foreach (var line in ValidLineSet)
                        {
                            //第2条线进行右平移
                            if (temp == 1)
                            {
                                offdis = Convert.ToInt32(offsetDistance * pix);
                            }
                        //g.DrawLine(new Pen(Color.Red, 3), line.X1 + offdis, line.Y1, line.X2+ offdis, line.Y2);
                        canvas.DrawLine(line.X1 + offdis, line.Y1, line.X2 + offdis, line.Y2, paint);
                        temp++;
                            offdis = 0;
                        

                    }
                }
                else
                {
                    //正常画线
                    canvas = new Canvas(OrialImg);

                    foreach (var line in ValidLineSet)
                        {
                        //g.DrawLine(new Pen(Color.Red, 3), line.X1, line.Y1, line.X2, line.Y2);
                        canvas.DrawLine(line.X1, line.Y1, line.X2, line.Y2,paint);
                        }

                    
                }

                //this.lblRoadWidth.Text = string.Format("当前检测到的车道宽度：{0},设定为：{1}", lastwidth, width);
                //计算车道与车身的像素点最大相差
                double upIncrement = Math.Abs(uplist[ValidLineSet.Count - 1].X - uplist[ValidLineSet.Count - 2].X);
               double downIncrement = Math.Abs(downlist[ValidLineSet.Count - 1].X - downlist[ValidLineSet.Count - 2].X);
               double maxIncrement = upIncrement > downIncrement ? upIncrement : downIncrement;

               double minIncrement = upIncrement < downIncrement ? upIncrement : downIncrement;
               //路宽15cm
               int distance = Convert.ToInt32((maxIncrement/width)* increment);
               int distance2 = Convert.ToInt32((minIncrement/width)* increment);

               //按距离宽的一边的1/4位置进行计算，梯形计算X*1/4 +Y*3/4
               var result = Convert.ToInt32((distance2 + distance*3)/4)- offsetDistance;
               ////再进行1.1倍缩放
               //result = Convert.ToInt32(result*11/10);
               value = result;
               //msgInfo = string.Format("车身到路边线,最大距离为：{0}cm，最小距离：{1}cm，结果：{2}", distance, distance2,
               //    result.ToString("N1"));
               msgInfo = string.Format("车身到路边线,距离：{0} cm", result.ToString("N1"));
               return OrialImg;
           }
           catch (Exception ex)
           {

               string msg = ex.Message;
           }

           return null;
       }



       /// <summary>
       /// 计算图片上下边线焦点
       /// </summary>
       /// <param name="points"></param>
        private void CalStraightLine(List<Point> points)
        {
            SpecifiedPointS = new List<Point>();
            float[] mX = new float[points.Count];
            float[] mY = new float[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                mX[i] = points[i].X;
                mY[i] = points[i].Y;
            }

            float k = 0;
            float b = 0;

            //LeastSquareMethod(mX, mY,ref k, ref b);
            Point Ps = points[5];
            Point Pe = points[points.Count - 2];
            k = Convert.ToSingle(Ps.Y - Pe.Y) / (Ps.X - Pe.X);
            if (Single.IsInfinity(k))
            {
                Point p1 = new Point(Ps.X, 0);
                //和下面焦点（y=图片的高）
                int y = Imageheight;

                Point p2 = new Point(Ps.X, y);
                newLinePoints.Add(p1);
                newLinePoints.Add(p2);
            }
            else
            {


                b = Pe.Y - k * Pe.X;
                //和上面（x=0）焦点
                int x = Convert.ToInt32(-b / k);
                Point p1 = new Point(x, 0);
                //和下面焦点（y=图片的高）
                int y = Imageheight;
                x = Convert.ToInt32((y - b) / k);
                Point p2 = new Point(x, y);
                newLinePoints.Add(p1);
                newLinePoints.Add(p2);
            }

            for (int i = 0; i < Imageheight; i++)
            {
                if (Single.IsInfinity(k))
                {
                    Point p = new Point(Ps.X, i);
                    SpecifiedPointS.Add(p);
                }
                else
                {
                    int x = Convert.ToInt32((i - b) / k);
                    Point p = new Point(x, i);
                    SpecifiedPointS.Add(p);
                }


            }

        }


    }

   public class Line
   {
       public Line(int x1, int y1, int x2, int y2)
       {
           X1 = x1;
           X2 = x2;
           Y1 = y1;
           Y2 = y2;
       }

       public int X1 { get; set; }
       public int Y1 { get; set; }
       public int X2 { get; set; }
       public int Y2 { get; set; }

   }



   public class Filter
   {

       //作者：aaaSoft
       //日期：2009年12月11日
       //论坛：http://www.scbeta.com/bbs
       //说明：原创文章，转载请注明出处并保留作者信息
       //===================

       /// <summary>
       /// 中值滤波算法处理
       /// </summary>
       /// <param name="bmp">原始图片</param>
       /// <param name="bmp">是否是彩色位图</param>
       /// <param name="windowRadius">过滤半径</param>
       public Bitmap ColorfulBitmapMedianFilterFunction(Bitmap srcBmp, int windowRadius, bool IsColorfulBitmap)
       {
           if (windowRadius < 1)
           {
               throw new Exception("过滤半径小于1没有意义");
           }
            //创建一个新的位图对象
            // Bitmap bmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Bitmap bmp = Bitmap.CreateScaledBitmap(srcBmp,srcBmp.Width, srcBmp.Height,true);
            //存储该图片所有点的RGB值
            byte[,] mR, mG, mB;
            int mA = -1;
           mR = new byte[srcBmp.Width, srcBmp.Height];
           if (IsColorfulBitmap)
           {
               mG = new byte[srcBmp.Width, srcBmp.Height];
               mB = new byte[srcBmp.Width, srcBmp.Height];
           }
           else
           {
               mG = mR;
               mB = mR;
           }
            mA = new Color(srcBmp.GetPixel(10,10)).A;
           for (int i = 0; i <= srcBmp.Width - 1; i++)
           {
               for (int j = 0; j <= srcBmp.Height - 1; j++)
               {
                   mR[i, j] = new Color(srcBmp.GetPixel(i, j)).R;
                   if (IsColorfulBitmap)
                   {
                       mG[i, j] = new Color(srcBmp.GetPixel(i, j)).G;
                       mB[i, j] = new Color(srcBmp.GetPixel(i, j)).B;
                   }
               }
           }

           mR = MedianFilterFunction(mR, windowRadius);
           if (IsColorfulBitmap)
           {
               mG = MedianFilterFunction(mG, windowRadius);
               mB = MedianFilterFunction(mB, windowRadius);
           }
           else
           {
               mG = mR;
               mB = mR;
           }
           for (int i = 0; i <= bmp.Width - 1; i++)
           {
               for (int j = 0; j <= bmp.Height - 1; j++)
               {
                   bmp.SetPixel(i, j, Color.Rgb(mR[i, j], mG[i, j], mB[i, j]));
               }
           }
           return bmp;
       }

       /// <summary>
       /// 对矩阵M进行中值滤波
       /// </summary>
       /// <param name="m">矩阵M</param>
       /// <param name="windowRadius">过滤半径</param>
       /// <returns>结果矩阵</returns>
       private byte[,] MedianFilterFunction(byte[,] m, int windowRadius)
       {
           int width = m.GetLength(0);
           int height = m.GetLength(1);

           byte[,] lightArray = new byte[width, height];

           //开始滤波
           for (int i = 0; i <= width - 1; i++)
           {
               for (int j = 0; j <= height - 1; j++)
               {
                   //得到过滤窗口矩形
                   Rectangle rectWindow = new Rectangle(i - windowRadius, j - windowRadius, 2 * windowRadius + 1, 2 * windowRadius + 1);
                   if (rectWindow.Left < 0) rectWindow.X = 0;
                   if (rectWindow.Top < 0) rectWindow.Y = 0;
                   if (rectWindow.Right > width - 1) rectWindow.Width = width - 1 - rectWindow.Left;
                   if (rectWindow.Bottom > height - 1) rectWindow.Height = height - 1 - rectWindow.Top;
                   //将窗口中的颜色取到列表中
                   List<byte> windowPixelColorList = new List<byte>();
                   for (int oi = rectWindow.Left; oi <= rectWindow.Right - 1; oi++)
                   {
                       for (int oj = rectWindow.Top; oj <= rectWindow.Bottom - 1; oj++)
                       {
                           windowPixelColorList.Add(m[oi, oj]);
                       }
                   }
                   //排序
                   windowPixelColorList.Sort();
                   //取中值
                   byte middleValue = 0;
                   if ((windowRadius * windowRadius) % 2 == 0)
                   {
                       //如果是偶数
                       middleValue = Convert.ToByte((windowPixelColorList[windowPixelColorList.Count / 2] + windowPixelColorList[windowPixelColorList.Count / 2 - 1]) / 2);
                   }
                   else
                   {
                       //如果是奇数
                       middleValue = windowPixelColorList[(windowPixelColorList.Count - 1) / 2];
                   }
                   //设置为中值
                   lightArray[i, j] = middleValue;
               }
           }
           return lightArray;
       }
   }
}
