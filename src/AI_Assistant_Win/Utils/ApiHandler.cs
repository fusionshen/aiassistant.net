﻿using AI_Assistant_Win.Business;

namespace AI_Assistant_Win.Utils
{
    public class ApiHandler
    {

        private readonly ApiBLL _apiBLL;

        public ApiHandler()
        {
            _apiBLL = new ApiBLL();
        }

        // 私有静态内部类，负责持有唯一实例
        private static class Nested
        {
            // 静态成员变量，在第一次使用时才创建实例
            internal static readonly ApiHandler instance = new();
        }

        /// <summary>
        /// 公共静态方法提供全局访问点
        /// Eager Initialization 饿汉式
        /// Lazy Initialization 懒汉式
        /// Double-Checked Locking 双重检查锁定
        /// ✔ Bill Pugh Singleton 静态内部类 这种方法利用了C#静态内部类的特性，实现了线程安全的懒加载，且无需显式锁定。
        /// </summary>
        public static ApiHandler Instance => Nested.instance;

        public ApiBLL GetApiBLL()
        {
            return _apiBLL;
        }
    }
}
