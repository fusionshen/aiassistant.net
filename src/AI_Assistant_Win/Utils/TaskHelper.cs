using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AI_Assistant_Win.Utils
{
    public class TaskHelper
    {
        public static async Task<List<T>> SafeExecuteAsync<T>(Task<List<T>> task)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                // 记录日志，避免静默失败
                Console.WriteLine($"Error: {ex.Message}");
                return new List<T>(); // 发生异常时返回空列表，防止访问Result时报错
            }
        }
    }
}
