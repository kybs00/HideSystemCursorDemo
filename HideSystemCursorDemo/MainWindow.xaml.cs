using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace HideSystemCursorDemo
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

    private void HideCursor()
    {
        CURSORINFO pci;
        pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
        GetCursorInfo(out pci);
        _cursorHandle = CopyIcon(pci.hCursor);
        //替换为空白鼠标光标
        var cursorFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Blank.cur");
        IntPtr cursor = LoadCursorFromFile(cursorFile);
        SetSystemCursor(cursor, OcrNormal);
        ShowButton.IsEnabled = true;
        HideButton.IsEnabled = false;
    }
        private void ShowCursor()
        {
            //Show the mouse with the mouse handle we copied earlier.
            bool success = SetSystemCursor(_cursorHandle, OcrNormal);
            ShowButton.IsEnabled = false;
            HideButton.IsEnabled = true;
        }


    private IntPtr _cursorHandle;
    private const uint OcrNormal = 32512;

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