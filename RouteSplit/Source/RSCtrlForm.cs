/****************************** Module Header ******************************\
* Module Name:  RouteSplitCtrl.cs
* Project:      RouteSplit
\***************************************************************************/

#region Using directives
using System;
using System.Collections; 
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using System.Security.Permissions;
using SAPDataProvider;
using SAPTableFactoryCtrl;
using System.Data.OleDb;

using c = RouteSplit.BuildConstants;
using RouteSplit.Types;

#endregion

namespace RouteSplit
{
    public partial class RSCtrlForm : UserControl
    {

        #region Initialization

        public RSTState State { get; set; }

        public RSCtrlForm()
        {
            InitializeComponent();

            this.plants = new Dictionary<string,Plant>();
            this.packProcesses = new Dictionary<int,PackProcess>();
            this.splitContainers = new List<SplitContainer>();
        }

        #endregion


        #region Properties


        private class Plant 
        {
            public Plant()
            {
                this.vptyps = new Dictionary<string,Vptyp>();
            }
            public string name;
            public Panel panel;
            public RouteListView listView;

            public Dictionary<string, Vptyp> vptyps; 
        }

        private class PackProcess
        {
            public PackProcess()
            {
            }
            public string name;
            public RouteListView listView;
        }

        private Dictionary<string, Plant> plants;
        private Dictionary<int, PackProcess> packProcesses;
        private List<SplitContainer> splitContainers;

        #endregion


        #region Methods

        public void InitDemo()
        {
            //MessageBox.Show("Hello!");

            plants.Clear();

            //listBox1.BeginUpdate();
            //listBox1.Items.Clear();

            try
            {
                Hashtable hash = new Hashtable();
                Control.ControlCollection cc;
                SplitContainer splitContainer = null;
                cc = splitContainer1.Panel1.Controls;

                foreach (RSTRoute r in State.RouteTab)
                {
                    // FIXME ugly mess of data models /views / ugh

                    string strWerksId = r.werksS.werksId;
                    string strWerksName1 = r.werksS.name1;
                    string strVpType = r.vpType.vpTypeId + " - " + r.vpType.text;
                    string strShortRoute = r.routeId.Substring(3, 3); // XXX
                    string strRouteText = r.text; //.Substring(7);

                    /* MultiSelListBox code */
                    //listBox1.Items.Add(strShortRoute + ": " + strRouteText);

                    /* Shiny new dynamic listview code */

                    Plant p;

                    if (!plants.ContainsKey(strWerksId))
                    {
                        splitContainer = new SplitContainer();
                        splitContainer.Dock = DockStyle.Fill;
                        cc.Add(splitContainer);
                        cc = splitContainer.Panel2.Controls;

                        p = new Plant();
                        p.name = strWerksId;
                        //splitContainer.SplitterDistance = 200;
                        p.panel = splitContainer.Panel1;

                        p.listView = new RouteListView();
                        p.listView.auth = strWerksId;
                        p.listView.HideSelection = false;
                        p.listView.Sorting = SortOrder.Ascending;
                        p.listView.AllowDrop = true;
                        p.listView.Columns.Add("Route");
                        p.listView.Columns.Add("Description");
                        p.listView.View = View.Details;
                        p.listView.Dock = DockStyle.Fill;
                        p.panel.Controls.Add(p.listView);
                        plants[strWerksId] = p;

                        Label lbl = new Label();
                        lbl.Text = strWerksId + " " + strWerksName1;
                        lbl.Dock = DockStyle.Top;
                        p.panel.Controls.Add(lbl);

                        splitContainers.Add(splitContainer);
                    }
                    else
                    {
                        p = plants[strWerksId];
                    }

                    // FIXME - oh god, fixme.
                    RouteListViewItem i = new RouteListViewItem(strShortRoute, strRouteText, strVpType, strWerksId,
                        State.VPTypeGroupItemConfigTab
                            [
                                new RSTVPTypeGroupItemConfigKey(
                                    State.VPTypeGroupConfigTab[
                                        new RSTVPTypeGroupConfigKey(
                                            State.WerksTab[new RSTWerksKey(r.werksS.werksId)],
                                            State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")]
                                        )
                                    ],
                                    State.VPTypeTab[new RSTVPTypeKey(r.vpType.vpTypeId)]
                                )
                            ]
                    );
                    //i.BackColor = Color.FromArgb(255, ((16 * p.listView.Items.Count) % 255), ((16 * p.listView.Items.Count) % 255));
                    if (r.vpType.dummyType) i.BackColor = Color.FromArgb(255, 255, 0, 0);
                    p.listView.Add(i);


                }
                if (splitContainer != null)
                {
                    Control parent = splitContainer.Parent;
                    parent.Controls.Remove(splitContainer);
                    splitContainers.Remove(splitContainer);

                    for (int i = splitContainer.Panel1.Controls.Count; i > 0; i--)
                    {
                        Control control = splitContainer.Panel1.Controls[0];
                        control.Parent.Controls.Remove(control);
                        parent.Controls.Add(control);
                    }

                    splitContainer.Dispose();
                    splitContainer = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Caught:" + ex.Message);
            }
            //listBox1.EndUpdate();

            this.Dock = DockStyle.Fill;
            //this.Padding = new Padding(0, 0, 16, 16);

            Control.ControlCollection cc2;
            SplitContainer splitContainer2 = null;
            cc2 = splitContainer1.Panel2.Controls;

            int processIdx = 0;
            foreach (RSTProcess process in State.ProcessTab)
            {
                // XXX - these should be enumerated in a specific order

                // XXX - this is a horrible way to split dummy/non-dummy processes
                if (process.unassigned) continue;

                PackProcess pk = new PackProcess();

                splitContainer2 = new SplitContainer();
                splitContainer2.Dock = DockStyle.Fill;
                cc2.Add(splitContainer2);
                cc2 = splitContainer2.Panel2.Controls;


                pk.listView = new RouteListView();
                pk.name = process.template.werksP.werksId + " " + process.template.werksP.name1 + ": " + process.template.templateName + " " + process.shipDate.ToShortDateString() + " #" + process.seqNo.ToString();
                pk.listView.Sorting = SortOrder.Ascending;
                pk.listView.Dock = DockStyle.Fill;
                pk.listView.View = View.Details;
                pk.listView.AllowDrop = true;
                pk.listView.Columns.Add("Route");
                pk.listView.Columns.Add("Description").AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                splitContainer2.Panel1.Controls.Add(pk.listView);

                packProcesses[processIdx++] = pk;

                Label lbl = new Label();
                lbl.Dock = DockStyle.Top;
                lbl.Text = pk.name;
                splitContainer2.Panel1.Controls.Add(lbl);
            }
            if (splitContainer2 != null)
            {
                Control parent = splitContainer2.Parent;
                parent.Controls.Remove(splitContainer2);
                splitContainers.Remove(splitContainer2);

                for (int i = splitContainer2.Panel1.Controls.Count; i > 0; i--)
                {
                    Control control = splitContainer2.Panel1.Controls[0];
                    control.Parent.Controls.Remove(control);
                    parent.Controls.Add(control);
                }

                splitContainer2.Dispose();
                splitContainer2 = null;
            }


            foreach (KeyValuePair<string, Plant> kvp in plants)
            {
                kvp.Value.listView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            foreach (KeyValuePair<int, PackProcess> kvp in packProcesses)
            {
                kvp.Value.listView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }
        #endregion Methods
    }

}
