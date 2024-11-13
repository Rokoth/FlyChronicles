using DB.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyChronicles.DB.Models
{
    [TableName("settings")]
    public class Settings
    {
        [ColumnName("id")]
        public int Id { get; set; }

        [ColumnName("param_name")]
        public string ParamName { get; set; }

        [ColumnName("param_value")]
        public string ParamValue { get; set; }
    }
}
