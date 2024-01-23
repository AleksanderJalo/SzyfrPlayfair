using System;
using System.Globalization;
using System.IO;

namespace Projekt { 
    class Program
    {
        static void Main(string[] args)
        {
            string sciezkaKlucz = "../../../slowo_klucz.txt";
            string sciezkaTekst = "../../../tekst.txt";
            string sciezkaSzyfr = "../../../zaszyfrowane.txt";
            string klucz = "";
            string[] wszystkieZnaki = { "A", "B", "C", "D", "E", "F", "G", "H", "I/J", "K", "L", "M", "N", "O", "P", "Q", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] macierzSzyfrowania = new string[25];
            string[,] macierzSzyfrowaniaDwaWymiary = new string[5,5];
            try
            {
                using (StreamReader reader = new StreamReader(sciezkaKlucz))
                {
                    klucz = reader.ReadLine();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd: " + e.Message);
            }
            int index = 0;
            foreach (char c in klucz)
            {
                string znak = c.ToString().ToUpper();
                if (znak == "I" || znak == "J")
                {
                    znak = "I/J";
                }
                if (!macierzSzyfrowania.Contains(znak))
                {
                    macierzSzyfrowania[index] = znak;
                    index++;
                }
            }
            foreach (string i in wszystkieZnaki)
            {
                string znak = i.ToUpper();
                if (!macierzSzyfrowania.Contains(znak))
                {
                    macierzSzyfrowania[index] = znak;
                    index++;
                }
            }
            string tekst = "";
            try
            {
                using (StreamReader reader = new StreamReader(sciezkaTekst))
                {
                    tekst = reader.ReadLine().ToUpper();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd: " + e.Message);
            }

            int xIndeks = 0;
            int yIndeks = 0;
            foreach(string i in macierzSzyfrowania)
            {
                if(yIndeks > 4)
                {
                    xIndeks++;
                    yIndeks = 0;
                }
                macierzSzyfrowaniaDwaWymiary[xIndeks,yIndeks] = i;
                yIndeks++;
            }
            tekst = tekst.Replace(" ", "");
            if(tekst.Length % 2 != 0)
            {
                tekst = tekst + "X";

            }

            string[] digramy = new string[tekst.Length / 2 ];
            int digramIndex = 0;
            for(int i = 0; i < tekst.Length; i += 2)
            {
                string digram = tekst[i].ToString() + tekst[i + 1].ToString();
                digramy[digramIndex] = digram;
                digramIndex++;
            }

            string szyfr = "";
            foreach(string digram in digramy)
            {
                Tuple<int, int> indeksyPierwszejLitery = znajdzIndeksy(macierzSzyfrowaniaDwaWymiary, digram[0].ToString());
                Tuple<int,int> indeksyDrugiejLitery = znajdzIndeksy(macierzSzyfrowaniaDwaWymiary, digram[1].ToString());
                szyfr+=SzyfrujDigram(macierzSzyfrowaniaDwaWymiary, indeksyPierwszejLitery, indeksyDrugiejLitery);
            }
            if (!File.Exists(sciezkaSzyfr))
            {
                FileStream sciezka = File.Create(sciezkaSzyfr);
            }
            using(StreamWriter zapis = new StreamWriter(sciezkaSzyfr))
            {
                zapis.Write(szyfr);
            }
        }
        static Tuple<int,int> znajdzIndeksy(string[,] macierzSzyfrowania, string litera)
        {
            if(litera == "I" || litera == "J")
            {
                litera = "I/J";
            }
            for (int i = 0; i < 5; i++) { 
                for(int j = 0; j < 5; j++)
                {
                    if (macierzSzyfrowania[i,j] == litera)
                    {
                        return Tuple.Create(i,j); 
                    }
                }
            }
            return null;
        }
        static string SzyfrujDigram(string[,] macierzSzyfrowania, Tuple<int, int> indeksyPierwszejLitery, Tuple<int, int> indeksyDrugiejLitery)
        {
            if(indeksyPierwszejLitery.Item1 == indeksyDrugiejLitery.Item1)
            {
                
                int pierwszaLiteraY = indeksyPierwszejLitery.Item2 + 1;
                if(pierwszaLiteraY == 5)
                {
                    pierwszaLiteraY = 0;
                }
                int drugaLiteraY = indeksyDrugiejLitery.Item2 + 1;
                if (  drugaLiteraY  == 5)
                {
                    drugaLiteraY = 0;
                }
                string pierwszaNowa = macierzSzyfrowania[indeksyPierwszejLitery.Item1, pierwszaLiteraY];
                string drugaNowa  = macierzSzyfrowania[indeksyDrugiejLitery.Item1, drugaLiteraY];
                return pierwszaNowa + drugaNowa;
            }
            if(indeksyPierwszejLitery.Item2 == indeksyDrugiejLitery.Item2)
            {
               
                int pierwszaLiteraX = indeksyPierwszejLitery.Item1 + 1;
                if (pierwszaLiteraX == 5)
                {
                    pierwszaLiteraX = 0;
                }
                int drugaLiteraX = indeksyDrugiejLitery.Item1 + 1;
                if (drugaLiteraX == 5)
                {
                    drugaLiteraX = 0;
                }

                string pierwszaNowa = macierzSzyfrowania[pierwszaLiteraX, indeksyPierwszejLitery.Item2];
                string drugaNowa = macierzSzyfrowania[drugaLiteraX, indeksyDrugiejLitery.Item2];
                return pierwszaNowa + drugaNowa;
            }
            string pierwszaNowav2 = macierzSzyfrowania[indeksyPierwszejLitery.Item1, indeksyDrugiejLitery.Item2];
            string drugaNowav2 = macierzSzyfrowania[indeksyDrugiejLitery.Item1, indeksyPierwszejLitery.Item2];
            return pierwszaNowav2 + drugaNowav2;

        }
    }
}