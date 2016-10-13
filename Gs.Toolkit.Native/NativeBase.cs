using System;
using System.Dynamic;
using System.Runtime.InteropServices;

namespace Gs.Toolkit.Native
{
    public abstract class NativeBase : SafeHandle
    {
        public NativeBase(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle)
        {

        }
    }
}