using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyncData.Models
{
    public class ColumnMapping
    {
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public string DATA_LENGTH { get; set; }
        public string NULLABLE { get; set; }
    }
}