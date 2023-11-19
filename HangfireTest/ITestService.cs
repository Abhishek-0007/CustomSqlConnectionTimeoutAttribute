using Microsoft.Data.SqlClient;

namespace HangfireTest
{
    [SqlConnectionExceptionRetry(Attempts = 10)]
    public interface ITestService
    {
        void Check();
    }

    public class TestService : ITestService
    {
        public void Check()
        {
            try
            {
                SqlConnection sql = new SqlConnection("Server=MSI;Database=TodoDB;Trusted_Connection=true;encrypt=false;");
                sql.Open();

                SqlCommand cmd = sql.CreateCommand();
                cmd.CommandText = "DECLARE @i int WHILE EXISTS (SELECT 1 from sysobjects) BEGIN SELECT @i = 1 END";
                cmd.ExecuteNonQuery(); // This line will timeout.

                cmd.Dispose();
                sql.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    Console.WriteLine("Timeout occurred");
                }
                throw;
            }
        }
    }
}
