using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyncData.Models
{
    public class DatabaseInfo
    {
        public DatabaseInfo()
        {

        }

        public string SERVER { get; set; }
        public string DATABASE { get; set; }
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
        public string TYPE { get; set; }
        public string CONNECTIONSTRING
        {
            get
            {
                return string.Format("data source={0};user id={1};password={2}", SERVER, USERID, PASSWORD);
            }
        }
    }
}