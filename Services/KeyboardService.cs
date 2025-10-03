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

        public void PressKey(ushort keyCode)
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, keyCode);
        }
        public void ReleaseKey(ushort keyCode)
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, keyCode);
        }


        public void PressUp()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, 0x26);
        }
        public void ReleaseUp()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, 0x26);
        }
        public void PressDown()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, 0x28);
        }
        public void ReleaseDown()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, 0x28);
        }
        public void PressLeft()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, 0x25);
        }
        public void ReleaseLeft()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, 0x25);
        }
        public void PressRight()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYDOWN, 0x27);
        }
        public void ReleaseRight()
        {
            SendKeyboardInput(KeyboardEventFlags.KEYEVENTF_KEYUP, 0x27);
        }

    }
}