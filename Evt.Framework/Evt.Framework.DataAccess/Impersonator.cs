using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Evt.Framework.DataAccess
{
    internal sealed class Impersonator : IDisposable
    {
        private const int ERROR_NO_MORE_ITEMS = 259;
        private bool disposed;
        private bool forceImpersonate;
        private bool impersonate;
        private bool threadTokenCleared;
        private IntPtr token;
        public string DomainUser = "";

        public Impersonator()
        {
            this.token = IntPtr.Zero;
            this.DoImpersonate(false);
        }

        public Impersonator(bool force)
        {
            this.token = IntPtr.Zero;
            this.DoImpersonate(force);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                throw new Exception("ReImpersonator: ReImpersonator - Encountered disposed ReImpersonator when it should not be disposed");
            }
            if (this.impersonate)
            {
                if (!NativeMethods.RevertToSelf())
                {
                    int num = Marshal.GetLastWin32Error();
                    throw new Exception("RevertToSelf for reverting to impersonation failed with hr = " + num);
                }
                this.impersonate = false;
            }
            if (this.threadTokenCleared)
            {
                if (!NativeMethods.SetThreadToken(IntPtr.Zero, this.token))
                {
                    int num2 = Marshal.GetLastWin32Error();
                    throw new Exception("SetThreadToken for reverting to impersonation failed with hr = " + num2);
                }
                if ((!this.forceImpersonate || (IntPtr.Zero != this.token)) && !NativeMethods.CloseHandle(this.token))
                {
                    int num3 = Marshal.GetLastWin32Error();
                    throw new Exception("CloseHandle failed with hr = " + num3);
                }
                this.token = IntPtr.Zero;
                this.threadTokenCleared = false;
            }
            this.disposed = true;
            GC.KeepAlive(this);
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        private void DoImpersonate(bool force)
        {
            this.forceImpersonate = force;
            IntPtr currentThread = NativeMethods.GetCurrentThread();
            if (currentThread == IntPtr.Zero)
            {
                throw new Exception("Invalid Thread Id returned by GetCurrentThread. thread == IntPtr.Zero");
            }
            if (!NativeMethods.OpenThreadToken(currentThread, 4, true, ref this.token))
            {
                int num = Marshal.GetLastWin32Error();
                if (num != 0x3f0)
                {
                    throw new Exception("OpenThreadToken failed with hr = " + num);
                }
                
                if (!this.forceImpersonate)
                {
                    return;
                }
            }
            if (!NativeMethods.SetThreadToken(IntPtr.Zero, IntPtr.Zero))
            {
                throw new Exception("SetThreadToken(null, null) failed with hr =" + Marshal.GetLastWin32Error());
            }
            this.threadTokenCleared = true;
            if (!NativeMethods.ImpersonateSelf(3))
            {
                throw new Exception("ImpersonateSelf(3) failed with hr =" + Marshal.GetLastWin32Error());
            }

            this.impersonate = true;
        }

        ~Impersonator()
        {
            this.Dispose(false);
        }

        private enum Error
        {
            NoToken = 0x3f0
        }

        [Flags]
        private enum TokenAccess
        {
            Impersonate = 4
        }

        private void PerformDump(IntPtr token)
        {
            TOKEN_USER tokUser;
            const int bufLength = 256;
            IntPtr tu = Marshal.AllocHGlobal(bufLength);
            int cb = bufLength;
            NativeMethods.GetTokenInformation(token, TOKEN_INFORMATION_CLASS.TokenUser, tu, cb, ref cb);
            tokUser = (TOKEN_USER)Marshal.PtrToStructure(tu, typeof(TOKEN_USER));
            DumpAccountSid(tokUser.User.Sid);
            Marshal.FreeHGlobal(tu);
        }

        private void DumpAccountSid(IntPtr SID)
        {
            int cchAccount = 0;
            int cchDomain = 0;
            int snu = 0;

            StringBuilder Account = null;
            StringBuilder Domain = null;

            bool ret = NativeMethods.LookupAccountSid(null, SID, Account, ref cchAccount, Domain, ref cchDomain, ref snu);

            if (ret == true)
            {
                if (Marshal.GetLastWin32Error() == ERROR_NO_MORE_ITEMS)
                {
                    throw new Exception("LookupAccountSid faild");
                }
                DomainUser = Domain.ToString() + "\\" + Account.ToString();
            }
        }
    }

}
