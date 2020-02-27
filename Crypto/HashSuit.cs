using System.Security.Cryptography;

namespace Blog_Rest_Api.Crypto{
    public class HashSuit 
    {
        public static byte[] ComputeSha256(byte[] plainBytes){
            SHA256 sha256=SHA256.Create();
            byte[] hashValue=sha256.ComputeHash(plainBytes);
            return hashValue;
        }
    }
}