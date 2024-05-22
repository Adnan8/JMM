using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Common.ViewModel
{
    public class DDLDtlsVM
    {
        public string? Id { get; set; }=null;
        public string? DdlhdrId { get; set; }
        public int Compid { get; set; }
        public string Ddltxt { get; set; }
        public int Ddlorder { get; set; }
        public bool Active { get; set; }
        public Guid? DdlGuid { get; set; }
        public int? Data1 { get; set; }
        public int? Data2 { get; set; }

    }
}
