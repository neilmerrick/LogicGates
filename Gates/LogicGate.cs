using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using LGSupport;

namespace Gates
{
    public class LogicGate: Control
    {
        protected enum State { NOTMOVING, MOVING };
        protected State state = State.NOTMOVING;
        private Point mouseLocation;
        private Boolean draggable = true;
        private Boolean clickable = true;
        private Boolean deleteable = true;

        public delegate void DraggableEventHandler(Object sender, EventArgs e);
        public delegate void ClickableEventHandler(Object sender, EventArgs e);
        public delegate void DeleteableEventHandler(Object sender, EventArgs e);

        public event DraggableEventHandler DraggableChanged;
        public event ClickableEventHandler ClickableChanged;
        public event DeleteableEventHandler DeletableChanged;

        protected ContextMenuStrip cms;
        protected ToolStripMenuItem del;

        public LogicGate()
        {
            cms = new ContextMenuStrip();
            del = new ToolStripMenuItem();
            del.Text = "Delete";
            del.AutoSize = true;
            del.Click += new EventHandler(Delete_Click);
            cms.Items.Add(del);
        }

        protected virtual void OnDraggableChanged(EventArgs e)
        {
            if (DraggableChanged != null)
            {
                DraggableChanged(this, e);
            }
        }

        protected virtual void OnClickableChanged(EventArgs e)
        {
            if (ClickableChanged != null)
            {
                ClickableChanged(this, e);
            }
        }

        protected virtual void OnDeletableChanged(EventArgs e)
        {
            if (DeletableChanged != null)
            {
                DeletableChanged(this, e);
            }
        }

        public static readonly Size small = new Size(40, 40);
        public static readonly Size medium = new Size(60, 60);
        public static readonly Size large = new Size(80, 80);

        public static readonly float[] FontSize = { 5.75f, 8f, 10.25f };

        public Boolean Clickable { get { return clickable; } set { clickable = value; OnClickableChanged(EventArgs.Empty); } }
        public Boolean Draggable { get { return draggable; } set { draggable = value; OnDraggableChanged(EventArgs.Empty); } }

        public Boolean Deletable { get { return deleteable; } set { deleteable = value; OnDeletableChanged(EventArgs.Empty); } }
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
            if (draggable)
            {
                if (state == State.MOVING)
                {
                    Point newMouseLocation = e.Location;
                    Point moved = newMouseLocation - (Size)mouseLocation;
                    this.Location += (Size)moved;
                    this.Parent.Refresh();
                }
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
        protected void setSize(Bit bit)
        {
            if (this.Size.Equals(LogicGate.small)) bit.Size = Bit.small;
            else if (this.Size.Equals(LogicGate.medium)) bit.Size = Bit.medium;
            else bit.Size = Bit.large;
        }
        protected void UpdateBitLocation(Bit bit, int offsetX, float offsetY)
        {
            bit.Location = new Point(offsetX, (int)(this.Height * offsetY) - bit.Height / 2);
            bit.UpdateLocation(
                new PointF(this.Location.X + bit.Location.X + bit.Width / 2, this.Location.Y + bit.Location.Y + bit.Height / 2),
                new PointF(this.Location.X + bit.Location.X + bit.Width / 2, this.Location.Y + bit.Location.Y + bit.Height / 2)
                );
        }

        public virtual void DeleteConnections()
        {

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

        //Static Functions
        public static Boolean isLogicGate(object c)
        {
            return (
                c.GetType() == typeof(AndGate) || c.GetType() == typeof(OrGate) || c.GetType() == typeof(XOrGate) ||
                c.GetType() == typeof(NotGate) || c.GetType() == typeof(NorGate) || c.GetType() == typeof(NandGate)
                );
        }

        public static Boolean IsBit(object c)
        {
            return c.GetType() == typeof(Bit);
        }

        public static Boolean IsNibble(object c)
        {
            return c.GetType() == typeof(Nibble);
        }
    }
}
