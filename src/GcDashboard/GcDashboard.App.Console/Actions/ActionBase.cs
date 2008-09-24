using System;

namespace GcDashboard.App.AdminConsole.Actions
{
    public abstract class ActionBase : IActionHandler
    {

        #region [ Methods (1) ]

        public int Run()
        {
            try
            {
                RunHandler();

                Console.WriteLine(".");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Program.LastThrownException = ex;

                Console.WriteLine("Error.  {0}:  {1}", ex.GetType().Name, ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);

                    if (ex.InnerException.InnerException != null)
                        Console.WriteLine(ex.InnerException.InnerException.Message);
                }

                Console.WriteLine();
            }

            return 0;
        }

        #endregion [ Methods ]

        public abstract void RunHandler();
    }
}
