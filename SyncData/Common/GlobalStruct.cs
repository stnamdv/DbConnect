namespace SyncData.Common
{
    public class GlobalStruct
    {
        public struct ActionType
        {
            public static string ExecuteReader = "ExecuteReader";
            public static string ExecuteDataTable = "ExecuteDataTable";
            public static string ExecuteScalar = "ExecuteScalar";
            public static string ExecuteNonQuery = "ExecuteNonQuery";
        }

        public struct Message
        {
            public static string Successfully = "Successfully";
            public static string Failure = "Failure";
            public static string Exception = "Threw an exception";
        }
    }
}