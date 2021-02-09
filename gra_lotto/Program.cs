using System;
using System.Collections.Generic;

namespace gra_lotto
{
    class Program
    {
        static int kumulacja; //do wygrania danego dnia
        static int START = 40; //tyle mamy w portfelu na poczatku
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            int pieniadze = START;
            int dzien = 0;
            do
            {
                pieniadze = START;
                dzien = 0;
                ConsoleKey wybor;
                do
                {
                    kumulacja = rnd.Next(2, 37) * 1000000;
                    dzien++;
                    int l_losow = 0;
                    List<int[]> kupon = new List<int[]>();
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Grasz dzień {0}.", dzien);
                        Console.WriteLine("Dziś do wygrania {0} zł!", kumulacja);
                        Console.WriteLine("Stan Twojego konta to: {0} zł.", pieniadze);
                        WyswietlKupon(kupon);
                        if (pieniadze >= 3 && l_losow < 8)
                        {
                            Console.WriteLine("1 - Postaw los -3 zł [{0}/8]", l_losow + 1);
                        }
                        Console.WriteLine("2 - Sprawdż kupon - losowanie.");
                        Console.WriteLine("3 - Zakończ grę.");
                        wybor = Console.ReadKey().Key;
                        if (wybor == ConsoleKey.NumPad1 && pieniadze >= 3 && l_losow < 8)
                        {
                            kupon.Add(PostawLos());
                            pieniadze -= 3;
                            l_losow++;
                        }

                    } while (wybor == ConsoleKey.NumPad1);
                    Console.Clear();
                    if(kupon.Count > 0)
                    {
                        int wygrana = Sprawdz(kupon);
                        if (wygrana > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Brawo! Wygrałeś {0} zł w tym losowaniu!", wygrana);
                            Console.ForegroundColor = ConsoleColor.Black;
                            pieniadze += wygrana;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Niestey, nic nie wygrałeś :(");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.WriteLine("W tym losowaniu nie miałes żadnych losów.");
                    }
                    Console.WriteLine("Wciśnij Enter by kontynuować.");
                    Console.ReadKey();
                } while (pieniadze >= 3 && wybor != ConsoleKey.NumPad3);
                Console.Clear();
                Console.WriteLine("Dzień {0}. Koniec gry. Twój wynik to {1} zł.", dzien, pieniadze - START);
                Console.WriteLine("Wciśnij Enter by grac od nowa :)");
            } while (Console.ReadKey().Key == ConsoleKey.Enter);
        }

        private static int Sprawdz(List<int[]> kupon)
        {
            int[] wylosowane = new int[6];
            for (int i = 0; i < wylosowane.Length; i++)
            {
                int los = rnd.Next(1, 50);
                if (Array.IndexOf(wylosowane, los) == -1)
                {
                    wylosowane[i] = los;
                }
                else
                {
                    i--;
                }
            }
            Console.WriteLine("Wylosowane liczby to: ");
            foreach(int liczba in wylosowane)
            {
                Console.Write("{0}, ", liczba);
            }
            int[] trafione = SprawdzKupon(kupon, wylosowane);
            int wartosc = 0;
            int wygrana = 0;
            Console.WriteLine();
            if (trafione[0] > 0)
            {
                // trafilismy w 3 liczby
                wartosc = trafione[0] * 24;
                Console.WriteLine("Trzy trafienia: {0} + {1} zł", trafione[0], wartosc);
                wygrana += wartosc;
            }
            if (trafione[1] > 0)
            {
                // trafilismy w 4 liczby
                wartosc = trafione[1] * rnd.Next(100, 301);
                Console.WriteLine("Cztery trafienia: {0} + {1} zł", trafione[1], wartosc);
                wygrana += wartosc;
            }
            if (trafione[2] > 0)
            {
                wartosc = trafione[2] * rnd.Next(4000, 8001);
                Console.WriteLine("Pięć trafień: {0} + {1} zł", trafione[2], wartosc);
                wygrana += wartosc;
            }
            if (trafione[3] > 0)
            {
                wartosc = (trafione[3] * kumulacja) / (trafione[3] + rnd.Next(0, 5)); // zakladamy ze maks 4 osoby moja jednoczenie trafic szostke
                Console.WriteLine("Sześć trafień: {0} + {1} zł", trafione[3], wartosc);
                wygrana += wartosc;
            }

            return wygrana;
        }

        private static int[] SprawdzKupon(List<int[]> kupon, int[] wylosowane)
        {
            int[] wygrane = new int[4];
            int i = 0;
            Console.WriteLine("\n\n TWÓJ KUPON:");
            foreach(int[] los in kupon)
            {
                i++;
                Console.WriteLine("Los {0}: ", i);
                int trafien = 0;
                foreach(int liczba in los)
                {
                    if(Array.IndexOf(wylosowane, liczba) != -1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("{0}, ", liczba);
                        Console.ResetColor();
                        trafien++;
                    }
                    else
                    {
                        Console.Write("{0}, ", liczba);
                    }
                }
                switch(trafien)
                {
                    case 3:
                        wygrane[0]++;
                        break;
                    case 4:
                        wygrane[1]++;
                        break;
                    case 5:
                        wygrane[2]++;
                        break;
                    case 6:
                        wygrane[3]++;
                        break;
                }
                Console.WriteLine(" - Trafiono {0}/6", trafien);
            }
            return wygrane;
        }

        private static int[] PostawLos()
        {
            int[] liczby = new int[6];
            int liczba;
            for(int i = 0; i < liczby.Length; i++)
            {
                liczba = -1;
                Console.Clear();
                Console.WriteLine("Postawione liczby: ");
                foreach (int l in liczby)
                {
                    if (l > 0)
                    {
                        Console.Write("{0}, ", l);
                    }
                }
                Console.WriteLine("\n\nWybierz liczbę od 1 do 49:");
                Console.WriteLine("{0}/6: ", i+1);
                bool prawidlowa = int.TryParse(Console.ReadLine(), out liczba);
                //Console.WriteLine(liczba);
                //Console.WriteLine(prawidlowa);
                if(prawidlowa && liczba >= 1 && liczba <= 49 && Array.IndexOf(liczby, liczba) == -1)
                {
                    liczby[i] = liczba;
                    //Console.WriteLine(Array.IndexOf(liczby, liczba));
                    //Console.WriteLine("i= {0}", i);
                    //Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Niestety błędna liczba.");
                    //Console.WriteLine(Array.IndexOf(liczby, liczba));
                    i--;
                    Console.ReadKey();
                }
                //Array.Sort(liczby);
            }
            return liczby;
        }

        private static void WyswietlKupon(List<int[]> kupon)
        {
            if(kupon.Count == 0)
            {
                Console.WriteLine("Nie postawiłeś jeszcze żadnych losów");
            }
            else
            {
                int i = 0;
                Console.WriteLine("\nTwój kupon:");
                foreach (int[] los in kupon )
                {
                    i++;
                    Console.WriteLine("Los nr {0}: ", i);
                    foreach(int liczba in los)
                    {
                        Console.Write("{0}, ", liczba);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
