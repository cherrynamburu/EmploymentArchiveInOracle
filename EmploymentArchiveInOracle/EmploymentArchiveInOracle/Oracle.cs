using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;


namespace EmploymentArchiveInOracle
{
    class Oracle
    {
        private static OracleConnection _oracleConnection = new OracleConnection(Config.ConnectionString);

        static Oracle()
        {    
            try
            {
                _oracleConnection.Open();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }
        }

        internal static void RetrieveJob()
        {
            OracleCommand command = _oracleConnection.CreateCommand();     
            OracleParameter[] prm = new OracleParameter[1];
            prm[0] = command.Parameters.Add("paramJobno", OracleDbType.Decimal, JobModel.JobId , ParameterDirection.Input);
         
            command.CommandText = "SELECT * FROM EmployeeJob JOIN EmploymentHistory ON EmployeeJob.EmploymentHistoryId = EmploymentHistory.EmploymentHistoryId WHERE EmployeeJob.Jobid=:1";
            try
            {
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["JobId"] + " - " + reader["JobTitle"] + " - " + reader["Employer"] + " - " + reader["Description"] + " - " + Convert.ToDateTime(reader["EmploymentFrom"]).ToShortDateString() + " - " + Convert.ToDateTime(reader["EmploymentTo"]).ToShortDateString());
                    }        
                }
                else
                {
                    Console.WriteLine("No record with this JobId..!");
                }
                reader.Close();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }
        }

        internal static void RetrieveJobs()
        {
            OracleCommand command = _oracleConnection.CreateCommand();
            command.CommandText = "SELECT * FROM EmployeeJob JOIN EmploymentHistory ON EmployeeJob.EmploymentHistoryId = EmploymentHistory.EmploymentHistoryId ORDER BY EmployeeJob.JobId";

            try
            {
                OracleDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["JobId"] + " - " + reader["JobTitle"] + " - " + reader["Employer"] + " - " + reader["Description"] + " - " + Convert.ToDateTime(reader["EmploymentFrom"]).ToShortDateString() + " - " + Convert.ToDateTime(reader["EmploymentTo"]).ToShortDateString()); ;
                    }               
                }
                else
                {
                    Console.WriteLine("No records in this entity...!");
                }
                reader.Close();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }
        }

        internal static void InsertToEmployementHistory()
        {
            OracleCommand command = _oracleConnection.CreateCommand();
            OracleParameter[] prm = new OracleParameter[2];
            prm[0] = command.Parameters.Add("EmploymentFrom", OracleDbType.Date, EmploymentHistoryModel.From, ParameterDirection.Input);
            prm[1] = command.Parameters.Add("EmploymentTo", OracleDbType.Date, EmploymentHistoryModel.To, ParameterDirection.Input);
            command.CommandText = "INSERT INTO EmploymentHistory(EmploymentFrom, EmploymentTo) VALUES(:1, :2)"; 
            

            try
            {
                command.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }
            Console.WriteLine("inserting into Employement history is completed..!");
        }

        internal static void InsertJob()
        {

            InsertToEmployementHistory();
            OracleCommand command = _oracleConnection.CreateCommand();

            command.CommandText = "SELECT MAX(EmploymentHistoryId) from EmploymentHistory";

            using (OracleDataReader dr = command.ExecuteReader())

                while (dr.Read())
                {
                    JobModel.EmploymentHistoryId = dr.GetInt32(0);

                }

            
            command.CommandText = "INSERT INTO  EmployeeJob(JobTitle, Employer, Description, EmploymentHistoryId) values(:1, :2, :3, :4)";
            OracleParameter[] prm = new OracleParameter[4];
            prm[0] = command.Parameters.Add("JobTitle", OracleDbType.Varchar2, JobModel.JobTitle, ParameterDirection.Input);
            prm[1] = command.Parameters.Add("Employer", OracleDbType.Varchar2, JobModel.Employer, ParameterDirection.Input);
            prm[2] = command.Parameters.Add("Description", OracleDbType.Varchar2, JobModel.Description, ParameterDirection.Input);
            prm[3] = command.Parameters.Add("EmploymentHistoryId", OracleDbType.Decimal, JobModel.EmploymentHistoryId, ParameterDirection.Input);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }
            Console.WriteLine("Inserting into Job is Completed");
        }

        internal static bool CheckJobExists()
        {
            object employmentHistoryId = null;
            OracleCommand command = _oracleConnection.CreateCommand();
            command.CommandText = "select EmploymentHistoryId from EmployeeJob where JobId = :1 ";
            OracleParameter[] prm = new OracleParameter[1];
            prm[0] = command.Parameters.Add("JobId", OracleDbType.Decimal, JobModel.JobId, ParameterDirection.Input);

            try
            {
                employmentHistoryId = command.ExecuteScalar();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }

            if (employmentHistoryId != null)
            {
                Console.WriteLine("JobId exists..!");
                JobModel.EmploymentHistoryId = Convert.ToInt32(employmentHistoryId);
                return true;
            }

            Console.WriteLine("JobId Doesnt Exist..!");
            return false;
        }


        internal static void UpdateJob()
        {
            OracleCommand command = _oracleConnection.CreateCommand();
            command.CommandText = "UPDATE EmployeeJob SET JobTitle = '" + JobModel.JobTitle + "',Employer = '" + JobModel.Employer + "',Description = '" + JobModel.Description + "' WHERE JobId = '" + JobModel.JobId + "'";
            try
            {
                command.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }
            Console.WriteLine("Updating job completed..!");
            UpadteEmploymentHistory();

        }

        internal static void UpadteEmploymentHistory()
        {
            OracleCommand command = _oracleConnection.CreateCommand();
            command.CommandText = "UPDATE EmploymentHistory SET EmploymentFrom = :1, EmploymentTo = :2 WHERE EmploymentHistoryId = :3";

            OracleParameter[] prm = new OracleParameter[3];
            prm[0] = command.Parameters.Add("EmploymentFrom", OracleDbType.Date, EmploymentHistoryModel.From, ParameterDirection.Input);
            prm[1] = command.Parameters.Add("EmploymentTo", OracleDbType.Date, EmploymentHistoryModel.To, ParameterDirection.Input);
            prm[2] = command.Parameters.Add("EmploymentHistoryId", OracleDbType.Decimal, JobModel.EmploymentHistoryId, ParameterDirection.Input);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }

            Console.WriteLine("Updating Employment history is completed..!");
        }


        internal static void DeleteJob()
        {
            OracleCommand command = _oracleConnection.CreateCommand();
            command.CommandText = "DELETE FROM EmploymentHistory WHERE EmploymentHistory.EmploymentHistoryId = :1";
            OracleParameter[] prm = new OracleParameter[1];
            prm[0] = command.Parameters.Add("EmploymentFrom", OracleDbType.Decimal, JobModel.EmploymentHistoryId, ParameterDirection.Input);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Console.WriteLine("OracleException {0} occured", e.Message);
            }
            Console.WriteLine("All the records related to entered JobId are deleted..!");
        }


    }
}


