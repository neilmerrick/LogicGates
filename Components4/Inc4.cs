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
    public class Inc4: LGComponent4, ILGComponent4
    {
        Add4 add;
        Bit one;
        Rectangle r;

        public Inc4()
        {
            input0 = new Nibble();
            output0 = new Nibble();

            input0.Deletable = false;
            input0.Draggable = false;
            output0.Deletable = false;
            output0.Draggable = false;

            one = new Bit();
            add = new Add4();

            Connections.CrossConnect(input0.OutputPoint0, add.Input0.InputPoint0);
            Connections.CrossConnect(input0.OutputPoint1, add.Input0.InputPoint1);
            Connections.CrossConnect(input0.OutputPoint2, add.Input0.InputPoint2);
            Connections.CrossConnect(input0.OutputPoint3, add.Input0.InputPoint3);
            Connections.CrossConnect(one.OutputPoint, add.Input1.InputPoint1);
            Connections.CrossConnect(add.Output0.OutputPoint0, output0.InputPoint0);
            Connections.CrossConnect(add.Output0.OutputPoint1, output0.InputPoint1);
            Connections.CrossConnect(add.Output0.OutputPoint2, output0.InputPoint2);
            Connections.CrossConnect(add.Output0.OutputPoint3, output0.InputPoint3);

            one.Input = true;

            this.Controls.Add(input0);
            this.Controls.Add(output0);
            this.ContextMenuStrip = cms;
        }

        public void updatelocation()
        {
            UpdateNibbleLocation(input0);
            UpdateNibbleLocation(output0);

            input0.Location = new Point(0, (this.Height - input0.Height) / 2);
            output0.Location = new Point(this.Width - output0.Width - 1, (this.Height - output0.Height) / 2);

            input0.UpdateLocation();
            output0.UpdateLocation();
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
            e.Graphics.DrawString("INC", this.Font, Support.boff, r, Support.sf());
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (base.state == State.MOVING)
                updatelocation();
            
            base.OnMouseMove(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            SetSize(input0);
            SetSize(output0);
            updatelocation();
            r = new Rectangle(input0.Width / 2, 0, this.Width - input0.Width - 1, this.Height - 1);
        }


    }
}
