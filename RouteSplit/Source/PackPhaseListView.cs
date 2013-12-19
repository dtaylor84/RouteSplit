using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using RouteSplit.Types;

namespace RouteSplit
{

    public class PackPhaseListViewItem : ListViewItem
    {
        private RSTPackPhase PackPhase;

        public PackPhaseListViewItem(RSTPackPhase packPhase)
        {
            this.PackPhase = packPhase;

            this.Text = packPhase.phase.issue.issueId;
            this.SubItems.Add(packPhase.phase.issue.text);
            this.SubItems.Add(packPhase.phase.phaseModel);
            this.SubItems.Add(packPhase.phase.phaseNo != 0 ? packPhase.phase.phaseNo.ToString() : "");
            this.SubItems.Add(packPhase.werksP.werksId);
            this.SubItems.Add(packPhase.werksP.name1);
            this.SubItems.Add(packPhase.wave.waveId);
            this.SubItems.Add(packPhase.wave.text);
            this.SubItems.Add(packPhase.active ? "Y" : "N");
        }
    }

    public class PackPhaseListView : ListView
    {

        //private Dictionary<string,Vptyp> vptyps = new Dictionary<string,Vptyp>();
        //public string auth;

        public PackPhaseListView()
        {
            //this.ItemDrag  += new ItemDragEventHandler(PackPhaseListView_ItemDrag);
            //this.DragEnter += new DragEventHandler(PackPhaseListView_DragEnter);
            //this.DragDrop  += new DragEventHandler(PackPhaseListView_DragDrop);
        }

        private void PackPhaseListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //DragDropEffects dde = DoDragDrop(this.SelectedItems, DragDropEffects.Move);
        }

        private void PackPhaseListView_DragEnter(object sender, DragEventArgs e)
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

        private void PackPhaseListView_DragDrop(object sender, DragEventArgs e)
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

        public void Add(PackPhaseListViewItem item)
        {
#if false
            Vptyp v;

            if (!vptyps.ContainsKey(item.vptyp))
            {
                v = new Vptyp();
                v.name = item.vptyp;
                v.group = new ListViewGroup(item.vptyp, "VP Type " + item.vptyp);

                vptyps[item.vptyp] = v;
                Groups.Add(v.group);
            }
            else
            {
                v = vptyps[item.vptyp];
            }

            v.group.Items.Add(item);
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
