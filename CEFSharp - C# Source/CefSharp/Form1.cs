using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CefSharp;
using CefSharp.WinForms;

namespace CefSharp
{
    
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool ShowWindowAsync(IntPtr windowHandle, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);


        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        private IntPtr HWNDPtr;
        public int x;
        public int y;
        public int w;
        public int h;
        public int titleHeight;
        public int borderWidth;
        public int borderHeight;
        public bool attached = false;

        public const long WS_BORDER = 0x00800000L;
        public const long WS_CLIPCHILDREN = 0x02000000L;
        public const long WS_CLIPSIBLINGS = 0x04000000L;
        public const int GWL_STYLE = -16;

        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        public Form1()
        {
            InitializeComponent();
            InitializeChromium();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 0)
            {
                //MessageBox.Show(args[1] + " - " + args[2]);
                Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
                titleHeight = screenRectangle.Top - this.Top;
                borderWidth = screenRectangle.Left - this.Left;
                borderHeight = Math.Abs(screenRectangle.Bottom - this.Bottom);
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                string url = args[1];
                string name = args[2];
                AttachToWindow(name);
                Focus();
            }
            
        }
        public void AttachToWindow(string windowName)
        {
            Process[] processes = Process.GetProcessesByName(windowName);
            if (processes.Length != 0)
            {
                Process attachedProcess = processes[0];
                HWNDPtr = attachedProcess.MainWindowHandle;
                SetParent(this.Handle, HWNDPtr);
                SetWindowPos(this.Handle, -1, this.Left, this.Top, this.Left + this.Width, this.Top + this.Height, 1);
                Rect windowRect = new Rect();
                GetWindowRect(HWNDPtr, ref windowRect);
                x = windowRect.Left;
                y = windowRect.Top;
                w = windowRect.Right - windowRect.Left;
                h = windowRect.Bottom - windowRect.Top;
                this.WindowState = FormWindowState.Maximized;
                SetWindowPos(this.Handle, 0, this.Left, this.Top, this.Left + this.Width, this.Top + this.Height, 0x0004);
                this.WindowState = FormWindowState.Normal;
                //initWindow();
                tmrMove.Start();    
                ShowWindow(HWNDPtr, ShowWindowEnum.Restore);
                //lblDimensions.Text = "x: " + x.ToString() + "\r\ny: " + y.ToString() + "\r\nw: " + w.ToString() + "\r\nh: " + h.ToString(); ;
            }
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.Focus();
            SetWindowPos(this.Handle, 0, 0, 0, this.Width, this.Height, 0x0004);

        }
        public ChromiumWebBrowser chromeBrowser;

        public void InitializeChromium()
        {
            string[] args = Environment.GetCommandLineArgs();

            CefSettings settings = new CefSettings();

            Cef.Initialize(settings);

            chromeBrowser = new ChromiumWebBrowser(args[1]);

            chromeBrowser.BrowserSettings.FileAccessFromFileUrls = CefSharp.CefState.Enabled;
            //this.Controls.Add(chromeBrowser);
            pnlDisplay.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            button1.BringToFront();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void tmrMove_Tick(object sender, EventArgs e)
        {
            SetWindowLong(HWNDPtr, -16, GetWindowLong(HWNDPtr, GWL_STYLE) | WS_CLIPCHILDREN | WS_CLIPSIBLINGS);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }      
}
