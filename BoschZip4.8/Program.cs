using System;

namespace BoschZip
{
    public class Program
    {
        static int Main(string[] args)
        {
            int retVal = 0;
            ZipSettings sets = ZipSettings.FromArgs(args);

            if (sets.ShowHelp || !sets.ValidSettings())
            {
                DisplayText(sets.GetHelpString());
                retVal = -1;
            }
            else
            {
                ZipWorker zipper = new ZipWorker(sets);
                try
                {
                    var result = zipper.CheckResources();
                    if (result.IsSuccess)
                    {
                        result = zipper.DoZip();
                    }

                    if (result.IsSuccess)
                    {
                        // Try to send the file to the correct path
                        result = zipper.DoSend();
                    }
                    if (result.IsSuccess)
                    {
                        DisplayText($"Success.");
                        retVal = 0;
                    }
                    else if (result.IsFailure)
                    {
                        DisplayText($"Failure:\n{result.Message.ToString()}");
                        retVal = -1;
                    }
                    else if (result.IsWarning)
                    {
                        DisplayText($"Warning:\n{result.Message.ToString()}");
                        retVal = -1;
                    }
                }
                catch (Exception ex)
                {
                    DisplayText($"Error raised:\n{ex.Message}");
                    retVal = -1;
                }
            }
#if DEBUG
            Console.ReadKey();
#endif

            // Exist code
            return retVal;
        }

        public static void DisplayText(string text)
        {
            Console.WriteLine(text);
        }

    }

}
