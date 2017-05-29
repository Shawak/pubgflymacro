using ShawLib;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pubgflymacro
{
    class App : Overlay
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        bool active;

        KeyboardHook hook;

        public App()
        {
            this.Location = new Point(0, 10);
            this.Width = 100;
            this.Height = 100;
            hook = new KeyboardHook(onKey);
            new Task(() => loop()).Start();
        }

        bool onKey(KeyHookEventArgs e)
        {
            if (e.Key == Keys.PageUp)
            {
                active = !active;
                this.Invalidate(false);
            }
            return true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(active ? Brushes.Green : Brushes.Blue, new Rectangle(10, 10, 5, 5));
        }

        async void loop()
        {
            while(true)
            {
                if (active)
                {
                    keybd_event(0x57, 0, 0x1, 0);
                    await Task.Delay(300);
                    keybd_event(0x57, 0, 0x2, 0);
                }
                await Task.Delay(500);
            }
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("PUBG fly macro by Shawak");
            Console.WriteLine("press PageUp to toggle");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new App());
        }
    }
}
