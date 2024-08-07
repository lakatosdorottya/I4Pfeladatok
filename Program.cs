using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace I4Pfeladatok
{
    public class Program
    {
        public static List<string> szavak;
        public static Dictionary<char, int> abc;
        public static Dictionary<int, char> revabc;
        public static List<string> JoKulcsok;

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
            TitkositasOsztaly elso = new TitkositasOsztaly();
            Console.WriteLine("1. feladat: Üzenet és kód rejtett üzenetté alakítása");
            Console.Write("Írja ide az üzenetet! ");
            elso.uzenet = Console.ReadLine();
            Console.Write("Írja ide a kódot! ");
            elso.kulcs = Console.ReadLine();
            if (elso.uzenet.Length > elso.kulcs.Length)
            {
                Console.WriteLine("A kulcs nem elég hosszú.");
            }
            else
            {

                Console.WriteLine("A rejtjelezett üzenet: {0}", elso.Rejtjelezes());
            }

            //rejtjelezett üzenet dekódolása
            TitkositasOsztaly masodik = new TitkositasOsztaly();
            Console.WriteLine("");
            Console.WriteLine("Kódolt üzenet dekódolása kód segítségével");
            Console.Write("Írja ide a kódolt üzenetet! ");
            masodik.kodolt = Console.ReadLine(); //kódolt üzenet
            Console.Write("Írja ide a kulcsot! ");
            masodik.kulcs = Console.ReadLine(); masodik.Dekódolás();

            //2. feladat
            TitkositasOsztaly negyedik = new TitkositasOsztaly();
            Console.WriteLine("2. feladat");
            Console.Write("Írja ide az első kódolt üzenetet! ");
            string kodolt1 = "ebtobehpzmjnmfqwuirlazvslpl"; // Console.ReadLine();
            Console.Write("Írja ide a második kódolt üzenetet! ");
            negyedik.kodolt = "cvtlsxo fiutxysspjzxtxwp"; // Console.ReadLine();

            Console.WriteLine("");
            Console.WriteLine("");

            Kozoskulcs(kodolt1, negyedik, "");

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("A talált közös kulcsok:");
            foreach (string kulcs in JoKulcsok)
            {
                if ((kulcs.Length == kodolt1.Length) || (kulcs.Length == negyedik.kodolt.Length))
                {
                    Console.WriteLine("-" + kulcs + "-");
                }
            }
        }
        static void Kozoskulcs(string p_kodolt, TitkositasOsztaly p_negyedik, string p_kulcs)
        {
            Console.WriteLine(" - '" + p_kulcs + "'");
            if ((p_kodolt.Length <= p_kulcs.Length) || (p_negyedik.kodolt.Length <= p_kulcs.Length))
            {
                return;
            }
            TitkositasOsztaly harmadik = new TitkositasOsztaly();
            harmadik.kulcs = p_kulcs;
            harmadik.kodolt = p_kodolt;
            harmadik.uzenet = harmadik.Dekódolás(); //1. üzenet dekódolva
            string VizsgaltUzenet = harmadik.uzenet;
            for (int i = 0; i < szavak.Count; i++)
            {
                harmadik.uzenet = VizsgaltUzenet + " " + szavak[i];
                harmadik.uzenet = harmadik.uzenet.Trim();
                Console.WriteLine("    = '" + harmadik.uzenet + "'");
                p_kulcs = harmadik.Visszafejtes();
                p_negyedik.kulcs = p_kulcs;
                JoeErtek eredmeny = p_negyedik.Joe();
                if (eredmeny != JoeErtek.NemJo)
                {

                    if (eredmeny == JoeErtek.TeljesenJo)
                    {
                        if (harmadik.Joe() == JoeErtek.TeljesenJo)
                        {
                            JoKulcsok.Add(p_kulcs);
                        }
                        
                    }
                    
                    Kozoskulcs(p_kodolt, p_negyedik, p_kulcs);
                }
            }
            return;
        }
    }
}