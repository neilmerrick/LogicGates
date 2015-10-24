using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gates;
using LGSupport;
using System.Drawing;

namespace Components4
{
    public class Not4: LGComponent4, ILGComponent4
    {
        NotGate ng1, ng2, ng3, ng4;
        Rectangle r;

        public Not4()
        {
            input0 = new Nibble();
            output0 = new Nibble();

            input0.Deletable = false;
            input0.Draggable = false;

            output0.Deletable = false;
            output0.Draggable = false;

            ng1 = new NotGate();
            ng2 = new NotGate();
            ng3 = new NotGate();
            ng4 = new NotGate();

            Connections.CrossConnect(ng1.InputPoint, input0.OutputPoint0);
            Connections.CrossConnect(ng2.InputPoint, input0.OutputPoint1);
            Connections.CrossConnect(ng3.InputPoint, input0.OutputPoint2);
            Connections.CrossConnect(ng4.InputPoint, input0.OutputPoint3);
            Connections.CrossConnect(output0.InputPoint0, ng1.OutputPoint);
            Connections.CrossConnect(output0.InputPoint1, ng2.OutputPoint);
            Connections.CrossConnect(output0.InputPoint2, ng3.OutputPoint);
            Connections.CrossConnect(output0.InputPoint3, ng4.OutputPoint);

            this.Controls.Add(input0);
            this.Controls.Add(output0);
            this.ContextMenuStrip = cms;
        }


        public void updatelocation()
        {
            UpdateNibbleLocation(input0);
            UpdateNibbleLocation(output0);

            input0.Location = new Point(0, (this.Height - input0.Height) / 2);
            output0.Location = new Point(this.Width - output0.Width, (this.Height - output0.Height) / 2);

            input0.UpdateLocation();
            output0.UpdateLocation();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            SetSize(input0);
            SetSize(output0);
            updatelocation();

            r = new Rectangle(input0.Width / 2, 0, this.Width - input0.Width - 1, this.Height - 1);
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
            output0.DeleteConnections();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Support.p, r);
            e.Graphics.DrawString("NOT", this.Font, Support.boff, r, Support.sf());
        }

    }
}
