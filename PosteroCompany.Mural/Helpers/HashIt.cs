namespace PosteroCompany.Mural.Helpers
{
    public class HashIt
    {
        public static string SHA256(string value)
        {
            System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sha.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(value));
            byte[] result = sha.Hash;
            for (int i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("x2"));
            return sb.ToString();
        }
    }
}