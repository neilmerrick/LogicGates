using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
//using LGSupport;

namespace LGSupport
{
    [Serializable]
    public class IOPoint
    {
        public delegate void ChangedEventHandler(object sender, EventArgs e);

        private Boolean io;
        public Boolean IO { get { return io; } set { io = value; /* CheckPoint(); */ OnChanged(EventArgs.Empty); } }

        public IOPoint(String name)
        {
            this.Name = name;
            
        }

        public void CheckPoint()
        {
            Boolean v = io;
            foreach (IOPoint iop in Connected)
            {
                v = iop.IO || v;
            }
            io = v;
        }

        public void Remove(IOPoint iop)
        {
            Connected.Remove(iop);
            if (Connected.Count == 0) IO = false;
        }

        public void Clear()
        {
            Connected.Clear();
            IO = false;
        }

        public List<IOPoint> Connected = new List<IOPoint>();
        public RectangleF Rect { get; set; }

        public PointF Location { get; set; }

        public String Name { get; set; }

        public event ChangedEventHandler Changed;

        public Boolean Connecting { get; set; }

        public object Parent { get; set; }

        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        public void onPaint(Graphics g)
        {
            //Brush b = Brushes.Black;
            Brush b = Connecting ? Support.bconn : IO ? Support.bon : Support.boff;
            g.FillEllipse(b, Rect);
        }
    }
}
