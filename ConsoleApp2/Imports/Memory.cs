using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static calc.NativeMethods;

namespace calc
{
    public static class Memory
    {
        public static Process m_iProcess;

        public static bool Attatch(string Name)
        {
            Process[] Processes = Process.GetProcessesByName(Name);
            if (Processes.Length > 0)
            {
                m_iProcess = Processes[0];
                return true;
            }
            return false;
        }

        public static IntPtr GetModuleAddress(string Name)
        {
            foreach (ProcessModule Module in m_iProcess.Modules)
            {
                if (Name == Module.ModuleName)
                {
                    return Module.BaseAddress;
                }
            }
            return IntPtr.Zero;
        }
        public static bool Write<T>(IntPtr lpBaseAddress, T lpBuffer) where T : struct
        {
            return WriteProcessMemory(m_iProcess.Handle, lpBaseAddress, lpBuffer, Marshal.SizeOf<T>(), out var lpNumberOfBytesWritten);
        }

        public static T Read<T>(IntPtr lpBaseAddress) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = (object)default(T);
            ReadProcessMemory(m_iProcess.Handle, lpBaseAddress, buffer, size, out var lpNumberOfBytesRead);
            return lpNumberOfBytesRead == size ? (T)buffer : default;
        }

        public static unsafe T ByteArrayToStructure<T>(byte[] lpBuffer) where T : struct
        {
            fixed (byte* p = lpBuffer)
            {
                return Marshal.PtrToStructure<T>((IntPtr)p);
            }
        }
    }
}
