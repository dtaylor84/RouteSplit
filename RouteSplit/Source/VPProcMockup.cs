using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RouteSplit.Schema;

namespace RouteSplit
{
    public partial class VPProcMockup : UserControl
    {
        public RSDataSet State;

        public VPProcMockup()
        {
            InitializeComponent();
        }

        private void VPProcMockup_Load(object sender, EventArgs e)
        {
            if (State == null) return;

            foreach (var pp in State.PackPhase)
            {
                lvPackPhase.Add(new PackPhaseListViewItem(pp));
            }

            this.lvPackPhase.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);

            foreach (var vptgiConfig in State.VPTypeGroupItemConfig)
            {
                // FIXME - satanically evil
                if (!vptgiConfig.vpTypeGroupId.Equals("A")) continue;
                lvVptgiConfig.Add(new VptgiConfigListViewItem(vptgiConfig));
            }
            this.lvVptgiConfig.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
            foreach (var process in State.Process)
            {
                // FIXME - ugly
                if (process.unassigned) continue;

                lvProcess.Add(new ProcessListViewItem(process));
            }
            this.lvProcess.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);

        }
    }
}
