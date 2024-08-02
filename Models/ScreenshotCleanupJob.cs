

//using System.Configuration;
//using System.Data.SqlClient;

//namespace EMP.Models
//{
//    public class ScreenshotCleanupJob
//    {
//        public void DeleteOldScreenshots()
//        {
//            var connectionstring = ConfigurationManager.ConnectionStrings["EMBContext"].ConnectionString;
//            using (var connection = new SqlConnection(connectionstring))
//            {
//                connection.Open();
//                string query = @"
//                DELETE FROM UserScreenShots
//                WHERE ScreenShotDate < DATEADD(day, -1, GETDATE())";

//                var command = new SqlCommand(query, connection);
//                var result = command.ExecuteNonQuery();
//            }
//        }
//    }
//}