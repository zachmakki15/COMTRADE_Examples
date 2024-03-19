using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMTRADEinXML
{
    internal class Reader
    {
        //Global vars
        private COMTRADE myCOMTRADERecord = null;

        /// <summary>
        /// The Reader constructor is passed a COMTRADE object. This object must not be populated as this class is
        /// used to populate the object vars.
        /// </summary>
        /// <param name="myCOMTRADERecord">An empty COMTRADE object</param>
        public Reader(COMTRADE myCOMTRADERecord) 
        { 
            this.myCOMTRADERecord = myCOMTRADERecord;
        }

        /// <summary>
        /// Reads the CFG file of a COMTRADE record and saves all fields to the COMTRADE object
        /// </summary>
        /// <param name="cfgFile">The name of the CFG file to read in.</param>
        private void ReadCFGFile(string cfgFile)
        {
            int lineNumber = 0;
            int analogChannelCount = 0;
            int digitalChannelCount = 0;
            //Read CFG file into COMTRADE object
            string[] cfgContents = File.ReadAllLines(cfgFile);
            foreach (string line in cfgContents)
            {
                //Line length must be greater then 0 or line is not counted as part of COMTRADE file
                if (line.Length > 0)
                {
                    //Split line into elements splitting on commas
                    string[] lineElements = line.Split(',');

                    //Keep track of line number
                    lineNumber++;
                    if (lineNumber == 1)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 3)
                        {
                            //if it does set the COMTRADE object vars
                            //(doing individual conformance checks on each element is outside the scope of this task)
                            myCOMTRADERecord.StationName = lineElements[0].Trim();
                            myCOMTRADERecord.DeviceName = lineElements[1].Trim();
                            myCOMTRADERecord.RevisionYear = lineElements[2].Trim();
                        }
                    }
                    else if (lineNumber == 2)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 3)
                        {
                            //if it does set the COMTRADE object vars
                            myCOMTRADERecord.TotalChannelCount = int.Parse(lineElements[0].Trim());

                            //remove last char from analog channel count
                            lineElements[1] = lineElements[1].Trim();
                            lineElements[1] = lineElements[1].Substring(0, lineElements[1].Length - 1);
                            myCOMTRADERecord.AnalogChannelCount = int.Parse(lineElements[1]);

                            //remove last char from digital channel count
                            lineElements[2] = lineElements[2].Trim();
                            lineElements[2] = lineElements[2].Substring(0, lineElements[2].Length - 1);
                            myCOMTRADERecord.DigitalChannelCount = int.Parse(lineElements[2]);
                        }
                    }
                    else if (lineNumber > 2 &&
                        lineNumber <= 2 + myCOMTRADERecord.AnalogChannelCount)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 13)
                        {
                            //Add analog channel
                            myCOMTRADERecord.AddAnalogChannel(int.Parse(lineElements[0].Trim()), lineElements[1].Trim(), 
                                lineElements[2].Trim(), lineElements[3].Trim(), lineElements[4].Trim(), 
                                double.Parse(lineElements[5].Trim()), double.Parse(lineElements[6].Trim()), 
                                double.Parse(lineElements[7].Trim()), double.Parse(lineElements[8].Trim()), 
                                double.Parse(lineElements[9].Trim()), double.Parse(lineElements[10].Trim()), 
                                double.Parse(lineElements[11].Trim()), lineElements[12].Trim());
                        }
                    }
                    else if (lineNumber > 2 + myCOMTRADERecord.AnalogChannelCount &&
                        lineNumber <= 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 5)
                        {
                            //Add digital channel
                            myCOMTRADERecord.AddDigitalChannel(int.Parse(lineElements[0].Trim()), 
                                lineElements[1].Trim(), lineElements[2].Trim(),
                                lineElements[3].Trim(), int.Parse(lineElements[4].Trim()));
                        }
                    }
                    else if (lineNumber == 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount + 1)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 1)
                        {
                            //set line frequency
                            if (lineElements[0].Length > 0) myCOMTRADERecord.Frequency = double.Parse(lineElements[0].Trim());
                        }
                    }
                    else if (lineNumber == 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount + 2)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 1)
                        {
                            //set number of rates
                            myCOMTRADERecord.NumberOfRates = int.Parse(lineElements[0].Trim());
                        }
                    }
                    else if (lineNumber > 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount + 2 &&
                        lineNumber <= 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount + 2 + myCOMTRADERecord.NumberOfRates)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 2)
                        {
                            //add sampling rate
                            if (lineElements[0].Length > 0 && lineElements[1].Length > 0)
                            {
                                myCOMTRADERecord.AddRate(double.Parse(lineElements[0].Trim()), int.Parse(lineElements[1].Trim()));
                            }
                        }
                    }
                    else if (lineNumber == 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount + 3 + myCOMTRADERecord.NumberOfRates)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 2)
                        {
                            //add start date and time
                            if (lineElements[0].Length > 0 && lineElements[1].Length > 0)
                            {
                                myCOMTRADERecord.StartDate = lineElements[0].Trim();
                                myCOMTRADERecord.StartTime = lineElements[1].Trim();
                            }
                        }
                    }
                    else if (lineNumber == 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount + 4 + myCOMTRADERecord.NumberOfRates)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 2)
                        {
                            //add trigger date and time
                            if (lineElements[0].Length > 0 && lineElements[1].Length > 0)
                            {
                                myCOMTRADERecord.TriggerDate = lineElements[0].Trim();
                                myCOMTRADERecord.TriggerTime = lineElements[1].Trim();
                            }
                        }
                    }
                    else if (lineNumber == 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount + 5 + myCOMTRADERecord.NumberOfRates)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 1)
                        {
                            //set dat file type
                            myCOMTRADERecord.DatFileType = lineElements[0].Trim();
                        }
                    }
                    else if (lineNumber == 2 + myCOMTRADERecord.AnalogChannelCount + myCOMTRADERecord.DigitalChannelCount + 6 + myCOMTRADERecord.NumberOfRates)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 1)
                        {
                            //set time multiplier
                            myCOMTRADERecord.TimeMultiplier = double.Parse(lineElements[0].Trim());
                        }
                    }
                    else if (lineNumber == 2 + analogChannelCount + digitalChannelCount + 7 + myCOMTRADERecord.NumberOfRates)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 2)
                        {
                            if (lineElements[0].Length > 0) myCOMTRADERecord.TimeCode = lineElements[0].Trim();
                            if (lineElements[1].Length > 0) myCOMTRADERecord.LocalCode = lineElements[1].Trim();
                        }
                    }
                    else if (lineNumber == 2 + analogChannelCount + digitalChannelCount + 8 + myCOMTRADERecord.NumberOfRates)
                    {
                        //check if line has the right amount of elements
                        if (lineElements.Length == 2)
                        {
                            if (lineElements[0].Length > 0) myCOMTRADERecord.TmqCode = lineElements[0].Trim();
                            if (lineElements[1].Length > 0) myCOMTRADERecord.LeapSecond = int.Parse(lineElements[1].Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads an ASCII DAT file of a COMTRADE record and saves all fields to the COMTRADE object.
        /// </summary>
        /// <param name="datFile">The name of the ASCII DAT file to read in.</param>
        private void ReadASCIIDatFile(string datFile)
        {
            //Get full file content
            string[] datContents = File.ReadAllLines(datFile);

            //Loop through file
            for (int i = 0; i < datContents.Length; i++)
            {
                //Split line elements on commas and save to array
                string[] lineElements = datContents[i].Split(',');

                //Add each field to the COMTRADE object data container
                for (int j = 0; j < lineElements.Length; j++)
                {
                    myCOMTRADERecord.AddValueToDataContainer(i, j, double.Parse(lineElements[j]));
                }
            }
        }

            /// <summary>
            /// Method used to get the bits from a byte.
            /// </summary>
            /// <param name="dig">The byte used to store digital channels</param>
            /// <param name="bitnumb">The bit number</param>
            /// <returns>The value of the bit in the passed byte.</returns>
            private int GetBits(byte dig, int bitnumb)
        {
            int result = 0;
            switch (bitnumb)
            {
                case 0:
                    if ((dig & 1) != 0) result = 1;
                    break;
                case 1:
                    if ((dig & 2) != 0) result = 1;
                    break;
                case 2:
                    if ((dig & 4) != 0) result = 1;
                    break;
                case 3:
                    if ((dig & 8) != 0) result = 1;
                    break;
                case 4:
                    if ((dig & 16) != 0) result = 1;
                    break;
                case 5:
                    if ((dig & 32) != 0) result = 1;
                    break;
                case 6:
                    if ((dig & 64) != 0) result = 1;
                    break;
                case 7:
                    if ((dig & 128) != 0) result = 1;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Reads a binary DAT file of a COMTRADE record and saves all fields to the COMTRADE object.
        /// </summary>
        /// <param name="datFile">The name of the binary DAT file to read in.</param>
        /// <param name="totalSamples">The total number of samples in the DAT file.</param>
        private void ReadBinaryDatFile(string datFile, int totalSamples)
        {
            //Local vars
            int timeStamp = 0;
            int sampleCount = 0;
            int sampleNumber = 0;
            int digitalValue = 0;
            int digitalCount = 0;
            short analogValue = 0;
            double digitalBytes = 0;
            byte[] sampleNumberByte = new byte[4];
            byte[] timeStampByte = new byte[4];
            byte[] analogChannels = new byte[2];
            byte[] digitalChannel = new byte[1];

            //Open file for reading
            using (FileStream fsSource = new FileStream(datFile, FileMode.Open, FileAccess.Read))
            {
                //loop until total samples reached
                while (sampleCount < totalSamples)
                {
                    //reset digital channel counter
                    digitalCount = 0;

                    //Read sample number
                    fsSource.Read(sampleNumberByte, 0, 4);
                    sampleNumber = BitConverter.ToInt32(sampleNumberByte, 0);

                    //Add sample number to data container
                    myCOMTRADERecord.AddValueToDataContainer(sampleCount, 0, sampleNumber);

                    //Read timestamp
                    fsSource.Read(timeStampByte, 0, 4);
                    timeStamp = BitConverter.ToInt32(timeStampByte, 0);

                    //Add timestamp to data container
                    myCOMTRADERecord.AddValueToDataContainer(sampleCount, 1, timeStamp);

                    //Read analog channels
                    for (int i = 0; i < myCOMTRADERecord.AnalogChannelCount; i++)
                    {
                        fsSource.Read(analogChannels, 0, 2);
                        analogValue = BitConverter.ToInt16(analogChannels, 0);
                        myCOMTRADERecord.AddValueToDataContainer(sampleCount, i + 2, analogValue);
                    }

                    //Read digital channels
                    digitalBytes = 2 * Math.Ceiling((double)myCOMTRADERecord.DigitalChannelCount / 16);
                    for (int i = 0; i < digitalBytes; i++)
                    {
                        fsSource.Read(digitalChannel, 0, 1);
                        for (int j = 0; j < 8; j++)
                        {
                            //Get the bits from the byte
                            if (digitalCount < myCOMTRADERecord.DigitalChannelCount)
                            {
                                digitalValue = GetBits(digitalChannel[0], j);
                                myCOMTRADERecord.AddValueToDataContainer(sampleCount,
                                    digitalCount + myCOMTRADERecord.AnalogChannelCount + 2, digitalValue);
                                digitalCount++;
                            }
                            else
                            {
                                //break out of loop when digital count is no longer less then total digital channels
                                break;
                            }
                        }
                    }
                    sampleCount++;
                }
            }
        }

        /// <summary>
        /// Reads a binary32 DAT file of a COMTRADE record and saves all fields to the COMTRADE object.
        /// </summary>
        /// <param name="datFile">The name of the binary32 DAT file to read in.</param>
        /// <param name="totalSamples">The total number of samples in the DAT file.</param>
        private void ReadBinary32DatFile(string datFile, int totalSamples)
        {
            //Local vars
            int timeStamp = 0;
            int sampleCount = 0;
            int sampleNumber = 0;
            int digitalValue = 0;
            int digitalCount = 0;
            int analogValue = 0;
            double digitalBytes = 0;
            byte[] sampleNumberByte = new byte[4];
            byte[] timeStampByte = new byte[4];
            byte[] analogChannels = new byte[4];
            byte[] digitalChannel = new byte[1];

            //Open file for reading
            using (FileStream fsSource = new FileStream(datFile, FileMode.Open, FileAccess.Read))
            {
                //loop until total samples reached
                while (sampleCount < totalSamples)
                {
                    //reset digital channel counter
                    digitalCount = 0;

                    //Read sample number
                    fsSource.Read(sampleNumberByte, 0, 4);
                    sampleNumber = BitConverter.ToInt32(sampleNumberByte, 0);

                    //Add sample number to data container
                    myCOMTRADERecord.AddValueToDataContainer(sampleCount, 0, sampleNumber);

                    //Read timestamp
                    fsSource.Read(timeStampByte, 0, 4);
                    timeStamp = BitConverter.ToInt32(timeStampByte, 0);

                    //Add timestamp to data container
                    myCOMTRADERecord.AddValueToDataContainer(sampleCount, 1, timeStamp);

                    //Read analog channels
                    for (int i = 0; i < myCOMTRADERecord.AnalogChannelCount; i++)
                    {
                        fsSource.Read(analogChannels, 0, 4);
                        analogValue = BitConverter.ToInt32(analogChannels, 0);
                        myCOMTRADERecord.AddValueToDataContainer(sampleCount, i + 2, analogValue);
                    }

                    //Read digital channels
                    digitalBytes = 2 * Math.Ceiling((double)myCOMTRADERecord.DigitalChannelCount / 16);
                    for (int i = 0; i < digitalBytes; i++)
                    {
                        //Read the byte
                        fsSource.Read(digitalChannel, 0, 1);
                        for (int j = 0; j < 8; j++)
                        {
                            //Get the bits from the byte
                            if (digitalCount < myCOMTRADERecord.DigitalChannelCount)
                            {
                                digitalValue = GetBits(digitalChannel[0], j);
                                myCOMTRADERecord.AddValueToDataContainer(sampleCount,
                                    digitalCount + myCOMTRADERecord.AnalogChannelCount + 2, digitalValue);
                                digitalCount++;
                            }
                            else
                            {
                                //break out of loop when digital count is no longer less then total digital channels
                                break;
                            }
                        }
                    }
                    sampleCount++;
                }
            }
        }

        /// <summary>
        /// Uses the DAT file type to determine which of the DAT file reader methods to call.
        /// </summary>
        /// <param name="datFile">The name of the DAT file to read.</param>
        private void ReadDATFile(string datFile)
        {
            //initalize data container size
            //get number of samples
            int totalSamples = myCOMTRADERecord.GetTotalSamples();
            int totalColumns = myCOMTRADERecord.TotalChannelCount + 2;
            if (totalSamples != -1)
            {
                //Create dat file container
                myCOMTRADERecord.IntializeDataConatiner(totalSamples, totalColumns);

                //I am only covering ASCII, Binary, and Binary32 data types, please feel free to add Float32
                //Read DAT file
                if (myCOMTRADERecord.DatFileType.ToUpper() == "ASCII")
                {
                    //read ASCII dat file
                    ReadASCIIDatFile(datFile);
                }
                else if (myCOMTRADERecord.DatFileType.ToUpper() == "BINARY")
                {
                    //read Binary dat file
                    ReadBinaryDatFile(datFile, totalSamples);
                }
                else if (myCOMTRADERecord.DatFileType.ToUpper() == "BINARY32")
                {
                    //read Binary32 dat file
                    ReadBinary32DatFile(datFile, totalSamples);
                }
            }
        }

        /// <summary>
        /// The public method used to read a COMTRADE record, this method takes the COMTRADE CFG and DAT files
        /// as input then calls the private reader methods to read the COMTRADE fields into the COMTRADE object.
        /// </summary>
        /// <param name="cfgFile">The name of the CFG file to read.</param>
        /// <param name="datFile">The name of the DAT file to read.</param>
        public void ReadCOMTRADERecord(string cfgFile, string datFile)
        {
            //Check to ensure COMTRADE object has been intiliazed
            if (myCOMTRADERecord != null)
            {
                //Check to ensure both files exist
                if (File.Exists(cfgFile) && File.Exists(datFile))
                {
                    //Read CFG File
                    ReadCFGFile(cfgFile);
                    ReadDATFile(datFile);
                }
            }
        }
    }
}
