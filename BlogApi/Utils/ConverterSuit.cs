using System;
using System.Linq;
using System.Text;

namespace Blog_Rest_Api.Utils
{
    public class ConverterSuit
    {
            public static string ByteArrayToHex(byte[] bytes){
                StringBuilder stringBuilder=new StringBuilder();
                foreach (var byteValue in bytes)
                {
                    stringBuilder.AppendFormat("{0:x2}",byteValue);
                }
                return stringBuilder.ToString();
            }

            public static byte[] HexToByteArray(string hexString){
                    byte[] bytes= Enumerable.Range(0,hexString.Length)
                                            .Where(index=>index%2==0)
                                            .Select(index=>Convert.ToByte(hexString.Substring(index,2),16))
                                            .ToArray();
                    return bytes;
            }    
    }
}