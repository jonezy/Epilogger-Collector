using System;
using System.Collections.Generic;
using System.Text;

namespace FoursquareNET.OAuth
{
    /// <summary>Provides methods for encoding and decoding URLs when processing Web requests. This class cannot be inherited.</summary>
    public sealed class HttpUtility
    {
        /// <summary>
        /// Decodes all the bytes in the specified byte array into a string.
        /// </summary>
        /// <remarks>
        /// Replace the method "System.Text.Encoding.ASCII.GetString(byte[] bytes);" in .Net Framework.
        /// </remarks>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
        /// <returns>A String containing the results of decoding the specified sequence of bytes.</returns>
        private static string ASCIIGetString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            return Encoding.ASCII.GetString(bytes, 0, bytes.Length);

        }


        private static int HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        internal static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 0x30);
            }
            return (char)((n - 10) + 0x61);
        }

        private static bool IsNonAsciiByte(byte b)
        {
            if (b < 0x7f)
            {
                return (b < 0x20);
            }
            return true;
        }

        internal static bool IsSafe(char ch)
        {
            if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9')))
            {
                return true;
            }
            switch (ch)
            {
                case '\'':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
        }

        /// <summary>Converts a string that has been encoded for transmission in a URL into a decoded string.</summary>
        /// <returns>A decoded string.</returns>
        /// <param name="str">The string to decode. </param>
        public static string UrlDecode(string str)
        {
            return str == null ? null : UrlDecode(str, Encoding.UTF8);
        }

        /// <summary>Converts a URL-encoded byte array into a decoded string using the specified decoding object.</summary>
        /// <returns>A decoded string.</returns>
        /// <param name="e">The <see cref="T:System.Text.Encoding"></see> that specifies the decoding scheme. </param>
        /// <param name="bytes">The array of bytes to decode. </param>
        public static string UrlDecode(byte[] bytes, Encoding e)
        {
            return bytes == null ? null : UrlDecode(bytes, 0, bytes.Length, e);
        }

        /// <summary>Converts a URL-encoded string into a decoded string, using the specified encoding object.</summary>
        /// <returns>A decoded string.</returns>
        /// <param name="e">The <see cref="T:System.Text.Encoding"></see> that specifies the decoding scheme. </param>
        /// <param name="str">The string to decode. </param>
        public static string UrlDecode(string str, Encoding e)
        {
            return str == null ? null : UrlDecodeStringFromStringInternal(str, e);
        }

        /// <summary>Converts a URL-encoded byte array into a decoded string using the specified encoding object, starting at the specified position in the array, and continuing for the specified number of bytes.</summary>
        /// <returns>A decoded string.</returns>
        /// <param name="offset">The position in the byte to begin decoding. </param>
        /// <param name="count">The number of bytes to decode. </param>
        /// <param name="e">The <see cref="T:System.Text.Encoding"></see> object that specifies the decoding scheme. </param>
        /// <param name="bytes">The array of bytes to decode. </param>
        public static string UrlDecode(byte[] bytes, int offset, int count, Encoding e)
        {
            if ((bytes == null) && (count == 0))
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if ((offset < 0) || (offset > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || ((offset + count) > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return UrlDecodeStringFromBytesInternal(bytes, offset, count, e);
        }

        private static byte[] UrlDecodeBytesFromBytesInternal(IList<byte> buf, int offset, int count)
        {
            var length = 0;
            var sourceArray = new byte[count];
            for (var i = 0; i < count; i++)
            {
                var index = offset + i;
                var num4 = buf[index];
                if (num4 == 0x2b)
                {
                    num4 = 0x20;
                }
                else if ((num4 == 0x25) && (i < (count - 2)))
                {
                    var num5 = HexToInt((char)buf[index + 1]);
                    var num6 = HexToInt((char)buf[index + 2]);
                    if ((num5 >= 0) && (num6 >= 0))
                    {
                        num4 = (byte)((num5 << 4) | num6);
                        i += 2;
                    }
                }
                sourceArray[length++] = num4;
            }
            if (length < sourceArray.Length)
            {
                var destinationArray = new byte[length];
                Array.Copy(sourceArray, destinationArray, length);
                sourceArray = destinationArray;
            }
            return sourceArray;
        }

        private static string UrlDecodeStringFromBytesInternal(IList<byte> buf, int offset, int count, Encoding e)
        {
            var decoder = new UrlDecoder(count, e);
            for (var i = 0; i < count; i++)
            {
                var index = offset + i;
                var b = buf[index];
                if (b == 0x2b)
                {
                    b = 0x20;
                }
                else if ((b == 0x25) && (i < (count - 2)))
                {
                    if ((buf[index + 1] == 0x75) && (i < (count - 5)))
                    {
                        var num4 = HexToInt((char)buf[index + 2]);
                        var num5 = HexToInt((char)buf[index + 3]);
                        var num6 = HexToInt((char)buf[index + 4]);
                        var num7 = HexToInt((char)buf[index + 5]);
                        if (((num4 < 0) || (num5 < 0)) || ((num6 < 0) || (num7 < 0)))
                        {
                            break;
                        }
                        var ch = (char)((((num4 << 12) | (num5 << 8)) | (num6 << 4)) | num7);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }
                    var num8 = HexToInt((char)buf[index + 1]);
                    var num9 = HexToInt((char)buf[index + 2]);
                    if ((num8 >= 0) && (num9 >= 0))
                    {
                        b = (byte)((num8 << 4) | num9);
                        i += 2;
                    }
                }
                decoder.AddByte(b);
            }
            return decoder.GetString();
        }

        private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
        {
            var length = s.Length;
            var decoder = new UrlDecoder(length, e);
            for (var i = 0; i < length; i++)
            {
                var ch = s[i];
                if (ch == '+')
                {
                    ch = ' ';
                }
                else if ((ch == '%') && (i < (length - 2)))
                {
                    if ((s[i + 1] == 'u') && (i < (length - 5)))
                    {
                        var num3 = HexToInt(s[i + 2]);
                        var num4 = HexToInt(s[i + 3]);
                        var num5 = HexToInt(s[i + 4]);
                        var num6 = HexToInt(s[i + 5]);
                        if (((num3 < 0) || (num4 < 0)) || ((num5 < 0) || (num6 < 0)))
                        {
                            break;
                        }
                        ch = (char)((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }
                    var num7 = HexToInt(s[i + 1]);
                    var num8 = HexToInt(s[i + 2]);
                    if ((num7 >= 0) && (num8 >= 0))
                    {
                        var b = (byte)((num7 << 4) | num8);
                        i += 2;
                        decoder.AddByte(b);
                        continue;
                    }
                }
                if ((ch & 0xff80) == 0)
                {
                    decoder.AddByte((byte)ch);
                }
                else
                {
                    decoder.AddChar(ch);
                }
            }
            return decoder.GetString();
        }

        /// <summary>Converts a URL-encoded array of bytes into a decoded array of bytes.</summary>
        /// <returns>A decoded array of bytes.</returns>
        /// <param name="bytes">The array of bytes to decode. </param>
        public static byte[] UrlDecodeToBytes(byte[] bytes)
        {
            return bytes == null ? null : UrlDecodeToBytes(bytes, 0, bytes.Length);
        }

        /// <summary>Converts a URL-encoded string into a decoded array of bytes.</summary>
        /// <returns>A decoded array of bytes.</returns>
        /// <param name="str">The string to decode. </param>
        public static byte[] UrlDecodeToBytes(string str)
        {
            return str == null ? null : UrlDecodeToBytes(str, Encoding.UTF8);
        }

        /// <summary>Converts a URL-encoded string into a decoded array of bytes using the specified decoding object.</summary>
        /// <returns>A decoded array of bytes.</returns>
        /// <param name="e">The <see cref="T:System.Text.Encoding"></see> object that specifies the decoding scheme. </param>
        /// <param name="str">The string to decode. </param>
        public static byte[] UrlDecodeToBytes(string str, Encoding e)
        {
            return str == null ? null : UrlDecodeToBytes(e.GetBytes(str));
        }

        /// <summary>Converts a URL-encoded array of bytes into a decoded array of bytes, starting at the specified position in the array and continuing for the specified number of bytes.</summary>
        /// <returns>A decoded array of bytes.</returns>
        /// <param name="offset">The position in the byte array at which to begin decoding. </param>
        /// <param name="count">The number of bytes to decode. </param>
        /// <param name="bytes">The array of bytes to decode. </param>
        public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
        {
            if ((bytes == null) && (count == 0))
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if ((offset < 0) || (offset > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || ((offset + count) > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return UrlDecodeBytesFromBytesInternal(bytes, offset, count);
        }

        /// <summary>Converts a byte array into an encoded URL string.</summary>
        /// <returns>An encoded string.</returns>
        /// <param name="bytes">The array of bytes to encode. </param>
        public static string UrlEncode(byte[] bytes)
        {
            return bytes == null ? null : ASCIIGetString(UrlEncodeToBytes(bytes));
            // return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes));
        }

        /// <summary>Encodes a URL string.</summary>
        /// <returns>An encoded string.</returns>
        /// <param name="str">The text to encode. </param>
        public static string UrlEncode(string str)
        {
            return str == null ? null : UrlEncode(str, Encoding.UTF8);
        }

        /// <summary>Encodes a URL string using the specified encoding object.</summary>
        /// <returns>An encoded string.</returns>
        /// <param name="e">The <see cref="T:System.Text.Encoding"></see> object that specifies the encoding scheme. </param>
        /// <param name="str">The text to encode. </param>
        public static string UrlEncode(string str, Encoding e)
        {
            return str == null ? null : ASCIIGetString(UrlEncodeToBytes(str, e));
            // return Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));
        }

        /// <summary>Converts a byte array into a URL-encoded string, starting at the specified position in the array and continuing for the specified number of bytes.</summary>
        /// <returns>An encoded string.</returns>
        /// <param name="offset">The position in the byte array at which to begin encoding. </param>
        /// <param name="count">The number of bytes to encode. </param>
        /// <param name="bytes">The array of bytes to encode. </param>
        public static string UrlEncode(byte[] bytes, int offset, int count)
        {
            return bytes == null ? null : ASCIIGetString(UrlEncodeToBytes(bytes, offset, count));
            // return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, offset, count));
        }

        private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            var num = 0;
            var num2 = 0;
            for (var i = 0; i < count; i++)
            {
                var ch = (char)bytes[offset + i];
                if (ch == ' ')
                {
                    num++;
                }
                else if (!IsSafe(ch))
                {
                    num2++;
                }
            }
            if ((!alwaysCreateReturnValue && (num == 0)) && (num2 == 0))
            {
                return bytes;
            }
            var buffer = new byte[count + (num2 * 2)];
            var num4 = 0;
            for (var j = 0; j < count; j++)
            {
                var num6 = bytes[offset + j];
                var ch2 = (char)num6;
                if (IsSafe(ch2))
                {
                    buffer[num4++] = num6;
                }
                else if (ch2 == ' ')
                {
                    buffer[num4++] = 0x2b;
                }
                else
                {
                    buffer[num4++] = 0x25;
                    buffer[num4++] = (byte)IntToHex((num6 >> 4) & 15);
                    buffer[num4++] = (byte)IntToHex(num6 & 15);
                }
            }
            return buffer;
        }

        private static byte[] UrlEncodeBytesToBytesInternalNonAscii(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            var num = 0;
            for (var i = 0; i < count; i++)
            {
                if (IsNonAsciiByte(bytes[offset + i]))
                {
                    num++;
                }
            }
            if (!alwaysCreateReturnValue && (num == 0))
            {
                return bytes;
            }
            var buffer = new byte[count + (num * 2)];
            var num3 = 0;
            for (var j = 0; j < count; j++)
            {
                var b = bytes[offset + j];
                if (IsNonAsciiByte(b))
                {
                    buffer[num3++] = 0x25;
                    buffer[num3++] = (byte)IntToHex((b >> 4) & 15);
                    buffer[num3++] = (byte)IntToHex(b & 15);
                }
                else
                {
                    buffer[num3++] = b;
                }
            }
            return buffer;
        }

        internal static string UrlEncodeNonAscii(string str, Encoding e)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (e == null)
            {
                e = Encoding.UTF8;
            }
            var bytes = e.GetBytes(str);
            bytes = UrlEncodeBytesToBytesInternalNonAscii(bytes, 0, bytes.Length, false);
            // return Encoding.ASCII.GetString(bytes);
            return ASCIIGetString(bytes);
        }

        internal static string UrlEncodeSpaces(string str)
        {
            if ((str != null) && (str.IndexOf(' ') >= 0))
            {
                str = str.Replace(" ", "%20");
            }
            return str;
        }

        /// <summary>Converts a string into a URL-encoded array of bytes.</summary>
        /// <returns>An encoded array of bytes.</returns>
        /// <param name="str">The string to encode. </param>
        public static byte[] UrlEncodeToBytes(string str)
        {
            return str == null ? null : UrlEncodeToBytes(str, Encoding.UTF8);
        }

        /// <summary>Converts an array of bytes into a URL-encoded array of bytes.</summary>
        /// <returns>An encoded array of bytes.</returns>
        /// <param name="bytes">The array of bytes to encode. </param>
        public static byte[] UrlEncodeToBytes(byte[] bytes)
        {
            return bytes == null ? null : UrlEncodeToBytes(bytes, 0, bytes.Length);
        }

        /// <summary>Converts a string into a URL-encoded array of bytes using the specified encoding object.</summary>
        /// <returns>An encoded array of bytes.</returns>
        /// <param name="e">The <see cref="T:System.Text.Encoding"></see> that specifies the encoding scheme. </param>
        /// <param name="str">The string to encode </param>
        public static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            var bytes = e.GetBytes(str);
            return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }

        /// <summary>Converts an array of bytes into a URL-encoded array of bytes, starting at the specified position in the array and continuing for the specified number of bytes.</summary>
        /// <returns>An encoded array of bytes.</returns>
        /// <param name="offset">The position in the byte array at which to begin encoding. </param>
        /// <param name="count">The number of bytes to encode. </param>
        /// <param name="bytes">The array of bytes to encode. </param>
        public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            if ((bytes == null) && (count == 0))
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if ((offset < 0) || (offset > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || ((offset + count) > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return UrlEncodeBytesToBytesInternal(bytes, offset, count, true);
        }

        /// <summary>Converts a string into a Unicode string.</summary>
        /// <returns>A Unicode string in %UnicodeValue notation.</returns>
        /// <param name="str">The string to convert. </param>
        public static string UrlEncodeUnicode(string str)
        {
            return str == null ? null : UrlEncodeUnicodeStringToStringInternal(str, false);
        }

        private static string UrlEncodeUnicodeStringToStringInternal(string s, bool ignoreAscii)
        {
            var length = s.Length;
            var builder = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var ch = s[i];
                if ((ch & 0xff80) == 0)
                {
                    if (ignoreAscii || IsSafe(ch))
                    {
                        builder.Append(ch);
                    }
                    else if (ch == ' ')
                    {
                        builder.Append('+');
                    }
                    else
                    {
                        builder.Append('%');
                        builder.Append(IntToHex((ch >> 4) & '\x000f'));
                        builder.Append(IntToHex(ch & '\x000f'));
                    }
                }
                else
                {
                    builder.Append("%u");
                    builder.Append(IntToHex((ch >> 12) & '\x000f'));
                    builder.Append(IntToHex((ch >> 8) & '\x000f'));
                    builder.Append(IntToHex((ch >> 4) & '\x000f'));
                    builder.Append(IntToHex(ch & '\x000f'));
                }
            }
            return builder.ToString();
        }

        /// <summary>Converts a Unicode string into an array of bytes.</summary>
        /// <returns>A byte array.</returns>
        /// <param name="str">The string to convert. </param>
        public static byte[] UrlEncodeUnicodeToBytes(string str)
        {
            return str == null ? null : Encoding.ASCII.GetBytes(UrlEncodeUnicode(str));
        }

        /// <summary>Encodes the path portion of a URL string for reliable HTTP transmission from the Web server to a client.</summary>
        /// <returns>The URL-encoded text.</returns>
        /// <param name="str">The text to URL-encode. </param>
        public static string UrlPathEncode(string str)
        {
            if (str == null)
            {
                return null;
            }
            var index = str.IndexOf('?');
            if (index >= 0)
            {
                return (UrlPathEncode(str.Substring(0, index)) + str.Substring(index));
            }
            return UrlEncodeSpaces(UrlEncodeNonAscii(str, Encoding.UTF8));
        }

        private class UrlDecoder
        {
            private int _bufferSize;
            private byte[] _byteBuffer;
            private char[] _charBuffer;
            private Encoding _encoding;
            private int _numBytes;
            private int _numChars;

            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                _bufferSize = bufferSize;
                _encoding = encoding;
                _charBuffer = new char[bufferSize];
            }

            internal void AddByte(byte b)
            {
                if (_byteBuffer == null)
                {
                    _byteBuffer = new byte[_bufferSize];
                }
                _byteBuffer[_numBytes++] = b;
            }

            internal void AddChar(char ch)
            {
                if (_numBytes > 0)
                {
                    FlushBytes();
                }
                _charBuffer[_numChars++] = ch;
            }

            private void FlushBytes()
            {
                if (_numBytes <= 0) return;
                _numChars += _encoding.GetChars(_byteBuffer, 0, _numBytes, _charBuffer, _numChars);
                _numBytes = 0;
            }

            internal string GetString()
            {
                if (_numBytes > 0)
                {
                    FlushBytes();
                }
                return _numChars > 0 ? new string(_charBuffer, 0, _numChars) : string.Empty;
            }
        }
    }
}