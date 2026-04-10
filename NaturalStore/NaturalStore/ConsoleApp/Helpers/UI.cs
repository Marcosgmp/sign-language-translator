namespace ConsoleApp.Helpers;

public static class UI
{
    public static void Header(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{'='.Repeat(50)}");
        Console.WriteLine($"  {title}");
        Console.WriteLine($"{'='.Repeat(50)}");
        Console.ResetColor();
    }

    public static void Success(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✔ {msg}");
        Console.ResetColor();
    }

    public static void Error(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✘ {msg}");
        Console.ResetColor();
    }

    public static void Info(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  {msg}");
        Console.ResetColor();
    }

    public static void Separator() => Console.WriteLine(new string('-', 50));

    public static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write($"  {prompt}: ");
            var val = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(val)) return val;
            Error("Campo obrigatório. Tente novamente.");
        }
    }

    public static decimal ReadDecimal(string prompt, decimal min = 0)
    {
        while (true)
        {
            Console.Write($"  {prompt}: ");
            if (decimal.TryParse(Console.ReadLine()?.Replace(",", "."), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var val) && val >= min)
                return val;
            Error($"Valor inválido. Deve ser numérico e >= {min}.");
        }
    }

    public static int ReadInt(string prompt, int min = 1)
    {
        while (true)
        {
            Console.Write($"  {prompt}: ");
            if (int.TryParse(Console.ReadLine(), out var val) && val >= min) return val;
            Error($"Valor inválido. Deve ser inteiro e >= {min}.");
        }
    }

    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            var val = ReadInt(prompt, min);
            if (val <= max) return val;
            Error($"Valor deve ser entre {min} e {max}.");
        }
    }

    public static TimeSpan ReadTime(string prompt)
    {
        while (true)
        {
            Console.Write($"  {prompt} (HH:mm): ");
            if (TimeSpan.TryParseExact(Console.ReadLine()?.Trim(), @"hh\:mm",
                System.Globalization.CultureInfo.InvariantCulture, out var ts))
                return ts;
            Error("Formato inválido. Use HH:mm (ex: 08:00).");
        }
    }

    public static bool Confirm(string prompt)
    {
        Console.Write($"  {prompt} (s/n): ");
        return Console.ReadLine()?.Trim().ToLower() == "s";
    }

    public static void Pause()
    {
        Console.WriteLine("\n  Pressione qualquer tecla para continuar...");
        Console.ReadKey(true);
    }

    public static int Menu(string title, string[] options)
    {
        Header(title);
        for (int i = 0; i < options.Length; i++)
            Console.WriteLine($"  [{i + 1}] {options[i]}");
        Console.WriteLine($"  [0] Voltar / Sair");
        return ReadInt("Opção", 0, options.Length);
    }
}

public static class CharExtensions
{
    public static string Repeat(this char c, int count) => new string(c, count);
}
