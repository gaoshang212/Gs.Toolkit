using System;

namespace Gs.Toolkit.Native
{
    public interface INative : IDisposable
    {
        bool IsDisposed { get; }

        T GetFunction<T>();

        TResult Call<TResult, T>(params object[] p_params);

        void Run<T>(params object[] p_params);

        TResult Call<TResult>(string p_funName, params object[] p_params);

        void Run(string p_funName, params object[] p_params);
    }
}