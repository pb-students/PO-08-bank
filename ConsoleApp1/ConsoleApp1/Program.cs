

// Author: Najlepszy Wyraz Twarzy


using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank(16);

            Console.WriteLine(bank);

            Console.WriteLine("Łączne środki firm:" + bank.GetŁączneSaldoKlientów(Bank.TypKlienta.Firmy));
            Console.WriteLine("Łączne środki osób fizycznych:" + bank.GetŁączneSaldoKlientów(Bank.TypKlienta.OsobyFizyczne));
            Console.WriteLine("Łączne środki dużych firm i ważnych osób:" + bank.GetŁączneSaldoKlientów(Bank.TypKlienta.VIP));
            Console.WriteLine("Łączne środki zwykłych osób fizycznych:" + bank.GetŁączneSaldoKlientów(Bank.TypKlienta.ZwykłeOsobyFizyczne));

        }
    }


    class Bank
    {
        List<Klient> klienci = new List<Klient>();

        /// <summary>
        /// Generuje i dodaje do puli klientów ilość klientów sprecyzowaną w parametrze
        /// </summary>
        /// <param name="liczbaKlientów">Ilość klientów do wygenerowania</param>
        public Bank(int liczbaKlientów)
        {
            Random rng = new Random();

            int rodzajKlienta;
            Klient nowyKlient;
            int ilośćKont;
            short[] numerKonta = new short[7];
            Konto noweKonto;

            for (int i = 0; i < liczbaKlientów; i++)
            {
                rodzajKlienta = rng.Next(0, 4);

                switch(rodzajKlienta)
                {
                    case 0:
                        nowyKlient = new Firma();
                        break;

                    case 1:
                        nowyKlient = new DużaFirma();
                        break;

                    case 2:
                        nowyKlient = new Osoba();
                        break;

                    case 3:
                        nowyKlient = new WażnaOsoba();
                        break;

                    default:
                        nowyKlient = new Klient();
                        Console.WriteLine("Błąd w generowaniu klientów");
                        break;
                }

                ilośćKont = rng.Next(1, 4);

                for(int ii = 0; ii < ilośćKont; ii++)
                {
                    numerKonta[0] = (short)rng.Next(80, 100);
                    for (int iii = 1; iii < 7; iii++)
                    {
                        numerKonta[iii] = (short)rng.Next(0, 10000);
                    }

                    noweKonto = new Konto(ConvertToAccountNumber(numerKonta));
                    noweKonto.Wpłać(rng.Next(0, 1000000));

                    nowyKlient.DodajKonto(noweKonto);
                }

                klienci.Add(nowyKlient);
            }
        }
        public Bank()
        {

        }
        

        public enum TypKlienta { Firmy, OsobyFizyczne, VIP, ZwykłeOsobyFizyczne}

        public double GetŁączneSaldoKlientów(TypKlienta typKlienta)
        {
            double suma = 0;

            foreach(Klient klient in klienci)
            {
                switch(typKlienta)
                {
                    case TypKlienta.Firmy:
                        if (klient.GetType().Name == "Firma" || klient.GetType().Name == "DużaFirma")
                            foreach (Konto konto in klient.konta)
                                suma += konto.saldo;
                        break;

                    case TypKlienta.OsobyFizyczne:
                        if (klient.GetType().Name == "Osoba" || klient.GetType().Name == "WażnaOsoba")
                            foreach (Konto konto in klient.konta)
                                suma += konto.saldo;
                        break;

                    case TypKlienta.VIP:
                        if (klient.GetType().Name == "DużaFirma" || klient.GetType().Name == "WażnaOsoba")
                            foreach (Konto konto in klient.konta)
                                suma += konto.saldo;
                        break;

                    case TypKlienta.ZwykłeOsobyFizyczne:
                        if (klient.GetType().Name == "Osoba")
                            foreach (Konto konto in klient.konta)
                                suma += konto.saldo;
                        break;

                    default:
                        break;
                }
            }

            return suma;
        }
        

        public static string ConvertToAccountNumber(short[] array)
        {
            string result = array[0].ToString();

            for (int i = 1; i < array.Length; i++)
                result += $" {(array[i] < 1000 ? (array[i] < 100 ? (array[i] < 10 ? "000" : "00") : "0") : "")}{array[i]}";

            return result;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();


            foreach (Klient klient in klienci)
                builder.AppendLine(klient.ToString());

            return builder.ToString();
        }
    }


    class WażnaOsoba : Osoba
    {
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"  Ważna osoba: {imie} {nazwisko} Pesel: {pesel} Łączne saldo: {łączneSaldo}zł");
            builder.Append(KontaString);

            return builder.ToString();
        }
    }

    class Osoba : Klient
    {
        public string imie;
        public string nazwisko;
        public string pesel;

        //public Osoba(int index)
        //{
        //    imie = "Imie_" + index;
        //    imie = "Nazwisko_" + index;
        //}

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"  Osoba: {imie} {nazwisko} Pesel: {pesel} Łączne saldo: {łączneSaldo}zł");
            builder.Append(KontaString);

            return builder.ToString();
        }

        public static string GetRandomPesel()
        {
            Random rng = new Random();

            return "";
        }
    }


    class DużaFirma : Firma
    {
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"  Duża firma: {nazwa} KRS: {krs} Łączne saldo: {łączneSaldo}zł");
            builder.Append(KontaString);

            return builder.ToString();
        }
    }

    class Firma : Klient
    {
        public string nazwa;
        public string krs;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"  Firma: {nazwa} KRS: {krs} Łączne saldo: {łączneSaldo}zł");
            builder.Append(KontaString);

            return builder.ToString();
        }
    }


    class Klient
    {
        List<Konto> __konta = new List<Konto>();
        public List<Konto> konta { get { return __konta; } }
        
        // Wcześniej to było w ToString() ale zauważyłem żę klasy DużaFirma i WażnaOsoba nie mogły z tego korzystać tak jak Firma i Osoba
        // korzystały za pomocą base.ToString() :/
        public string KontaString
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                // W tym miejscu ulewał się stos, ale magicznie przestał.
                // Jeśli znowu zacznie polecam usunąć kilka linijek i je napisać na nowo, nie wiem dlaczego ale działa.
                foreach (Konto konto in konta)
                    builder.AppendLine($"    {konto}");

                return builder.ToString();
            }
        }

        public double łączneSaldo
        {
            get
            {
                double sum = 0;
                foreach (Konto konto in konta)
                    sum += konto.saldo;
                return sum;
            }
        }

        public void DodajKonto(Konto konto)
        {
            __konta.Add(konto);
        }


        public override string ToString()
        {
            // O i patrzcie jak czyściutko!
            return KontaString;
            //return $"    Tu powinno być {konta.Count} kont...";//ToString();
        }

        public static bool operator ==(Klient a, Klient b)
        {
            return a.konta == b.konta;
        }
        public static bool operator !=(Klient a, Klient b)
        {
            return a.konta != b.konta;
        }
    }


    class Konto
    {
        string __numer;
        public string numer { get { return __numer; } }

        double __saldo;
        public double saldo { get { return __saldo; } }

        public Konto(string numer)
        {
            __numer = numer;
        }

        public void Wpłać(double ilość)
        {
            __saldo += ilość;
        }
        public void Wypłać(double ilość)
        {
            __saldo -= ilość;
        }


        public override string ToString()
        {
            //StringBuilder builder = new StringBuilder(":");

            //builder.Append(numer);
            //builder.Append(" : ");
            //builder.Append(saldo);

            //Console.WriteLine(builder.ToString());

            return $": {numer} : {saldo}zł";
            //return ":" + numer + " : " + saldo;
            //return builder.ToString();
            //return "";
        }
    }
}
