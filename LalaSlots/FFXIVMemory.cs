namespace LalaSlots
{
    using Sharlayan;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    class FFXIVMemory
    {
        #region // Windows Keybind Imports

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        #endregion

        public static List<LalaSlot> AllAvailableLalas { get; private set; }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Static Accessors
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static void InitializeMemory()
        {
            AllAvailableLalas = new List<LalaSlot>();
            var processes = Process.GetProcesses().Where(p => (p.MainWindowTitle.Length != 0 && p.ProcessName.Contains("ffxiv")));
            foreach (var process in processes)
            {
                string characterName = FFXIVMemory.GetPlayerNameFromProcess(process);
                AllAvailableLalas.Add(new LalaSlot
                {
                    Name = characterName,
                    Process = process
                });
            }
        }

        public static LalaSlot GetLalaSlotFromProcess(Process process)
        {
            return AllAvailableLalas.Where(l => l.Id == process.Id).Single();
        }

        private static string GetPlayerNameFromProcess(Process process)
        {
            string playerName = "(Unknown)";
            MemoryHandler.Instance.SetProcess(new Sharlayan.Models.ProcessModel
            {
                Process = process,
                IsWin64 = process.ProcessName.Contains("_dx11")
            });

            while (Scanner.Instance.IsScanning)
            {
                // TODO: Make this safe
                Thread.Sleep(10);
            }

            if (Reader.CanGetPlayerInfo())
            {
                playerName = Reader.GetCurrentPlayer().CurrentPlayer.Name;
            }

            MemoryHandler.Instance.UnsetProcess();
            return playerName;
        }

        private static void SendSyncKey(IntPtr ffxivWindow, Keys key, int waitBetweenKeyUpInMs)
        {
            bool modifier = false;

            // This sleeps the thread intentionally. We are asking the game to take key inputs like a human, and want to insure everything is actually taken in.
            Keys key2 = (key & ~Keys.Control) & (key & ~Keys.Shift) & (key & ~Keys.Alt);

            if (key2 != key) modifier = true;

            if (modifier)
            {
                for (int i = 0; i < 5; i++)
                {
                    if ((key & Keys.Control) == Keys.Control) SendMessage(ffxivWindow, WM_KEYDOWN, ((IntPtr)Keys.ControlKey), ((IntPtr)0));
                    if ((key & Keys.Alt) == Keys.Alt) SendMessage(ffxivWindow, WM_SYSKEYDOWN, ((IntPtr)Keys.Menu), ((IntPtr)0));
                    if ((key & Keys.Shift) == Keys.Shift) SendMessage(ffxivWindow, WM_KEYDOWN, ((IntPtr)Keys.ShiftKey), ((IntPtr)0));

                    Thread.Sleep(5);
                }
            }

            SendMessage(ffxivWindow, WM_KEYDOWN, ((IntPtr)key2), ((IntPtr)0));
            Thread.Sleep(waitBetweenKeyUpInMs);
            SendMessage(ffxivWindow, WM_KEYUP, ((IntPtr)key2), ((IntPtr)0));

            if (modifier)
            {
                if ((key & Keys.Shift) == Keys.Shift)
                {
                    Thread.Sleep(5);
                    SendMessage(ffxivWindow, WM_KEYUP, ((IntPtr)Keys.ShiftKey), ((IntPtr)0));
                }
                if ((key & Keys.Alt) == Keys.Alt)
                {
                    Thread.Sleep(5);
                    SendMessage(ffxivWindow, WM_SYSKEYUP, ((IntPtr)Keys.Menu), ((IntPtr)0));
                }
                if ((key & Keys.Control) == Keys.Control)
                {
                    Thread.Sleep(5);
                    SendMessage(ffxivWindow, WM_KEYUP, ((IntPtr)Keys.ControlKey), ((IntPtr)0));
                }
            }
            Thread.Sleep(50);
        }

        public static void PerformActionThroughKeybind(LalaSlot lala, Enums.KeybindAction action, int kbWaitInMs)
        {
            Keys k = Enums.GetKeyFromKeybindAction(action);
            SendSyncKey(lala.Window, k, kbWaitInMs);
        }
    }

    public class LalaSlot
    {
        public Process Process { get; set; }
        public string Name { get; set; }
        public bool Hooked { get; set; }

        public int Id { get { return Process.Id; } }
        public IntPtr Window { get { return Process.MainWindowHandle; } }

        public void PerformAction(Enums.KeybindAction action, int kbWaitInMs)
        {
            Task.Run(() => FFXIVMemory.PerformActionThroughKeybind(this, action, kbWaitInMs));
        }
    }
}
