using System.Security.Cryptography.X509Certificates;

namespace óbudai
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1. feladat
            //üzenet és kód rejtjelezése
            Console.WriteLine("1. feladat: Üzenet és kód rejtett üzenetté alakítása");
            Console.Write("Írja ide az üzenetet! ");
            string uzenet = Console.ReadLine();
            Console.Write("Írja ide a kódot! ");
            string kulcs1 = Console.ReadLine();
            if (uzenet.Length > kulcs1.Length)
            {
                Console.WriteLine("A kulcs nem elég hosszú.");
            }
            else
            {
                
                Console.WriteLine("A rejtjelezett üzenet: {0}", Rejtjelezes(uzenet, kulcs1));
            }

            //rejtjelezett üzenet dekódolása
            Console.WriteLine("");
            Console.WriteLine("Kódolt üzenet dekódolása kód segítségével");
            Console.Write("Írja ide a kódolt üzenetet! ");
            string kodolt = Console.ReadLine(); //kódolt üzenet
            Console.Write("Írja ide a kulcsot! ");
            string kulcs2 = Console.ReadLine();
            Console.WriteLine("A rejtjelezett üzenet: {0}", Dekódolás(kodolt, kulcs2));
        }
        static string Rejtjelezes(string uzenet, string kulcs1)
        {
            Dictionary<char, int> abc = new Dictionary<char, int>();
            Dictionary<int, char> revabc = new Dictionary<int, char>();
            for (char c = 'a'; c <= 'z'; c++)
            {

                abc.Add(c, (int)c - (int)'a');
                revabc.Add((int)c - (int)'a', c);
            }
            abc.Add(' ', 26);
            string ruzenet = ""; //rejtjelezett üzenet
            int szam = 0; //rejtjelezett üzenet i. karakterének kódja
            for (int i = 0; i < uzenet.Length; i++)
            {
                char nu = uzenet[i]; //i. vizsgált karaktere az üzenetnek
                char nk = kulcs1[i]; //i. vizsgált karaktere a kulcsnak
                szam = abc[nu] + abc[nk];
                szam = szam % 27;
                ruzenet += revabc[szam];

            }
            return ruzenet;
        }
        static string Dekódolás(string kodolt, string kulcs2)
        {
            Dictionary<char, int> abc = new Dictionary<char, int>();
            Dictionary<int, char> revabc = new Dictionary<int, char>();
            for (char c = 'a'; c <= 'z'; c++)
            {

                abc.Add(c, (int)c - (int)'a');
                revabc.Add((int)c - (int)'a', c);
            }
            abc.Add(' ', 26);
            string uzenet = ""; //dekódolt üzenet
            for (int i = 0; i < kodolt.Length; i++)
            {
                int szamrejt = 0; //kódolt üzenet i. karakterének kódja
                int szamkulcs = 0; //kulcs i. karakterének kódja
                int szamdekod = 0; //dekódolt üzenet i. karakterének kódja
                char nrejt = kodolt[i]; //rejtett uzenet i. karaktere
                char nkulcs = kulcs2[i]; //kulcs i. karaktere
                szamrejt = abc[nrejt];
                szamkulcs = abc[nkulcs];
                if (szamrejt < szamkulcs)
                {
                    szamdekod = (szamrejt + 27) - szamkulcs;
                    uzenet += revabc[szamdekod];
                }
                else
                {
                    szamdekod = szamrejt - szamkulcs;
                    uzenet += revabc[szamdekod];
                }
            }
            return uzenet;
        }
    }
}