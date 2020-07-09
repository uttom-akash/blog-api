using System.Security.Cryptography;
using System.Text;

namespace Blog_Rest_Api.Crypto{
    public class HashSuit 
    {
        public static byte[] ComputeSha256(byte[] plainBytes){
            SHA256 sha256=SHA256.Create();
            byte[] hashValue=sha256.ComputeHash(plainBytes);
            return hashValue;
        }

        public static byte[] ComputeMD5(byte[] plainBytes){
            MD5 md5=MD5.Create();
            byte[] hashValue=md5.ComputeHash(plainBytes);
            return hashValue;
        }
    }
}