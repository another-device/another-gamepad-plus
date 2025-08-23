using System.Runtime.InteropServices;
using AnotherGamepadPlus.Helpers;

namespace AnotherGamepadPlus.Services
{
    public class KeyboardService
    {
        private static void SendKeyboardInput(KeyboardEventFlags flags, ushort virtualKey)
        {
            var input = new INPUT[]
            {
                new INPUT
                {
                    type = InputType.INPUT_KEYBOARD,
                    mi = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = virtualKey,
                            wScan = 0,
                            dwFlags = flags,
                            time = 0,
                            dwExtraInfo = UIntPtr.Zero
                        }
                    }
                }
            };

            NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void PressUp()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, 0x26);
        }
        public static void ReleaseUp()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, 0x26);
        }
        public static void PressDown()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, 0x28);
        }
        public static void ReleaseDown()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, 0x28);
        }
        public static void PressLeft()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, 0x25);
        }
        public static void ReleaseLeft()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, 0x25);
        }
        public static void PressRight()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, 0x27);
        }
        public static void ReleaseRight()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, 0x27);
        }

    }
}