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
using Android.Graphics;
using Android.Util;

namespace TwoPole.Chameleon3
{
    public class TableTextView : TextView
    {
        Paint paint = new Paint();
        public TableTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            int color = Color.ParseColor("#FF0000");
            // Îª±ß¿òÉèÖÃÑÕÉ«
            paint.Color = new Color(color);
            // paint.SetColor(color);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            //canvas.DrawLine(0, 0, this.Width - 1, 0, paint);
            //canvas.DrawLine(0, 0, 0, this.Height - 1, paint);
            //canvas.DrawLine(this.Width - 1,0,this.Width - 1,this.Height - 1,paint);
            //canvas.DrawLine(0, this.Height - 1, this.Width - 1, this.Height - 1, paint);

            canvas.DrawLine(0, 0, this.Width-1, 0, paint);
            canvas.DrawLine(1,0, 1, this.Height, paint);
            canvas.DrawLine(this.Width, 0, this.Width, this.Height, paint);
            canvas.DrawLine(0, this.Height-1, this.Width-1, this.Height-1, paint);
        }

    }
}