using Solution.Business.Enums;
using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Solution.DAL.Models
{
    public class Document : BaseModel
    {
        public int Id { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Path { get; set; }
        public string OriginalFileName { get; set; } 
        public long Size { get; set; } 
        public string ContentType { get; set; } 

    }

}
