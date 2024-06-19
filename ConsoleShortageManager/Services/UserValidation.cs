using System.Security.Principal;

namespace ConsoleShortageManager;

public static class UserValidation
{
    static UserValidation()
    {
        // Initializes static User first time User methods are being used.
        InitializeUser();
    }

    public static void InitializeUser()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();

        User.Name = identity.Name;
        string userName = WindowsIdentity.GetCurrent().Name;

        WindowsPrincipal principal = new WindowsPrincipal(identity);
        if (principal.IsInRole(WindowsBuiltInRole.Administrator))
        {
            User.IsAdmin = true;
            return;
        }

        User.IsAdmin = false;
    }

    public static bool VerifyUser(Shortage shortage)
    {
        if (User.IsAdmin)
            return true;

        if (User.Name == shortage.CreatedBy)
            return true;

        return false;
    }

    public static string GetUser()
    {
        return User.Name;
    }

}