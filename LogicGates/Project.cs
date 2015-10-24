using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace LogicGates
{
    [Serializable]
    public class Project
    {
        public List<SerializedLogicGate> LogicGates = new List<SerializedLogicGate>();

        public Size Size { get; set; }
        public float FontSize { get; set; }
        public string FileName { get; set; }
        public Boolean IsDirty { get; set; }

        public Project()
        {
            FileName = "";
            IsDirty = false;
        }
    }

    [Serializable]
    public class SerializedLogicGate
    {
        public class Inputs
        {
            public Boolean Input;
            public Boolean IsSet;

            private Inputs()
            {

            }
            public Inputs(Boolean input, Boolean isset)
            {
                Input = input;
                IsSet = isset;
            }
        }

        public class Outputs
        {
            public List<String> ConnectedTo;

            public Outputs()
            {
                ConnectedTo = new List<String>();
            }
        }

        public String Name;
        public String Type;
        public Point Location;
        public List<Inputs> inputs = new List<Inputs>();
        public List<Outputs> outputs = new List<Outputs>();
    }
}
