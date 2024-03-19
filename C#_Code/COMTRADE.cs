using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMTRADEinXML
{
    internal class COMTRADE
    {
        //COMTRADE CFG file elements
        private string stationName;
        private string deviceName;
        private string revisionYear;
        private int analogChannelCount;
        private int digitalChannelCount;
        private int totalChannelCount;
        private double frequency;
        private int numberOfRates;
        private string startDate;
        private string startTime;
        private string triggerDate;
        private string triggerTime;
        private string datFileType;
        private double timeMultipier;
        private string timeCode;
        private string localCode;
        private string tmqCode;
        private int leapSecond;

        private List<AnalogChannel> analogChannels = new List<AnalogChannel>();
        private List<DigitalChannel> digitalChannels = new List<DigitalChannel>();

        private List<Tuple<double, int>> rates = new List<Tuple<double, int>>();

        //COMTRADE DAT file container
        private double[,] dataFrame = null;

        public COMTRADE()
        {
            //empty constructor
        }

        sealed class AnalogChannel
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

        //****************************************Global Fields Getters and Setters************************//
        //In c# the Get and Set methods can be done shorthand as shown below
        public string StationName
        {
            get => stationName;
            set => stationName = value;
        }

        public string DeviceName
        {
            get => deviceName;
            set => deviceName = value;
        }

        public string RevisionYear
        {
            get => revisionYear;
            set => revisionYear = value;
        }

        public int AnalogChannelCount
        {
            get => analogChannelCount;
            set => analogChannelCount = value;
        }

        public int DigitalChannelCount
        {
            get => digitalChannelCount;
            set => digitalChannelCount = value;
        }

        public int TotalChannelCount
        {
            get => totalChannelCount;
            set => totalChannelCount = value;
        }

        public double Frequency
        {
            get => frequency;
            set => frequency = value;
        }

        public int NumberOfRates
        {
            get => numberOfRates;
            set => numberOfRates = value;
        }

        public string StartDate
        {
            get => startDate;
            set => startDate = value;
        }

        public string StartTime
        {
            get => startTime;
            set => startTime = value;
        }

        public string TriggerDate
        {
            get => triggerDate;
            set => triggerDate = value;
        }

        public string TriggerTime
        {
            get => triggerTime;
            set => triggerTime = value;
        }

        public string DatFileType
        {
            get => datFileType; 
            set => datFileType = value;
        }

        public double TimeMultiplier
        {
            get => timeMultipier; 
            set => timeMultipier = value;
        }

        public string TimeCode
        {
            get => timeCode;
            set => timeCode = value;
        }

        public string LocalCode
        {
            get => localCode;
            set => localCode = value;
        }

        public string TmqCode
        {
            get => tmqCode;
            set => tmqCode = value;
        }

        public int LeapSecond
        {
            get => leapSecond;
            set => leapSecond = value;
        }

        /// <summary>
        /// Add an analog channel using the analog class primary constructor
        /// </summary>
        public void AddAnalogChannel()
        {
            analogChannels.Add(new AnalogChannel());
        }

        /// <summary>
        /// Add an analog channel using the analog class secondary constructor
        /// </summary>
        /// <param name="index">The analog channel index</param>
        /// <param name="identifier">The analog channel ID</param>
        /// <param name="phase">The analog channel phase</param>
        /// <param name="ccbm">The analog channel CCBM</param>
        /// <param name="units">The analog channel unit</param>
        /// <param name="multiplier">The analog channel multipier</param>
        /// <param name="adder">The analog channel offset</param>
        /// <param name="skew">The analog channel skew</param>
        /// <param name="min">The analog channel min</param>
        /// <param name="max">The analog channel max</param>
        /// <param name="primary">The analog channel primary</param>
        /// <param name="secondary">The analog channel secondary</param>
        /// <param name="p_s">The analog channel primary or secondary indicator</param>
        public void AddAnalogChannel(int index, string identifier, string phase, string ccbm, string units, double multiplier,
            double adder, double skew, double min, double max, double primary, double secondary, string p_s)
        {
            analogChannels.Add(new AnalogChannel(index, identifier, phase, ccbm, units, multiplier, adder, skew, min, max,
                primary, secondary, p_s));
        }

        /// <summary>
        /// Add a digital channel using the digital class primary constructor
        /// </summary>
        public void AddDigitalChannel()
        {
            digitalChannels.Add(new DigitalChannel());
        }

        /// <summary>
        /// Add a digital channel using the digital class secondary constructor
        /// </summary>
        /// <param name="index">The digital channel index</param>
        /// <param name="identifier">The digital channel ID</param>
        /// <param name="phase">The digital channel phase</param>
        /// <param name="ccbm">The digital channel CCBM</param>
        /// <param name="state">The digital channel normal state</param>
        public void AddDigitalChannel(int index, string identifier, string phase, string ccbm, int state)
        {
            digitalChannels.Add(new DigitalChannel(index, identifier, phase, ccbm, state));
        }

        //****************************************Analog Channel Getters*************************************************//
        public int GetAnalogChannelIndex(int index)
        {
            return analogChannels[index].Index;
        }

        public string GetAnalogChannelID(int index)
        {
            return analogChannels[index].Identifier;
        }

        public string GetAnalogChannelPhase(int index)
        {
            return analogChannels[index].Phase;
        }

        public string GetAnalogChannelCCBM(int index)
        {
            return analogChannels[index].CCbm;
        }

        public string GetAnalogChannelUnit(int index)
        {
            return analogChannels[index].Unit;
        }

        public double GetAnalogChannelMultipier(int index)
        {
            return analogChannels[index].Multiplier;
        }

        public double GetAnalogChannelOffset(int index)
        {
            return analogChannels[index].Adder;
        }

        public double GetAnalogChannelSkew(int index)
        {
            return analogChannels[index].Skew;
        }

        public double GetAnalogChannelMin(int index)
        {
            return analogChannels[index].Min;
        }

        public double GetAnalogChannelMax(int index)
        {
            return analogChannels[index].Max;
        }

        public double GetAnalogChannelPrimary(int index)
        {
            return analogChannels[index].Primary;
        }

        public double GetAnalogChannelSecondary(int index)
        {
            return analogChannels[index].Secondary;
        }

        public string GetAnalogChannelPrimarySecondaryIndicator(int index)
        {
            return analogChannels[index].P_S;
        }

        //****************************************Digital Channel Getters********************************************//
        public int GetDigitalChannelIndex(int index)
        {
            return digitalChannels[index].Index;
        }

        public string GetDigitalChannelID(int index)
        {
            return digitalChannels[index].Identifier;
        }

        public string GetDigitalChannelPhase(int index)
        {
            return digitalChannels[index].Phase;
        }

        public string GetDigitalChannelCCBM(int index)
        {
            return digitalChannels[index].CCbm;
        }

        public int GetDigitalChannelNormalState(int index)
        {
            return digitalChannels[index].State;
        }

        /// <summary>
        /// Method used to add sampling rate line (sampling rate,last sample) to COMTRADE object
        /// </summary>
        /// <param name="rate">The sampling rate.</param>
        /// <param name="lastSample">The last sample the sampling rate is used on.</param>
        public void AddRate(double rate, int lastSample)
        {
            rates.Add(new Tuple<double, int>(rate, lastSample));
        }

        /// <summary>
        /// Get a sampling rate
        /// </summary>
        /// <param name="index">The index of the sampling rate to return</param>
        /// <returns></returns>
        public double GetRate(int index)
        {
            return rates[index].Item1;
        }

        /// <summary>
        /// Get the last sample a sampling rate is used on.
        /// </summary>
        /// <param name="index">The index of the last sample number to return.</param>
        /// <returns></returns>
        public int GetLastSample(int index)
        {
            return rates[index].Item2;
        }

        /// <summary>
        /// Gets the total number of samples in the DAT file
        /// </summary>
        /// <returns>The total number of samples in the DAT file.</returns>
        public int GetTotalSamples()
        {
            if (rates.Count > 0)
            {
                return rates[rates.Count - 1].Item2;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Intialize the data container the the size needed to store the full DAT file.
        /// </summary>
        /// <param name="rows">The number of samples in the DAT file.</param>
        /// <param name="cols">2 + the total number of channels in the DAT file.</param>
        public void IntializeDataConatiner(int rows, int cols)
        {
            dataFrame = new double[rows, cols];
        }

        /// <summary>
        /// Add a value to the data container.
        /// </summary>
        /// <param name="row">The row number.</param>
        /// <param name="col">The column number.</param>
        /// <param name="value">The value to add.</param>
        public void AddValueToDataContainer(int row, int col, double value)
        {
            dataFrame[row, col] = value;
        }

        /// <summary>
        /// Gets a sample from the data container
        /// </summary>
        /// <param name="rowNumber">The sample number to return.</param>
        /// <returns>The full sample returned as an array of doubles.</returns>
        public double[] GetRow(int rowNumber)
        {
            return Enumerable.Range(0, dataFrame.GetLength(1))
                    .Select(x => dataFrame[rowNumber, x])
                    .ToArray();
        }
    }
}
