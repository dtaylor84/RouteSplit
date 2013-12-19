using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using RouteSplit.Types;

namespace RouteSplit
{
    public class ProcessListViewItem : ListViewItem
    {
        private RSTProcess Process;

        public ProcessListViewItem(RSTProcess process)
        {
            this.Process = process;

            this.Text = "";
            this.SubItems.Add(process.template.werksP.werksId);
            this.SubItems.Add(process.template.templateName);
            this.SubItems.Add(process.template.text);
            this.SubItems.Add(process.shipDate.ToShortDateString());
            this.SubItems.Add(process.seqNo.ToString());
            this.SubItems.Add(process.extract ? "Y" : "N");
        }
    }

    public class ProcessListView : ListView
    {

        //private Dictionary<string,Vptyp> vptyps = new Dictionary<string,Vptyp>();
        //public string auth;

        public ProcessListView()
        {
            //this.ItemDrag  += new ItemDragEventHandler(ProcessListView_ItemDrag);
            //this.DragEnter += new DragEventHandler(ProcessListView_DragEnter);
            //this.DragDrop  += new DragEventHandler(ProcessListView_DragDrop);
        }

        private void ProcessListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //DragDropEffects dde = DoDragDrop(this.SelectedItems, DragDropEffects.Move);
        }

        private void ProcessListView_DragEnter(object sender, DragEventArgs e)
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

        private void ProcessListView_DragDrop(object sender, DragEventArgs e)
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

        public void Add(ProcessListViewItem item)
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
