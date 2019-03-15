using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI_PE_Submit_Tool.Common
{
    class Constant
    {
        public static string API_URL => ConfigurationManager.AppSettings["server"];
    }
}
