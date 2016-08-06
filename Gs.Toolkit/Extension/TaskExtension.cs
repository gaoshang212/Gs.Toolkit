using System.Threading.Tasks;

namespace Gs.Toolkit.Extension
{
    public class TaskExtension
    {
        public static Task Delay(double milliseconds)
        {
            var tcs = new TaskCompletionSource<bool>();
            var timer = new System.Timers.Timer();
            timer.Elapsed += (obj, args) =>
            {
                tcs.TrySetResult(true);
                timer.Dispose();
            };
            timer.Interval = milliseconds;
            timer.AutoReset = false;
            timer.Start();
            return tcs.Task;
        }
    }
}
