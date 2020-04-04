﻿using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Altseed
{
    /// <summary>
    /// Altseed2における<see cref="SynchronizationContext"/>
    /// </summary>
    [Serializable]
    public class AltseedContext : SynchronizationContext
    {
        [Serializable]
        private readonly struct Entry
        {
            private readonly SendOrPostCallback d;
            private readonly object state;

            internal Entry(SendOrPostCallback d, object state)
            {
                this.d = d ?? throw new ArgumentNullException(nameof(d), "引数がnullです");
                this.state = state;
            }

            internal void Invoke() => d.Invoke(state);
        }

        private readonly ConcurrentQueue<Entry> actions = new ConcurrentQueue<Entry>();

        /// <summary>
        /// 溜められた処理の個数を取得する
        /// </summary>
        public int Count => actions.Count;

        /// <summary>
        /// 更新されるかどうかを取得または設定する
        /// </summary>
        public bool IsUpdated { get; set; } = true;

        public override void Post(SendOrPostCallback d, object state)
        {
            actions.Enqueue(new Entry(d, state));
        }

        /// <summary>
        /// 溜められた処理の実行を行う
        /// </summary>
        public void Update()
        {
            if (!IsUpdated) return;
            while (actions.TryDequeue(out var e)) e.Invoke();
        }
    }
}
