using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBI_PE_Submit_Tool.Entity
{
    [Serializable]
    public class ResponseData
    {
        public Int32 Exp { get; set; }
        public int QuestionNumber { get; set; }
        public string Token { get; set; }
    }
}
