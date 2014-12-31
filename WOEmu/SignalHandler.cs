using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Text;

namespace WOEmu
{
    public static class SignalHandler
    {
        public enum CtrlTypes
        {

            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        private static bool Check(CtrlTypes t)
        {
            switch (t)
            {
                case CtrlTypes.CTRL_C_EVENT:
                    Program.doExit();
                    break;

                case CtrlTypes.CTRL_CLOSE_EVENT:
                    Program.doExit();
                    break;

                case CtrlTypes.CTRL_LOGOFF_EVENT:
                    Program.doExit();
                    break;
                    
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    Program.doExit();
                    break;

                case CtrlTypes.CTRL_BREAK_EVENT:
                    break;
            }

            return true;
        }

        public static void hookKeys()
        {
            SetConsoleCtrlHandler(new HandlerRoutine(Check), true);
        }

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine r, bool Add);

        public delegate bool HandlerRoutine(CtrlTypes t);
    }
}
