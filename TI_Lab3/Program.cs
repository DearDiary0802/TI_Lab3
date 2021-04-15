using System;
using System.Numerics;

namespace TI_Lab3
{
    class Program
    {
        static bool IsTheNumberSimple(long Number)
        {
            if (Number < 2)
                return false;
            if (Number == 2)
                return true;
            for (int i = 2; i < Math.Sqrt(Number) + 1; i++)
                if (Number % i == 0)
                    return false;
            return true;
        }

        static bool AreMutuallySimple(long fi, long e)
        {
            while (fi != e)
            {
                if (fi < e)
                    e = e - fi;
                else
                    fi = fi - e;
            }
            return (fi == 1);
        }

        static (long, long, long) Euclidex(long a1, long b1)
        {
            long a = a1, b = b1, x, y, d;
            long q, r, x1, x2, y1, y2;

            if (b == 0)
            {
                d = a;
                x = 1;
                y = 0;
                return (x, y, d);
            }

            x2 = 1;
            x1 = 0;
            y2 = 0;
            y1 = 1;

            while (b > 0)
            {
                q = a / b;
                r = a - q * b;
                x = x2 - q * x1;
                y = y2 - q * y1;
                a = b;
                b = r;
                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            d = a;
            x = x2;
            y = y2;
            return (x, y, d);
        }
        static BigInteger FastExp(BigInteger m, BigInteger d, BigInteger r)
        {
            BigInteger a1 = m, z1 = d, x = 1;
            while (z1 != 0)
            {
                while (z1 % 2 == 0)
                {
                    z1 = z1 / 2;
                    a1 = (a1 * a1) % r;
                }
                z1 -= 1;
                x = (x * a1) % r;
            }
            return x;
        }

        static BigInteger h(BigInteger[] M, long n)
        {
            BigInteger H0 = 100;
            BigInteger Hi = 0, tmp = 0;

            for (int i = 0; i < M.Length; i++)
            {
                Hi = FastExp((H0 + M[i]), 2, n);
                H0 = Hi;
            }
            return Hi;
        }

        static (BigInteger, BigInteger) AlgorithmRSAEncrypt(long r, long d, string sourceText)
        {
            BigInteger m;
            BigInteger[] arr = new BigInteger[sourceText.Length];
            for (int i = 0; i < sourceText.Length; i++)
                arr[i] = sourceText[i];

            m = h(arr, r);

            BigInteger S;
            S = FastExp(m, d, r);

            return (m, S);
        }

        static string AlgorithmRSADecrypt(long e, long r, BigInteger S, string sourceText)
        {
            string res = "";
            BigInteger m;
            m = FastExp(S, e, r);

            BigInteger m1;
            BigInteger[] arr = new BigInteger[sourceText.Length];
            for (int i = 0; i < sourceText.Length; i++)
                arr[i] = sourceText[i];
            m1 = h(arr, r);

            if (m != m1)
            {
                res = "Подпись недействительна";
            }
            else 
            {
                res = "Подпись действительна";
            }

            return res;
        }

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Выберите действие: ");
                Console.WriteLine("1. Вычисление цифровой подписи, 2. Проверка цифровой подписи");
                int act = Convert.ToInt32(Console.ReadLine());

                long p = 0, q = 0, e = 0;
                long d, r, fi;
                string text = "", res;
                bool CheckingConditions = false;
                Random rnd = new Random();
                bool isOK = true;

                if (act == 1)
                {
                    Console.WriteLine("Введите текст: ");
                    text += Console.ReadLine();
                    do
                    {
                        while (!CheckingConditions)
                        {
                            p = rnd.Next(1, 10000);
                            CheckingConditions = IsTheNumberSimple(p);

                        }
                        CheckingConditions = false;
                        while (!CheckingConditions)
                        {
                            q = rnd.Next(1, 10000);
                            CheckingConditions = IsTheNumberSimple(q);
                        }
                        r = p * q;
                        fi = (p - 1) * (q - 1);
                        CheckingConditions = false;
                        while (!CheckingConditions)
                        {
                            e = rnd.Next(600, 10000);
                            CheckingConditions = AreMutuallySimple(fi, e);
                        }
                        var tuple = Euclidex(e, fi);
                        long temp = 0;
                        if (tuple.Item3 == 1)
                            temp = tuple.Item1;
                        else
                            isOK = false;
                        if (temp < 0)
                            temp += fi;
                        if (temp == 0)
                            isOK = false;
                        d = temp;

                    } while (!isOK);

                    Console.WriteLine("Секретный ключ подписи: ({0}, {1})", d, r);
                    var result = AlgorithmRSAEncrypt(r, d, text);

                    Console.WriteLine("Электронный документ: ({0}, {1})", text, result.Item2);
                    Console.WriteLine("Открытый ключ: ({0}, {1})", e, r);
                }

                if (act == 2)
                {
                    BigInteger S, M;
                    Console.WriteLine("Введите сообщение: ");
                    text = Console.ReadLine();
                    Console.WriteLine("Введите цифровую подпись S: ");
                    S = BigInteger.Parse(Console.ReadLine());

                    Console.WriteLine("Введите открытый ключ e: ");
                    e = Convert.ToInt64(Console.ReadLine());
                    Console.WriteLine("Введите открытый ключ r: ");
                    r = Convert.ToInt64(Console.ReadLine());

                    res = AlgorithmRSADecrypt(e, r, S, text);

                    Console.WriteLine("Результат проверки: {0}", res);
                }
                Console.WriteLine("Нажмите ESC для выхода или другую клавишу для продолжения работы");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}
