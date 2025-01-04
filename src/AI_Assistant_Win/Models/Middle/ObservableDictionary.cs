using System;
using System.Collections.Generic;

namespace AI_Assistant_Win.Models.Middle
{
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        // 定义一个委托来处理字典内容变化事件
        public delegate void ChangedEventHandler(object sender, ChangedEventArgs<TKey, TValue> e);

        // 定义一个事件来通知字典内容变化
        public event ChangedEventHandler Changed;

        // 定义一个事件参数类来传递变化的信息
        public class ChangedEventArgs<TKey, TValue> : EventArgs
        {
            public ChangedAction Action { get; }
            public TKey Key { get; }
            public TValue OldValue { get; }
            public TValue NewValue { get; }

            public ChangedEventArgs(ChangedAction action, TKey key, TValue oldValue = default, TValue newValue = default)
            {
                Action = action;
                Key = key;
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        // 定义一个枚举来表示字典的操作类型
        public enum ChangedAction
        {
            Add,
            Remove,
            Update
        }

        // 重写 Add 方法
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnChanged(ChangedAction.Add, key, default, value);
        }

        // 重写 this[] 索引器来捕获更新操作
        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                if (ContainsKey(key))
                {
                    TValue oldValue = base[key];
                    base[key] = value;
                    OnChanged(ChangedAction.Update, key, oldValue, value);
                }
                else
                {
                    base[key] = value;
                    OnChanged(ChangedAction.Add, key, default, value);
                }
            }
        }

        // 重写 Remove 方法
        public new bool Remove(TKey key)
        {
            if (base.Remove(key))
            {
                OnChanged(ChangedAction.Remove, key, base[key], default); // 注意：这里 base[key] 实际上已经移除了，这里只是为了事件参数
                return true;
            }
            return false;
        }

        // 触发 Changed 事件的方法
        protected virtual void OnChanged(ChangedAction action, TKey key, TValue oldValue, TValue newValue)
        {
            Changed?.Invoke(this, new ChangedEventArgs<TKey, TValue>(action, key, oldValue, newValue));
        }
    }
}