using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMTRADEinXML
{
    internal class Program
    {
        //COMTRADE object used to store all COMTRADE fields
        private COMTRADE myCOMTRADERecord = null;

        static void Main(string[] args)
        {
            //Get comtrade files from command line args
            string cfgFile = args[0];
            string datFile = args[1];

            //Create program class
            Program myProgram = new Program();
            myProgram.myCOMTRADERecord = new COMTRADE();

            //Create Reader class and read COMTRADE CFG file into COMTRADE object
            Reader myReader = new Reader(myProgram.myCOMTRADERecord);
            myReader.ReadCOMTRADERecord(cfgFile, datFile);

            //Create XML Writer class and use the COMTRADE object to create an XML version of a COMTRADE record
            XMLWriter myWriter = new XMLWriter(myProgram.myCOMTRADERecord);
            myWriter.CreateFile(Directory.GetCurrentDirectory() + "\\TestRecord_XML.xml");
        }
    }
}
