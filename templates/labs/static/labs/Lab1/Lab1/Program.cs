using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Визначення шляхів до файлів
        string inputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\INPUT.txt");
        string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\OUTPUT.txt");

        // Виведення шляхів для діагностики
        Console.WriteLine($"Input path: {inputPath}");
        Console.WriteLine($"Output path: {outputPath}");

        // Перевірка існування файлу INPUT.txt
        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Помилка: Файл INPUT.txt не знайдено.");
            return;
        }

        // Зчитування даних з файлу
        var lines = File.ReadAllLines(inputPath);
        int N = int.Parse(lines[0]); // Кількість листів
        var sheets = new List<Sheet>();

        for (int i = 0; i < N; i++)
        {
            // Зчитування значень ai та bi для кожного листа
            var values = lines[i + 1].Split(' ');
            double ai = double.Parse(values[0], CultureInfo.InvariantCulture);
            double bi = double.Parse(values[1], CultureInfo.InvariantCulture);

            sheets.Add(new Sheet(i + 1, ai, bi)); // Додаємо лист до списку
        }

        // Сортування для оптимального порядку розташування
        sheets.Sort((x, y) => Math.Min(y.A, y.B).CompareTo(Math.Min(x.A, x.B)));

        // Обчислення часу розчинення перегородки
        double totalTime = 0;
        double dissolutionTimeA = 0;
        foreach (var sheet in sheets)
        {
            dissolutionTimeA += sheet.A;
            totalTime = Math.Max(totalTime, dissolutionTimeA) + sheet.B;
        }

        // Запис результатів у файл OUTPUT.TXT
        using (var writer = new StreamWriter(outputPath))
        {
            writer.WriteLine(totalTime.ToString("F3", CultureInfo.InvariantCulture));
            writer.WriteLine(string.Join(" ", sheets.Select(s => s.Index)));
        }

        Console.WriteLine("Результати успішно записані у файл OUTPUT.txt.");
    }

    // Клас для зберігання інформації про лист
    class Sheet
    {
        public int Index { get; }
        public double A { get; }
        public double B { get; }

        public Sheet(int index, double a, double b)
        {
            Index = index;
            A = a;
            B = b;
        }
    }
}
