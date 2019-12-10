using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyncData.Models
{
    public class DbSync
    {
        public string DB_SOURCE { get; set; }
        public string DB_DESTINATION { get; set; }
        public string SOURCE_COLUMN { get; set; }
        public string WHERE { get; set; }
        public string KEY { get; set; }
    }
}