using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using csound6netlib;

namespace CSoundBridgeTests {

    class Program {

        static Csound6Table dur1;
        static Csound6Table notes1;
        static Csound6Table enabled1;

        static void Main(string[] args) {
            //string[] a = ;
            Console.Out.WriteLine();
            CsoundHandler csHandler = new CsoundHandler(new string[]{ @"Files\sequencer.csd" });
            csHandler.Csound.ReadScore("i10 0 100\n");  //Read in a score to run for x seconds 

            csHandler.Csound.OutputChannelCallback += OnOutPutChannelData;


            Csound6NetThread perfThread = new Csound6NetThread(csHandler);
            int userInput = 1;
            Random r = new Random();
            dur1 = new Csound6Table(10, csHandler.Csound);  // Acceso a la tabla de duracciones del primer synth
            notes1 = new Csound6Table(11, csHandler.Csound);
            enabled1 = new Csound6Table(12, csHandler.Csound);

            while (userInput > 0) {
                Console.Out.Write("\nEnter a positive Integer (0 to exit):");
                string s = Console.In.ReadLine();
                if (Int32.TryParse(s, out userInput) && (userInput > 0)) {
                    dur1[userInput - 1] = r.Next(1, 5);
                }
                if (userInput == 9) {
                    foreach (var chan in csHandler.Csound.GetSoftwareBus().Channels){
                        Console.Write("\n\n" + chan.Name + "   " + chan.Type);
                    }
                }
            }
            csHandler.Done = true;
            perfThread.Join();
            while (perfThread.IsRunning) ;
            perfThread.Dispose();
        }

        static private void OnOutPutChannelData(object sender, Csound6ChannelEventArgs e) {
            Random r = new Random();
            switch (e.Name) {
                case "durIndex_1":
                    int index = 0;
                    Int32.TryParse(e.Value.ToString(), out index);
                    index++;
                    if (index >= dur1.Length)
                        index = 0;
                    //dur1[index] = r.Next(4) + 1;
                    notes1[index] = 6.00 + (double)(r.Next(1,13)/100d);
                    enabled1[index] = r.Next(0, 2);
                    Console.WriteLine(String.Format("notes1[{0}] = {1}", index, notes1[index]));
                    break;
            }
        }

    }
}
