using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Gs.Toolkit.Emit;
using Gs.Toolkit.Extension;

namespace Gs.Toolkit.Native
{
    public class Native : DynamicObject, INative
    {
        private Func<CustomAttributeBuilder[]> _cabfunc = () =>
        {
            var cinfo = typeof(UnmanagedFunctionPointerAttribute).GetConstructor(new[] { typeof(CallingConvention) });

            if (cinfo == null)
            {
                return new CustomAttributeBuilder[0];
            }

            return new[] { new CustomAttributeBuilder(cinfo, new object[] { CallingConvention.Cdecl }) };
        };

        private IntPtr _handle;

        private DelegateBuilder _delegateBuilder;

        public string FileName { get; private set; }

        public bool IsDisposed { get; protected set; }

        internal Native(string p_filename)
        {
            FileName = p_filename;
            LoadLibrary();
        }

        private void LoadLibrary()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new ArgumentNullException();
            }

            if (!File.Exists(FileName))
            {
                throw new FileNotFoundException("dll is not found.");
            }

            Console.WriteLine("the process is {0}bit", Environment.Is64BitProcess ? 64 : 32);

            _handle = NativeMethods.LoadLibrary(FileName);
            if (_handle == IntPtr.Zero)
            {
                throw new Exception("LoadLibray is faild, the handle is zero.");
            }
        }

        public T GetFunction<T>()
        {
            var attribute = GetNativeFunctonAttribute(typeof(T));
            return GetFunction<T>(attribute.FunctionName);
        }

        public T GetFunction<T>(string p_funName)
        {
            var dg = GetFunctionDelegate<T>(p_funName);
            var t = (T)(object)dg;
            return t;
        }

        public TResult Call<TResult, T>(params object[] p_params)
        {
            if (_handle == IntPtr.Zero)
            {
                throw new Exception("The library can not be loaded.");
            }

            var attribute = GetNativeFunctonAttribute(typeof(T));

            return Call<TResult, T>(attribute.FunctionName, p_params);
        }

        public TResult Call<TResult>(string p_funName, params object[] p_params)
        {
            if (_handle == IntPtr.Zero)
            {
                throw new Exception("The library can not be loaded.");
            }

            var type = CreateDelegateType<TResult>(p_funName, p_params);

            var dg = GetFunctionDelegate(type, p_funName);
            if (dg == null)
            {
                return default(TResult);
            }
            return (TResult)dg.DynamicInvoke(p_params);
        }

        public void Run(string p_funName, params object[] p_params)
        {
            if (_handle == IntPtr.Zero)
            {
                throw new Exception("The library can not be loaded.");
            }
            var type = CreateDelegateType(p_funName, p_params, null);
            var dg = GetFunctionDelegate(type, p_funName);
            if (dg != null)
            {
                dg.DynamicInvoke(p_params);
            }
        }

        public void Run<T>(params object[] p_params)
        {
            if (_handle == IntPtr.Zero)
            {
                throw new Exception("The library can not be loaded.");
            }

            var type = typeof(T);

            var attribute = GetNativeFunctonAttribute(type);
            var dg = GetFunctionDelegate(type, attribute.FunctionName);

            if (dg == null)
            {
                throw new EntryPointNotFoundException();
            }

            dg.DynamicInvoke(p_params);
        }


        #region Private Methods

        private Delegate GetFunctionDelegate<T>(string p_funName)
        {
            return GetFunctionDelegate(typeof(T), p_funName);
        }

        private Delegate GetFunctionDelegate(Type p_type, string p_funName)
        {
            if (_handle == IntPtr.Zero)
            {
                throw new Exception("The library can not be loaded.");
            }

            if (string.IsNullOrEmpty(p_funName))
            {
                throw new ArgumentException("The FunctionName is not be empty or null");
            }

            var type = p_type;
            if (!typeof(Delegate).IsAssignableFrom(type))
            {
                throw new ArgumentException("The T is not a Delegate");
            }

            var address = NativeMethods.GetProcAddress(_handle, p_funName);

            var dg = Marshal.GetDelegateForFunctionPointer(address, type);

            return dg;
        }

        private NativeFunctonAttribute GetNativeFunctonAttribute(Type p_type)
        {
            var type = p_type;
            var attribute = type.GetCustomAttributes(typeof(NativeFunctonAttribute), false).FirstOrDefault() as NativeFunctonAttribute;

            if (attribute == null)
            {
                throw new Exception("The Attribute of [NativeFunctonAttribute] can not found.");
            }

            return attribute;
        }

        private DelegateBuilder GetDelegateBuilder()
        {
            if (_delegateBuilder == null)
            {
                _delegateBuilder = new DelegateBuilder();
            }

            return _delegateBuilder;
        }

        private Type CreateDelegateType<T>(string p_funName, object[] p_params)
        {
            return CreateDelegateType(p_funName, p_params, typeof(T));
        }

        private Type CreateDelegateType(string p_funName, object[] p_params, Type p_retrunType)
        {
            var builder = GetDelegateBuilder();
            var type = builder.CreateDelegateBySingle(p_funName, p_params.GetTypes(), p_retrunType, _cabfunc);
            return type;
        }

        #endregion

        #region Dynamic

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] p_args, out object p_result)
        {
            var csharpBinder = binder.GetType().GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder");
            var typeArgs = csharpBinder.GetProperty("TypeArguments").GetValue(binder, null) as IList<Type>;

            Type retrunType = null;
            if (typeArgs != null && typeArgs.Count > 0)
            {
                retrunType = typeArgs.First();
            }

            var funcName = binder.Name;

            var type = CreateDelegateType(funcName, p_args, retrunType);

            var dg = GetFunctionDelegate(type, funcName);
            if (dg == null)
            {
                p_result = null;
                return false;
            }

            p_result = dg.DynamicInvoke(p_args);

            return true;
        }


        #endregion

        public void Dispose()
        {
            var handle = _handle;
            _handle = IntPtr.Zero;

            if (handle == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.FreeLibrary(handle);

            IsDisposed = true;
        }
    }
}
