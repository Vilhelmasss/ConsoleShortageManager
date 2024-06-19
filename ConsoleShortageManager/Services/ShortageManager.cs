using System.Text.Json;

namespace ConsoleShortageManager;

public static class ShortageManager
{
    public const string JSON_FILE_NAME = "ShortageList.json";
    private static List<Shortage> shortages;

    static ShortageManager()
    {
        // Reads Shortages first time ShortageManager methods are being used.
        shortages = ReadShortages();
    }

    private static void SortShortages()
    {
        shortages = shortages.OrderByDescending(s => s.Priority).ToList();
    }

    public static void AddShortage(Shortage shortage)
    {
        shortages.Add(shortage);
        SaveShortages();
    }

    public static bool DeleteShortage(Shortage shortage)
    {
        if (!UserValidation.VerifyUser(shortage))
            return false;

        shortages.Remove(shortage);
        SaveShortages();
        return true;
    }

    public static List<Shortage> GetShortageList()
    {
        SortShortages();
        return shortages;
    }

    private static void SaveShortages()
    {
        SortShortages();
        var shortageJson = JsonSerializer.Serialize(shortages);
        File.WriteAllText(JSON_FILE_NAME, shortageJson);
    }

    public static void PrintShortageList()
    {
        foreach (var shortage in shortages)
            // is equal to user or admin user
            PrintShortage(shortage);
    }

    public static void PrintShortage(Shortage shortage)
    {
        if (UserValidation.VerifyUser(shortage))
            Console.WriteLine($"{shortage.Title} {shortage.Name} {shortage.Room} {shortage.Category} {shortage.Priority} {shortage.CreatedOn} {shortage.CreatedBy}");
    }

    private static List<Shortage> ReadShortages()
    {
        if (!File.Exists(JSON_FILE_NAME))
            return new List<Shortage>();

        var shortageJson = File.ReadAllText(JSON_FILE_NAME);
        return JsonSerializer.Deserialize<List<Shortage>>(shortageJson) ?? new List<Shortage>();
    }

    public static bool MatchShortage(Shortage shortage, string title, string name)
    {
        if (shortage.Title == title && shortage.Name == name && UserValidation.VerifyUser(shortage))
            return true;

        return false;
    }

    public static bool MatchShortage(Shortage shortage, Room room)
    {
        if (shortage.Room == room && UserValidation.VerifyUser(shortage))
            return true;

        return false;
    }

    public static bool MatchShortage(Shortage shortage, Category category)
    {
        if (shortage.Category == category && UserValidation.VerifyUser(shortage))
            return true;

        return false;
    }

    public static bool MatchShortage(Shortage shortage, DateTime start, DateTime end)
    {
        if(shortage.CreatedOn >= start && shortage.CreatedOn <= end && UserValidation.VerifyUser(shortage))
            return true;

        return false;
    }
}