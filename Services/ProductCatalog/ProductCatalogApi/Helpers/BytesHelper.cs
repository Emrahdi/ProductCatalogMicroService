using System;
using System.Text;

namespace ProductCatalogApi.Helpers {
    /// <summary>
    /// Byte array operation class
    /// </summary>
    public static class BytesHelper {

        /// <summary>
        /// Get string from byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetString(byte[] input) {
            return Encoding.Default.GetString(input);
        }
        /// <summary>
        /// Encode UTF bytes from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GetUTFBytes(string input) {
            return Encoding.UTF8.GetBytes(input);
        }
        /// <summary>
        /// Encode ASCII bytes from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GetASCIIBytes(string input) {
            return Encoding.UTF8.GetBytes(input);
        }
    }
}
