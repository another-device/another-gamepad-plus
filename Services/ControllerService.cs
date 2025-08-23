using System.Windows.Threading;
using AnotherGamepadPlus.Helpers;

namespace AnotherGamepadPlus.Services
{
    public class ControllerService : IDisposable
    {
        private readonly uint _controllerIndex = 0; // 默认使用第一个手柄
        private bool _isRunning;
        private CancellationTokenSource _cts;
        private Task _pollingTask;

        // 事件定义
        public event Action<bool> ConnectionStatusChanged;
        public event Action<float, float> StickMoved;
        public event Action<byte> LeftTriggerChanged;
        public event Action<byte> RightTriggerChanged;
        public event Action<bool> StartButtonStateChanged;
        public event Action<bool> BackButtonStateChanged;
        public event Action<bool> AButtonStateChanged;
        public event Action<bool> BButtonStateChanged;
        public event Action<bool> XButtonStateChanged;
        // public event Action<bool> YButtonStateChanged;
        public event Action<bool> LBStateChanged;
        public event Action<bool> RBStateChanged;
        public event Action<bool> LButtonStateChanged;
        public event Action<bool> RButtonStateChanged;
        public event Action<bool> DPadUpStateChanged;
        public event Action<bool> DPadDownStateChanged;
        public event Action<bool> DPadLeftStateChanged;
        public event Action<bool> DPadRightStateChanged;

        // 状态跟踪
        private bool _isConnected;
        private bool _startButtonPressed;
        private bool _backButtonPressed;
        private bool _aButtonPressed;
        private bool _bButtonPressed;
        private bool _xButtonPressed;
        private bool _lbPressed;
        private bool _rbPressed;
        private bool _lButtonPressed;
        private bool _rButtonPressed;
        private bool _dPadUpPressed;
        private bool _dPadDownPressed;
        private bool _dPadLeftPressed;
        private bool _dPadRightPressed;

        public void StartMonitoring()
        {
            _isRunning = true;
            _cts = new CancellationTokenSource();
            _pollingTask = Task.Run(PollControllerState, _cts.Token);
        }

        public void StopMonitoring()
        {
            _isRunning = false;
            _cts?.Cancel();
            _pollingTask?.Wait();
        }

        public void SetVibration(ushort leftMotor, ushort rightMotor)
        {
            if (!_isConnected) return;

            var vibration = new XInputVibration
            {
                wLeftMotorSpeed = leftMotor,
                wRightMotorSpeed = rightMotor
            };

            NativeMethods.XInputSetState(_controllerIndex, ref vibration);
        }

        // 震动
        public void Vibrate(ushort leftMotor, ushort rightMotor, int duration)
        {
            if (!_isConnected) return;
            SetVibration(leftMotor, rightMotor);
            DispatcherTimer vibrateTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(duration) };
            vibrateTimer.Tick += (s, e) =>
            {
                SetVibration(0, 0);
                vibrateTimer.Stop();
            };
            vibrateTimer.Start();
        }

        private void PollControllerState()
        {
            while (_isRunning)
            {
                var result = NativeMethods.XInputGetState(_controllerIndex, out var state);
                var isConnected = result == 0;

                // 连接状态变化
                if (isConnected != _isConnected)
                {
                    _isConnected = isConnected;
                    ConnectionStatusChanged?.Invoke(isConnected);
                }

                if (isConnected)
                {
                    // 处理左摇杆
                    var lx = NormalizeThumbValue(state.Gamepad.sThumbLX);
                    var ly = NormalizeThumbValue(state.Gamepad.sThumbLY);
                    StickMoved?.Invoke(lx, ly);

                    // 处理右摇杆
                    var rx = NormalizeThumbValue(state.Gamepad.sThumbRX);
                    var ry = NormalizeThumbValue(state.Gamepad.sThumbRY);
                    StickMoved?.Invoke(rx, ry);

                    // 处理扳机键
                    LeftTriggerChanged?.Invoke(state.Gamepad.bLeftTrigger);
                    RightTriggerChanged?.Invoke(state.Gamepad.bRightTrigger);


                    // 处理Start按钮
                    var startPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_START) != 0;
                    if (startPressed != _startButtonPressed)
                    {
                        _startButtonPressed = startPressed;
                        StartButtonStateChanged?.Invoke(startPressed);
                    }

                    // 处理Back按钮
                    var backPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_BACK) != 0;
                    if (backPressed != _backButtonPressed)
                    {
                        _backButtonPressed = backPressed;
                        BackButtonStateChanged?.Invoke(backPressed);
                    }

                    // 处理A按钮
                    var aPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_A) != 0;
                    if (aPressed != _aButtonPressed)
                    {
                        _aButtonPressed = aPressed;
                        AButtonStateChanged?.Invoke(aPressed);
                    }

