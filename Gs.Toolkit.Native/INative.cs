using System;
using System.Runtime.InteropServices;

namespace Gs.Toolkit.Native
{
    public interface INative : IDisposable
    {
        bool HasDisposed { get; }

        /// <summary>
        /// 获取函数委托
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <returns></returns>
        TDelegate GetFunction<TDelegate>();

        TResult Invoke<TResult, TDelegate>(params object[] p_params);

        TResult Invoke<TResult>(string p_funName, params object[] p_params);

        TResult Invoke<TResult>(string p_funName, CallingConvention p_calling, params object[] p_params);

        object Invoke(string p_funName, Type p_retrunType, params object[] p_params);

        void Call<TDelegate>(params object[] p_params);

        void Call(string p_funName, params object[] p_params);

        void Call(string p_funName, CallingConvention p_calling, params object[] p_params);
    }
}