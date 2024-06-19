namespace ConsoleShortageManager;

public static class CommandListener
{
    public static void StartListening()
    {
        while (true)
            ReadCommands();
    }

    private static void ReadCommands()
    {
        while (true)
        {
            Console.WriteLine("\nEnter one of the listed `commands`");
            Console.WriteLine("`filter` `delete` `add`");
            var input = Console.ReadLine()?.ToLower().Trim(' ');

            switch (input)
            {
                case "filter":
                    ReadFilterFlags();
                    return;
                case "add":
                    ReadAddParameters();
                    return;
                case "delete":
                    ReadAndValidateDelete();
                    return;
                default:
                    Console.WriteLine("Wrong Input. Try again.");
                    break;
            }
        }
    }

    private static void ReadFilterFlags()
    {
        while (true)
        {
            Console.WriteLine("| Filter flags: All `a` | By title `t` | By date `d` | By category `c` | By room `r` |");
            var input = Console.ReadLine()?.ToLower().Trim(' ');

            List<Shortage> shortages = ShortageManager.GetShortageList();

            switch (input)
            {
                case "a":
                    FilterAll();
                    return;
                case "t":
                    FilterByTitle(shortages);
                    return;
                case "d":
                    FilterByDate(shortages);
                    return;
                case "c":
                    var category = ReadCategory();
                    FilterByCategory(shortages, category);
                    return;
                case "r":
                    var room = ReadRoom();
                    FilterByRoom(shortages, room);
                    return;
                default:
                    Console.WriteLine("Wrong input. Try again.");
                    break;
            }
        }
    }

    private static void FilterAll()
    {
        ShortageManager.PrintShortageList();
    }

    private static void FilterByTitle(List<Shortage> shortages)
    {
        Console.WriteLine("Write a title to filter by: ");
        var input = Console.ReadLine()?.ToLower().Trim(' ');

        foreach (var shortage in shortages)
        {
            var title = shortage.Title;

            if (input is not null && title.ToLower().Trim(' ').Contains(input))
                ShortageManager.PrintShortage(shortage);
        }
    }

    private static void FilterByDate(List<Shortage> shortages)
    {
        Console.WriteLine("Write a starting date in this format: YYYY-MM-DD");
        var startInput = Console.ReadLine()?.ToLower().Trim(' ');
        var startDateTime = DateTime.Parse(startInput);

        Console.WriteLine("Write an ending date in this format: YYYY-MM-DD");
        var endInput = Console.ReadLine()?.ToLower().Trim(' ');
        var endDateTime = DateTime.Parse(endInput);

        Console.WriteLine($"Querying shortage between these dates: {startDateTime} {endDateTime}");

        foreach (var shortage in shortages)
        {
            if (ShortageManager.MatchShortage(shortage, startDateTime, endDateTime))
                ShortageManager.PrintShortage(shortage);
        }
    }

    private static Category ReadCategory()
    {
        while (true)
        {
            Console.WriteLine("Write a number for one of the listed categories: `1` - Electronics | `2` - Food | `3` - Other");
            var input = Console.ReadLine()?.Trim(' ');

            switch (input)
            {
                case "1":
                    return Category.Electronics;
                case "2":
                    return Category.Food;
                case "3":
                    return Category.Other;
                default:
                    Console.WriteLine("Wrong input. Try again.");
                    break;
            }
        }
    }

    private static void FilterByCategory(List<Shortage> shortages, Category category)
    {
        foreach (var shortage in shortages)
        {
            if (shortage.Category == category)
            {
                ShortageManager.PrintShortage(shortage);
            }
        }
    }

    private static Room ReadRoom()
    {
        while (true)
        {
            Console.WriteLine("Write a number for one of the listed categories: `1` - Meeting Room | `2` - Kitchen | `3` - Bathroom");
            var input = Console.ReadLine()?.Trim(' ');

            switch (input)
            {
                case "1":
                    return Room.MeetingRoom;
                case "2":
                    return Room.Kitchen;
                case "3":
                    return Room.Bathroom;
                default:
                    Console.WriteLine("Wrong input. Try again.");
                    break;
            }
        }
    }

    private static void FilterByRoom(List<Shortage> shortages, Room room)
    {
        foreach (var shortage in shortages)
        {
            if (shortage.Room == room && ShortageManager.MatchShortage(shortage, room))
            {
                ShortageManager.PrintShortage(shortage);
            }
        }
    }

    private static void ReadAndValidateDelete()
    {
        ShortageManager.PrintShortageList();
        var shortages = ShortageManager.GetShortageList();

        while (true)
        {
            Console.WriteLine("\nWrite a matching title within the shortage list:");
            var inputTitle = Console.ReadLine();

            Console.WriteLine("Write a matching name within the shortage list:");
            var inputName = Console.ReadLine();

            foreach (var shortage in shortages)
            {
                if (ShortageManager.MatchShortage(shortage, inputTitle, inputName));
                {
                    Console.WriteLine("Found a match to delete: ");
                    ShortageManager.PrintShortage(shortage);

                    while (true)
                    {
                        Console.WriteLine("Confirm delete? | Yes - `y` | No - `n` |");
                        var input = Console.ReadLine()?.ToLower().Trim(' ');

                        switch (input)
                        {
                            case "y":
                                ShortageManager.DeleteShortage(shortage);
                                Console.WriteLine("Match deleted.");
                                return;
                            case "n":
                                Console.WriteLine("Match not deleted.");
                                return;
                            default:
                                Console.WriteLine("Wrong input. Try again.");
                                return;
                        }
                    }
                }
            }
        }
    }

    private static void ReadAddParameters()
    {
        var shortage = new Shortage()
        {
            Title = ReadTitle(),
            Name = ReadName(),
            Room = ReadRoom(),
            Category = ReadCategory(),
            Priority = ReadPriority(),
            CreatedOn = DateTime.Now,
            CreatedBy = UserValidation.GetUser(),
        };

        ValidateAdd(shortage);
    }

    private static string ReadTitle()
    {
        Console.WriteLine($"Input a title:");
        return Console.ReadLine();
    }

    private static string ReadName()
    {
        while (true)
        {
            Console.WriteLine($"Input a name:");
            return Console.ReadLine();
        }
    }

    private static int ReadPriority()
    {
        int minPriority = 1;
        int maxPriority = 10;

        while (true)
        {
            Console.WriteLine($"Write a priority between {minPriority}-{maxPriority}");
            var input = Console.ReadLine();

            if (Int32.TryParse(input, out int inputNumber))
            {
                if (inputNumber >= minPriority && inputNumber <= maxPriority)
                {
                    return inputNumber;
                }
            }

            Console.WriteLine("Wrong input. Try again.");
        }
    }

    private static void ValidateAdd(Shortage shortage)
    {
        var shortages = ShortageManager.GetShortageList();

        foreach (var currShortage in shortages)
        {
            if (ShortageManager.MatchShortage(shortage, currShortage.Title, currShortage.Name))
            {
                if (shortage.Priority > currShortage.Priority)
                {
                    ShortageManager.DeleteShortage(currShortage);
                    ShortageManager.AddShortage(shortage);

                    Console.WriteLine("Shortage priority updated");
                    return;
                }

                Console.WriteLine("Shortage already listed.");
                return;
            }
        }

        ShortageManager.AddShortage(shortage);

        Console.WriteLine("Shortage added:");
        ShortageManager.PrintShortageList();
    }
}