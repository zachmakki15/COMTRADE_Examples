using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMTRADEinXML
{
    internal class DigitalChannel
    {
        private int index;
        private string identifier;
        private string phase;
        private string ccbm;
        private int state;

        public DigitalChannel()
        {
            //empty constructor
        }

        //Secondary constructor used to pass all digital channel fields at once
        public DigitalChannel(int index, string identifier, string phase, string ccbm, int state)
        {
            this.index = index;
            this.identifier = identifier;
            this.phase = phase;
            this.ccbm = ccbm;
            this.state = state;
        }

        //****************************************Digital Channel Fields Getters and Setters************************//
        //In c# the Get and Set methods can be done shorthand as shown below
        public int Index
        {
            get => index;
            set => index = value;
        }

        public string Identifier
        {
            get => identifier;
            set => identifier = value;
        }

        public string Phase
        {
            get => phase;
            set => phase = value;
        }

        public string CCbm
        {
            get => ccbm;
            set => ccbm = value;
        }

        public int State
        {
            get => state;
            set => state = value;
        }
    }
}
