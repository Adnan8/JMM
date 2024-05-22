using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solution.DAL.Models;

public partial class FrontendPermission : BaseModel
{
    [Key]
    public int Id { get; set; }

    public string? URL { get; set; }

    public string? OperationType { get; set; }

    public string? PermissionName { get; set; }
    public string ScreenCode { get; set; }

    public string? PageName { get; set; }

    public int? ParentId { get; set; }
    public bool? HasChild { get; set; }

    public ICollection<RoleFrontEndPermission> RoleFrontEndPermission { get; set; }

}
