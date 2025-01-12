using System;
using System.IO;

public class Program
{
    // Описуємо рухи, які можна здійснити (вправо, вниз)
    private static readonly int[] dRow = { 1, 0 };
    private static readonly int[] dCol = { 0, 1 };

    public static void Main(string[] args)
    {
        // Шляхи до файлів
        string inputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\INPUT.TXT");
        string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\OUTPUT.TXT");

        // Перевірка наявності файлу INPUT.TXT
        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Помилка: файл INPUT.TXT не знайдено.");
            return;
        }

        try
        {
            // Зчитування вхідних даних
            string[] input = File.ReadAllLines(inputPath);

            // Перевірка, чи є хоча б один рядок в файлі
            if (input.Length == 0)
            {
                Console.WriteLine("Помилка: вхідний файл порожній.");
                return;
            }

            // Парсимо перший рядок для отримання N та M
            string[] dimensions = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (dimensions.Length != 2)
            {
                Console.WriteLine("Помилка: перший рядок повинен містити два числа (N та M).");
                return;
            }

            int N = int.Parse(dimensions[0]);
            int M = int.Parse(dimensions[1]);

            // Перевірка на коректність розмірів
            if (N < 1 || N > 30 || M < 1 || M > 30)
            {
                Console.WriteLine("Помилка: розміри карти повинні бути між 1 та 30.");
                return;
            }

            // Зчитування карти зараженості
            int[,] radiationMap = new int[N, M];
            for (int i = 0; i < N; i++)
            {
                // Перевірка, чи є вхідні дані в рядку
                if (i + 1 >= input.Length)
                {
                    Console.WriteLine("Помилка: недостатньо рядків для карти зараженості.");
                    return;
                }

                string[] line = input[i + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (line.Length != M)
                {
                    Console.WriteLine($"Помилка: кількість елементів у рядку {i + 1} не відповідає кількості стовпців ({M}).");
                    return;
                }

                for (int j = 0; j < M; j++)
                {
                    radiationMap[i, j] = int.Parse(line[j]);
                }
            }

            // Використовуємо алгоритм Дейкстри для пошуку мінімального шляху
            int result = FindMinimumRadiationPath(N, M, radiationMap);

            // Перевірка чи вже існує OUTPUT.TXT, якщо ні, то створюємо його
            if (!File.Exists(outputPath))
            {
                using (File.Create(outputPath)) { } // Створюємо порожній файл
            }

            // Запис результату у файл OUTPUT.TXT
            File.WriteAllText(outputPath, result.ToString());
            Console.WriteLine("Результати успішно записані у файл OUTPUT.TXT.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка: {ex.Message}");
        }
    }

    public static int FindMinimumRadiationPath(int N, int M, int[,] radiationMap)
    {
        // Масив для зберігання мінімальних доз радіації
        int[,] minRadiation = new int[N, M];
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                minRadiation[i, j] = int.MaxValue; // Ініціалізація значеннями нескінченності
            }
        }
        minRadiation[0, 0] = radiationMap[0, 0]; // Початкова точка

        // Черга пріоритетів для пошуку мінімального шляху
        PriorityQueue<(int, int), int> pq = new PriorityQueue<(int, int), int>();
        pq.Enqueue((0, 0), radiationMap[0, 0]); // Початок з верхнього лівого кута

        // Алгоритм Дейкстри
        while (pq.Count > 0)
        {
            var (row, col) = pq.Dequeue();

            // Перевірка сусідів (вниз та вправо)
            for (int i = 0; i < 2; i++)
            {
                int newRow = row + dRow[i];
                int newCol = col + dCol[i];

                // Перевірка коректності координат
                if (newRow >= 0 && newRow < N && newCol >= 0 && newCol < M)
                {
                    int newRadiation = minRadiation[row, col] + radiationMap[newRow, newCol];

                    // Якщо новий шлях має меншу сумарну дозу радіації, оновлюємо
                    if (newRadiation < minRadiation[newRow, newCol])
                    {
                        minRadiation[newRow, newCol] = newRadiation;
                        pq.Enqueue((newRow, newCol), newRadiation);
                    }
                }
            }
        }

        // Повертаємо мінімальну дозу радіації до правого нижнього кута
        return minRadiation[N - 1, M - 1];
    }
}