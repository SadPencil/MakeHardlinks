namespace MakeHardlinks
{

    #region struct

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

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public struct LARGE_INTEGER
    {

        /// Anonymous_9320654f_2227_43bf_a385_74cc8c562686
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public Anonymous_9320654f_2227_43bf_a385_74cc8c562686 Struct1;

        /// Anonymous_947eb392_1446_4e25_bbd4_10e98165f3a9
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public Anonymous_947eb392_1446_4e25_bbd4_10e98165f3a9 u;

        /// LONGLONG->__int64
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public long QuadPart;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Anonymous_9320654f_2227_43bf_a385_74cc8c562686
    {

        /// DWORD->unsigned int
        public uint LowPart;

        /// LONG->int
        public int HighPart;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Anonymous_947eb392_1446_4e25_bbd4_10e98165f3a9
    {

        /// DWORD->unsigned int
        public uint LowPart;

        /// LONG->int
        public int HighPart;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Anonymous_ac6e4301_4438_458f_96dd_e86faeeca2a6
    {

        /// DWORD->unsigned int
        public uint Offset;

        /// DWORD->unsigned int
        public uint OffsetHigh;
    }

    #endregion
    public partial class NativeConstants
    {
        /// INFINITE -> 0xFFFFFFFF
        public const uint INFINITE = uint.MaxValue;

        public static readonly System.IntPtr INVALID_HANDLE_VALUE = new System.IntPtr(-1);

        /// CREATE_SUSPENDED -> 0x00000004
        public const uint CREATE_SUSPENDED = 4;

        /// JOB_OBJECT_MSG_ACTIVE_PROCESS_ZERO -> 4
        public const uint JOB_OBJECT_MSG_ACTIVE_PROCESS_ZERO = 4;

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

        /// STARTF_FORCEONFEEDBACK -> 0x00000040
        public const uint STARTF_FORCEONFEEDBACK = 64;

        /// CREATE_BREAKAWAY_FROM_JOB -> 0x01000000
        public const uint CREATE_BREAKAWAY_FROM_JOB = 16777216;
    }

    public enum JOBOBJECTINFOCLASS
    {
        JobObjectAssociateCompletionPortInformation = 7,
        JobObjectBasicLimitInformation = 2,
        JobObjectBasicUIRestrictions = 4,
        JobObjectCpuRateControlInformation = 15,
        JobObjectEndOfJobTimeInformation = 6,
        JobObjectExtendedLimitInformation = 9,
        JobObjectGroupInformation = 11,
        JobObjectGroupInformationEx = 14,
        JobObjectLimitViolationInformation2 = 35,
        JobObjectNetRateControlInformation = 32,
        JobObjectNotificationLimitInformation = 12,
        JobObjectNotificationLimitInformation2 = 34,
        JobObjectSecurityLimitInformation = 5
    }

    #region methods

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
        ///lpJobAttributes: LPSECURITY_ATTRIBUTES->_SECURITY_ATTRIBUTES*
        ///lpName: LPCWSTR->WCHAR*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "CreateJobObjectW", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern System.IntPtr CreateJobObjectW([System.Runtime.InteropServices.InAttribute()] System.IntPtr lpJobAttributes, [System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpName);

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

        /// Return Type: HANDLE->void*
        ///FileHandle: HANDLE->void*
        ///ExistingCompletionPort: HANDLE->void*
        ///CompletionKey: ULONG_PTR->unsigned int
        ///NumberOfConcurrentThreads: DWORD->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "CreateIoCompletionPort", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern System.IntPtr CreateIoCompletionPort([System.Runtime.InteropServices.InAttribute()] System.IntPtr FileHandle, [System.Runtime.InteropServices.InAttribute()] System.IntPtr ExistingCompletionPort, uint CompletionKey, uint NumberOfConcurrentThreads);

    }

    public partial class NativeMethods
    {

        /// Return Type: DWORD->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetLastError", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern uint GetLastError();

    }

    #endregion

    public partial class NativeConstants
    {

        /// MF_BYCOMMAND -> 0x00000000L
        public const uint MF_BYCOMMAND = 0;
    }

    public partial class NativeConstants
    {

        /// MF_GRAYED -> 0x00000001L
        public const uint MF_GRAYED = 1;
    }

    public partial class NativeConstants
    {

        /// SC_CLOSE -> 0xF060
        public const uint SC_CLOSE = 61536;
    }

    public partial class NativeConstants
    {

        /// MF_ENABLED -> 0x00000000L
        public const uint MF_ENABLED = 0;
    }

}
