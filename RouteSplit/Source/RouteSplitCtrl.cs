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

#if false
        [DispId(c.DispId_TestEvent)]
        void TestEvent(string str);
#endif

        #endregion
    }

    #endregion

    [ProgId(c.ProgIdVersioned)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(RouteSplitCtrlEvents))]
    [Guid(c.Guid_RouteSplitCtrl)]
    [ComVisible(true)]
    [DesignerAttribute(typeof(RouteSplitCtrlDesigner))]
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

            InitDemo();
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

        private RSTState State { get; set; }

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
#if false
#region hide
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
#endregion
#endif
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

        public void InitDemo()
        {
            //MessageBox.Show("Hello!");

            State = RSTestData.RSTestData.InitData(); // grab hardcoded test data

            plants.Clear();

            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            try
            {
                Hashtable hash = new Hashtable();
                ControlCollection cc;
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
                    listBox1.Items.Add(strShortRoute + ": " + strRouteText);

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

                    RouteListViewItem i = new RouteListViewItem(strShortRoute, strRouteText, strVpType, strWerksId);
                    i.BackColor = Color.FromArgb(255, ((16 * p.listView.Items.Count) % 255), ((16 * p.listView.Items.Count) % 255));
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
            listBox1.EndUpdate();
            
            this.Dock = DockStyle.Fill;
            //this.Padding = new Padding(0, 0, 16, 16);

            ControlCollection cc2;
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

    }

#region designer
// This class demonstrates how to build a custom designer. 
// When an instance of the associated control type is created 
// in a design environment like Visual Studio, this designer 
// provides custom design-time behavior. 
// 
// When you drop an instance of DemoControl onto a form, 
// this designer creates two adorner windows: one is used 
// for glyphs that represent the Margin and Padding properties 
// of the control, and the other is used for glyphs that 
// represent the Anchor property. 
// 
// The AnchorGlyph type defines an AnchorBehavior type that 
// allows you to change the value of the Anchor property  
// by double-clicking on an AnchorGlyph. 
// 
// This designer also offers a smart tag for changing the  
// Anchor property.
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class RouteSplitCtrlDesigner : ControlDesigner
    {
        // This adorner holds the glyphs that represent the Anchor property. 
        private Adorner anchorAdorner = null;

        // This adorner holds the glyphs that represent the Margin and 
        // Padding properties. 
        private Adorner marginAndPaddingAdorner = null;

        // This defines the size of the anchor glyphs. 
        private const int glyphSize = 6;

        // This defines the size of the hit bounds for an AnchorGlyph. 
        private const int hitBoundSize = glyphSize + 4;

        // References to designer services, for convenience. 
        private IComponentChangeService changeService = null;
        private ISelectionService selectionService = null;
        private BehaviorService behaviorSvc = null;

        // This is the collection of DesignerActionLists that 
        // defines the smart tags offered on the control.  
        private DesignerActionListCollection actionLists = null;

        public RouteSplitCtrlDesigner()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.behaviorSvc != null)
                {
                    // Remove the adorners added by this designer from 
                    // the BehaviorService.Adorners collection. 
                    this.behaviorSvc.Adorners.Remove(this.marginAndPaddingAdorner);
                    this.behaviorSvc.Adorners.Remove(this.anchorAdorner);
                }
            }

            base.Dispose(disposing);
        }

        // This method is where the designer initializes its state when 
        // it is created. 
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            // Connect to various designer services.
            InitializeServices();

            // Initialize adorners. 
            this.InitializeMarginAndPaddingAdorner();
            this.InitializeAnchorAdorner();

        }

        // This demonstrates changing the appearance of a control while 
        // it is being designed. In this case, the BackColor property is 
        // set to LightBlue.  

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            PropertyDescriptor colorPropDesc =
                TypeDescriptor.GetProperties(Component)["BackColor"];

            if (colorPropDesc != null &&
                colorPropDesc.PropertyType == typeof(Color) &&
                !colorPropDesc.IsReadOnly &&
                colorPropDesc.IsBrowsable)
            {
                colorPropDesc.SetValue(Component, Color.LightBlue);
            }
        }

        // This utility method creates an adorner for the anchor glyphs. 
        // It then creates four AnchorGlyph objects and adds them to  
        // the adorner's Glyphs collection. 
        private void InitializeAnchorAdorner()
        {
            this.anchorAdorner = new Adorner();
            this.behaviorSvc.Adorners.Add(this.anchorAdorner);

            this.anchorAdorner.Glyphs.Add(new AnchorGlyph(
                AnchorStyles.Left,
                this.behaviorSvc,
                this.changeService,
                this.selectionService,
                this,
                this.anchorAdorner)
                );

            this.anchorAdorner.Glyphs.Add(new AnchorGlyph(
                AnchorStyles.Top,
                this.behaviorSvc,
                this.changeService,
                this.selectionService,
                this,
                this.anchorAdorner)
                );

            this.anchorAdorner.Glyphs.Add(new AnchorGlyph(
                AnchorStyles.Right,
                this.behaviorSvc,
                this.changeService,
                this.selectionService,
                this,
                this.anchorAdorner)
                );

            this.anchorAdorner.Glyphs.Add(new AnchorGlyph(
                AnchorStyles.Bottom,
                this.behaviorSvc,
                this.changeService,
                this.selectionService,
                this,
                this.anchorAdorner)
                );
        }

        // This utility method creates an adorner for the margin and  
        // padding glyphs. It then creates a MarginAndPaddingGlyph and  
        // adds it to the adorner's Glyphs collection. 
        private void InitializeMarginAndPaddingAdorner()
        {
            this.marginAndPaddingAdorner = new Adorner();
            this.behaviorSvc.Adorners.Add(this.marginAndPaddingAdorner);

            this.marginAndPaddingAdorner.Glyphs.Add(new MarginAndPaddingGlyph(
                this.behaviorSvc,
                this.changeService,
                this.selectionService,
                this,
                this.marginAndPaddingAdorner));
        }

        // This utility method connects the designer to various services. 
        // These references are cached for convenience. 
        private void InitializeServices()
        {
            // Acquire a reference to IComponentChangeService. 
            this.changeService =
                GetService(typeof(IComponentChangeService))
                as IComponentChangeService;

            // Acquire a reference to ISelectionService. 
            this.selectionService =
                GetService(typeof(ISelectionService))
                as ISelectionService;

            // Acquire a reference to BehaviorService. 
            this.behaviorSvc =
                GetService(typeof(BehaviorService))
                as BehaviorService;
        }

        // This method creates the DesignerActionList on demand, causing 
        // smart tags to appear on the control being designed. 
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (null == actionLists)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(
                        new AnchorActionList(this.Component));
                }

                return actionLists;
            }
        }

        // This class defines the smart tags that appear on the control 
        // being designed. In this case, the Anchor property appears 
        // on the smart tag and its value can be changed through a  
        // UI Type Editor created automatically by the  
        // DesignerActionService. 
        public class AnchorActionList :
              System.ComponentModel.Design.DesignerActionList
        {
            // Cache a reference to the control. 
            private RouteSplitCtrl relatedControl;

            //The constructor associates the control  
            //with the smart tag list. 
            public AnchorActionList(IComponent component)
                : base(component)
            {
                this.relatedControl = component as RouteSplitCtrl;
            }

            // Properties that are targets of DesignerActionPropertyItem entries. 
            public AnchorStyles Anchor
            {
                get
                {
                    return this.relatedControl.Anchor;
                }
                set
                {
                    PropertyDescriptor pdAnchor = TypeDescriptor.GetProperties(this.relatedControl)["Anchor"];
                    pdAnchor.SetValue(this.relatedControl, value);
                }
            }

            // This method creates and populates the  
            // DesignerActionItemCollection which is used to  
            // display smart tag items. 
            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items =
                    new DesignerActionItemCollection();

                // Add a descriptive header.
                items.Add(new DesignerActionHeaderItem("Anchor Styles"));

                // Add a DesignerActionPropertyItem for the Anchor 
                // property. This will be displayed in a panel using 
                // the AnchorStyles UI Type Editor.
                items.Add(new DesignerActionPropertyItem(
                    "Anchor",
                    "Anchor Style"));

                return items;
            }
        }


        #region Glyph Implementations

        // This class implements a MarginAndPaddingGlyph, which draws  
        // borders highlighting the value of the control's Margin  
        // property and the value of the control's Padding property. 
        // 
        // This glyph has no mouse or keyboard interaction, so its 
        // related behavior class, MarginAndPaddingBehavior, has no 
        // implementation. 

        public class MarginAndPaddingGlyph : Glyph
        {
            private BehaviorService behaviorService = null;
            private IComponentChangeService changeService = null;
            private ISelectionService selectionService = null;
            private IDesigner relatedDesigner = null;
            private Adorner marginAndPaddingAdorner = null;
            private Control relatedControl = null;

            public MarginAndPaddingGlyph(
                BehaviorService behaviorService,
                IComponentChangeService changeService,
                ISelectionService selectionService,
                IDesigner relatedDesigner,
                Adorner marginAndPaddingAdorner)
                : base(new MarginAndPaddingBehavior())
            {
                this.behaviorService = behaviorService;
                this.changeService = changeService;
                this.selectionService = selectionService;
                this.relatedDesigner = relatedDesigner;
                this.marginAndPaddingAdorner = marginAndPaddingAdorner;

                this.relatedControl =
                    this.relatedDesigner.Component as Control;

                this.changeService.ComponentChanged += new ComponentChangedEventHandler(changeService_ComponentChanged);
            }

            void changeService_ComponentChanged(object sender, ComponentChangedEventArgs e)
            {
                if (object.ReferenceEquals(
                    e.Component,
                    this.relatedControl))
                {
                    if (e.Member.Name == "Margin" ||
                        e.Member.Name == "Padding")
                    {
                        this.marginAndPaddingAdorner.Invalidate();
                    }
                }
            }

            // This glyph has no mouse or keyboard interaction, so  
            // GetHitTest can return null. 
            public override Cursor GetHitTest(Point p)
            {
                return null;
            }

            // This method renders the glyph as a simple focus rectangle. 
            public override void Paint(PaintEventArgs e)
            {
                ControlPaint.DrawFocusRectangle(
                        e.Graphics,
                        this.Bounds);

                ControlPaint.DrawFocusRectangle(
                        e.Graphics,
                        this.PaddingBounds);
            }

            // This glyph's Bounds property is a Rectangle defined by  
            // the value of the control's Margin property. 
            public override Rectangle Bounds
            {
                get
                {
                    Control c = this.relatedControl;
                    Rectangle controlRect =
                        this.behaviorService.ControlRectInAdornerWindow(this.relatedControl);

                    Rectangle boundsVal = new Rectangle(
                        controlRect.Left - c.Margin.Left,
                        controlRect.Top - c.Margin.Top,
                        controlRect.Width + c.Margin.Right * 2,
                        controlRect.Height + c.Margin.Bottom * 2);

                    return boundsVal;
                }
            }

            // The PaddingBounds property is a Rectangle defined by  
            // the value of the control's Padding property. 
            public Rectangle PaddingBounds
            {
                get
                {
                    Control c = this.relatedControl;
                    Rectangle controlRect =
                        this.behaviorService.ControlRectInAdornerWindow(this.relatedControl);

                    Rectangle boundsVal = new Rectangle(
                        controlRect.Left + c.Padding.Left,
                        controlRect.Top + c.Padding.Top,
                        controlRect.Width - c.Padding.Right * 2,
                        controlRect.Height - c.Padding.Bottom * 2);

                    return boundsVal;
                }
            }

            // There are no keyboard or mouse behaviors associated with  
            // this glyph, but you could add them to this class. 
            internal class MarginAndPaddingBehavior : Behavior
            {

            }
        }

        // This class implements an AnchorGlyph, which draws grab handles 
        // that represent the value of the control's Anchor property. 
        // 
        // This glyph has mouse and keyboard interactions, which are 
        // handled by the related behavior class, AnchorBehavior. 
        // Double-clicking on an AnchorGlyph causes its value to be  
        // toggled between enabled and disable states.  

        public class AnchorGlyph : Glyph
        {
            // This defines the bounds of the anchor glyph. 
            protected Rectangle boundsValue;

            // This defines the bounds used for hit testing. 
            // These bounds are typically different than the bounds  
            // of the glyph itself. 
            protected Rectangle hitBoundsValue;

            // This is the cursor returned if hit test is positive. 
            protected Cursor hitTestCursor = Cursors.Hand;

            // Cache references to services that will be needed. 
            private BehaviorService behaviorService = null;
            private IComponentChangeService changeService = null;
            private ISelectionService selectionService = null;

            // Keep a reference to the designer for convenience. 
            private IDesigner relatedDesigner = null;

            // Keep a reference to the adorner for convenience. 
            private Adorner anchorAdorner = null;

            // Keep a reference to the control being designed. 
            private Control relatedControl = null;

            // This defines the AnchorStyle which this glyph represents. 
            private AnchorStyles anchorStyle;

            public AnchorGlyph(
                AnchorStyles anchorStyle,
                BehaviorService behaviorService,
                IComponentChangeService changeService,
                ISelectionService selectionService,
                IDesigner relatedDesigner,
                Adorner anchorAdorner)
                : base(new AnchorBehavior(relatedDesigner))
            {
                // Cache references for convenience. 
                this.anchorStyle = anchorStyle;
                this.behaviorService = behaviorService;
                this.changeService = changeService;
                this.selectionService = selectionService;
                this.relatedDesigner = relatedDesigner;
                this.anchorAdorner = anchorAdorner;

                // Cache a reference to the control being designed. 
                this.relatedControl =
                    this.relatedDesigner.Component as Control;

                // Hook the SelectionChanged event.  
                this.selectionService.SelectionChanged +=
                    new EventHandler(selectionService_SelectionChanged);

                // Hook the ComponentChanged event so the anchor glyphs 
                // can correctly track the control's bounds. 
                this.changeService.ComponentChanged +=
                    new ComponentChangedEventHandler(changeService_ComponentChanged);
            }

            #region Overrides

            public override Rectangle Bounds
            {
                get
                {
                    return this.boundsValue;
                }
            }

            // This method renders the AnchorGlyph as a filled rectangle 
            // if the glyph is enabled, or as an open rectangle if the 
            // glyph is disabled. 
            public override void Paint(PaintEventArgs e)
            {
                if (this.IsEnabled)
                {
                    using (Brush b = new SolidBrush(Color.Tomato))
                    {
                        e.Graphics.FillRectangle(b, this.Bounds);
                    }
                }
                else
                {
                    using (Pen p = new Pen(Color.Tomato))
                    {
                        e.Graphics.DrawRectangle(p, this.Bounds);
                    }
                }
            }

            // An AnchorGlyph has keyboard and mouse interaction, so it's 
            // important to return a cursor when the mouse is located in  
            // the glyph's hit region. When this occurs, the  
            // AnchorBehavior becomes active. 

            public override Cursor GetHitTest(Point p)
            {
                if (hitBoundsValue.Contains(p))
                {
                    return hitTestCursor;
                }

                return null;
            }

            #endregion

            #region Event Handlers

            // The AnchorGlyph objects should mimic the resize glyphs; 
            // they should only be visible when the control is the  
            // primary selection. The adorner is enabled when the  
            // control is the primary selection and disabled when  
            // it is not. 

            void selectionService_SelectionChanged(object sender, EventArgs e)
            {
                if (object.ReferenceEquals(
                    this.selectionService.PrimarySelection,
                    this.relatedControl))
                {
                    this.ComputeBounds();
                    this.anchorAdorner.Enabled = true;
                }
                else
                {
                    this.anchorAdorner.Enabled = false;
                }
            }

            // If any of several properties change, the bounds of the  
            // AnchorGlyph must be computed again. 
            void changeService_ComponentChanged(
                object sender,
                ComponentChangedEventArgs e)
            {
                if (object.ReferenceEquals(
                    e.Component,
                    this.relatedControl))
                {
                    if (e.Member.Name == "Anchor" ||
                        e.Member.Name == "Size" ||
                        e.Member.Name == "Height" ||
                        e.Member.Name == "Width" ||
                        e.Member.Name == "Location")
                    {
                        // Compute the bounds of this glyph. 
                        this.ComputeBounds();

                        // Tell the adorner to repaint itself. 
                        this.anchorAdorner.Invalidate();
                    }
                }
            }

            #endregion

            #region Implementation

            // This utility method computes the position and size of  
            // the AnchorGlyph in the Adorner window's coordinates. 
            // It also computes the hit test bounds, which are 
            // slightly larger than the glyph's bounds. 
            private void ComputeBounds()
            {
                Rectangle translatedBounds = new Rectangle(
                    this.behaviorService.ControlToAdornerWindow(this.relatedControl),
                    this.relatedControl.Size);

                if ((this.anchorStyle & AnchorStyles.Top) == AnchorStyles.Top)
                {
                    this.boundsValue = new Rectangle(
                        translatedBounds.X + (translatedBounds.Width / 2) - (glyphSize / 2),
                        translatedBounds.Y + glyphSize,
                        glyphSize,
                        glyphSize);
                }
                if ((this.anchorStyle & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                {
                    this.boundsValue = new Rectangle(
                        translatedBounds.X + (translatedBounds.Width / 2) - (glyphSize / 2),
                        translatedBounds.Bottom - 2 * glyphSize,
                        glyphSize,
                        glyphSize);
                }
                if ((this.anchorStyle & AnchorStyles.Left) == AnchorStyles.Left)
                {
                    this.boundsValue = new Rectangle(
                        translatedBounds.X + glyphSize,
                        translatedBounds.Y + (translatedBounds.Height / 2) - (glyphSize / 2),
                        glyphSize,
                        glyphSize);
                }
                if ((this.anchorStyle & AnchorStyles.Right) == AnchorStyles.Right)
                {
                    this.boundsValue = new Rectangle(
                        translatedBounds.Right - 2 * glyphSize,
                        translatedBounds.Y + (translatedBounds.Height / 2) - (glyphSize / 2),
                        glyphSize,
                        glyphSize);
                }

                this.hitBoundsValue = new Rectangle(
                    this.Bounds.Left - hitBoundSize / 2,
                    this.Bounds.Top - hitBoundSize / 2,
                    hitBoundSize,
                    hitBoundSize);
            }

            // This utility property determines if the AnchorGlyph is  
            // enabled, according to the value specified by the  
            // control's Anchor property. 
            private bool IsEnabled
            {
                get
                {
                    return
                        ((this.anchorStyle & this.relatedControl.Anchor) ==
                        this.anchorStyle);
                }
            }

            #endregion




            #region Behavior Implementation


            // This Behavior specifies mouse and keyboard handling when 
            // an AnchorGlyph is active. This happens when  
            // AnchorGlyph.GetHitTest returns a non-null value. 
            internal class AnchorBehavior : Behavior
            {
                private IDesigner relatedDesigner = null;
                private Control relatedControl = null;

                internal AnchorBehavior(IDesigner relatedDesigner)
                {
                    this.relatedDesigner = relatedDesigner;
                    this.relatedControl = relatedDesigner.Component as Control;
                }

                // When you double-click on an AnchorGlyph, the value of  
                // the control's Anchor property is toggled. 
                // 
                // Note that the value of the Anchor property is not set 
                // by direct assignment. Instead, the  
                // PropertyDescriptor.SetValue method is used. This  
                // enables notification of the design environment, so  
                // related events can be raised, for example, the 
                // IComponentChangeService.ComponentChanged event. 

                public override bool OnMouseDoubleClick(
                    Glyph g,
                    MouseButtons button,
                    Point mouseLoc)
                {
                    base.OnMouseDoubleClick(g, button, mouseLoc);

                    if (button == MouseButtons.Left)
                    {
                        AnchorGlyph ag = g as AnchorGlyph;
                        PropertyDescriptor pdAnchor =
                            TypeDescriptor.GetProperties(ag.relatedControl)["Anchor"];

                        if (ag.IsEnabled)
                        {
                            // The glyph is enabled.  
                            // Clear the AnchorStyle flag to disable the Glyph.
                            pdAnchor.SetValue(
                                ag.relatedControl,
                                ag.relatedControl.Anchor ^ ag.anchorStyle);
                        }
                        else
                        {
                            // The glyph is disabled.  
                            // Set the AnchorStyle flag to enable the Glyph.
                            pdAnchor.SetValue(
                                ag.relatedControl,
                                ag.relatedControl.Anchor | ag.anchorStyle);
                        }

                    }

                    return true;
                }
            }


            #endregion
        }
        #endregion
    }

#endregion

}
