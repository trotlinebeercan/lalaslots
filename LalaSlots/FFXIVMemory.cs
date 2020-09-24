namespace LalaSlots
{
    using Sharlayan;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    class FFXIVMemory
    {
        #region // Windows Keybind Imports

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

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

        public static void PerformActionThroughKeybind(LalaSlot lala, Enums.KeybindAction action)
        {
            (Keys k, Keys m) = Enums.GetKeyFromKeybindAction(action);

            if (m != Keys.None) SendMessage(lala.Window, WM_KEYDOWN, (IntPtr)m, IntPtr.Zero);
            if (k != Keys.None) SendMessage(lala.Window, WM_KEYDOWN, (IntPtr)k, IntPtr.Zero);
            if (k != Keys.None) SendMessage(lala.Window, WM_KEYUP,   (IntPtr)k, IntPtr.Zero);
            if (m != Keys.None) SendMessage(lala.Window, WM_KEYUP,   (IntPtr)m, IntPtr.Zero);
        }
    }

    public class LalaSlot
    {
        public Process Process { get; set; }
        public string Name { get; set; }
        public bool Hooked { get; set; }

        public int Id { get { return Process.Id; } }
        public IntPtr Window { get { return Process.MainWindowHandle; } }

        public void PerformAction(Enums.KeybindAction action)
        {
            FFXIVMemory.PerformActionThroughKeybind(this, action);
        }
    }
}
