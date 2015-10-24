using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using LGSupport;
using Gates;

namespace Components4
{
    public class LGComponent4: Control
    {
        protected enum State { NOTMOVING, MOVING };
        protected State state = State.NOTMOVING;
        protected Point mouseLocation;

        public static readonly Size small = new Size(100, 108);
        public static readonly Size medium = new Size(110, 134);
        public static readonly Size large = new Size(120, 160);

        public static readonly float[] FontSize = { 6f, 8.25f, 10.5f };

        protected ContextMenuStrip cms;
        protected ToolStripMenuItem del;

        protected Nibble input0, input1, output0, output1;
        protected Bit carry, borrow;

        public LGComponent4()
        {
            cms = new ContextMenuStrip();
            del = new ToolStripMenuItem();
            del.Text = "Delete";
            del.AutoSize = true;
            del.Click += new EventHandler(Delete_Click);
            cms.Items.Add(del);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (state == State.NOTMOVING)
            {
                mouseLocation = e.Location;
                state = State.MOVING;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (state == State.MOVING)
            {
                Point newMouseLocation = e.Location;
                Point moved = newMouseLocation - (Size)mouseLocation;
                this.Location += (Size)moved;
                this.Parent.Refresh();
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (state == State.MOVING)
            {
                state = State.NOTMOVING;
            }
        }

        private void Delete_Click(Object sender, EventArgs e)
        {
            DeleteConnections();
            //if (!(Support.IsComponent(this.Parent) || Support.IsNibble(this.Parent)))
            {
                try
                {
                    System.Windows.Forms.Form p = (System.Windows.Forms.Form)this.Parent;
                    p.Controls.Remove(this);
                    p.Refresh();
                }
                catch
                {
                    // Ignore error
                }
            }
        }

        protected void UpdateNibbleLocation(Nibble nibble)
        {
            nibble.Offset = this.Location;
        }

        protected void UpdateBitLocation(Bit bit, int offsetX, float offsetY)
        {
            bit.Location = new Point(offsetX, (int)(this.Height * offsetY) - bit.Height / 2);
            bit.UpdateLocation(
                new PointF(this.Location.X + bit.Location.X + bit.Width / 2, this.Location.Y + bit.Location.Y + bit.Height / 2),
                new PointF(this.Location.X + bit.Location.X + bit.Width / 2, this.Location.Y + bit.Location.Y + bit.Height / 2),
                false
                );

        }

        protected void SetSize(Nibble nibble)
        {
            if (this.Size.Equals(LGComponent4.small)) nibble.Size = Nibble.small;
            else if (this.Size.Equals(LGComponent4.medium)) nibble.Size = Nibble.medium;
            else nibble.Size = Nibble.large;
        }

        protected void SetBitSize(Bit bit)
        {
            if (this.Size.Equals(LGComponent4.small)) bit.Size = Bit.small;
            else if (this.Size.Equals(LGComponent4.medium)) bit.Size = Bit.medium;
            else bit.Size = Bit.large;
        }

        public virtual void DeleteConnections()
        {

        }

        public override void Refresh()
        {
            base.Refresh();
            this.Parent.Refresh();
        }
    }
}
