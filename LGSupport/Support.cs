using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace LGSupport
{
    public static class Support
    {
        public static Pen p = new Pen(Color.Black);
        public static Brush boff = new SolidBrush(Color.Black);
        public static Brush bon = new SolidBrush(Color.Red);
        public static Brush bconn = new SolidBrush(Color.Blue);

        public static StringFormat sf()
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            return sf;
        }
    }
}
