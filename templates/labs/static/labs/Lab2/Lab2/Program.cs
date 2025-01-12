using System;
using System.IO;

public class Program
{
    // Функція перевірки, чи є в візерунку неприпустимий квадрат 2x2
    public static bool IsSymmetric(int M, int N, int[,] pattern)
    {
        for (int i = 0; i < M - 1; i++)
        {
            for (int j = 0; j < N - 1; j++)
            {
                // Перевіряємо квадрат 2x2
                if (pattern[i, j] == pattern[i, j + 1] &&
                    pattern[i, j] == pattern[i + 1, j] &&
                    pattern[i, j] == pattern[i + 1, j + 1])
                {
                    return false; // Якщо квадрат 2x2 однотонний, візерунок не симпатичний
                }
            }
        }
        return true; // Якщо неприпустимих квадратів немає
    }

    // Функція для обчислення всіх візерунків
    public static int CountSymmetricPatterns(int M, int N)
    {
        int totalPatterns = 0;
        int maxPattern = (int)Math.Pow(2, M * N); // Кількість всіх можливих варіантів плиток

        // Перебираємо всі можливі варіанти плиток (0 - чорна плитка, 1 - біла плитка)
        for (int i = 0; i < maxPattern; i++)
        {
            int[,] pattern = new int[M, N];

            // Заповнюємо плитки для поточного варіанту
            for (int j = 0; j < M * N; j++)
            {
                pattern[j / N, j % N] = (i >> j) & 1; // Генеруємо плитки за допомогою біта
            }

            // Перевіряємо, чи є неприпустимий квадрат 2x2
            if (IsSymmetric(M, N, pattern))
            {
                totalPatterns++; // Якщо візерунок симпатичний, збільшуємо лічильник
            }
        }

        return totalPatterns; // Повертаємо кількість симпатичних візерунків
    }

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
            string[] dimensions = input[0].Split(' ');
            int M = int.Parse(dimensions[0]);
            int N = int.Parse(dimensions[1]);

            // Обчислення кількості симпатичних візерунків
            int result = CountSymmetricPatterns(M, N);

            // Запис результату у файл OUTPUT.TXT
            File.WriteAllText(outputPath, result.ToString());
            Console.WriteLine("Результати успішно записані у файл OUTPUT.TXT.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка: {ex.Message}");
        }
    }
}
