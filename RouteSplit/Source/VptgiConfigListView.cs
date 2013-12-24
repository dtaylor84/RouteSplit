using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using RouteSplit.Schema;

namespace RouteSplit
{
    public class WerksS
    {
        public WerksS()
        {
            this.VptgiConfigs = new Dictionary<RSDataSet.VPTypeGroupItemConfigRow, RouteListViewItem>();
        }
        public Dictionary<RSDataSet.VPTypeGroupItemConfigRow, RouteListViewItem> VptgiConfigs;
        public ListViewGroup group;
    }

    public class VptgiConfigListViewItem : ListViewItem
    {
        public RSDataSet.VPTypeGroupItemConfigRow VptgiConfig { get; set; }

        public VptgiConfigListViewItem(RSDataSet.VPTypeGroupItemConfigRow vptgiConfig)
        {
            this.VptgiConfig = vptgiConfig;

            this.Text = vptgiConfig.currentSeq.ToString();
            this.SubItems.Add(vptgiConfig.vpTypeId);
            this.SubItems.Add(vptgiConfig.VPTypeGroupItemRowParent.VPTypeRow.text);
            this.SubItems.Add(vptgiConfig.currentSel ? "Y" : "N");
        }
    }

    public class VptgiConfigListView : ListView
    {

        private Dictionary<string,WerksS> WerksSes = new Dictionary<string,WerksS>();
        //public string auth;

        public VptgiConfigListView()
        {
            //this.ItemDrag  += new ItemDragEventHandler(VptgiConfigListView_ItemDrag);
            //this.DragEnter += new DragEventHandler(VptgiConfigListView_DragEnter);
            //this.DragDrop  += new DragEventHandler(VptgiConfigListView_DragDrop);
        }

        private void VptgiConfigListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //DragDropEffects dde = DoDragDrop(this.SelectedItems, DragDropEffects.Move);
        }

        private void VptgiConfigListView_DragEnter(object sender, DragEventArgs e)
        {
#if false
            try
            {
                if (e.Data.GetDataPresent(typeof(SelectedListViewItemCollection)))
                {
                    if (this.auth != null)
                    {
                        SelectedListViewItemCollection c = (SelectedListViewItemCollection)e.Data.GetData(typeof(SelectedListViewItemCollection));
                        foreach (RouteListViewItem i in c)
                        {
                            if (i.auth.CompareTo(this.auth) != 0)
                            {
                                e.Effect = DragDropEffects.None;
                                return;
                            }
                        }
                    }
                    e.Effect = DragDropEffects.Move;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
#endif
        }

        private void VptgiConfigListView_DragDrop(object sender, DragEventArgs e)
        {
#if false
            SelectedListViewItemCollection c = (SelectedListViewItemCollection)e.Data.GetData(typeof(SelectedListViewItemCollection));
  
             foreach (RouteListViewItem i in c) {
                 try
                 {
                     i.Remove();
                     Add(i);
                 } catch (Exception ex) {
                     MessageBox.Show(ex.Message);
                 }

            }
            Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            //Sort();
            e.Effect = DragDropEffects.Move;
#endif
        }

        public void Add(VptgiConfigListViewItem item)
        {
#if true
            WerksS w;

            if (!WerksSes.ContainsKey(item.VptgiConfig.werksS))
            {
                w = new WerksS();
                w.group = new ListViewGroup(item.VptgiConfig.werksS, item.VptgiConfig.werksS + ": " + item.VptgiConfig.VPTypeGroupConfigRowParent.WerksRow.name1);

                WerksSes[item.VptgiConfig.werksS] = w;
                Groups.Add(w.group);
            }
            else
            {
                w = WerksSes[item.VptgiConfig.werksS];
            }

            w.group.Items.Add(item);
#endif
            Items.Add(item);
#if false
            // XXX - without doing this it doesn't sort properly???
            this.Sorting = SortOrder.None;
            this.Sorting = SortOrder.Ascending;

            this.Sorting = SortOrder.Ascending;
#endif
        }

    }
}
