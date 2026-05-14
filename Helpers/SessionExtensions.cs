using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace CNPM_LIBRARY_MANAGEMENT.Helpers 
{
    public static class SessionExtensions
    {
        // Hàm hỗ trợ lưu Object vào Session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Hàm hỗ trợ lấy Object từ Session
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}