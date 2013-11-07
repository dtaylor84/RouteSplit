/****************************** Module Header ******************************\
* Module Name:  RouteSplitCtrl.cs
* Project:      RouteSplit
\***************************************************************************/

#region Using directives
using System;
using System.Collections; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using System.Security.Permissions;
using SAPDataProvider;
using SAPTableFactoryCtrl;
using System.Data.OleDb;
using c = RouteSplit.BuildConstants;

#endregion

namespace RouteSplit
{
    #region Interfaces

    /// <summary>
    /// RouteSplitCtrlIntf describes the COM interface of the coclass 
    /// </summary>
    [Guid(c.Guid_RouteSplitCtrlIntf)]
    [ComVisible(true)]
    public interface RouteSplitCtrlIntf
    {
        #region Properties

        /*Recordset rs { set; }*/
        Table t { set; get; }

//        System.Runtime.InteropServices.ComTypes.IStream istream { set; get; }
        ISAPDataProviderFormat istream { set; get; }

        #endregion

        #region Methods

        /*void Shutdown();*/

        string HelloWorld(string msg);      // Custom method
        Table Table(Table dp, int proc);
        
        #endregion
    }

    /// <summary>
    /// RouteSplitCtrlEvents describes the events the coclass can sink
    /// </summary>
    [Guid(c.Guid_RouteSplitCtrlEvents)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    // The public interface describing the events of the control
    public interface RouteSplitCtrlEvents
    {
        #region Events
        [DispId(c.DispId_TestEvent)]
        void TestEvent(string str);

        #endregion
    }

    #endregion

    [ProgId(c.ProgIdVersioned)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(RouteSplitCtrlEvents))]
    [Guid(c.Guid_RouteSplitCtrl)]
    [ComVisible(true)]
    public partial class RouteSplitCtrl : UserControl, RouteSplitCtrlIntf
    {

        #region Initialization

        public RouteSplitCtrl()
        {
            //MessageBox.Show("START");
            //return;

            InitializeComponent();


            this.plants = new Dictionary<string,Plant>();
            this.packProcesses = new Dictionary<int,PackProcess>();
            this.splitContainers = new List<SplitContainer>();

            // These functions are used to handle Tab-stops for the ActiveX 
            // control (including its child controls) when the control is 
            // hosted in a container.
            this.LostFocus += new EventHandler(CSActiveXCtrl_LostFocus);
            this.ControlAdded += new ControlEventHandler(
                CSActiveXCtrl_ControlAdded);

            // Raise custom Load event
            this.OnCreateControl(); 
        }

        // This event will hook up the necessary handlers
        void CSActiveXCtrl_ControlAdded(object sender, ControlEventArgs e)
        {
            // Register tab handler and focus-related event handlers for 
            // the control and its child controls.
            ActiveXCtrlHelper.WireUpHandlers(e.Control, ValidationHandler);
        }

