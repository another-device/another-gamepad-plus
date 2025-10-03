using NLua;
using System.IO;

namespace AnotherGamepadPlus.Services
{
    public class LuaScriptService
    {
        private readonly Lua _lua;
        private readonly string _scriptPath;

        public LuaScriptService(MouseService mouseService, KeyboardService keyboardService, ControllerService controllerService, string scriptPath = "config.lua")
        {
            _lua = new Lua();
            _scriptPath = Path.Combine(AppContext.BaseDirectory, scriptPath);

            // 注册C#服务到Lua环境
            _lua["MouseService"] = mouseService;
            _lua["KeyboardService"] = keyboardService;
            _lua["ControllerService"] = controllerService;

            LoadScript();
        }

        // 加载/重新加载Lua脚本
        public bool LoadScript()
        {
            if (File.Exists(_scriptPath))
            {
                try
                {
                    var results = _lua.DoFile(_scriptPath);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lua Exception: {ex.Message}");
                }
            }
            return false;
        }

        // 执行按键事件的Lua回调（不存在则返回false，使用默认行为）
        public bool InvokeButtonCallback(string buttonName, bool pressed)
        {
            if (_lua[buttonName] is LuaFunction func)
            {
                func.Call(pressed);
                return true;
            }
            return false;
        }

        // 执行摇杆事件的Lua回调
        public bool InvokeStickCallback(float x, float y)
        {
            if (_lua["OnStickMoved"] is LuaFunction func)
            {
                func.Call(x, y);
                return true;
            }
            return false;
        }

        // 执行扳机键事件的Lua回调
        public bool InvokeTriggerCallback(string triggerName, byte value)
        {
            if (_lua[triggerName] is LuaFunction func)
            {
                func.Call(value);
                return true;
            }
            return false;
        }
    }
}
