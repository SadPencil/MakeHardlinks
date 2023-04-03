namespace MakeHardlinks
{

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct BY_HANDLE_FILE_INFORMATION
    {

        /// DWORD->unsigned int
        public uint dwFileAttributes;

        /// FILETIME->_FILETIME
        public FILETIME ftCreationTime;

        /// FILETIME->_FILETIME
        public FILETIME ftLastAccessTime;

        /// FILETIME->_FILETIME
        public FILETIME ftLastWriteTime;

        /// DWORD->unsigned int
        public uint dwVolumeSerialNumber;

        /// DWORD->unsigned int
        public uint nFileSizeHigh;

        /// DWORD->unsigned int
        public uint nFileSizeLow;

        /// DWORD->unsigned int
        public uint nNumberOfLinks;

        /// DWORD->unsigned int
        public uint nFileIndexHigh;

        /// DWORD->unsigned int
        public uint nFileIndexLow;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct FILETIME
    {

        /// DWORD->unsigned int
        public uint dwLowDateTime;

        /// DWORD->unsigned int
        public uint dwHighDateTime;
    }

    public partial class NativeConstants
    {

        public static readonly System.IntPtr INVALID_HANDLE_VALUE = new System.IntPtr(-1);

        /// FILE_SHARE_READ -> 0x00000001
        public const uint FILE_SHARE_READ = 1;

        /// FILE_SHARE_WRITE -> 0x00000002
        public const uint FILE_SHARE_WRITE = 2;

        /// FILE_ATTRIBUTE_NORMAL -> 0x00000080
        public const uint FILE_ATTRIBUTE_NORMAL = 128;

        /// OPEN_EXISTING -> 3
        public const uint OPEN_EXISTING = 3;

        /// GENERIC_READ -> (0x80000000L)
        public const uint GENERIC_READ = 0x80000000;

    }

    public partial class NativeMethods
    {

        /// Return Type: BOOL->int
        ///hFile: HANDLE->void*
        ///lpFileInformation: LPBY_HANDLE_FILE_INFORMATION->_BY_HANDLE_FILE_INFORMATION*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetFileInformationByHandle")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool GetFileInformationByHandle([System.Runtime.InteropServices.InAttribute()] System.IntPtr hFile, [System.Runtime.InteropServices.OutAttribute()] out BY_HANDLE_FILE_INFORMATION lpFileInformation);

    }

    public partial class NativeMethods
    {

        /// Return Type: BOOL->int
        ///lpFileName: LPCWSTR->WCHAR*
        ///lpExistingFileName: LPCWSTR->WCHAR*
        ///lpSecurityAttributes: LPSECURITY_ATTRIBUTES->_SECURITY_ATTRIBUTES*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "CreateHardLinkW", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool CreateHardLinkW([System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpFileName, [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpExistingFileName, System.IntPtr lpSecurityAttributes);

    }

    public partial class NativeMethods
    {

        /// Return Type: HANDLE->void*
        ///lpFileName: LPCWSTR->WCHAR*
        ///dwDesiredAccess: DWORD->unsigned int
        ///dwShareMode: DWORD->unsigned int
        ///lpSecurityAttributes: LPSECURITY_ATTRIBUTES->_SECURITY_ATTRIBUTES*
        ///dwCreationDisposition: DWORD->unsigned int
        ///dwFlagsAndAttributes: DWORD->unsigned int
        ///hTemplateFile: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "CreateFileW")]
        public static extern System.IntPtr CreateFileW([System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpFileName, uint dwDesiredAccess, uint dwShareMode, [System.Runtime.InteropServices.InAttribute()] System.IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, [System.Runtime.InteropServices.InAttribute()] System.IntPtr hTemplateFile);

    }

    public partial class NativeMethods
    {

        /// Return Type: BOOL->int
        ///hObject: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "CloseHandle", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool CloseHandle([System.Runtime.InteropServices.InAttribute()] System.IntPtr hObject);

    }

    public partial class NativeMethods
    {

        /// Return Type: DWORD->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetLastError", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern uint GetLastError();

    }

}
