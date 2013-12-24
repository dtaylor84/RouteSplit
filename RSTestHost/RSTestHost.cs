using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using RouteSplit.Schema;

namespace RSTestHost
{
    public static partial class RSTestHost
    {
        private static RSDataSet State = new RSDataSet();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if false
            foreach (RSTWerks s in WerksTab) {
                Console.WriteLine("{0,4}: {1}", s.werksId, s.name1);
            }

            RSTWerks werks = WerksTab[new RSTWerksKey("310")];
            Console.WriteLine("{0,4}: {1}", werks.werksId, werks.name1);

            werks = WerksTab[werks];
            Console.WriteLine("{0,4}: {1}", werks.werksId, werks.name1);

            //IEnumerable<RSTWerks>  iWerks = werks.Where(w => w.werksId.Equals("310"));

            var query = from RSTWerks w in WerksTab
                        where w.werksId.Equals("310")
                        select w;

            foreach (RSTWerks w2 in query)
            {
                Console.WriteLine("{0,4}: {1}", w2.werksId, w2.name1);
            }
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RSForm());
        }
    }
}
