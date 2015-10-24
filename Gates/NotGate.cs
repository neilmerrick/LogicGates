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
    public class NotGate: LogicGate1, ILogicGate
    {
        private int w, h;

        public NotGate()
        {
            input = new Bit(); 
            output = new Bit();

            input.BitChanged += new Bit.BitChangedEventHandler(InputChanged);

            input.Draggable = false;
            input.Deletable = false;
            input.Bittype = Bit.BitType.Input;

            output.Draggable = false;
            output.Deletable = false;
            output.Bittype = Bit.BitType.Output;

            this.Controls.Add(input);
            this.Controls.Add(output);

            this.ContextMenuStrip = cms;
        }

        private void InputChanged(Object sender, EventArgs e)
        {
            output.InputPoint.IO = !input.OutputPoint.IO;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.DrawLine(Support.p, new PointF(w * 0.25f, 0f), new PointF(w * 0.25f, (float)h));
            g.DrawLine(Support.p, new PointF(w * 0.25f, 0f), new PointF(w * 0.75f, h * 0.5f));
            g.DrawLine(Support.p, new PointF(w * 0.25f, (float)h), new PointF(w * 0.75f, h * 0.5f));
            g.DrawArc(Support.p, new RectangleF(w * 0.75f, h * 0.45f, w * 0.1f, h * 0.1f), 0f, 360f);
            g.DrawLine(Support.p, new PointF(0, h * 0.5f), new PointF(w * 0.25f, h * 0.5f));
            g.DrawLine(Support.p, new PointF(w * 0.85f, w * 0.5f), new PointF((float)w, w * 0.5f));

            g.DrawString("Not", this.Font, Support.boff, this.ClientRectangle, Support.sf());
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            w = this.Size.Width;
            h = this.Size.Height;

            setSize(input);
            setSize(output);
            UpdateLocation();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (base.state == State.MOVING)
                UpdateLocation();
            base.OnMouseMove(e);
        }

        public void UpdateLocation()
        {
            UpdateBitLocation(input, 0, 1f / 2f);
            UpdateBitLocation(output, this.Width - output.Width, 1f / 2f);
        }

        public void Set()
        {

        }
        public override void Refresh()
        {
            base.Refresh();
            this.Parent.Refresh();
        }

        public override void DeleteConnections()
        {
            input.DeleteConnections();
            output.DeleteConnections();
        }
    }
}
