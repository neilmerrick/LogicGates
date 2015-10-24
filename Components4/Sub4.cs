using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Components;
using Gates;
using LGSupport;
using System.Drawing;

namespace Components4
{
    public class Sub4: LGComponent4, ILGComponent4
    {
        FullSubtractor fs1, fs2, fs3, fs4;
        Rectangle r;

        public Nibble Input0 { get { return input0; } }
        public Nibble Input1 { get { return input1; } }
        public Nibble Output { get { return output0; } }
        public Bit Borrow { get { return carry; } }

        public Sub4()
        {
            input0 = new Nibble();
            input1 = new Nibble();
            output0 = new Nibble();
            borrow = new Bit();
            borrow.Bittype = Bit.BitType.Output;

            input0.Deletable = false;
            input0.Draggable = false;

            input1.Deletable = false;
            input1.Draggable = false;

            output0.Deletable = false;
            output0.Draggable = false;

            borrow.Deletable = false;
            borrow.Draggable = false;

            fs1 = new FullSubtractor();
            fs2 = new FullSubtractor();
            fs3 = new FullSubtractor();
            fs4 = new FullSubtractor();

            Connections.CrossConnect(input0.OutputPoint0, fs1.Input0);
            Connections.CrossConnect(input1.OutputPoint0, fs1.Input1);
            Connections.CrossConnect(fs1.OutputB, fs2.InputB);
            Connections.CrossConnect(input0.OutputPoint1, fs2.Input0);
            Connections.CrossConnect(input1.OutputPoint1, fs2.Input1);
            Connections.CrossConnect(fs2.OutputB, fs3.InputB);
            Connections.CrossConnect(input0.OutputPoint2, fs3.Input0);
            Connections.CrossConnect(input1.OutputPoint2, fs3.Input1);
            Connections.CrossConnect(fs3.OutputB, fs4.InputB);
            Connections.CrossConnect(input0.OutputPoint3, fs4.Input0);
            Connections.CrossConnect(input1.OutputPoint3, fs4.Input1);
            Connections.CrossConnect(fs4.OutputB, borrow.InputPoint);
            Connections.CrossConnect(fs1.OutputS, output0.InputPoint0);
            Connections.CrossConnect(fs2.OutputS, output0.InputPoint1);
            Connections.CrossConnect(fs3.OutputS, output0.InputPoint2);
            Connections.CrossConnect(fs4.OutputS, output0.InputPoint3);

            fs1.InputB.IO = false;

            this.Controls.Add(input0);
            this.Controls.Add(input1);
            this.Controls.Add(output0);
            this.Controls.Add(borrow);

            this.ContextMenuStrip = cms;
        }

        public void updatelocation()
        {
            UpdateNibbleLocation(input0);
            UpdateNibbleLocation(input1);
            UpdateNibbleLocation(output0);

            int left = this.Height - input0.Height - input1.Height;
            int right = this.Height - output0.Height - borrow.Height;
            input0.Location = new Point(0, input1.Height + left * 2 / 3);
            input1.Location = new Point(0, left / 3);
            UpdateBitLocation(borrow, this.Width - output0.Width + (output0.Width - borrow.Width) / 2 - 1, ((float)right / 3) / this.Height);
            output0.Location = new Point(this.Width - output0.Width - 1, right * 2 / 3 + borrow.Height);

            input0.UpdateLocation();
            input1.UpdateLocation();
            output0.UpdateLocation();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            SetSize(input0);
            SetSize(input1);
            SetSize(output0);
            SetBitSize(borrow);

            updatelocation();

            r = new Rectangle(this.input0.Width / 2, 0, this.Width - input0.Width - 1, this.Height - 1);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Support.p, r);
            e.Graphics.DrawString("SUB", this.Font, Support.boff, r, Support.sf());
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (base.state == State.MOVING)
                updatelocation();

            base.OnMouseMove(e);
        }

        public override void DeleteConnections()
        {
            base.DeleteConnections();
            input0.DeleteConnections();
            input1.DeleteConnections();
            output0.DeleteConnections();
            borrow.DeleteConnections();
        }
    }
}
