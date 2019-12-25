using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace apigateway
{
    public static class Extensions
    {
        public static Task WhenAllThrottled<T>(this IEnumerable<T> source,
            Func<T, Task> func, CancellationToken cancellationToken = default)
            => source.WhenAllThrottled(10, func, cancellationToken);

        public static async Task WhenAllThrottled<T>(this IEnumerable<T> source, int degreeOfParallelism,
            Func<T, Task> func, CancellationToken cancellationToken = default)
        {
            var runningTasks = new List<Task>();
            using var throttler = new SemaphoreSlim(degreeOfParallelism);
            foreach (var element in source)
            {
                await throttler.WaitAsync(cancellationToken);

                runningTasks.Add(func(element).ContinueWith(t =>
                {
                    // ReSharper disable once AccessToDisposedClosure
                    throttler.Release();
                    t.GetAwaiter().GetResult();
                }, cancellationToken));
            }

            await Task.WhenAll(runningTasks);
        }

        public static Task<TResult[]> WhenAllThrottled<T, TResult>(this IEnumerable<T> source,
            Func<T, Task<TResult>> func, CancellationToken cancellationToken = default)
            => source.WhenAllThrottled(5, func, cancellationToken);

        public static async Task<TResult[]> WhenAllThrottled<T, TResult>(this IEnumerable<T> source,
            int degreeOfParallelism,
            Func<T, Task<TResult>> func, CancellationToken cancellationToken = default)
        {
            var runningTasks = new List<Task<TResult>>();
            using var throttler = new SemaphoreSlim(degreeOfParallelism);
            foreach (var element in source)
            {
                await throttler.WaitAsync(cancellationToken);

                runningTasks.Add(func(element).ContinueWith(t =>
                {
                    // ReSharper disable once AccessToDisposedClosure
                    throttler.Release();
                    return t.GetAwaiter().GetResult();
                }, cancellationToken));
            }

            return await Task.WhenAll(runningTasks);
        }

        public static async Task<bool> AnyAsync<T>(
            this IEnumerable<T> source, Func<T, Task<bool>> func)
        {
            foreach (var element in source)
            {
                if (await func(element))
                    return true;
            }

            return false;
        }
    }
}
