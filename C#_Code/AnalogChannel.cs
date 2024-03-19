using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMTRADEinXML
{
    internal class AnalogChannel
    {
        private int index;
        private string identifier;
        private string phase;
        private string ccbm;
        private string units;
        private double multiplier;
        private double adder;
        private double skew;
        private double min;
        private double max;
        private double primary;
        private double secondary;
        private string p_s;

        public AnalogChannel()
        {
            //empty constructor
        }

        //Secondary constructor used to pass all analog channel fields at once
        public AnalogChannel(int index, string identifier, string phase, string ccbm, string units, double multiplier,
            double adder, double skew, double min, double max, double primary, double secondary, string p_s)
        {
            this.index = index;
            this.identifier = identifier;
            this.phase = phase;
            this.ccbm = ccbm;
            this.units = units;
            this.multiplier = multiplier;
            this.adder = adder;
            this.skew = skew;
            this.min = min;
            this.max = max;
            this.primary = primary;
            this.secondary = secondary;
            this.p_s = p_s;
        }

        //****************************************Analog Channel Fields Getters and Setters************************//
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

        public string Unit
        {
            get => units;
            set => units = value;
        }

        public double Multiplier
        {
            get => multiplier;
            set => multiplier = value;
        }

        public double Adder
        {
            get => adder;
            set => adder = value;
        }

        public double Skew
        {
            get => skew;
            set => skew = value;
        }

        public double Min
        {
            get => min;
            set => min = value;
        }

        public double Max
        {
            get => max;
            set => max = value;
        }

        public double Primary
        {
            get => primary;
            set => primary = value;
        }

        public double Secondary
        {
            get => secondary;
            set => secondary = value;
        }

        public string P_S
        {
            get => p_s;
            set => p_s = value;
        }
    }
}
