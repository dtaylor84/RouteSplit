using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RouteSplit
{

    public class Vptyp
    {
        public Vptyp()
        {
            this.routes = new Dictionary<string, RouteListViewItem>();
        }
        public string name;
        public Dictionary<string, RouteListViewItem> routes;
        public ListViewGroup group;
    }

    public class RouteListViewItem : ListViewItem
    {
        public RouteListViewItem(string name, string description, string vptyp, string auth)
        {
            this.name = name;
            this.description = description;
            this.vptyp = vptyp;
            this.auth = auth;

            this.Text = name;
            this.SubItems.Add(description);
        }

        public string name;
        public string description;
        public string auth;
        public string vptyp;
    }

    public class RouteListView : ListView
    {

        private Dictionary<string,Vptyp> vptyps = new Dictionary<string,Vptyp>();
        public string auth;

        public RouteListView()
        {
            this.ItemDrag  += new ItemDragEventHandler(RouteListView_ItemDrag);
            this.DragEnter += new DragEventHandler(RouteListView_DragEnter);
            this.DragDrop  += new DragEventHandler(RouteListView_DragDrop);
        }

        private void RouteListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DragDropEffects dde = DoDragDrop(this.SelectedItems, DragDropEffects.Move);
        }

        private void RouteListView_DragEnter(object sender, DragEventArgs e)
        {
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
        }

        private void RouteListView_DragDrop(object sender, DragEventArgs e)
        {
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
        }

        public void Add(RouteListViewItem item)
        {
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
            Items.Add(item);

            // XXX - without doing this it doesn't sort properly???
            this.Sorting = SortOrder.None;
            this.Sorting = SortOrder.Ascending;

            this.Sorting = SortOrder.Ascending;
        }

    }
}
