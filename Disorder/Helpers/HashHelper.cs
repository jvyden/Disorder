using System.Security.Cryptography;
using System.Text;

namespace Disorder.Helpers; 

public static class HashHelper {
    private static readonly SHA512 sha512 = SHA512.Create();

    public static string Sha512Hash(string str) => Sha512Hash(Encoding.UTF8.GetBytes(str));

    public static string Sha512Hash(byte[] bytes) => BitConverter.ToString(sha512.ComputeHash(bytes)).Replace("-", "").ToLower();
}