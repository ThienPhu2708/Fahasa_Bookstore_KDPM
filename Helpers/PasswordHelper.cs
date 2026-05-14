using System.Security.Cryptography;
using System.Text;

namespace CNPM_LIBRARY_MANAGEMENT.Helpers
{
    public static class PasswordHelper
    {
        // Hàm này tạo ra Hash và Salt mới khi Đăng ký
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Hàm này kiểm tra mật khẩu khi Đăng nhập và Đổi mật khẩu
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}