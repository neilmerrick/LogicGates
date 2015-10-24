using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Gates;
using LGSupport;

namespace Components
{
    public class LGComponent: Control
    {
        protected Bit input0, input1, outputS;

        protected enum State { NOTMOVING, MOVING };
        protected State state = State.NOTMOVING;
        private Point mouseLocation;

        public static readonly Size small = new Size(40, 40);
        public static readonly Size medium = new Size(60, 60);
        public static readonly Size large = new Size(80, 80);

        public static readonly float[] FontSize = { 6f, 8.25f, 10.5f };

        protected ContextMenuStrip cms;
        protected ToolStripMenuItem del;

        public IOPoint Input0 { get { return input0.InputPoint; } }
        public IOPoint Input1 { get { return input1.InputPoint; } }
        public IOPoint OutputS { get { return outputS.OutputPoint; } }

        public LGComponent()
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

        protected void UpdateBitLocation(Bit bit, int offsetX, float offsetY)
        {
            bit.Location = new Point(offsetX, (int)(this.Height * offsetY) - bit.Height / 2);
            bit.UpdateLocation(
                new PointF(this.Location.X + bit.Location.X + bit.Width / 2, this.Location.Y + bit.Location.Y + bit.Height / 2),
                new PointF(this.Location.X + bit.Location.X + bit.Width / 2, this.Location.Y + bit.Location.Y + bit.Height / 2),
                false
                );
        }

        protected void setSize(Bit bit)
        {
            if (this.Size.Equals(LGComponent.small)) bit.Size = Bit.small;
            else if (this.Size.Equals(LGComponent.medium)) bit.Size = Bit.medium;
            else bit.Size = Bit.large;
        }

        public virtual void DeleteConnections()
        {

        }
        private void Delete_Click(Object sender, EventArgs e)
        {
            DeleteConnections();
            //if (!(Support.IsComponent(this.Parent) || Support.IsNibble(this.Parent)))
            {
                System.Windows.Forms.Form p = (System.Windows.Forms.Form)this.Parent;
                p.Controls.Remove(this);
                p.Refresh();
            }
        }
        public static Boolean IsComponent(object c)
        {
            return (c.GetType() == typeof(Components.HalfAdder) || c.GetType() == typeof(Components.FullAdder));
        }
    }
}
