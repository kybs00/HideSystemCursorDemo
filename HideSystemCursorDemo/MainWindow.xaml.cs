using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Cursors = System.Windows.Input.Cursors;

namespace SystemCursorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void HideButton_OnClick(object sender, RoutedEventArgs e)
        {
            HideCursor();
        }

        private void ShowButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowCursor();
        }

        private readonly int[] _systemCursorIds = new int[] { 32512, 32513, 32514, 32515, 32516, 32642, 32643, 32644, 32645, 32646, 32648, 32649, 32650 };
        private readonly IntPtr[] _previousCursorHandles = new IntPtr[13];
        private void HideCursor()
        {
            for (int i = 0; i < _systemCursorIds.Length; i++)
            {
                var cursor = LoadCursor(IntPtr.Zero, _systemCursorIds[i]);
                var cursorHandle = CopyIcon(cursor);
                _previousCursorHandles[i] = cursorHandle;
                //替换为空白鼠标光标
                var cursorFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Blank.cur");
                IntPtr blankCursor = LoadCursorFromFile(cursorFile);
                SetSystemCursor(blankCursor, (uint)_systemCursorIds[i]);
            }

            ShowButton.IsEnabled = true;
            HideButton.IsEnabled = false;
        }
    /// <summary>
    /// 获取当前显示光标句柄
    /// </summary>
    /// <returns></returns>
    private IntPtr GetCurrentCursor()
    {
        CURSORINFO cursorInfo;
        cursorInfo.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
        GetCursorInfo(out cursorInfo);
        var cursorId = cursorInfo.hCursor;
        var cursorHandle = CopyIcon(cursorId);
        return cursorHandle;
    }
        private void ShowCursor()
        {
            for (int i = 0; i < _systemCursorIds.Length; i++)
            {
                SetSystemCursor(_previousCursorHandles[i], (uint)_systemCursorIds[i]);
            }
            ShowButton.IsEnabled = false;
            HideButton.IsEnabled = true;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
        [DllImport("user32.dll")]
        public static extern IntPtr CopyIcon(IntPtr cusorId);
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string lpFileName);
        [DllImport("user32.dll")]
        public static extern bool SetSystemCursor(IntPtr hcur, uint id);
        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
                                        // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
                                        //    0             The cursor is hidden.
                                        //    CURSOR_SHOWING    The cursor is showing.
            public IntPtr hCursor;          // Handle to the cursor. 
            public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
        }
    }
}