using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace COMTRADEinXML
{
    internal class XMLWriter
    {
        //Global vars
        private COMTRADE recordToConvert = null;
        private XElement cfgTag = null;
        private XElement datTag = null;

        /// <summary>
        /// The XMLWriter constructor is passed a COMTRADE object, this object must be populated and is used to
        /// construct the XML file, do not pass an empty object to XMLWriter.
        /// </summary>
        /// <param name="recordToConvert">A COMTRADE object holidng all relevant COMTRADE record information</param>
        public XMLWriter(COMTRADE recordToConvert) 
        {
            this.recordToConvert = recordToConvert;
        }

        /// <summary>
        /// The create CFG tag is a private method used to construct the full CFG tag.
        /// </summary>
        private void CreateCFGTag()
        {
            //First create analog channels tag
            XElement analogChannels = new XElement("Analog_Channels");
            for (int i = 0; i < recordToConvert.AnalogChannelCount; i++)
            {
                XElement analogChannel = new XElement("Analog_Channel",
                    new XElement("Index", recordToConvert.GetAnalogChannelIndex(i)),
                    new XElement("ID", recordToConvert.GetAnalogChannelIdentifier(i)),
                    new XElement("Phase", recordToConvert.GetAnalogChannelPhase(i)),
                    new XElement("CCBM", recordToConvert.GetAnalogChannelCCBM(i)),
                    new XElement("Unit", recordToConvert.GetAnalogChannelUnit(i)),
                    new XElement("Multiplier", recordToConvert.GetAnalogChannelMultiplier(i)),
                    new XElement("Offset", recordToConvert.GetAnalogChannelAdder(i)),
                    new XElement("Skew", recordToConvert.GetAnalogChannelSkew(i)),
                    new XElement("Min", recordToConvert.GetAnalogChannelMin(i)),
                    new XElement("Max", recordToConvert.GetAnalogChannelMax(i)),
                    new XElement("Primary_Ratio", recordToConvert.GetAnalogChannelPrimary(i)),
                    new XElement("Secondary_Ratio", recordToConvert.GetAnalogChannelSecondary(i)),
                    new XElement("Primary_or_Secondary_Indicator", recordToConvert.GetAnalogChannelPS(i)));
                analogChannels.Add(analogChannel);
            }


            //Next create the digital channels tag
            XElement digitalChannels = new XElement("Digital_Channel");
            for (int i = 0; i < recordToConvert.DigitalChannelCount; i++)
            {
                XElement digitalChannel = new XElement("Digital_Channel",
                    new XElement("Index", recordToConvert.GetDigitalChannelIndex(i)),
                    new XElement("ID", recordToConvert.GetDigitalChannelIdentifier(i)),
                    new XElement("Phase", recordToConvert.GetDigitalChannelPhase(i)),
                    new XElement("CCBM", recordToConvert.GetDigitalChannelCCBM(i)),
                    new XElement("Normal_State", recordToConvert.GetDigitalChannelState(i)));
                digitalChannels.Add(digitalChannel);
            }

            //Next do the sampling rate information
            XElement samplingRates = new XElement("Sampling_Rate_Information");
            XElement numberOfRates = new XElement("Number_Of_Rates", recordToConvert.NumberOfRates);
            samplingRates.Add(numberOfRates);
            for (int i = 0; i < recordToConvert.NumberOfRates; i++)
            {
                XElement samplingRate = new XElement("Sampling_Rate",
                    new XElement("Rate", recordToConvert.GetRate(i)),
                    new XElement("Last_Sample_Number", recordToConvert.GetLastSample(i)));
                samplingRates.Add(samplingRate);
            }

            //Finish composing the CFG tag by putting everything together and adding all other needed tags
            cfgTag = new XElement("CFG",
                new XElement("Station_Name", recordToConvert.StationName),
                new XElement("Device_Name", recordToConvert.DeviceName),
                new XElement("Rev_Year", recordToConvert.RevisionYear),
                new XElement("Total_Channels", recordToConvert.TotalChannelCount),
                new XElement("Total_Analog_Channels", recordToConvert.AnalogChannelCount),
                new XElement("Total_Digital_Channels", recordToConvert.DigitalChannelCount),
                analogChannels,
                digitalChannels,
                new XElement("Line_Frequency", recordToConvert.Frequency),
                samplingRates,
                new XElement("DateTime_Stamps",
                    new XElement("Start_DateTime", recordToConvert.StartDate + ' ' + recordToConvert.StartTime),
                    new XElement("Trigger_DateTime", recordToConvert.TriggerDate + ' ' + recordToConvert.TriggerTime)),
                new XElement("DAT_File_Type", recordToConvert.DatFileType.ToUpper()),
                new XElement("Time_Stamp_Multiplication_Factor", recordToConvert.TimeMultiplier),
                new XElement("Time_Information",
                    new XElement("Time_Code", recordToConvert.TimeCode),
                    new XElement("Local_Code", recordToConvert.LocalCode)),
                new XElement("Time_Quality",
                    new XElement("Time_Quality_Indicator_Code", recordToConvert.TmqCode),
                    new XElement("Leap_Second_Indicator", recordToConvert.LeapSecond))
            );
        }

        /// <summary>
        /// The create ASCII DAT tag is a private method used to construct a full ASCII dat tag.
        /// </summary>
        private void CreateASCIIDatTag()
        {
            //Local vars
            string analogValues = "";
            string digitalValues = "";
            int scanNumber = 0;
            double timeDelta = 0;
            double[] scan = null;

            //Create DAT tag
            datTag = new XElement("DAT");

            //Create Samples tag
            XElement samples = new XElement("Samples");

            //Populate samples tag with samples
            for (int i = 0; i < recordToConvert.GetTotalSamples(); i++)
            {
                //Clear analog and digital values vars
                analogValues = "";
                digitalValues = "";

                //Get the full sample
                scan = recordToConvert.GetRow(i);

                //Set the sample number and sample timestamp
                scanNumber = (int)scan[0];
                timeDelta = scan[1];
                
                //Get the analog values from the sample
                for (int j = 2; 
                    j < recordToConvert.AnalogChannelCount + 2; 
                    j++)
                {
                    if (analogValues.Length == 0) analogValues = scan[j].ToString();
                    else analogValues += "," + scan[j].ToString();
                }

                //Get the digital values from the sample
                for (int j = 2 + recordToConvert.AnalogChannelCount; 
                    j < recordToConvert.DigitalChannelCount + recordToConvert.AnalogChannelCount + 2; 
                    j++)
                {
                    if (digitalValues.Length == 0) digitalValues = scan[j].ToString();
                    else digitalValues += "," + scan[j].ToString();
                }

                //Create full sample tag
                XElement sample = new XElement("Sample",
                    new XElement("Sample_Number", scanNumber),
                    new XElement("Timestamp", timeDelta),
                    new XElement("Analog_Channel_Values", analogValues),
                    new XElement("Digital_Channel_Values", digitalValues));
                samples.Add(sample);
            }
            datTag.Add(samples);
        }


        /// <summary>
        /// The public create file method uses the CreateCFGTag and CreateASCIIDatTag methods to construct a full
        /// XML COMTRADE file.
        /// </summary>
        /// <param name="filename">The name of the outputted XML file</param>
        public void CreateFile(string filename)
        {
            //Create CFG and DAT tags
            CreateCFGTag();
            CreateASCIIDatTag();

            //Assemble full XML COMTRADE File
            XElement comtradeRecord = new XElement("COMTRADE",
                new XElement("RECORD",
                    cfgTag,
                    datTag,
                    new XElement("HDR"),
                    new XElement("INF")));

            //Save xml to file
            comtradeRecord.Save(filename);
        }
    }
}