        // Ensures that the Validating and Validated events fire properly
        internal void ValidationHandler(object sender, System.EventArgs e)
        {
            if (this.ContainsFocus) return;

            this.OnLeave(e); // Raise Leave event

            if (this.CausesValidation)
            {
                CancelEventArgs validationArgs = new CancelEventArgs();
                this.OnValidating(validationArgs);

                if (validationArgs.Cancel && this.ActiveControl != null)
                    this.ActiveControl.Focus();
                else
                    this.OnValidated(e); // Raise Validated event
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_SETFOCUS = 0x7;
            const int WM_PARENTNOTIFY = 0x210;
            const int WM_DESTROY = 0x2;
            const int WM_LBUTTONDOWN = 0x201;
            const int WM_RBUTTONDOWN = 0x204;

            if (m.Msg == WM_SETFOCUS)
            {
                // Raise Enter event
                this.OnEnter(System.EventArgs.Empty);
            }
            else if (m.Msg == WM_PARENTNOTIFY && (
                m.WParam.ToInt32() == WM_LBUTTONDOWN || 
                m.WParam.ToInt32() == WM_RBUTTONDOWN))
            {
                if (!this.ContainsFocus)
                {
                    // Raise Enter event
                    this.OnEnter(System.EventArgs.Empty);
                }
            }
            else if (m.Msg == WM_DESTROY && 
                !this.IsDisposed && !this.Disposing)
            {
                // Used to ensure the cleanup of the control
                this.Dispose();
            }

            base.WndProc(ref m);
        }

        // Ensures that tabbing across the container and the .NET controls
        // works as expected
        void CSActiveXCtrl_LostFocus(object sender, EventArgs e)
        {
            ActiveXCtrlHelper.HandleFocus(this);
        }

        #endregion


        #region Properties
        /*
        private Recordset _rs;
        public Recordset rs
        {
            //[param: MarshalAs(UnmanagedType.Interface)]
            set
            {
                if (value == null)
                {
//                    MessageBox.Show("set rs NULL!");
                }
                else
                {
//                    MessageBox.Show("set rs; ToString=" + value.ToString() + "; TypeName=" + Microsoft.VisualBasic.Information.TypeName(value));

                    lbRecordSet.Text = value.RecordCount.ToString();
                }

                _rs = value;
            }
        }
         * */

        private Table _t;
        public Table t
        {
            get {
                if (_t == null)
                {
//                    MessageBox.Show("get t NULL");
                }
                else
                {
//                    MessageBox.Show("get t; return" + _t.ToString() + "; TypeName=" + Microsoft.VisualBasic.Information.TypeName(_t));
                }

                return _t;
            }
            set
            {
                if (value == null)
                {
//                    MessageBox.Show("set t NULL!");
                }
                else
                {
//                    MessageBox.Show("set t; ToString=" + value.ToString() + "; TypeName=" + Microsoft.VisualBasic.Information.TypeName(value));

                    lbTable.Text = value.RowCount.ToString();
                }

                _t = value;
            }
        }

//        private System.Runtime.InteropServices.ComTypes.IStream _istream;
//        public System.Runtime.InteropServices.ComTypes.IStream istream
        private ISAPDataProviderFormat _istream;
        public ISAPDataProviderFormat istream
        {
            get
            {
                return _istream;
            }
            set
            {
                _istream = value;
            }
        }

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

        [ComVisible(true)]
        public string HelloWorld(string msg)
        {
//            SAPDataProvider.SAPDataProvider dp = new SAPDataProvider.SAPDataProvider()
            MessageBox.Show(msg, "Message in C#",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return "Message from C#";
        }

        [ComVisible(true)]
        public Table Table(Table dp, int proc)
        {
//            MessageBox.Show(dp.ToString(), "Hmm");
//            MessageBox.Show(Microsoft.VisualBasic.Information.TypeName(dp), "Hmm2");

            try
            {
//                MessageBox.Show(dp[1, "WERKS"], "Wow!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught:" + ex.GetType() + ex.Message + ex.ToString(), "EXCEPTION");
            }

            plants.Clear();

            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            try
            {
                Hashtable hash = new Hashtable();
                ControlCollection cc;
                SplitContainer splitContainer = null;
                cc = splitContainer1.Panel1.Controls;

                foreach (_CSAPTaFacRow row in dp.Rows)
                {
                    string strp = ((string)row.get_Value("AUTH"));
                    string strtyp = ((string)row.get_Value("VPTYP"));
                    string strr = ((string)row.get_Value("ROUTE")).Substring(3, 3);
                    string strd = ((string)row.get_Value("VCTEXT")).Substring(7);

                    /* MultiSelListBox code */
                    listBox1.Items.Add(strr + ": " + strd);

                    /* Shiny new dynamic listview code */

                    Plant p;

                    if (!plants.ContainsKey(strp))
                    {
                        splitContainer = new SplitContainer();
                        splitContainer.Dock = DockStyle.Fill;
                        cc.Add(splitContainer);
                        cc = splitContainer.Panel2.Controls;

                        p = new Plant();
                        p.name = strp;
                        //splitContainer.SplitterDistance = 200;
                        p.panel = splitContainer.Panel1;

                        p.listView = new RouteListView();
                        p.listView.auth = strp;
                        p.listView.HideSelection = false;
                        p.listView.Sorting = SortOrder.Ascending;
                        p.listView.AllowDrop = true;
                        p.listView.Columns.Add("Route");
                        p.listView.Columns.Add("Description");
                        p.listView.View = View.Details;
                        p.listView.Dock = DockStyle.Fill;
                        p.panel.Controls.Add(p.listView);
                        plants[strp] = p;

                        Label lbl = new Label();
                        lbl.Text = "Plant " + strp;
                        lbl.Dock = DockStyle.Top;
                        p.panel.Controls.Add(lbl);

                        splitContainers.Add(splitContainer);
                    }
                    else
                    {
                        p = plants[strp];
                    }

                    RouteListViewItem i = new RouteListViewItem(strr, strd, strtyp, strp);
                    i.BackColor = Color.FromArgb(255, ((16 * p.listView.Items.Count) % 255), ((16 * p.listView.Items.Count) % 255));
                    p.listView.Add(i);


                }
                if (splitContainer != null)
                {
                    Control parent = splitContainer.Parent;
                    parent.Controls.Remove(splitContainer);
                    splitContainers.Remove(splitContainer);

                    for (int i = splitContainer.Panel1.Controls.Count; i > 0;  i--)
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
            listBox1.EndUpdate();
            dp.AppendRow();
            dp.AppendRow();

            this.Dock = DockStyle.Fill;
            //this.Padding = new Padding(0, 0, 16, 16);

            ControlCollection cc2;
            SplitContainer splitContainer2 = null;
            cc2 = splitContainer1.Panel2.Controls;

            for (int i = 0; i < proc; i++)
            {
                PackProcess pk = new PackProcess();

                splitContainer2 = new SplitContainer();
                splitContainer2.Dock = DockStyle.Fill;
                cc2.Add(splitContainer2);
                cc2 = splitContainer2.Panel2.Controls;


                pk.listView = new RouteListView();
                pk.name = "Process " + (i + 1);
                pk.listView.Sorting = SortOrder.Ascending;
                pk.listView.Dock = DockStyle.Fill;
                pk.listView.View = View.Details;
                pk.listView.AllowDrop = true;
                pk.listView.Columns.Add("Route");
                pk.listView.Columns.Add("Description").AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                splitContainer2.Panel1.Controls.Add(pk.listView);

                packProcesses[i] = pk;

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

                for (int i = splitContainer2.Panel1.Controls.Count; i > 0;  i--)
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
            return dp;

        }



        /*
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(380, 141);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // listView2
            // 
            this.listView2.AllowDrop = true;
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView2.CheckBoxes = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(0, -5);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(379, 80);
            this.listView2.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView2.TabIndex = 15;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
        */
        #endregion


        #region Events

        // This section shows the examples of exposing a control's events.
        // Typically, you just need to
        // 1) Declare the event as you want it.
        // 2) Raise the event in the appropriate control event.
        [ComVisible(false)]
        public delegate void TestEventHandler(string str);
        public event TestEventHandler TestEvent;
        void OnTestEvent(string str)
        {

            if (null != TestEvent)
            {
//                MessageBox.Show("Sending event");
                TestEvent(str);
//                MessageBox.Show("Sent event");
            }
//            else
//                MessageBox.Show("null! no event for you.");
        }

        #endregion


        private void btnMessage_Click(object sender, EventArgs e)
        {
//            MessageBox.Show("Raising TestEvent with:" + tbMessage.Text, "Raise Event",
//                MessageBoxButtons.OK, MessageBoxIcon.Information);

            OnTestEvent(tbMessage.Text); //save_dp);
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                btnMessage.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (istream != null)
            {
                System.Runtime.InteropServices.ComTypes.IStream i = (System.Runtime.InteropServices.ComTypes.IStream)istream.Stream;
                System.Runtime.InteropServices.ComTypes.STATSTG st;
                i.Stat(out st, 0);

                byte[] buffer = new byte[st.cbSize];
                IntPtr ptr = Marshal.AllocHGlobal(sizeof(int));
                i.Read(buffer, (int)st.cbSize, ptr);
                OnTestEvent("Size=" + st.cbSize.ToString() + "; read:" + Marshal.ReadIntPtr(ptr).ToString() + " bytes, starting:" + buffer[0].ToString("X"));
            }
        }

    }

}
