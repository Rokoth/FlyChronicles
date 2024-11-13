using System;
using System.Collections.Generic;
using System.Text;

namespace FlyCronicles.Common.Options
{
    public class CommonOptions
    {
        public Dictionary<ConnectionName, string> ConnectionStrings { get; set; }
    }

    public enum ConnectionName
    { 
       Main, Admin
    }
}
