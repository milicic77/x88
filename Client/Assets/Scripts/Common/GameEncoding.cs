using System;
using System.Collections;
using GBK;
using System.Text;
using System.IO;

namespace Game.Common
{
    public class GameEncoding
    {
        public static System.Text.Encoding Encoding = GBKEncodingManager.Encoding;

        // string转c风格定长gbk bytes，超长截断，末尾加0
        static public byte[] GetFixedBytes(string str, int bytesCount)
        {
            byte[] bytesReturn = new byte[bytesCount];
            byte[] srcBytes = Encoding.GetBytes(str);

            int bytesToCopy = srcBytes.Length >= bytesCount ? (bytesCount - 1) : srcBytes.Length;

            Array.Copy(srcBytes, bytesReturn, bytesToCopy);
            bytesReturn[bytesToCopy] = 0;
            return bytesReturn;
        }
        // C风格字符串转string
        static public string GetCString(byte[] bytes)
        {
            int nullCharPos = Array.IndexOf<byte>(bytes, byte.MinValue, 0);
            if (nullCharPos == -1)
                return Encoding.GetString(bytes);
            return Encoding.GetString(bytes, 0, nullCharPos);
        }
        // C风格字符串转string
        static public string ReadFixedString(BinaryReader streamReader, int maxBytes)
        {
            var bytes = streamReader.ReadBytes(maxBytes);
            int nullCharPos = Array.IndexOf<byte>(bytes, byte.MinValue, 0);
            if (nullCharPos == -1)
                return Encoding.GetString(bytes);
            return Encoding.GetString(bytes, 0, nullCharPos);
        }


        static public byte[] GetBytes(string str)
        {
            return Encoding.GetBytes(str);
        }

        static public string GetString(byte[] stringByte)
        {
            return Encoding.GetString(stringByte);
        }




        /// <summary>
        /// 取得一个文本文件的编码方式。如果无法在文件头部找到有效的前导符，Encoding.Default将被返回。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }
        /// <summary>
        /// 取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream">文本文件流。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }
        /// <summary>
        /// 取得一个文本文件的编码方式。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Encoding targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            return targetEncoding;
        }
        /// <summary>
        /// 取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream">文本文件流。</param>
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                //byte byte4 = 0;

                //保存当前Seek位置
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                //if (stream.Length >= 4)
                //{
                //    byte4 = Convert.ToByte(stream.ReadByte());
                //}
                //根据文件流的前4个字节判断Encoding
                //Unicode {0xFF, 0xFE};
                //BE-Unicode {0xFE, 0xFF};
                //UTF8 = {0xEF, 0xBB, 0xBF};
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8
                {
                    targetEncoding = Encoding.UTF8;
                }
                //恢复Seek位置 
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }

        public static string GBKToUTF8(string gbkString)
        {
            string utfinfo = string.Empty;
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            System.Text.Encoding gbk = System.Text.Encoding.GetEncoding("gb2312");

            // Convert the string into a byte[].
            byte[] unicodeBytes = gbk.GetBytes(gbkString);
            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(gbk, utf8, unicodeBytes);
            // Convert the new byte[] into a char[] and then into a string.
            // This is a slightly different approach to converting to illustrate
            // the use of GetCharCount/GetChars.
            char[] asciiChars = new char[utf8.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            utf8.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            utfinfo = new string(asciiChars);

            return utfinfo;

        }
    }
}


