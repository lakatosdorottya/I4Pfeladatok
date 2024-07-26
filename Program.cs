using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace óbudai
{
    internal class Program
    {
        static List<string> szavak;
        static Dictionary<char, int> abc;
        static Dictionary<int, char> revabc;
        static List<string> JoKulcsok;

        static void Main(string[] args)
        {
            JoKulcsok = new List<string>();
            abc = new Dictionary<char, int>();
            revabc = new Dictionary<int, char>();
            szavak = new List<string>();
            szavak.AddRange(File.ReadAllLines("words.txt"));
            for (char c = 'a'; c <= 'z'; c++)
            {

                abc.Add(c, (int)c - (int)'a');
                revabc.Add((int)c - (int)'a', c);
            }
            abc.Add(' ', 26);
            revabc.Add(26, ' ');

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
            Console.WriteLine("A dekódolt üzenet: {0}", Dekódolás(kodolt, kulcs2));
            
            //2. feladat
             Console.WriteLine("2. feladat");
            Console.Write("Írja ide az első kódolt üzenetet! ");
            string uz1 = Console.ReadLine();
            Console.Write("Írja ide a második kódolt üzenetet! "); 
            string uz2 = Console.ReadLine();

            Kozoskulcs(uz1, uz2, "");

            Console.WriteLine("A talált közös kulcsok:");
            foreach (string kulcs in JoKulcsok)
            {
                if ((kulcs.Length >= uz1.Length) || (kulcs.Length >= uz2.Length))
                {
                    Console.WriteLine(kulcs);
                }
            }
        }

        static string Rejtjelezes(string uzenet, string kulcs1)
        {
            string ruzenet = ""; //rejtjelezett üzenet
            int szam = 0; //rejtjelezett üzenet i. karakterének kódja
            int n = 0;
            if (uzenet.Length > kulcs1.Length)
            {
                n = kulcs1.Length;
            }
            else
            {
                n = uzenet.Length;
            }
            for (int i = 0; i < n; i++)
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
            string uzenet = ""; //dekódolt üzenet
            int n = 0;
            if (kodolt.Length > kulcs2.Length)
            {
                n = kulcs2.Length;
            }
            else
            {
                n = kodolt.Length;
            }
            for (int i = 0; i < n; i++)
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

        static bool Joe(string uz2, string kulcs)
        {
            string dekuz2 = Dekódolás(uz2, kulcs); //2. üzenet dekódolva
            List<string> words = new List<string>();
            words.AddRange(dekuz2.Split(" "));
            for (int i = 0; i < words.Count - 1; i++)
            {
                if (!szavak.Contains(words[i]))
                {
                    return false;
                }

            }
            for (int i = 0; i < szavak.Count; i++)
            {
                if (szavak[i].StartsWith(words[words.Count - 1]))
                {
                    return true;
                }
            }
            return false;
        }
        static void Kozoskulcs(string uz1, string uz2, string kulcs)
        {
            if ((uz1.Length <= kulcs.Length) || (uz2.Length <= kulcs.Length))
            {
                return;
            }
            string dekuz1 = Dekódolás(uz1, kulcs); //1. üzenet dekódolva
            for (int i = 0; i < szavak.Count; i++)
            {
                string tesztuz1 = dekuz1 + " " + szavak[i];
                tesztuz1 = tesztuz1.Trim();
                kulcs = Dekódolás(uz1, tesztuz1);
                if (Joe(uz2, kulcs))
                {
                    JoKulcsok.Add(kulcs);
                    Kozoskulcs(uz1, uz2, kulcs);
                }
            }
            return;
        }
    }
}