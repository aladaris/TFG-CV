using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sequencer {
    class DataLogging {
        /// <summary>
        /// Error logging file name
        /// </summary>
        public static string ErrorFilePath = "error_log.txt";

        /// <summary>
        /// Write to the error log file a meesage.
        /// </summary>
        /// <param name="i_exceptName">Exception name</param>
        /// <param name="i_sender">Exception sender</param>
        /// <param name="i_text">Text message</param>
        public static void ErrorLog(string i_exceptName, string i_sender, string i_text) {
            StreamWriter log;
            if (!File.Exists(ErrorFilePath)) {
                log = new StreamWriter(ErrorFilePath);
            } else {
                log = File.AppendText(ErrorFilePath);
            }
            // Write to the file:
            log.WriteLine("#### [" + DateTime.Now+ "] ####");
            log.WriteLine("Exception:" + i_exceptName);
            log.WriteLine("Sender:" + i_sender);
            log.WriteLine("Text: " + i_text);
            // Close the stream:
            log.Close();
        }
    }
}
