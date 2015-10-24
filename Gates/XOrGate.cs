using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using LGSupport;

namespace Gates
{
    public class XOrGate : LogicGate2, ILogicGate2
    {
        private int w, h;

        public void Set()
        {
        }

        public XOrGate()
        {
            input0 = new Bit();
            input1 = new Bit();
            output = new Bit();

            input0.Draggable = false;
            input1.Draggable = false;
            output.Draggable = false;

            input0.Deletable = false;
            input1.Deletable = false;
            output.Deletable = false;

            input0.Bittype = Bit.BitType.Input;
            input1.Bittype = Bit.BitType.Input;
            output.Bittype = Bit.BitType.Output;

            input0.BitChanged += new Bit.BitChangedEventHandler(InputChanged);
            input1.BitChanged += new Bit.BitChangedEventHandler(InputChanged);

            this.Controls.Add(input0);
            this.Controls.Add(input1);
            this.Controls.Add(output);

            this.ContextMenuStrip = cms;
        }

        private void InputChanged(object sender, EventArgs e)
        {
            output.InputPoint.IO = input0.OutputPoint.IO ^ input1.OutputPoint.IO;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.DrawLine(Support.p, new PointF(0f, h * 0.33f), new PointF(w * 0.25f, h * 0.33f));
            g.DrawLine(Support.p, new PointF(0f, h * 0.67f), new PointF(w * 0.25f, h * 0.67f));
            g.DrawArc(Support.p, new RectangleF(0f, 0f, w * 0.25f, (float)(h - 1)), 270f, 180f);
            g.DrawArc(Support.p, new RectangleF(w * 0.1f, 0f, w * 0.25f, (float)(h - 1)), 270f, 180f);

            g.DrawArc(Support.p, new RectangleF(-(w * 0.3f), 0f, w * 1.05f, (float)(h - 1)), 270f, 180f);
            g.DrawLine(Support.p, new PointF(w * 0.75f, h * 0.5f), new PointF((float)w, (h * 0.5f)));

            g.DrawString("Xor", this.Font, Support.boff, this.ClientRectangle, Support.sf());
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            w = this.Size.Width;
            h = this.Size.Height;

            setSize(input0);
            setSize(input1);
            setSize(output);

            UpdateLocation();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (base.state == State.MOVING)
            {
                UpdateLocation();
            }
            base.OnMouseMove(e);
        }

        public void UpdateLocation()
        {
            UpdateBitLocation(input0, 0, 2f / 3f);
            UpdateBitLocation(input1, 0, 1f / 3f);
            UpdateBitLocation(output, this.Width - output.Width, 1f / 2f);
        }
        public override void Refresh()
        {
            base.Refresh();
            this.Parent.Refresh();
        }

        public override void DeleteConnections()
        {
            input0.DeleteConnections();
            input1.DeleteConnections();
            output.DeleteConnections();
        }
    }
}
