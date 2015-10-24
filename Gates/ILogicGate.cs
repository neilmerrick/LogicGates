using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Gates
{
    interface ILogicGate
    {
        Boolean Input { set; }

        Boolean Output { get; }

        void Set();

        void UpdateLocation();

        //void DeleteConnections();
    }
}
