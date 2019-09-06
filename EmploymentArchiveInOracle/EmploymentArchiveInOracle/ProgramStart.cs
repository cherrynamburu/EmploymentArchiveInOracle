using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmploymentArchiveInOracle
{
    class ProgramStart
    {

        static void HandleRetrieveOperation()
        {
            while (true)
            {
                Console.Write("Single Job       : 0 \nMultiple Jobs    : 1 \nPick Your Choice : ");
                if (!int.TryParse(Console.ReadLine(), out var temp))
                {
                    temp = 9;
                }
                if (temp == 0)
                {
                    Console.Write("Enter the JobId to retrieve related data: ");

                    if (int.TryParse(Console.ReadLine(), out temp))
                    {
                        JobModel.JobId = temp;
                    }
                    else
                    {
                        JobModel.JobId = 0;
                        Console.WriteLine("Input is not in numbers..INVALID");
                    }
                    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - -");
                    Oracle.RetrieveJob();
                    break;
                }
                if (temp == 1)
                {
                    Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - -");
                    Oracle.RetrieveJobs();
                    Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - -");
                    break;
                }
                Console.WriteLine("\nChoose from 0 and 1");
            }

        }

        static void HandleUpdateOperation()
        {
            Console.Write("Enter Job Id to Update: ");
            if (int.TryParse(Console.ReadLine(), out var temp))
            {
                JobModel.JobId = temp;
            }
            else
            {
                JobModel.JobId = 0;
                Console.WriteLine("Input is not in numbers..INVALID");
            }
            if (Oracle.CheckJobExists())
            {
                UserInteraction.GetJobDetails();
                Oracle.UpdateJob();
            }
        }

        private static void HandleDeleteOperation()
        {
            Console.Write("Enter the JobId to Delete : ");
            if (int.TryParse(Console.ReadLine(), out var temp))
            {
                JobModel.JobId = temp;
            }
            else
            {
                JobModel.JobId = 0;
                Console.WriteLine("Input is not in numbers..INVALID");
            }
            if (Oracle.CheckJobExists())
            {

                Oracle.DeleteJob();
            }

        }
        static void Main(string[] args)
        {
            while (true)
            {
                switch (UserInteraction.GetCRUDOperation())
                {

                    case 1:
                        UserInteraction.GetJobDetails();
                        Oracle.InsertJob();
                        break;

                    case 2:
                        HandleRetrieveOperation();
                        break;

                    case 3:
                        HandleUpdateOperation();
                        break;

                    case 4:
                        HandleDeleteOperation();
                        break;

                    default:
                        Console.WriteLine("Choise is Not awailable..exiting..!");
                        break;

                }

                Console.Write("\nWant to do another operation ? Y/N  :   ");
                var userInput = Console.ReadLine();
                if (!(userInput == "Y" || userInput == "y"))
                {
                    break;
                }
                Console.WriteLine();
            }
            Console.WriteLine("\nProgram is Terminating...!");
            Thread.Sleep(500);
        }


    }
}
