using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyncData.Models
{
    public class ObjectMapping
    {
        public string OBJECT_ID { get; set; }
        public string OBJECT_NAME { get; set; }
        public string OBJECT_TYPE { get; set; }
        public string CREATED_DATE { get; set; }
        public string TEMPORARY { get; set; }
    }
}