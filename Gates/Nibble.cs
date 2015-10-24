using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LGSupport;

namespace Gates
{
    public class Nibble: LogicGate
    {
        public static readonly new Size small = new Size(30, 39);   // 4 * bit.Height + 5 * (bit.Height / 2)
        public static readonly new Size medium = new Size(34, 52);  
        public static readonly new Size large = new Size(38, 65);   

        Bit[] bits = new Bit[4];
        int val;
        Rectangle r;
        int byteoffset;
        private Point offset = new Point(0, 0);

        public Point Offset { get { return offset; } set { offset = value; } }

        public int Value { get { return val; } set { val = value; while (val < 0) val += 16; while (val > 15) val -= 16; } }
        public int ByteOffset { get { return byteoffset; } set { byteoffset = value; } }


        public IOPoint InputPoint0 { get { return bits[0].InputPoint; } }
        public IOPoint InputPoint1 { get { return bits[1].InputPoint; } }
        public IOPoint InputPoint2 { get { return bits[2].InputPoint; } }
        public IOPoint InputPoint3 { get { return bits[3].InputPoint; } }
        public IOPoint OutputPoint0 { get { return bits[0].OutputPoint; } }
        public IOPoint OutputPoint1 { get { return bits[1].OutputPoint; } }
        public IOPoint OutputPoint2 { get { return bits[2].OutputPoint; } }
        public IOPoint OutputPoint3 { get { return bits[3].OutputPoint; } }

        public Nibble()
        {
            for (int n = 0; n < bits.Length; n++)
            {
                bits[n] = new Bit("InputPoint"+(n+1), "OutputPoint"+(n+1), this);
                bits[n].Name = "bit" + n;
                bits[n].Clickable = false;
                bits[n].Draggable = false;
                bits[n].Deletable = false;
            }

            this.DeletableChanged += new LogicGate.DeleteableEventHandler(Deletable_Changed);

            this.Controls.AddRange(bits);
            this.ContextMenuStrip = cms;
        }

        public void UpdateLocation()
        {

            if (this.Parent != null)
            {
                System.Reflection.MethodInfo mi = this.Parent.GetType().GetMethod("UpdateLocation");
                if (mi != null) { mi.Invoke(this.Parent, null); }
            }
            for (int n = 0; n < bits.Length; n++)
            {
                bits[n].Location = new Point(bits[n].Size.Width / 2, bits[n].Size.Height * (10 - (n * 3)) / 2);
                bits[n].UpdateLocation(
                    new PointF(
                        this.Location.X + offset.X,
                        this.Location.Y + offset.Y + bits[n].Location.Y + bits[n].Size.Height * 0.5f
                        ),
                    new PointF(
                        this.Location.X + offset.X + this.Width,
                        this.Location.Y + offset.Y + bits[n].Location.Y + bits[n].Size.Height * 0.5f
                        ),
                        false
                    );
            }
            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            for (int n = 0; n < bits.Length; n++)
            {
                if (this.Size.Equals(Nibble.small)) bits[n].Size = Bit.small;
                else if (this.Size.Equals(Nibble.medium)) bits[n].Size = Bit.medium;
                else bits[n].Size = Bit.large;
            }
            UpdateLocation();
            r = new Rectangle(bits[0].Width * 2 + 1, Size.Height / 2 - 10, 18, 20);
        }

        private void Set()
        {
            int cval = val;
            for (int n = 0; n < bits.Length; n++ )
                bits[n].Input = (cval & (int)Math.Pow(2,n)) == Math.Pow(2, n);
        }

        public void Calc()
        {
            val = 0;
            for (int n = 0; n < bits.Length; n++)
            {
                val += bits[n].Output ? (int)Math.Pow(2, n) : 0;
            }
            this.Invalidate();
        }

        public void CheckConnections()
        {
            if (InputConnections > 0)
            {
                for (int n = 0; n < bits.Length; n++)
                {
                    if (bits[n].InputPoint.Connected.Count == 0) bits[n].Input = false;
                }
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            this.Parent.Refresh();
        }

        protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            UpdateLocation();
            if (!Connections.Connecting)
            {
                if (InputConnections == 0)
                {
                    if (r.IntersectsWith(new Rectangle(e.Location, new Size(1, 1))))
                    {
                        if (Value == 15) Value = 0; else Value++;
                        Set();
                    }
                    this.Invalidate();
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.Focus();
            base.OnMouseEnter(e);
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            
            if (base.state == State.MOVING)
                UpdateLocation();
            
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (InputConnections == 0)
            {
                if (r.IntersectsWith(new Rectangle(e.Location, new Size(1, 1))))
                {
                    Value += (e.Delta / System.Windows.Forms.SystemInformation.MouseWheelScrollDelta);
                    Set();
                }
                this.Invalidate();
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.DrawRectangle(Support.p, new Rectangle(0, 0, this.Size.Width - 1, this.Size.Height - 1));
            g.DrawString(val.ToString("X1"), this.Font, Support.boff, r, Support.sf());
        }
        
        private int InputConnections
        {
            get
            {
                int cons = 0;
                foreach (Bit b in bits)
                {
                    cons += b.InputPoint.Connected.Count;
                }
                return cons;
            }
        }

        private int OutputConnections
        {
            get
            {
                int cons = 0;
                foreach (Bit b in bits)
                {
                    cons += b.OutputPoint.Connected.Count;
                }
                return cons;
            }
        }

        public override void DeleteConnections()
        {
            for (int n = 0; n < bits.Length; n++)
            {
                bits[n].DeleteConnections();
            }
        }
        private void Deletable_Changed(Object sender, EventArgs e)
        {
            if (Deletable)
            {
                del.Visible = true;
            }
            else
            {
                del.Visible = false;
            }
        }

    }
}
