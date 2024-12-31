using System;
using System.Globalization;

namespace AI_Assistant_Win.Utils
{
    public class TimeHelper
    {
        public static string FormateJavaScriptToString(DateTime dateTime)
        {
            DateTime now = DateTime.UtcNow.ToLocalTime(); // 假设你想要的是本地时间

            // 获取时区偏移量
            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(now);

            // 格式化日期和时间字符串，并添加时区偏移量
            string formattedDateTime = dateTime.ToString("ddd MMM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture) +
                                       " GMT" + offset.ToString(@"\+hhmm", CultureInfo.InvariantCulture);

            // 输出结果（注意：这里不会包含"(中国标准时间)"这样的时区名称）
            Console.WriteLine(formattedDateTime);

            // 如果你确实需要"(中国标准时间)"这样的名称，你可能需要手动添加它
            // 但请注意，这不是一个标准做法，且可能不适用于所有环境和系统
            string timeZoneName = "(中国标准时间)"; // 这通常不是从代码中获取的，而是根据上下文或配置确定的
            string finalFormattedDateTime = formattedDateTime + " " + timeZoneName;

            // 输出最终结果
            Console.WriteLine(finalFormattedDateTime);

            return finalFormattedDateTime;
        }
    }
}