                    // 处理B按钮
                    var bPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_B) != 0;
                    if (bPressed != _bButtonPressed)
                    {
                        _bButtonPressed = bPressed;
                        BButtonStateChanged?.Invoke(bPressed);
                    }

                    // 处理X按钮
                    var xPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_X) != 0;
                    if (xPressed != _xButtonPressed)
                    {
                        _xButtonPressed = xPressed;
                        XButtonStateChanged?.Invoke(xPressed);
                    }

                    // 处理LB按钮
                    var lbPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_LEFT_SHOULDER) != 0;
                    if (lbPressed != _lbPressed)
                    {
                        _lbPressed = lbPressed;
                        LBStateChanged?.Invoke(lbPressed);
                    }

                    // 处理RB按钮
                    var rbPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_RIGHT_SHOULDER) != 0;
                    if (rbPressed != _rbPressed)
                    {
                        _rbPressed = rbPressed;
                        RBStateChanged?.Invoke(rbPressed);
                    }

                    // 处理L按钮
                    var lButtonPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_LEFT_THUMB) != 0;
                    if (lButtonPressed != _lButtonPressed)
                    {
                        _lButtonPressed = lButtonPressed;
                        LButtonStateChanged?.Invoke(lButtonPressed);
                    }

                    // 处理R按钮
                    var rButtonPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_RIGHT_THUMB) != 0;
                    if (rButtonPressed != _rButtonPressed)
                    {
                        _rButtonPressed = rButtonPressed;
                        RButtonStateChanged?.Invoke(rButtonPressed);
                    }

                    // 处理DPad按钮
                    var dpadUpPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_DPAD_UP) != 0;
                    if (dpadUpPressed != _dPadUpPressed)
                    {
                        _dPadUpPressed = dpadUpPressed;
                        DPadUpStateChanged?.Invoke(dpadUpPressed);
                    }
                    var dpadDownPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_DPAD_DOWN) != 0;
                    if (dpadDownPressed != _dPadDownPressed)
                    {
                        _dPadDownPressed = dpadDownPressed;
                        DPadDownStateChanged?.Invoke(dpadDownPressed);
                    }
                    var dpadLeftPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_DPAD_LEFT) != 0;
                    if (dpadLeftPressed != _dPadLeftPressed)
                    {
                        _dPadLeftPressed = dpadLeftPressed;
                        DPadLeftStateChanged?.Invoke(dpadLeftPressed);
                    }
                    var dpadRightPressed = (state.Gamepad.wButtons & Constants.XINPUT_GAMEPAD_DPAD_RIGHT) != 0;
                    if (dpadRightPressed != _dPadRightPressed)
                    {
                        _dPadRightPressed = dpadRightPressed;
                        DPadRightStateChanged?.Invoke(dpadRightPressed);
                    }
                }

                // 控制 polling 频率 (约100Hz)
                Thread.Sleep(10);
            }
        }

        // 将摇杆值标准化到 [-1.0, 1.0] 范围
        private float NormalizeThumbValue(short value)
        {
            if (value == 0) return 0;
            return (float)value / (value > 0 ? Constants.MAX_THUMB_VALUE : -Constants.MIN_THUMB_VALUE);
        }

        public void Dispose()
        {
            StopMonitoring();
            _cts?.Dispose();
        }
    }
}
