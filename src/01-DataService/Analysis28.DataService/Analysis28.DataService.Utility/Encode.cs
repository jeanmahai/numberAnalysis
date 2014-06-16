using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis28.DataService.Utility
{
    /// <summary>
    /// 编码工具类
    /// </summary>
    public class Encode
    {
        /// <summary>
        /// 进行base64编码
        /// </summary>
        /// <param name="data">被编码数据</param>
        /// <returns></returns>
        public static string Base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in Base64Encode" + e.Message);
            }
        }

        /// <summary>
        /// 进行base64解码
        /// </summary>
        /// <param name="data">被解码数据</param>
        /// <returns></returns>
        public static string Base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in Base64Decode" + e.Message);
            }
        }
    }
}
