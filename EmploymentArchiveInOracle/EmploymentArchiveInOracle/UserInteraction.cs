using System;

namespace EmploymentArchiveInOracle
{
    class UserInteraction
    {
        internal static void GetJobDetails()
        {                        
            Console.WriteLine("\nEnter the Job Details: ");

            Console.Write("Job Title: ");
            JobModel.JobTitle = Console.ReadLine();

            Console.Write("Employer: ");
            JobModel.Employer = Console.ReadLine();

            Console.Write("Description: ");
            JobModel.Description = Console.ReadLine();

            Console.WriteLine("\nWorking Period of Employee");       
            
            while (true)
            {
                Console.Write("From: ");
                if (EmploymentHistoryModel.SetFrom(Console.ReadLine())) break;
                Console.WriteLine("Enter the date in correct format!");
            }

            while (true)
            {
                Console.Write("To: ");
                if (EmploymentHistoryModel.SetTo(Console.ReadLine())) break;
                Console.WriteLine("Enter the Date in Correct format!");
            }

        }

        internal static int GetCRUDOperation()
        {
            Console.WriteLine("CREATE     : 1\nRETRIEVE   : 2\nUPDATE     : 3\nDELETE     : 4\nTO QUIT    : Enter some other number");
            Console.Write("Pick Your Choice : ");

            if (!Int32.TryParse(Console.ReadLine(), out int selection))
            {
                Console.WriteLine("\nEnter a valid Input..!");
                GetCRUDOperation();                
            }
            Console.WriteLine();
            return selection;
        }

    }

}


