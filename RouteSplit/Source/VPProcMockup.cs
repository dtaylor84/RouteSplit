using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RouteSplit.Types;

namespace RouteSplit
{
    public partial class VPProcMockup : UserControl
    {
        public RSTState State;

        public VPProcMockup()
        {
            InitializeComponent();
        }

        private void VPProcMockup_Load(object sender, EventArgs e)
        {
            if (State == null) return;

            foreach (RSTPackPhase pp in State.PackPhaseTab)
            {
                lvPackPhase.Add(new PackPhaseListViewItem(pp));
            }

            this.lvPackPhase.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);

            foreach (RSTVPTypeGroupItemConfig vptgiConfig in State.VPTypeGroupItemConfigTab)
            {
                // FIXME - satanically evil
                if (!vptgiConfig.vpTypeGroupConfig.vpTypeGroup.vpTypeGroupId.Equals("A")) continue;
                lvVptgiConfig.Add(new VptgiConfigListViewItem(vptgiConfig));
            }
            this.lvVptgiConfig.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
            foreach (RSTProcess process in State.ProcessTab)
            {
                // FIXME - ugly
                if (process.unassigned) continue;

                lvProcess.Add(new ProcessListViewItem(process));
            }
            this.lvProcess.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);

        }
    }
}
