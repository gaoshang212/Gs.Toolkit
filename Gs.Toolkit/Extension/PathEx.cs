using System;
using System.IO;

namespace Gs.Toolkit.Extension
{
    public class PathEx
    {
        public static string GetAppData(string p_path)
        {
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dst = Path.Combine(appdata, p_path);

            return dst;
        }
    }
}