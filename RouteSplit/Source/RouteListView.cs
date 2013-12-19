using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RouteSplit.Types;

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
        public RouteListViewItem(string name, string description, string vptyp, string auth, RSTVPTypeGroupItemConfig config)
        {
            this.name = name;
            this.description = description;
            this.vptyp = vptyp;
            this.auth = auth;
            this.config = config;

            this.Text = name;
            this.SubItems.Add(description);
        }

        public string name;
        public string description;
        public string auth;
        public string vptyp;
        public RSTVPTypeGroupItemConfig config;
    }

    public class RouteListView : ListView
    {

        private Dictionary<string, Vptyp> vptyps = new Dictionary<string, Vptyp>();
        public string auth;

        public RouteListView()
        {
            this.ItemDrag += new ItemDragEventHandler(RouteListView_ItemDrag);
            this.DragEnter += new DragEventHandler(RouteListView_DragEnter);
            this.DragDrop += new DragEventHandler(RouteListView_DragDrop);
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

            foreach (RouteListViewItem i in c)
            {
                try
                {
                    i.Remove();
                    Add(i);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            e.Effect = DragDropEffects.Move;
        }

        public void Add(RouteListViewItem item)
        {
            Vptyp v;

            if (!vptyps.ContainsKey(item.config.currentSeq.ToString()))
            {
                v = new Vptyp();
                v.name = item.config.currentSeq.ToString();
                v.group = new ListViewGroup(item.config.currentSeq.ToString(), "VP Type " + item.vptyp);

                vptyps[item.config.currentSeq.ToString()] = v;
                Groups.Add(v.group);
            }
            else
            {
                v = vptyps[item.config.currentSeq.ToString()];
            }

            v.group.Items.Add(item);
            Items.Add(item);

#if false
            // XXX - without doing this it doesn't sort properly (in XP)
            this.Sorting = SortOrder.None;
            this.Sorting = SortOrder.Ascending;
            this.Sorting = SortOrder.Ascending;
#endif
        }

    }
}
