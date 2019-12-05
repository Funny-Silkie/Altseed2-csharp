using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace asd {
    struct MemoryHandle {
        public IntPtr selfPtr;
        public MemoryHandle(IntPtr p) {
            this.selfPtr = p;
        }
    }
    
    public enum ResourceType : int {
        StaticFile,
        StreamFile,
        Texture2D,
        Font,
        MAX,
    }
    
    public enum Keys : int {
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
    
    public enum ButtonState : int {
        Free = 0,
        Push = 1,
        Hold = 3,
        Release = 2,
    }
    
    public enum DeviceType : int {
    }
    
    public class Core {
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
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Core_Initialize([MarshalAs(UnmanagedType.LPWStr)] string title, int width, int height, ref CoreOption option);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Core_DoEvent(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Core_Terminate();
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_Core_GetInstance();
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Core_Release(IntPtr selfPtr);
        
        
        internal Core(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public static bool Initialize(string title, int width, int height, ref CoreOption option) {
            var ret = cbg_Core_Initialize(title, width, height, ref option);
            return ret;
        }
        
        public bool DoEvent() {
            var ret = cbg_Core_DoEvent(selfPtr);
            return ret;
        }
        
        public static void Terminate() {
            cbg_Core_Terminate();
        }
        
        public static Core GetInstance() {
            var ret = cbg_Core_GetInstance();
            return Core.TryGetFromCache(ret);
        }
        
        ~Core() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_Core_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class Window {
        private static Dictionary<IntPtr, WeakReference<Window>> cacheRepo = new Dictionary<IntPtr, WeakReference<Window>>();
        
        internal static Window TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Window cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Window_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Window(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Window>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Window_Release(IntPtr selfPtr);
        
        
        internal Window(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        ~Window() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_Window_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class Int8Array {
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
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Int8Array_CopyTo(IntPtr selfPtr, IntPtr array, int size);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Int8Array_Release(IntPtr selfPtr);
        
        
        internal Int8Array(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public void CopyTo(Int8Array array, int size) {
            cbg_Int8Array_CopyTo(selfPtr, array != null ? array.selfPtr : IntPtr.Zero, size);
        }
        
        ~Int8Array() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_Int8Array_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class Resources {
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
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_Resources_GetInstance();
        
        [DllImport("Altseed_Core.dll")]
        private static extern int cbg_Resources_GetResourcesCount(IntPtr selfPtr, int type);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Resources_Clear(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Resources_Reload(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Resources_Release(IntPtr selfPtr);
        
        
        internal Resources(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public static Resources GetInstance() {
            var ret = cbg_Resources_GetInstance();
            return Resources.TryGetFromCache(ret);
        }
        
        public int GetResourcesCount(ResourceType type) {
            var ret = cbg_Resources_GetResourcesCount(selfPtr, (int)type);
            return ret;
        }
        
        public void Clear() {
            cbg_Resources_Clear(selfPtr);
        }
        
        public void Reload() {
            cbg_Resources_Reload(selfPtr);
        }
        
        ~Resources() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_Resources_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class Keyboard {
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
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Keyboard_Initialize(IntPtr selfPtr, IntPtr window);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Keyboard_RefleshKeyStates(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern int cbg_Keyboard_GetKeyState(IntPtr selfPtr, int key);
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_Keyboard_GetInstance();
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Keyboard_Release(IntPtr selfPtr);
        
        
        internal Keyboard(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public bool Initialize(Window window) {
            var ret = cbg_Keyboard_Initialize(selfPtr, window != null ? window.selfPtr : IntPtr.Zero);
            return ret;
        }
        
        public void RefleshKeyStates() {
            cbg_Keyboard_RefleshKeyStates(selfPtr);
        }
        
        public ButtonState GetKeyState(Keys key) {
            var ret = cbg_Keyboard_GetKeyState(selfPtr, (int)key);
            return (ButtonState)ret;
        }
        
        public static Keyboard GetInstance() {
            var ret = cbg_Keyboard_GetInstance();
            return Keyboard.TryGetFromCache(ret);
        }
        
        ~Keyboard() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_Keyboard_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class Graphics {
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
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_Graphics_GetInstance();
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Graphics_Update(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Graphics_Release(IntPtr selfPtr);
        
        
        internal Graphics(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public static Graphics GetInstance() {
            var ret = cbg_Graphics_GetInstance();
            return Graphics.TryGetFromCache(ret);
        }
        
        public bool Update() {
            var ret = cbg_Graphics_Update(selfPtr);
            return ret;
        }
        
        ~Graphics() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_Graphics_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class Texture2D {
        private static Dictionary<IntPtr, WeakReference<Texture2D>> cacheRepo = new Dictionary<IntPtr, WeakReference<Texture2D>>();
        
        internal static Texture2D TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                Texture2D cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_Texture2D_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new Texture2D(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<Texture2D>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_Texture2D_Reload(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern Vector2DI cbg_Texture2D_GetSize(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_Texture2D_Release(IntPtr selfPtr);
        
        
        internal Texture2D(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public bool Reload() {
            var ret = cbg_Texture2D_Reload(selfPtr);
            return ret;
        }
        
        public Vector2DI GetSize() {
            var ret = cbg_Texture2D_GetSize(selfPtr);
            return ret;
        }
        
        ~Texture2D() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_Texture2D_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class StreamFile {
        private static Dictionary<IntPtr, WeakReference<StreamFile>> cacheRepo = new Dictionary<IntPtr, WeakReference<StreamFile>>();
        
        internal static StreamFile TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                StreamFile cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_StreamFile_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new StreamFile(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<StreamFile>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core.dll")]
        private static extern int cbg_StreamFile_GetSize(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern int cbg_StreamFile_GetCurrentPosition(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern int cbg_StreamFile_Read(IntPtr selfPtr, int size);
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_StreamFile_GetTempBuffer(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern int cbg_StreamFile_GetTempBufferSize(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_StreamFile_GetIsInPackage(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_StreamFile_Reload(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_StreamFile_Release(IntPtr selfPtr);
        
        
        internal StreamFile(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public int GetSize() {
            var ret = cbg_StreamFile_GetSize(selfPtr);
            return ret;
        }
        
        public int GetCurrentPosition() {
            var ret = cbg_StreamFile_GetCurrentPosition(selfPtr);
            return ret;
        }
        
        public int Read(int size) {
            var ret = cbg_StreamFile_Read(selfPtr, size);
            return ret;
        }
        
        public Int8Array GetTempBuffer() {
            var ret = cbg_StreamFile_GetTempBuffer(selfPtr);
            return Int8Array.TryGetFromCache(ret);
        }
        
        public int GetTempBufferSize() {
            var ret = cbg_StreamFile_GetTempBufferSize(selfPtr);
            return ret;
        }
        
        public bool GetIsInPackage() {
            var ret = cbg_StreamFile_GetIsInPackage(selfPtr);
            return ret;
        }
        
        public bool Reload() {
            var ret = cbg_StreamFile_Reload(selfPtr);
            return ret;
        }
        
        ~StreamFile() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_StreamFile_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class StaticFile {
        private static Dictionary<IntPtr, WeakReference<StaticFile>> cacheRepo = new Dictionary<IntPtr, WeakReference<StaticFile>>();
        
        internal static StaticFile TryGetFromCache(IntPtr native)
        {
            if(native == null) return null;
        
            if(cacheRepo.ContainsKey(native))
            {
                StaticFile cacheRet;
                cacheRepo[native].TryGetTarget(out cacheRet);
                if(cacheRet != null)
                {
                    cbg_StaticFile_Release(native);
                    return cacheRet;
                }
                else
                {
                    cacheRepo.Remove(native);
                }
            }
        
            var newObject = new StaticFile(new MemoryHandle(native));
            cacheRepo[native] = new WeakReference<StaticFile>(newObject);
            return newObject;
        }
        
        internal IntPtr selfPtr = IntPtr.Zero;
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_StaticFile_GetBuffer(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_StaticFile_GetPath(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern int cbg_StaticFile_GetSize(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_StaticFile_GetIsInPackage(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_StaticFile_Reload(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_StaticFile_Release(IntPtr selfPtr);
        
        
        internal StaticFile(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public Int8Array GetBuffer() {
            var ret = cbg_StaticFile_GetBuffer(selfPtr);
            return Int8Array.TryGetFromCache(ret);
        }
        
        public string GetPath() {
            var ret = cbg_StaticFile_GetPath(selfPtr);
            return System.Runtime.InteropServices.Marshal.PtrToStringUni(ret);
        }
        
        public int GetSize() {
            var ret = cbg_StaticFile_GetSize(selfPtr);
            return ret;
        }
        
        public bool GetIsInPackage() {
            var ret = cbg_StaticFile_GetIsInPackage(selfPtr);
            return ret;
        }
        
        public bool Reload() {
            var ret = cbg_StaticFile_Reload(selfPtr);
            return ret;
        }
        
        ~StaticFile() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_StaticFile_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
    public class File {
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
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_File_GetInstance();
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_File_CreateStaticFile(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core.dll")]
        private static extern IntPtr cbg_File_CreateStreamFile(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_AddRootDirectory(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_AddRootPackageWithPassword(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.LPWStr)] string password);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_AddRootPackage(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_File_ClearRootDirectories(IntPtr selfPtr);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_Exists(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string path);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_Pack(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string srcPath, [MarshalAs(UnmanagedType.LPWStr)] string dstPath);
        
        [DllImport("Altseed_Core.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool cbg_File_PackWithPassword(IntPtr selfPtr, [MarshalAs(UnmanagedType.LPWStr)] string srcPath, [MarshalAs(UnmanagedType.LPWStr)] string dstPath, [MarshalAs(UnmanagedType.LPWStr)] string password);
        
        [DllImport("Altseed_Core.dll")]
        private static extern void cbg_File_Release(IntPtr selfPtr);
        
        
        internal File(MemoryHandle handle) {
            this.selfPtr = handle.selfPtr;
        }
        
        public static File GetInstance() {
            var ret = cbg_File_GetInstance();
            return File.TryGetFromCache(ret);
        }
        
        public StaticFile CreateStaticFile(string path) {
            var ret = cbg_File_CreateStaticFile(selfPtr, path);
            return StaticFile.TryGetFromCache(ret);
        }
        
        public StreamFile CreateStreamFile(string path) {
            var ret = cbg_File_CreateStreamFile(selfPtr, path);
            return StreamFile.TryGetFromCache(ret);
        }
        
        public bool AddRootDirectory(string path) {
            var ret = cbg_File_AddRootDirectory(selfPtr, path);
            return ret;
        }
        
        public bool AddRootPackageWithPassword(string path, string password) {
            var ret = cbg_File_AddRootPackageWithPassword(selfPtr, path, password);
            return ret;
        }
        
        public bool AddRootPackage(string path) {
            var ret = cbg_File_AddRootPackage(selfPtr, path);
            return ret;
        }
        
        public void ClearRootDirectories() {
            cbg_File_ClearRootDirectories(selfPtr);
        }
        
        public bool Exists(string path) {
            var ret = cbg_File_Exists(selfPtr, path);
            return ret;
        }
        
        public bool Pack(string srcPath, string dstPath) {
            var ret = cbg_File_Pack(selfPtr, srcPath, dstPath);
            return ret;
        }
        
        public bool PackWithPassword(string srcPath, string dstPath, string password) {
            var ret = cbg_File_PackWithPassword(selfPtr, srcPath, dstPath, password);
            return ret;
        }
        
        ~File() {
            lock (this)  {
                if (selfPtr != IntPtr.Zero) {
                    cbg_File_Release(selfPtr);
                    selfPtr = IntPtr.Zero;
                }
            }
        }
    }
    
}
