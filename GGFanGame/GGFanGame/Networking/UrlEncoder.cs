using System.Text;

namespace GGFanGame.Networking
{
    /// <summary>
    /// This class encodes the special characters in URLs so we can send them over http requests.
    /// </summary>
    class UrlEncoder
    {
        /// <summary>
        /// Encodes an URL string.
        /// </summary>
        /// <param name="str">The url to encode.</param>
        /// <returns></returns>
        public static string encode(string str)
        {
            if (str == null)
                return null;

            return urlEncode(str, Encoding.UTF8);
        }

        private static string urlEncode(string str, Encoding e)
        {
            return Encoding.ASCII.GetString(urlEncodeToBytes(str, e));
        }

        private static byte[] urlEncodeToBytes(string str, Encoding e)
        {
            byte[] bytes = e.GetBytes(str);

            return urlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }

        private static byte[] urlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int cSpaces = 0;
            int cUnsafe = 0;

            // count them first
            for (int i = 0; i < count; i++)
            {
                char ch = (char)bytes[offset + i];

                if (ch == ' ')
                    cSpaces++;
                else if (!isSafe(ch))
                    cUnsafe++;
            }

            // nothing to expand?
            if (!alwaysCreateReturnValue && cSpaces == 0 && cUnsafe == 0)
                return bytes;

            // expand not 'safe' characters into %XX, spaces to +s
            byte[] expandedBytes = new byte[count + cUnsafe * 2];
            int pos = 0;

            for (int i = 0; i < count; i++)
            {
                byte b = bytes[offset + i];
                char ch = (char)b;

                if (isSafe(ch))
                {
                    expandedBytes[pos++] = b;
                }
                else if (ch == ' ')
                {
                    expandedBytes[pos++] = (byte)'+';
                }
                else
                {
                    expandedBytes[pos++] = (byte)'%';
                    expandedBytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
                    expandedBytes[pos++] = (byte)IntToHex(b & 0x0f);
                }
            }

            return expandedBytes;
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
                return (char)(n + (int)'0');
            else
                return (char)(n - 10 + (int)'a');
        }

        //Determines if a character is a safe URL character.    
        //-_.!*\() and alphanumeric are safe characters.
        private static bool isSafe(char ch)
        {
            if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9')
                return true;

            switch (ch)
            {
                case '-':
                case '_':
                case '.':
                case '!':
                case '*':
                case '\'':
                case '(':
                case ')':
                    return true;
            }

            return false;
        }
    }
}