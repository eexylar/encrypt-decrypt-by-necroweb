using System.Security.Cryptography;

namespace learn;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("encrypter by necroweb v1.0");
        Console.WriteLine("hello, bro, welcome!\n[1]Encrypt\n[2]Decrypt\nChoose function: ");
        int choose1 = Convert.ToInt32(Console.ReadLine());
        byte[] key = Enumerable.Range(0, 32).Select(x => (byte)x).ToArray();
        switch(choose1){

            case 1:
                Console.WriteLine("Choose file name for encrypt(.txt): ");
                string fileNameforEncrypt = Console.ReadLine();
                Console.WriteLine("");
                EncryptFile(fileNameforEncrypt, key);
                Console.WriteLine("Succes encrypt! Press any key for close...");
                Console.ReadKey();
                break;
            case 2:
                Console.WriteLine("Enter file name for decrypt(.txt): ");
                string fileNameforDecrypt = Console.ReadLine();
                DecryptFile(fileNameforDecrypt, key);
                Console.WriteLine("Succes decrypt! Press any key for close...");
                Console.ReadKey();
                break;
            default:
                Console.WriteLine("Error");
                break;
        }
    }

    private static void EncryptFile(string path, byte[] key)
    {
        string tmpPath = Path.GetTempFileName();
        using (FileStream fsSrc = File.OpenRead(path))
        using (Aes aes = Aes.Create())
        using (FileStream fsDst = File.Create(tmpPath))
        {
            aes.Key = key;
            fsDst.Write(aes.IV);
            using (CryptoStream cs = new CryptoStream(fsDst, aes.CreateEncryptor(), CryptoStreamMode.Write, true))
            {
                fsSrc.CopyTo(cs);
            }
        }
        File.Delete(path);
        File.Move(tmpPath, path);
    }

    private static void DecryptFile(string path, byte[] key)
    {
        string tmpPath = Path.GetTempFileName();
        using (FileStream fsSrc = File.OpenRead(path))
        {
            byte[] iv = new byte[16];
            fsSrc.Read(iv);
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (CryptoStream cs = new CryptoStream(fsSrc, aes.CreateDecryptor(), CryptoStreamMode.Read, true))
                using (FileStream fsDst = File.Create(tmpPath))
                {
                    cs.CopyTo(fsDst);
                }
            }
        }
        File.Delete(path);
        File.Move(tmpPath, path);
    }

    
}
