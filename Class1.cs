using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace I4Pfeladatok
{
    internal class TitkositasOsztaly
    {
        public string uzenet;
        public string kulcs;
        public string kodolt;


        public TitkositasOsztaly(string p_uzenet, string p_kulcs, string p_kodolt)
        {
            uzenet = p_uzenet;
            kulcs = p_kulcs;
            kodolt = p_kodolt;
        }
        public TitkositasOsztaly() : this("", "", "")
        {

        }

        public string Rejtjelezes()
        {
            kodolt = ""; //rejtjelezett üzenet
            int szam = 0; //rejtjelezett üzenet i. karakterének kódja
            int n = 0;
            if (uzenet.Length > kulcs.Length)
            {
                n = kulcs.Length;
            }
            else
            {
                n = uzenet.Length;
            }
            for (int i = 0; i < n; i++)
            {
                char nu = uzenet[i]; //i. vizsgált karaktere az üzenetnek
                char nk = kulcs[i]; //i. vizsgált karaktere a kulcsnak
                szam = Program.abc[nu] + Program.abc[nk];
                szam = szam % 27;
                kodolt += Program.revabc[szam];

            }
            return kodolt;
        }
        public string Dekódolás()
        {
            uzenet = ""; //dekódolt üzenet
            int n = 0;
            if (kodolt.Length > kulcs.Length)
            {
                n = kulcs.Length;
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
                char nkulcs = kulcs[i]; //kulcs i. karaktere
                szamrejt = Program.abc[nrejt];
                szamkulcs = Program.abc[nkulcs];
                if (szamrejt < szamkulcs)
                {
                    szamdekod = (szamrejt + 27) - szamkulcs;
                    uzenet += Program.revabc[szamdekod];
                }
                else
                {
                    szamdekod = szamrejt - szamkulcs;
                    uzenet += Program.revabc[szamdekod];
                }
            }
            return uzenet;
        }
        public string Visszafejtes()
        {
            kulcs = ""; //dekódolt üzenet
            int n = 0;
            if (kodolt.Length < uzenet.Length)
            {
                n = kodolt.Length;
            }
            else
            {
                n = uzenet.Length;
            }
            for (int i = 0; i < n; i++)
            {
                int szamrejt = 0; //kódolt üzenet i. karakterének kódja
                int szamkulcs = 0; //kulcs i. karakterének kódja
                int szamdekod = 0; //dekódolt üzenet i. karakterének kódja
                char nrejt = kodolt[i]; //rejtett uzenet i. karaktere
                char nuzenet = uzenet[i]; //uzenet i. karaktere
                szamrejt = Program.abc[nrejt];
                szamkulcs = Program.abc[nuzenet];
                if (szamrejt < szamkulcs)
                {
                    szamdekod = (szamrejt + 27) - szamkulcs;
                    kulcs += Program.revabc[szamdekod];
                }
                else
                {
                    szamdekod = szamrejt - szamkulcs;
                    kulcs += Program.revabc[szamdekod];
                }
            }
            return kulcs;
        }
        public JoeErtek Joe()
        {
            uzenet = Dekódolás();
            List<string> words = new List<string>();
            words.AddRange(uzenet.Split(" "));
            for (int i = 0; i < words.Count - 1; i++)
            {
                if (!Program.szavak.Contains(words[i]))
                {
                    return JoeErtek.NemJo;
                }

            }
            for (int i = 0; i < Program.szavak.Count; i++)
            {
                if (Program.szavak[i] == (words[words.Count - 1]))
                {
                    return JoeErtek.TeljesenJo;
                }
                if (Program.szavak[i].StartsWith(words[words.Count - 1]))
                {

                    return JoeErtek.ReszbenJo;
                }
            }
            return JoeErtek.NemJo;
        }

    }


    public enum JoeErtek
    {
        NemJo,
        ReszbenJo,
        TeljesenJo
    }
}
