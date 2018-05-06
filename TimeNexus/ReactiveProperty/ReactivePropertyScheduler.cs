using System;
using System.Reactive.Concurrency;
using System.Threading;

namespace Reactive.Bindings
{
    /// <summary>
    /// Default ReactiveProperty Scheduler provider.
    /// </summary>
    public static class ReactivePropertyScheduler
    {
        static IScheduler defaultScheduler;

        /// <summary>
        /// Get ReactiveProperty default scheduler.
        /// </summary>
        public static IScheduler Default
        {
            get
            {
                if (defaultScheduler != null)
                {
                    return defaultScheduler;
                }
				return Scheduler.CurrentThread;

			}
        }

        /// <summary>
        /// set default scheduler.
        /// </summary>
        /// <param name="defaultScheduler"></param>
        public static void SetDefault(IScheduler defaultScheduler)
        {
            ReactivePropertyScheduler.defaultScheduler = defaultScheduler;
        }
    }
}
