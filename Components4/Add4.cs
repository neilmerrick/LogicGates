using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gates;
using Components;
using System.Drawing;
using LGSupport;

namespace Components4
{
    public class Add4: LGComponent4, ILGComponent4
    {
        private FullAdder fa1, fa2, fa3, fa4;
        private Rectangle r;

        public Nibble Input0 { get { return input0; } }
        public Nibble Input1 { get { return input1; } }
        public Nibble Output0 { get { return output0; } }
        public Bit Carry { get { return carry; } }
        public Add4()
        {
            input0 = new Nibble();
            input1 = new Nibble();
            output0 = new Nibble();
            carry = new Bit();

            carry.Bittype = Bit.BitType.Output;

            input0.Draggable = false;
            input1.Draggable = false;
            output0.Draggable = false;
            carry.Draggable = false;

            input0.Deletable = false;
            input1.Deletable = false;
            output0.Deletable = false;
            carry.Deletable = false;

            fa1 = new FullAdder();
            fa2 = new FullAdder();
            fa3 = new FullAdder();
            fa4 = new FullAdder();

            Connections.CrossConnect(input0.OutputPoint0, fa1.Input0);
            Connections.CrossConnect(input1.OutputPoint0, fa1.Input1);
            Connections.CrossConnect(fa1.OutputC, fa2.InputC);
            Connections.CrossConnect(input0.OutputPoint1, fa2.Input0);
            Connections.CrossConnect(input1.OutputPoint1, fa2.Input1);
            Connections.CrossConnect(fa2.OutputC, fa3.InputC);
            Connections.CrossConnect(input0.OutputPoint2, fa3.Input0);
            Connections.CrossConnect(input1.OutputPoint2, fa3.Input1);
            Connections.CrossConnect(fa3.OutputC, fa4.InputC);
            Connections.CrossConnect(input0.OutputPoint3, fa4.Input0);
            Connections.CrossConnect(input1.OutputPoint3, fa4.Input1);
            Connections.CrossConnect(fa4.OutputC, carry.InputPoint);
            Connections.CrossConnect(fa1.OutputS, output0.InputPoint0);
            Connections.CrossConnect(fa2.OutputS, output0.InputPoint1);
            Connections.CrossConnect(fa3.OutputS, output0.InputPoint2);
            Connections.CrossConnect(fa4.OutputS, output0.InputPoint3);

            fa1.InputC.IO = false;

            Controls.Add(input0);
            Controls.Add(input1);
            Controls.Add(output0);
            Controls.Add(carry);

            this.ContextMenuStrip = cms;
        }

        protected override void OnResize(EventArgs e)
        {
            SetSize(input0);
            SetSize(input1);
            SetSize(output0);
            SetBitSize(carry);

            r = new Rectangle(input0.Width / 2, 0, this.Width - input0.Width - 1, this.Height - 1);
            updatelocation();

            base.OnResize(e);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Support.p, r);
            e.Graphics.DrawString("ADD", this.Font, Support.boff, r, Support.sf());
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (base.state == State.MOVING)
            {
                updatelocation();
            }
            base.OnMouseMove(e);
        }

        public void updatelocation()
        {
            UpdateNibbleLocation(input0);
            UpdateNibbleLocation(input1);
            UpdateNibbleLocation(output0);

            int left = this.Height - input0.Height - input1.Height;
            int right = this.Height - output0.Height - carry.Height;
            input0.Location = new Point(0, input1.Height + left * 2 / 3);
            input1.Location = new Point(0, left / 3);
            UpdateBitLocation(carry, this.Width - output0.Width + (output0.Width - carry.Width) / 2 - 1, ((float)right / 3) / this.Height);
            output0.Location = new Point(this.Width - output0.Width - 1, right * 2 / 3 + carry.Height);

            input0.UpdateLocation();
            input1.UpdateLocation();
            output0.UpdateLocation();
        }
        public override void DeleteConnections()
        {
            input0.DeleteConnections();
            input1.DeleteConnections();
            output0.DeleteConnections();
            carry.DeleteConnections();
        }
    }
}
