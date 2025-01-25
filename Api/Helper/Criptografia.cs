using System.Security.Cryptography;
using System.Text;

namespace Api
{
    public static class Criptografia
    {
        /// <summary>
        /// Criptografa uma chave utilizando MD5
        /// </summary>
        /// <param name="chaveDescriptografada">Chave a ser criptografada</param>
        /// <returns></returns>
        public static string EncriptarMd5(ref string chaveDescriptografada)
        {
            if (string.IsNullOrWhiteSpace(chaveDescriptografada)) return "";
            var password = chaveDescriptografada += "|674234bf-275a-4796-8654-1753960f9d53";
            var md5 = MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(password));
            var sbString = new StringBuilder();
            foreach (var i in data)
            {
                sbString.Append(data[i].ToString("x2"));
            }

            return sbString.ToString();
        }

        /// <summary>
        /// Criptografa uma chave utilizando Sha1
        /// </summary>
        /// <param name="chaveDescriptografada"></param>
        /// <returns></returns>
        public static string EncriptarSha1(string chaveDescriptografada)
        {
            if (string.IsNullOrWhiteSpace(chaveDescriptografada)) return "";
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(chaveDescriptografada));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
