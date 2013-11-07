/********************************** Module Header ***********************************\
* Module Name:  ActiveXCtrlHelper.cs
* Copyright (c) Microsoft Corporation.
* 
* ActiveXCtrlHelper provides the helper functions to register/unregister an ActiveX 
* control, and helps to handle the focus and tabbing across the container and the 
* .NET controls.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/en-us/openness/resources/licenses.aspx#MPL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\************************************************************************************/

#region Using directives
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using Microsoft.VisualBasic.Devices;
#endregion
 
[ComVisible(false)]
internal class ActiveXCtrlHelper : AxHost
{
    internal ActiveXCtrlHelper()
        : base(null)
    {
    }

    #region Type Converter

    /// <summary>
    /// Convert System.Drawing.Color to OleColor 
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    internal static new int GetOleColorFromColor(Color color)
    {
        return CUIntToInt(AxHost.GetOleColorFromColor(color));
    }

    /// <summary>
    /// Convert OleColor to System.Drawing.Color
    /// </summary>
    /// <param name="oleColor"></param>
    /// <returns></returns>
    internal static Color GetColorFromOleColor(int oleColor)
    {
        return AxHost.GetColorFromOleColor(CIntToUInt(oleColor));
    }

    internal static int CUIntToInt(uint uiArg)
    {
        if (uiArg <= int.MaxValue)
        {
            return (int)uiArg;
        }
        return (int)(uiArg - unchecked(2 * ((uint)(int.MaxValue) + 1)));
    }

    internal static uint CIntToUInt(int iArg)
    {
        if (iArg < 0)
        {
            return (uint)(uint.MaxValue + iArg + 1);
        }
        return (uint)iArg;
    }

    #endregion

    #region Tab Handler

    /// <summary>
    /// Register tab handler and focus-related event handlers for the control and its 
    /// child controls.
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="ValidationHandler"></param>
    internal static void WireUpHandlers(Control ctrl, EventHandler ValidationHandler)
    {
        if (ctrl != null)
        {
            ctrl.KeyDown += new KeyEventHandler(TabHandler);
            ctrl.LostFocus += new EventHandler(ValidationHandler);

            if (ctrl.HasChildren)
            {
                foreach (Control child in ctrl.Controls)
                {
                    WireUpHandlers(child, ValidationHandler);
                }
            }
        }
    }

    /// <summary>
    /// Handler of "Tab" and "Shift"+"Tab".
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void TabHandler(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Tab)
        {
            Control ctrl = sender as Control;
            UserControl usrCtrl = GetParentUserControl(ctrl);
            Control firstCtrl = usrCtrl.GetNextControl(null, true);
            do
            {
                firstCtrl = usrCtrl.GetNextControl(firstCtrl, true);
            } while (firstCtrl != null && !firstCtrl.CanSelect);

            Control lastCtrl = usrCtrl.GetNextControl(null, false);
            do
            {
                lastCtrl = usrCtrl.GetNextControl(lastCtrl, false);
            } while (lastCtrl != null && lastCtrl.CanSelect);

            if (ctrl.Equals(lastCtrl) || ctrl.Equals(firstCtrl) || 
                lastCtrl.Contains(ctrl) || firstCtrl.Contains(ctrl))
            {
                usrCtrl.SelectNextControl((Control)sender, 
                    lastCtrl.Equals(usrCtrl.ActiveControl), true, true, true);
            }
        }
    }

    private static UserControl GetParentUserControl(Control ctrl)
    {
        if (ctrl == null)
            return null;

        do
        {
            ctrl = ctrl.Parent;
        } while (ctrl.Parent != null);

        if (ctrl != null)
            return (UserControl)ctrl;

        return null;
    }

    #endregion

    #region Focus Handler

    /// <summary>
    /// Handle the focus of the ActiveX control, including its child controls
    /// </summary>
    /// <param name="usrCtrl">the ActiveX control</param>
    internal static void HandleFocus(UserControl usrCtrl)
    {
        Keyboard keyboard = new Keyboard();
        if (keyboard.AltKeyDown)
        {
            // Handle accessor key
            HandleAccessorKey(usrCtrl.GetNextControl(null, true), usrCtrl);
        }
        else
        {
            // Move to the first control that can receive focus, taking into account 
            // the possibility that the user pressed <Shift>+<Tab>, in which case we 
            // need to start at the end and work backwards.
            for (System.Windows.Forms.Control ctrl =
                usrCtrl.GetNextControl(null, !keyboard.ShiftKeyDown);
                ctrl != null;
                ctrl = usrCtrl.GetNextControl(ctrl, !keyboard.ShiftKeyDown))
            {
                if (ctrl.Enabled && ctrl.CanSelect)
                {
                    ctrl.Focus();
                    break;
                }
            }
        }

    }

    private const int KEY_PRESSED = 0x1000;
    [DllImport("user32.dll")]
    static extern short GetKeyState(int nVirtKey);

    /// <summary>
    /// Get X in the accessor key "Alt + X"
    /// </summary>
    /// <returns></returns>
    private static int CheckForAccessorKey()
    {
        Keyboard keyboard = new Keyboard();
        if (keyboard.AltKeyDown)
        {
            for (int i = (int)Keys.A; i <= (int)Keys.Z; i++)
            {
                if ((GetKeyState(i) != 0 && KEY_PRESSED != 0))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    /// <summary>
    /// Check the accessor key, find the next selectable control that matches the 
    /// accessor key and give it the focus.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="usrCtrl"></param>
    private static void HandleAccessorKey(object sender, UserControl usrCtrl)
    {
        // Get X in the accessor key <Alt + X>
        int key = CheckForAccessorKey();
        if (key == -1) return;

        Control ctrl = usrCtrl.GetNextControl((Control)sender, false);

        do
        {
            ctrl = usrCtrl.GetNextControl(ctrl, true);
            if (ctrl != null &&
                Control.IsMnemonic(Convert.ToChar(key), ctrl.Text) &&
                !KeyConflict(Convert.ToChar(key), usrCtrl))
            {
                // If we land on a non-selectable control then go to the next 
                // control in the tab order.
                if (!ctrl.CanSelect)
                {
                    Control ctlAfterLabel = usrCtrl.GetNextControl(ctrl, true);
                    if (ctlAfterLabel != null && ctlAfterLabel.CanFocus)
                        ctlAfterLabel.Focus();
                }
                else
                {
                    ctrl.Focus();
                }
                break;
            }
            // Loop until we hit the end of the tab order. If we have hit the end  
            // of the tab order we do not want to loop back because the parent 
            // form's controls come next in the tab order.
        } while (ctrl != null);
    }

    private static bool KeyConflict(char key, UserControl u)
    {
        bool flag = false;
        foreach (Control ctl in u.Controls)
        {
            if (Control.IsMnemonic(key, ctl.Text))
            {
                if (flag)
                {
                    return true;
                }
                flag = true;
            }
        }
        return false;
    }

    #endregion
}

