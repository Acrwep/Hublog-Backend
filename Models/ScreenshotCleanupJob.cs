

using System.Data.SqlClient;

namespace EMP.Models
{
    public class ScreenshotCleanupJob
    {
        public void DeleteOldScreenshots()
        {
            using (var connection = new SqlConnection("YourConnectionString"))
            {
                connection.Open();
                string query = @"
                DELETE FROM UserScreenShots
                WHERE ScreenShotDate < DATEADD(day, -1, GETDATE())";

                var command = new SqlCommand(query, connection);
                var result = command.ExecuteNonQuery();
            }
        }
    }
}