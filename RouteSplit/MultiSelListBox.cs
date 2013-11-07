


#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using System.Security.Permissions;
using SAPDataProvider;
using SAPTableFactoryCtrl;
using System.Data.OleDb;

#endregion


namespace RouteSplit
{
    public class MultiSelListBox : ListBox
    {
        protected int MouseDownOnIndex;
        protected bool bMouseDownOnSelection;
        protected bool bMouseDownOutsideSelection;

        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_MOUSEMOVE = 0x200;
        private const int MK_LBUTTON = 0x1;

        public MultiSelListBox()
        {
            this.DragOver += new System.Windows.Forms.DragEventHandler(handle_DragOver);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(handle_DragDrop);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            Point pt;

            switch (m.Msg)
            {
                case WM_LBUTTONDOWN:
                    pt = new Point(m.LParam.ToInt32());
                    this.MouseDownOnIndex = this.IndexFromPoint(pt);
                    if (this.SelectedItems.Count >= 1 &&
                        this.SelectedIndices.Contains(this.MouseDownOnIndex) &&
                        m.WParam.ToInt64() == MK_LBUTTON)
                    {
                        this.bMouseDownOnSelection = true;
                        return;
                    } else {
                        bMouseDownOutsideSelection = true;
                        base.WndProc(ref m);
                    }
                    break;
                case WM_MOUSEMOVE:
                    if (this.bMouseDownOnSelection)
                    {
                        DragDropEffects dde = this.DoDragDrop(this.SelectedItems, DragDropEffects.Move);
                        if (dde == DragDropEffects.Move)
                            for (int i = this.SelectedIndices.Count - 1; i >= 0; i--)
                                this.Items.RemoveAt(this.SelectedIndices[i]);
                    }
                    this.bMouseDownOnSelection = false;
                    base.WndProc(ref m);
                    break;
                case WM_LBUTTONUP:
                    pt = new Point(m.LParam.ToInt32());
                    if (this.MouseDownOnIndex == this.IndexFromPoint(pt) &&
                        m.WParam.ToInt64() == 0 &&
                        !this.bMouseDownOutsideSelection)
                    {
                        Message down = new Message();
                        down.HWnd = m.HWnd;
                        down.Msg = WM_LBUTTONDOWN;
                        down.WParam = m.WParam;
                        down.LParam = m.LParam;
                        down.Result = IntPtr.Zero;
                        base.WndProc(ref down);
                    }
                    this.bMouseDownOutsideSelection = false;
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        void handle_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            foreach (Object item in (ListBox.SelectedObjectCollection)e.Data.GetData(typeof(ListBox.SelectedObjectCollection)))
            {
                this.Items.Add(item);
            }
            e.Effect = DragDropEffects.Move;
        }

        void handle_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListBox.SelectedObjectCollection)))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
