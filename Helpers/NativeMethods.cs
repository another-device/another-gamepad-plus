using System.Runtime.InteropServices;

namespace AnotherGamepadPlus.Helpers
{
    internal static class NativeMethods
    {
        // 鼠标位置设置
        [DllImport("user32.dll")]
        internal static extern bool SetCursorPos(int x, int y);

        // 替换 mouse_event 为 SendInput
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // XInput相关函数 - 替代第三方库
        [DllImport("xinput1_4.dll")]
        internal static extern uint XInputGetState(uint dwUserIndex, out XInputState pState);

        [DllImport("xinput1_4.dll")]
        internal static extern uint XInputSetState(uint dwUserIndex, ref XInputVibration pVibration);
    }

    // 输入事件类型枚举
    internal enum InputType : uint
    {
        INPUT_MOUSE = 0,
        INPUT_KEYBOARD = 1,
        INPUT_HARDWARE = 2
    }

    // 鼠标事件标志
    [Flags]
    internal enum MouseEventFlags : uint
    {
        MOUSEEVENTF_LEFTDOWN = 0x0002,
        MOUSEEVENTF_LEFTUP = 0x0004,
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        MOUSEEVENTF_RIGHTUP = 0x0010,
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,
        MOUSEEVENTF_MIDDLEUP = 0x0040,
        MOUSEEVENTF_WHEEL = 0x0800,
        MOUSEEVENTF_ABSOLUTE = 0x8000
    }

    // 键盘事件标志
    [Flags]
    internal enum KeyboardEventFlags : uint
    {
        KEYEVENTF_KEYDOWN = 0x0000,
        KEYEVENTF_EXTENDEDKEY = 0x0001,
        KEYEVENTF_KEYUP = 0x0002,
        KEYEVENTF_UNICODE = 0x0004,
        KEYEVENTF_SCANCODE = 0x0008
    }

    // 输入事件结构体
    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT
    {
        public InputType type;
        public InputUnion mi;
    }

    // 鼠标输入联合体
    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
    }

    // 鼠标输入结构体
    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public MouseEventFlags dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    // 键盘输入结构体
    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        public ushort wVk;          // 虚拟按键码
        public ushort wScan;        // 扫描码
        public KeyboardEventFlags dwFlags; // 按键标志
        public uint time;           // 时间戳
        public UIntPtr dwExtraInfo; // 额外信息
    }

    // XInput结构体定义
    [StructLayout(LayoutKind.Sequential)]
    public struct XInputState
    {
        public uint dwPacketNumber;
        public XInputGamepad Gamepad;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputGamepad
    {
        public ushort wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputVibration
    {
        public ushort wLeftMotorSpeed;
        public ushort wRightMotorSpeed;
    }
}
