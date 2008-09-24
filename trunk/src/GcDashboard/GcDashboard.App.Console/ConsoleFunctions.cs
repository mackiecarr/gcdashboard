using System;

namespace GcDashboard.App.AdminConsole
{
    internal static class ConsoleFunctions
    {

		#region [ Private Methods (6) ]

        internal static string Prompt(string prompt)
        {
            Console.Write(string.Format("{0}:  ", prompt));
            return Console.ReadLine();
        }

        internal static string Prompt(string prompt, string defaultValue)
        {
            Console.Write(string.Format("{0} [{1}]:  ", prompt, defaultValue));
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                return defaultValue;
            else
                return input;
        }

        internal static void ShowError(string message)
        {
            Console.WriteLine();
            Console.WriteLine("*** Error! ***");
            Console.WriteLine(message);
            Console.WriteLine();
        }

        internal static bool Confirm()
        {
            return Confirm("Are you sure?");
        }

        internal static bool Confirm(string prompt)
        {
            Console.Write("{0}  ", prompt);
            string input = Console.ReadLine();

            return input.Equals("yes", StringComparison.CurrentCultureIgnoreCase);
        }

        internal static string Prompt(string prompt, bool hideInput)
        {
            if (!hideInput)
                return Prompt(prompt);

            Console.Write(string.Format("{0}:  ", prompt));

            //Credit:  http://blogs.msdn.com/shawnfa/archive/2004/05/27/143254.aspx

            string password = string.Empty;

            ConsoleKeyInfo nextKey = Console.ReadKey(true);

            while (nextKey.Key != ConsoleKey.Enter)
            {
                if (nextKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Remove(password.Length - 1);

                        Console.Write(nextKey.KeyChar);
                        Console.Write(' ');
                        Console.Write(nextKey.KeyChar);
                    }
                }
                else
                {
                    password += (nextKey.KeyChar);
                    Console.Write('*');
                }

                nextKey = Console.ReadKey(true);
            }

            Console.WriteLine();

            return password;

        }

		#endregion [ Private Methods ]

    }
}
