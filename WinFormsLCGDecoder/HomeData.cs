using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WinFormsLCGDecoder
{
    internal class HomeData
    {
        private Dictionary<string, double> Data { get; } = new Dictionary<string, double>();

        internal string GetData()
        {
            if (Data == null) throw new Exception("Необходимо выполнить метод [SetData(string text)]");
            return GetAnalysis();
        }

        internal string Analysis_2(string sentence)
        {
            var text = sentence.Split('\n');
            if (text.Length < 3) return null;

            var result = string.Empty;
            Data.Clear();

            const int m = 1000000; // Значения M - 10^6

            // Три числа подряд из файла
            var x = double.Parse(text[0]);
            var xn = double.Parse(text[1]);
            var xnn = double.Parse(text[2]);

            var sWatch = new Stopwatch();
            sWatch.Start();

            var found = false;

            var a = 0;
            var c = 0;

            for (; a < m; a++)
            {
                for (; c < m; c++)
                    if ((a * x + c) % m == xn && (a * xn + c) % m == xnn)
                    {
                        found = true;
                        break;
                    }

                if (found) break;
            }
                

            if (found)
            {
                result += $"Найдено!\na = {a}\nc = {c}\nПроверено вариантов: {a * c}\n";
                result += $"Текущие значения:\n{x}\n{xn}\n{xnn}\n";
                result += $"Следующие 10 значений:\n";

                x = xnn;
                for (var i = 0; i < 10; i++)
                {
                    x = (a * x + c) % m;
                    result += $"{x}\n";
                }
            }
            else
            {
                result = $"Коэффициенты A и C не найдены, попробуйте увеличить maxA или maxC\n";
            }
                
            sWatch.Stop();
            result += $"Время выполнения! {sWatch.ElapsedMilliseconds}";

            return result;
        }

        internal string Analysis(string sentence)
        {
            var text = sentence.Split('\n');
            if (text.Length < 3) return null;

            var result = string.Empty;
            Data.Clear();

            var m = Math.Pow(10, 6);          // Значения M - 10^6

            var x = double.Parse(text[0]);
            var xn = double.Parse(text[1]);
            var xnn = double.Parse(text[2]);

            var sWatch = new Stopwatch();
            sWatch.Start();

            for (int i = 0, a = 0; a < m && i < 10; a++)
            {
                if ((m * a + (xnn - xn)) / (xn - x) % 1 != 0) continue;

                var c = xn - x * a % m;
                if (c > m) continue;

                if ((a * x + c) % m != xn) continue;
                if ((a * xn + c) % m != xnn) continue;

                Data[$"A{i}"] = a;
                Data[$"C{i}"] = c;

                i++;
            }

            sWatch.Stop();
            result += $"Время выполнения! {sWatch.ElapsedMilliseconds}";
            result += GetAnalysis();

            return result;
        }

        private string GetAnalysis()
        {
            return Data.Aggregate("------\n", (current, d) => current + $"key: {d.Key}\t - value: {d.Value}\n");
        }
    }
}