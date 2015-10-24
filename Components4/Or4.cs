using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Gates;
using LGSupport;

namespace Components4
{
    public class Or4: LGComponent4, ILGComponent4
    {
        OrGate or1, or2, or3, or4;
        Rectangle r;

        public Or4()
        {
            input0 = new Nibble();
            input1 = new Nibble();
            output0 = new Nibble();

            input0.Draggable = false;
            input0.Deletable = false;
            input1.Draggable = false;
            input1.Deletable = false;
            output0.Draggable = false;
            output0.Deletable = false;

            or1 = new OrGate();
            or2 = new OrGate();
            or3 = new OrGate();
            or4 = new OrGate();

            Connections.CrossConnect(input0.OutputPoint0, or1.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint0, or1.InputPoint1);
            Connections.CrossConnect(input0.OutputPoint1, or2.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint1, or2.InputPoint1);
            Connections.CrossConnect(input0.OutputPoint2, or3.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint2, or3.InputPoint1);
            Connections.CrossConnect(input0.OutputPoint3, or4.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint3, or4.InputPoint1);
            Connections.CrossConnect(or1.OutputPoint, output0.InputPoint0);
            Connections.CrossConnect(or2.OutputPoint, output0.InputPoint1);
            Connections.CrossConnect(or3.OutputPoint, output0.InputPoint2);
            Connections.CrossConnect(or4.OutputPoint, output0.InputPoint3);

            this.Controls.Add(input0);
            this.Controls.Add(input1);
            this.Controls.Add(output0);

            this.ContextMenuStrip = cms;
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

        public override void DeleteConnections()
        {
            base.DeleteConnections();
            input0.DeleteConnections();
            input1.DeleteConnections();
            output0.DeleteConnections();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            SetSize(input0);
            SetSize(input1);
            SetSize(output0);

            updatelocation();
            r = new Rectangle(input0.Width / 2, 0, this.Width - input0.Width - 1, this.Height - 1);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Support.p, r);
            e.Graphics.DrawString("OR", this.Font, Support.boff, r, Support.sf());
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (base.state == State.MOVING)
                updatelocation();

            base.OnMouseMove(e);
        }
    }
}
