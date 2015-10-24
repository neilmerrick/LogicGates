using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gates;
using System.Diagnostics;
using LGSupport;
using System.Drawing;

namespace Components4
{
    public class And4: LGComponent4, ILGComponent4
    {
        private AndGate and0, and1, and2, and3;
        private Rectangle r;
        public And4()
        {
            input0 = new Nibble();
            input1 = new Nibble();
            output0 = new Nibble();

            input0.Deletable = false;
            input1.Deletable = false;
            output0.Deletable = false;

            input0.Draggable = false;
            input1.Draggable = false;
            output0.Draggable = false;

            and0 = new AndGate();
            and1 = new AndGate();
            and2 = new AndGate();
            and3 = new AndGate();
            
            Connections.CrossConnect(input0.OutputPoint0, and0.InputPoint0);
            Connections.CrossConnect(input0.OutputPoint1, and1.InputPoint0);
            Connections.CrossConnect(input0.OutputPoint2, and2.InputPoint0);
            Connections.CrossConnect(input0.OutputPoint3, and3.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint0, and0.InputPoint1);
            Connections.CrossConnect(input1.OutputPoint1, and1.InputPoint1);
            Connections.CrossConnect(input1.OutputPoint2, and2.InputPoint1);
            Connections.CrossConnect(input1.OutputPoint3, and3.InputPoint1);
            Connections.CrossConnect(and0.OutputPoint, output0.InputPoint0);
            Connections.CrossConnect(and1.OutputPoint, output0.InputPoint1);
            Connections.CrossConnect(and2.OutputPoint, output0.InputPoint2);
            Connections.CrossConnect(and3.OutputPoint, output0.InputPoint3);

            this.Controls.Add(input0);
            this.Controls.Add(input1);
            this.Controls.Add(output0);

            this.ContextMenuStrip = cms;
        }

        public Nibble Input0 { get { return input0; } }
        public Nibble Input1 { get { return input1; } }
        public Nibble Output { get { return output0; } }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            SetSize(input0);
            SetSize(input1);
            SetSize(output0);
            updatelocation();
            r = new Rectangle(input0.Width / 2, 0, this.Width - input0.Width - 1, this.Height - 1);
        }

        public void updatelocation()
        {
            UpdateNibbleLocation(input0);
            UpdateNibbleLocation(input1);
            UpdateNibbleLocation(output0);

            input1.Location = new Point(0, (this.Height - input0.Height - input1.Height) / 3);
            input0.Location = new Point(0, (this.Height - input0.Height - input1.Height) * 2 / 3 + input1.Height);
            output0.Location = new Point(this.Width - output0.Width, (this.Height - output0.Height) / 2);

            input0.UpdateLocation();
            input1.UpdateLocation();
            output0.UpdateLocation();
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (base.state == State.MOVING)
                updatelocation();

            base.OnMouseMove(e);
        }

        public override void DeleteConnections()
        {
            input0.DeleteConnections();
            input1.DeleteConnections();
            output0.DeleteConnections();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Support.p, r);
            e.Graphics.DrawString("AND", this.Font, Support.boff, r, Support.sf());
        }
    }
}
