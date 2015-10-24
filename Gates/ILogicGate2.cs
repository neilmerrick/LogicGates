using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Gates
{
    interface ILogicGate2
    {
        Boolean Input0 { set; }

        Boolean Input1 { set; }

        Boolean Output { get; }

        void Set();

        void UpdateLocation();

        // void DeleteConnections();
    }
}
