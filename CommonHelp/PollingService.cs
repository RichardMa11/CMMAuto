using System;
using System.Threading;
using System.Threading.Tasks;

namespace CMMAuto.CommonHelp
{
    public class PollingService
    {
        private CancellationTokenSource _cts;
        private TimeSpan _pollingInterval;
        private Func<Task<bool>> _checkAction; // 返回bool表示是否继续轮询

        /// <summary>
        /// 轮询时触发的事件
        /// </summary>
        public event Action<Exception> OnError;

        /// <summary>
        /// 初始化轮询服务
        /// </summary>
        /// <param name="pollingInterval">轮询间隔时间</param>
        /// <param name="checkAction">每次轮询执行的异步操作（返回false停止轮询）</param>
        public PollingService(TimeSpan pollingInterval, Func<Task<bool>> checkAction)
        {
            _pollingInterval = pollingInterval;
            _checkAction = checkAction ?? throw new ArgumentNullException(nameof(checkAction));
        }

        /// <summary>
        /// 启动轮询
        /// </summary>
        public void Start()
        {
            if (_cts != null && !_cts.IsCancellationRequested) return;

            _cts = new CancellationTokenSource();
            Task.Run(() => PollingLoop(_cts.Token), _cts.Token);
        }

        /// <summary>
        /// 停止轮询
        /// </summary>
        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        /// <summary>
        /// 轮询主循环
        /// </summary>
        private async Task PollingLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    bool shouldContinue = await _checkAction();
                    if (!shouldContinue) break;

                    await Task.Delay(_pollingInterval, token);
                }
                catch (TaskCanceledException)
                {
                    // 正常停止
                    break;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(ex);
                    break; // 根据需求决定是否继续
                }
            }
        }
    }
}

