using System.Security.Cryptography;
using System.Text;

namespace TaskManagerAPI.Utils
{
    public class MD5Utils
    {
        public static string CreateHashMD5(string input)
        {
            MD5 md5Hash = MD5.Create();
            var bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder stringBuilder = new StringBuilder();

            foreach(var byt in bytes)
            {
                stringBuilder.Append(byt.ToString("X2"));
            }
            return stringBuilder.ToString();
        }
        
    }
}
