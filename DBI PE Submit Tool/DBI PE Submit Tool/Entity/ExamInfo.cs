using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI_PE_Submit_Tool.Entity
{
    class ExamInfo
    {
        public string ExamCode { get; set; }
        public string PaperNo { get; set; }

        public ExamInfo(string examCode, string paperNo)
        {
            ExamCode = examCode;
            PaperNo = paperNo;
        }
    }
}
