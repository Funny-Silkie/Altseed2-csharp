using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Altseed
{
    struct MemoryHandle
    {
        public IntPtr selfPtr;
        public MemoryHandle(IntPtr p)
        {
            this.selfPtr = p;
        }
    }
    
    [System.Serializable]
    public enum ResourceType : int
    {
        StaticFile,
        StreamFile,
        Texture2D,
        Font,
        MAX,
    }
    
    [System.Serializable]
    public enum Keys : int
    {
        Unknown,
        Space,
        Apostrophe,
        Comma,
        Minus,
        Period,
        Slash,
        Num0,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Num9,
        Semicolon,
        Equal,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        LeftBracket,
        Backslash,
        RightBracket,
        GraveAccent,
        World1,
        World2,
        Escape,
        Enter,
        Tab,
        Backspace,
        Insert,
        Delete,
        Right,
        Left,
        Down,
        Up,
        PageUp,
        PageDown,
        Home,
        End,
        CapsLock,
        ScrollLock,
        NumLock,
        PrintScreen,
        Pause,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,
        F25,
        Keypad0,
        Keypad1,
        Keypad2,
        Keypad3,
        Keypad4,
        Keypad5,
        Keypad6,
        Keypad7,
        Keypad8,
        Keypad9,
        KeypadDecimal,
        KeypadDivide,
        KeypadMultiply,
        KeypadSubstract,
        KeypadAdd,
        KeypadEnter,
        KeypadEqual,
        LeftShift,
        LeftControl,
        LeftAlt,
        LeftWin,
        RightShift,
        RightControl,
        RightAlt,
        RightWin,
        Menu,
        Last,
        MAX,
    }
    
    [System.Serializable]
    public enum ButtonState : int
    {
        Free = 0,
        Push = 1,
        Hold = 3,
        Release = 2,
    }
    
    [System.Serializable]
    public enum MouseButtons : int
    {
        ButtonLeft = 0,
        ButtonRight = 1,
        ButtonMiddle = 2,
        SubButton1 = 3,
        SubButton2 = 4,
        SubButton3 = 5,
        SubButton4 = 6,
        SubButton5 = 7,
    }
    
    [System.Serializable]
    public enum CursorMode : int
    {
        Normal = 212993,
        Hidden = 212994,
        Disable = 212995,
    }
    
    [System.Serializable]
    public enum JoystickType : int
    {
        Other = 0,
        PS4 = 8200,
        XBOX360 = 8199,
        JoyconL = 8198,
        JoyconR = 8197,
    }
    
    [System.Serializable]
    public enum JoystickButtonType : int
    {
        Start,
        Select,
        Home,
        Release,
        Capture,
        LeftUp,
        LeftDown,
        LeftLeft,
        LeftRight,
        LeftPush,
        RightUp,
        RightRight,
        RightLeft,
        RightDown,
        RightPush,
        L1,
        R1,
        L2,
        R2,
        L3,
        R3,
        LeftStart,
        RightStart,
        Max,
    }
    
    [System.Serializable]
    public enum JoystickAxisType : int
    {
        Start,
        LeftH,
        LeftV,
        RightH,
        RightV,
        L2,
        R2,
        Max,
    }
    
    [System.Serializable]
    public enum DeviceType : int
    {
    }
    
    [System.Serializable]
    public enum EasingType : int
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack,
        InElastic,
        OutElastic,
        InOutElastic,
        InBounce,
        OutBounce,
        InOutBounce,
    }
    
    /// <summary>
    /// C++のAltseedCoreとの仲介クラス
    /// </summary>
    public partial class Core
    {
        private static Dictionary<IntPtr, WeakReference<Core>> cacheRepo = new Dictionary<IntPtr, WeakReference<Core>>();
        
        internal static Core TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Core cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Core_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Core(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Core>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Core_Initialize([MarshalAs(UnmanagedType.LPWStr)] string title, int width, int height, ref CoreOption option);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Core_DoEvent(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Core_Terminate();
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_Core_GetInstance();
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Core_Release(IntPtr selfPtr);
        
        
        internal Core(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// 初期化を行う
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="option"></param>
        public static bool Initialize(string title, int width, int height, ref CoreOption option)
        {
            var ret = cbg_Core_Initialize(title, width, height, ref option);
            return ret;
        }
        
        /// <summary>
        /// イベントを実行し成否を返す
        /// </summary>
        public bool DoEvent()
        {
            var ret = cbg_Core_DoEvent(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// 終了処理を行う
        /// </summary>
        public static void Terminate()
        {
            cbg_Core_Terminate();
        }
        
        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        public static Core GetInstance()
        {
            var ret = cbg_Core_GetInstance();
            return Core.TryGetFromCache(ret);
        }
        
        ~Core()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_Core_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// 8ビット整数の配列のクラス
    /// </summary>
    public partial class Int8Array
    {
        private static Dictionary<IntPtr, WeakReference<Int8Array>> cacheRepo = new Dictionary<IntPtr, WeakReference<Int8Array>>();
        
        internal static Int8Array TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Int8Array cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Int8Array_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Int8Array(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Int8Array>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Int8Array_CopyTo(IntPtr selfPtr, IntPtr array, int size);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Int8Array_Release(IntPtr selfPtr);
        
        
        internal Int8Array(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// 指定したインスタンスにデータをコピーする
        /// </summary>
        /// <param name="array"></param>
        /// <param name="size"></param>
        public void CopyTo(Int8Array array, int size)
        {
            cbg_Int8Array_CopyTo(selfPtr, array != null ? array.selfPtr : IntPtr.Zero, size);
        }
        
        ~Int8Array()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_Int8Array_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// リソースのクラス
    /// </summary>
    public partial class Resources
    {
        private static Dictionary<IntPtr, WeakReference<Resources>> cacheRepo = new Dictionary<IntPtr, WeakReference<Resources>>();
        
        internal static Resources TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Resources cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Resources_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Resources(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Resources>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_Resources_GetInstance();
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_Resources_GetResourcesCount(IntPtr selfPtr, int type);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Resources_Clear(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Resources_Reload(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Resources_Release(IntPtr selfPtr);
        
        
        internal Resources(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        public static Resources GetInstance()
        {
            var ret = cbg_Resources_GetInstance();
            return Resources.TryGetFromCache(ret);
        }
        
        /// <summary>
        /// リソースの個数を返す
        /// </summary>
        /// <param name="type"></param>
        public int GetResourcesCount(ResourceType type)
        {
            var ret = cbg_Resources_GetResourcesCount(selfPtr, (int)type);
            return ret;
        }
        
        /// <summary>
        /// 登録されているリソースをすべて削除する
        /// </summary>
        public void Clear()
        {
            cbg_Resources_Clear(selfPtr);
        }
        
        /// <summary>
        /// 再読み込みを行う
        /// </summary>
        public void Reload()
        {
            cbg_Resources_Reload(selfPtr);
        }
        
        ~Resources()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_Resources_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// キーボードを表します。
    /// </summary>
    public partial class Keyboard
    {
        private static Dictionary<IntPtr, WeakReference<Keyboard>> cacheRepo = new Dictionary<IntPtr, WeakReference<Keyboard>>();
        
        internal static Keyboard TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Keyboard cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Keyboard_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Keyboard(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Keyboard>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_Keyboard_GetKeyState(IntPtr selfPtr, int key);
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_Keyboard_GetInstance();
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Keyboard_Release(IntPtr selfPtr);
        
        
        internal Keyboard(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// キーの状態を取得します。
        /// </summary>
        /// <param name="key"></param>
        public ButtonState GetKeyState(Keys key)
        {
            var ret = cbg_Keyboard_GetKeyState(selfPtr, (int)key);
            return (ButtonState)ret;
        }
        
        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        internal static Keyboard GetInstance()
        {
            var ret = cbg_Keyboard_GetInstance();
            return Keyboard.TryGetFromCache(ret);
        }
        
        ~Keyboard()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_Keyboard_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// マウスを表します。
    /// </summary>
    public partial class Mouse
    {
        private static Dictionary<IntPtr, WeakReference<Mouse>> cacheRepo = new Dictionary<IntPtr, WeakReference<Mouse>>();
        
        internal static Mouse TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Mouse cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Mouse_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Mouse(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Mouse>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_Mouse_GetInstance(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Mouse_RefreshInputState(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Mouse_SetPosition(IntPtr selfPtr, ref Vector2DF vec);
        
        [DllImport("Altseed_Core")]
        private static extern Vector2DF cbg_Mouse_GetPosition(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern float cbg_Mouse_GetWheel(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Mouse_GetMouseButtonState(IntPtr selfPtr, int button);
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_Mouse_GetCursorMode(IntPtr selfPtr);
        [DllImport("Altseed_Core")]
        private static extern void cbg_Mouse_SetCursorMode(IntPtr selfPtr, int value);
        
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Mouse_Release(IntPtr selfPtr);
        
        
        internal Mouse(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        public CursorMode CursorMode
        {
            get
            {
                if (_CursorMode != null)
                {
                    return _CursorMode.Value;
                }
                var ret = cbg_Mouse_GetCursorMode(selfPtr);
                return (CursorMode)ret;
            }
            set
            {
                _CursorMode = value;
                cbg_Mouse_SetCursorMode(selfPtr, (int)value);
            }
        }
        private CursorMode? _CursorMode;
        
        internal Mouse GetInstance()
        {
            var ret = cbg_Mouse_GetInstance(selfPtr);
            return Mouse.TryGetFromCache(ret);
        }
        
        internal void RefreshInputState()
        {
            cbg_Mouse_RefreshInputState(selfPtr);
        }
        
        /// <summary>
        /// マウスカーソルの座標を設定します。
        /// </summary>
        /// <param name="vec"></param>
        public void SetPosition(ref Vector2DF vec)
        {
            cbg_Mouse_SetPosition(selfPtr, ref vec);
        }
        
        /// <summary>
        /// マウスカーソルの座標を取得します。
        /// </summary>
        public Vector2DF GetPosition()
        {
            var ret = cbg_Mouse_GetPosition(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// マウスホイールの回転量を取得します。
        /// </summary>
        public float GetWheel()
        {
            var ret = cbg_Mouse_GetWheel(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// マウスボタンの状態を取得します。
        /// </summary>
        /// <param name="button"></param>
        public void GetMouseButtonState(MouseButtons button)
        {
            cbg_Mouse_GetMouseButtonState(selfPtr, (int)button);
        }
        
        ~Mouse()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_Mouse_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// ジョイスティックのクラス
    /// </summary>
    public partial class Joystick
    {
        private static Dictionary<IntPtr, WeakReference<Joystick>> cacheRepo = new Dictionary<IntPtr, WeakReference<Joystick>>();
        
        internal static Joystick TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Joystick cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Joystick_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Joystick(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Joystick>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Joystick_IsPresent(IntPtr selfPtr, int joystickIndex);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Joystick_RefreshInputState(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Joystick_RefreshConnectedState(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_Joystick_GetButtonStateByIndex(IntPtr selfPtr, int joystickIndex, int buttonIndex);
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_Joystick_GetButtonStateByType(IntPtr selfPtr, int joystickIndex, int type);
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_Joystick_GetJoystickType(IntPtr selfPtr, int index);
        
        [DllImport("Altseed_Core")]
        private static extern float cbg_Joystick_GetAxisStateByIndex(IntPtr selfPtr, int joystickIndex, int axisIndex);
        
        [DllImport("Altseed_Core")]
        private static extern float cbg_Joystick_GetAxisStateByType(IntPtr selfPtr, int joystickIndex, int type);
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_Joystick_GetJoystickName(IntPtr selfPtr, int index);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Joystick_RefreshVibrateState(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Joystick_SetVibration(IntPtr selfPtr, int index, float high_freq, float low_freq, float high_amp, float low_amp, int life_time);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Joystick_Release(IntPtr selfPtr);
        
        
        internal Joystick(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        public void IsPresent(int joystickIndex)
        {
            cbg_Joystick_IsPresent(selfPtr, joystickIndex);
        }
        
        public void RefreshInputState()
        {
            cbg_Joystick_RefreshInputState(selfPtr);
        }
        
        public void RefreshConnectedState()
        {
            cbg_Joystick_RefreshConnectedState(selfPtr);
        }
        
        public ButtonState GetButtonStateByIndex(int joystickIndex, int buttonIndex)
        {
            var ret = cbg_Joystick_GetButtonStateByIndex(selfPtr, joystickIndex, buttonIndex);
            return (ButtonState)ret;
        }
        
        public ButtonState GetButtonStateByType(int joystickIndex, JoystickButtonType type)
        {
            var ret = cbg_Joystick_GetButtonStateByType(selfPtr, joystickIndex, (int)type);
            return (ButtonState)ret;
        }
        
        public JoystickType GetJoystickType(int index)
        {
            var ret = cbg_Joystick_GetJoystickType(selfPtr, index);
            return (JoystickType)ret;
        }
        
        public float GetAxisStateByIndex(int joystickIndex, int axisIndex)
        {
            var ret = cbg_Joystick_GetAxisStateByIndex(selfPtr, joystickIndex, axisIndex);
            return ret;
        }
        
        public float GetAxisStateByType(int joystickIndex, JoystickAxisType type)
        {
            var ret = cbg_Joystick_GetAxisStateByType(selfPtr, joystickIndex, (int)type);
            return ret;
        }
        
        public string GetJoystickName(int index)
        {
            var ret = cbg_Joystick_GetJoystickName(selfPtr, index);
            return System.Runtime.InteropServices.Marshal.PtrToStringUni(ret);
        }
        
        public void RefreshVibrateState()
        {
            cbg_Joystick_RefreshVibrateState(selfPtr);
        }
        
        public void SetVibration(int index, float high_freq, float low_freq, float high_amp, float low_amp, int life_time)
        {
            cbg_Joystick_SetVibration(selfPtr, index, high_freq, low_freq, high_amp, low_amp, life_time);
        }
        
        ~Joystick()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_Joystick_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// グラフィックの処理を行うクラス
    /// </summary>
    public partial class Graphics
    {
        private static Dictionary<IntPtr, WeakReference<Graphics>> cacheRepo = new Dictionary<IntPtr, WeakReference<Graphics>>();
        
        internal static Graphics TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Graphics cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Graphics_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Graphics(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Graphics>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_Graphics_GetInstance();
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Graphics_BeginFrame(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Graphics_EndFrame(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Graphics_Release(IntPtr selfPtr);
        
        
        internal Graphics(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        public static Graphics GetInstance()
        {
            var ret = cbg_Graphics_GetInstance();
            return Graphics.TryGetFromCache(ret);
        }
        
        public bool BeginFrame()
        {
            var ret = cbg_Graphics_BeginFrame(selfPtr);
            return ret;
        }
        
        public bool EndFrame()
        {
            var ret = cbg_Graphics_EndFrame(selfPtr);
            return ret;
        }
        
        ~Graphics()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_Graphics_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// 2D画像を表すクラス
    /// </summary>
    public partial class CoreTexture2D
    {
        private static Dictionary<IntPtr, WeakReference<CoreTexture2D>> cacheRepo = new Dictionary<IntPtr, WeakReference<CoreTexture2D>>();
        
        internal static CoreTexture2D TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                CoreTexture2D cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_CoreTexture2D_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new CoreTexture2D(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<CoreTexture2D>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_CoreTexture2D_Reload(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern Vector2DI cbg_CoreTexture2D_GetSize(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_CoreTexture2D_Release(IntPtr selfPtr);
        
        
        internal CoreTexture2D(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// 再読み込みを行う
        /// </summary>
        public bool Reload()
        {
            var ret = cbg_CoreTexture2D_Reload(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// テクスチャのサイズ(ピクセル)を返す
        /// </summary>
        public Vector2DI GetSize()
        {
            var ret = cbg_CoreTexture2D_GetSize(selfPtr);
            return ret;
        }
        
        ~CoreTexture2D()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_CoreTexture2D_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// 段階的に読み込むファイルのクラス
    /// </summary>
    public partial class CoreStreamFile
    {
        private static Dictionary<IntPtr, WeakReference<CoreStreamFile>> cacheRepo = new Dictionary<IntPtr, WeakReference<CoreStreamFile>>();
        
        internal static CoreStreamFile TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                CoreStreamFile cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_CoreStreamFile_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new CoreStreamFile(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<CoreStreamFile>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_CoreStreamFile_GetSize(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_CoreStreamFile_GetCurrentPosition(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_CoreStreamFile_Read(IntPtr selfPtr, int size);
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_CoreStreamFile_GetTempBuffer(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_CoreStreamFile_GetTempBufferSize(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_CoreStreamFile_GetIsInPackage(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_CoreStreamFile_Reload(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_CoreStreamFile_Release(IntPtr selfPtr);
        
        
        internal CoreStreamFile(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// ファイルのサイズを返す
        /// </summary>
        internal int GetSize()
        {
            var ret = cbg_CoreStreamFile_GetSize(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// 現在の読み取り位置を返す
        /// </summary>
        internal int GetCurrentPosition()
        {
            var ret = cbg_CoreStreamFile_GetCurrentPosition(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// 読み込みを行う
        /// </summary>
        /// <param name="size"></param>
        internal int Read(int size)
        {
            var ret = cbg_CoreStreamFile_Read(selfPtr, size);
            return ret;
        }
        
        /// <summary>
        /// 読み込まれたデータを返す
        /// </summary>
        internal Int8Array GetTempBuffer()
        {
            var ret = cbg_CoreStreamFile_GetTempBuffer(selfPtr);
            return Int8Array.TryGetFromCache(ret);
        }
        
        /// <summary>
        /// 現在読み込まれたデータのサイズを返す
        /// </summary>
        internal int GetTempBufferSize()
        {
            var ret = cbg_CoreStreamFile_GetTempBufferSize(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// ファイルパッケージ内にあるかどうかを返す
        /// </summary>
        internal bool GetIsInPackage()
        {
            var ret = cbg_CoreStreamFile_GetIsInPackage(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// 再読み込みを行う
        /// </summary>
        internal bool Reload()
        {
            var ret = cbg_CoreStreamFile_Reload(selfPtr);
            return ret;
        }
        
        ~CoreStreamFile()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_CoreStreamFile_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// 一度に読み込まれるファイルのクラス
    /// </summary>
    public partial class CoreStaticFile
    {
        private static Dictionary<IntPtr, WeakReference<CoreStaticFile>> cacheRepo = new Dictionary<IntPtr, WeakReference<CoreStaticFile>>();
        
        internal static CoreStaticFile TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                CoreStaticFile cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_CoreStaticFile_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new CoreStaticFile(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<CoreStaticFile>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_CoreStaticFile_GetBuffer(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_CoreStaticFile_GetPath(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern int cbg_CoreStaticFile_GetSize(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_CoreStaticFile_GetIsInPackage(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_CoreStaticFile_Reload(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_CoreStaticFile_Release(IntPtr selfPtr);
        
        
        internal CoreStaticFile(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// データをInt8Arrayの形式で返す
        /// </summary>
        internal Int8Array GetBuffer()
        {
            var ret = cbg_CoreStaticFile_GetBuffer(selfPtr);
            return Int8Array.TryGetFromCache(ret);
        }
        
        /// <summary>
        /// ファイルのパスを返す
        /// </summary>
        internal string GetPath()
        {
            var ret = cbg_CoreStaticFile_GetPath(selfPtr);
            return System.Runtime.InteropServices.Marshal.PtrToStringUni(ret);
        }
        
        /// <summary>
        /// データの大きさを返す
        /// </summary>
        internal int GetSize()
        {
            var ret = cbg_CoreStaticFile_GetSize(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// ファイルパッケージ内かどうかを返す
        /// </summary>
        internal bool GetIsInPackage()
        {
            var ret = cbg_CoreStaticFile_GetIsInPackage(selfPtr);
            return ret;
        }
        
        /// <summary>
        /// ファイルの再読み込みを実行する
        /// </summary>
        internal bool Reload()
        {
            var ret = cbg_CoreStaticFile_Reload(selfPtr);
            return ret;
        }
        
        ~CoreStaticFile()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_CoreStaticFile_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    /// <summary>
    /// ファイルの読み込みなどの操作を扱うクラス
    /// </summary>
    public partial class File
    {
        private static Dictionary<IntPtr, WeakReference<File>> cacheRepo = new Dictionary<IntPtr, WeakReference<File>>();
        
        internal static File TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                File cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_File_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new File(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<File>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_File_GetInstance();
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_File_CreateStaticFile(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core")]
        private static extern IntPtr cbg_File_CreateStreamFile(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_AddRootDirectory(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_AddRootPackageWithPassword(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.LPWStr)] string password);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_AddRootPackage(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_File_ClearRootDirectories(IntPtr selfPtr);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_Exists(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_Pack(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string srcPath, [MarshalAs(UnmanagedType.LPWStr)] string dstPath);
        
        [DllImport("Altseed_Core")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_PackWithPassword(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string srcPath, [MarshalAs(UnmanagedType.LPWStr)] string dstPath, [MarshalAs(UnmanagedType.LPWStr)] string password);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_File_Release(IntPtr selfPtr);
        
        
        internal File(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        public static File GetInstance()
        {
            var ret = cbg_File_GetInstance();
            return File.TryGetFromCache(ret);
        }
        
        /// <summary>
        /// CoreStaticFileのインスタンスを生成する
        /// </summary>
        /// <param name="path"></param>
        internal CoreStaticFile CreateStaticFile(string path)
        {
            var ret = cbg_File_CreateStaticFile(selfPtr, path);
            return CoreStaticFile.TryGetFromCache(ret);
        }
        
        /// <summary>
        /// CoreStreamFileのインスタンスを生成する
        /// </summary>
        /// <param name="path"></param>
        internal CoreStreamFile CreateStreamFile(string path)
        {
            var ret = cbg_File_CreateStreamFile(selfPtr, path);
            return CoreStreamFile.TryGetFromCache(ret);
        }
        
        /// <summary>
        /// ファイルパスの先頭に自動補完するディレクトリを追加する
        /// </summary>
        /// <param name="path"></param>
        public bool AddRootDirectory(string path)
        {
            var ret = cbg_File_AddRootDirectory(selfPtr, path);
            return ret;
        }
        
        /// <summary>
        /// 使用するファイルパッケージを追加する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="password"></param>
        public bool AddRootPackageWithPassword(string path, string password)
        {
            var ret = cbg_File_AddRootPackageWithPassword(selfPtr, path, password);
            return ret;
        }
        
        /// <summary>
        /// 使用するパスワード付きファイルパッケージを追加する
        /// </summary>
        /// <param name="path"></param>
        public bool AddRootPackage(string path)
        {
            var ret = cbg_File_AddRootPackage(selfPtr, path);
            return ret;
        }
        
        /// <summary>
        /// ファイルパッケージの登録をすべて削除する
        /// </summary>
        public void ClearRootDirectories()
        {
            cbg_File_ClearRootDirectories(selfPtr);
        }
        
        /// <summary>
        /// 指定したパスのファイルの存在の有無を返す
        /// </summary>
        /// <param name="path"></param>
        public bool Exists(string path)
        {
            var ret = cbg_File_Exists(selfPtr, path);
            return ret;
        }
        
        public bool Pack(string srcPath, string dstPath)
        {
            var ret = cbg_File_Pack(selfPtr, srcPath, dstPath);
            return ret;
        }
        
        public bool PackWithPassword(string srcPath, string dstPath, string password)
        {
            var ret = cbg_File_PackWithPassword(selfPtr, srcPath, dstPath, password);
            return ret;
        }
        
        ~File()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_File_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public partial class Easing
    {
        private static Dictionary<IntPtr, WeakReference<Easing>> cacheRepo = new Dictionary<IntPtr, WeakReference<Easing>>();
        
        internal static Easing TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Easing cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Easing_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Easing(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Easing>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Easing_GetEasing(int easing, float t);
        
        [DllImport("Altseed_Core")]
        private static extern void cbg_Easing_Release(IntPtr selfPtr);
        
        
        internal Easing(MemoryHandle handle)
        {
            this.selfPtr = handle.selfPtr;
        }
        
        public static void GetEasing(EasingType easing, float t)
        {
            cbg_Easing_GetEasing((int)easing, t);
        }
        
        ~Easing()
        {
            lock (this) 
            {
                if (selfPtr != IntPtr.Zero)
                {
                    cbg_Easing_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
}
