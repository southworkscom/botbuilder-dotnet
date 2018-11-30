using System;

namespace Microsoft.Bot.PublishValidation
{
    class Program
    {
        static int Main(string[] args)
        {
            var task = new BotConfigCheckerTask();
            task.ProjectPath = args[0];

            try
            {
                if (!task.Execute())
                {
                    return 1;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
                return 2;
            }

            return 0;
        }
    }
}
