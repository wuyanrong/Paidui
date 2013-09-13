using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Evt.Framework.DataAccess
{
    internal enum TOKEN_INFORMATION_CLASS
    {
        TokenUser = 1,
        TokenGroups,
        TokenPrivileges,
        TokenOwner,
        TokenPrimaryGroup,
        TokenDefaultDacl,
        TokenSource,
        TokenType,
        TokenImpersonationLevel,
        TokenStatistics,
        TokenRestrictedSids,
        TokenSessionId
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TOKEN_USER
    {
        public _SID_AND_ATTRIBUTES User;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct _SID_AND_ATTRIBUTES
    {
        public IntPtr Sid;
        public int Attributes;
    }

    internal static class NativeMethods
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr handle);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("advapi32.dll")]
        internal static extern bool ImpersonateSelf(int level);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("wininet.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool InternetGetCookie(string lpszUrl, string lpszCookieName, StringBuilder cookieData, ref int size);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("wininet.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrl, string lpszCookieName, string lpszCookieData);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool OpenThreadToken(IntPtr thread, uint desiredAccess, [MarshalAs(UnmanagedType.Bool)] bool openAsSelf, ref IntPtr handle);
        [DllImport("advapi32.dll")]
        internal static extern int RegCloseKey(IntPtr handle);
        [DllImport("advapi32.dll")]
        //internal static extern int RegNotifyChangeKeyValue(Microsoft.Crm.SafeRegistryHandle keyHandle, [MarshalAs(UnmanagedType.Bool)] bool watchSubTree, int notifyFilter, SafeWaitHandle eventHandle, [MarshalAs(UnmanagedType.Bool)] bool asynchronous);
        //[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        //internal static extern int RegOpenKeyEx(UIntPtr parentKey, string subKey, int options, int accessDesired, ref Microsoft.Crm.SafeRegistryHandle handle);
        [return: MarshalAs(UnmanagedType.Bool)]
        //[DllImport("advapi32.dll")]
        internal static extern bool RevertToSelf();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool SetThreadToken(IntPtr thread, IntPtr token);

        [DllImport("advapi32", CharSet = CharSet.Unicode,SetLastError=true)]
        internal static extern bool GetTokenInformation(IntPtr hToken, TOKEN_INFORMATION_CLASS tokenInfoClass, IntPtr TokenInformation, int tokeInfoLength, ref int reqLength);

        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern bool LookupAccountSid
        (
            [In, MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, // name of local or remote computer
            IntPtr pSid, // security identifier
            StringBuilder Account, // account name buffer
            ref int cbName, // size of account name buffer
            StringBuilder DomainName, // domain name
            ref int cbDomainName, // size of domain name buffer
            ref int peUse // SID type
        );

    }
}
