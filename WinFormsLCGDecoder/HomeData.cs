using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WinFormsLCGDecoder
{
    internal class HomeData
    {
        private Dictionary<string, Int64> Data { get; } = new Dictionary<string, Int64>();

        internal string AnalysisBruteforce(Home home, string sentence)
        {
            var text = sentence.Split('\n');
            if (text.Length < 3) return null;

            var result = string.Empty;

            const Int64 m = 1000000; // Значения M - 10^6

            // Три числа подряд из файла
            Int64 x = Int64.Parse(text[0]);
            Int64 xn = Int64.Parse(text[1]);
            Int64 xnn = Int64.Parse(text[2]);

            Int64 A = 0;
            Int64 C = 0;

            var sWatch = new Stopwatch();
            sWatch.Start();

            var found = false;

            for (Int64 a = 0; a < m; a++)
            {
                for (Int64 c = 0; c < m; c++)
                    if ((a * x + c) % m == xn && (a * xn + c) % m == xnn)
                    {
                        A = a;
                        C = c;

                        found = true;
                        break;
                    }

                home.SetStatusInvoke();
                if (found) break;
            }
                

            if (found)
            {
                result += $"Найдено!\nA = {A}\nC = {C}\nПроверено вариантов: {A * C}\n";
                result += $"Текущие значения:\n{x}\n{xn}\n{xnn}\n";
                result += $"Следующие 10 значений:\n";

                x = xnn;
                for (var i = 0; i < 10; i++)
                {
                    x = (A * x + C) % m;
                    result += $"{x}\n";
                }
            }
            else
            {
                result = $"Коэффициенты A и C не найдены, попробуйте увеличить maxA или maxC\n";
            }
                
            sWatch.Stop();
            result += $"Время выполнения: {sWatch.ElapsedMilliseconds} Milliseconds\n";

            return result;
        }

        internal string AnalysisJamesReeds(string sentence)
        {
            
            var text = sentence.Split('\n');
            if (text.Length < 3) return null;

            var result = string.Empty;
            Data.Clear();

            Int64 m = 1000000;          // Значения M - 10^6

            Int64 x = Int64.Parse(text[0]);
            Int64 xn = Int64.Parse(text[1]);
            Int64 xnn = Int64.Parse(text[2]);

            var sWatch = new Stopwatch();
            sWatch.Start();

            for (Int64 i = 0, a = 0; a < m && i < 10; a++)
            {
                if ((m * a + (xnn - xn)) / (xn - x) % 1 != 0) continue;

                Int64 c = xn - x * a % m;
                if (c > m) continue;

                if ((a * x + c) % m != xn) continue;
                if ((a * xn + c) % m != xnn) continue;

                Data[$"A{i}"] = a;
                Data[$"C{i}"] = c;

                i++;
            }

            sWatch.Stop();
            result += $"Время выполнения: {sWatch.ElapsedMilliseconds} Milliseconds\n";
            result += GetAnalysis();

            result += $"Текущие значения:\n{x}\n{xn}\n{xnn}\n";
            result += $"Следующие 10 значений:\n";

            x = xnn;
            for (var i = 0; i < 10; i++)
            {
                x = ( Data["A0"] * x + Data["C0"]) % m;
                result += $"{x}\n";
            }

            return result;
        }

        private string GetAnalysis()
        {
            return Data.Aggregate("------\n", (current, d) => current + $"key: {d.Key}\t - value: {d.Value}\n");
        }
    }
}