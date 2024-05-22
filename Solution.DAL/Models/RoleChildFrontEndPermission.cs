using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solution.DAL.Models;

public partial class RoleChildFrontEndPermission : BaseModel
{
    [Key]
    public int Id { get; set; }

    public string RoleId { get; set; } = null!;
    public int? FrontendPermissionsId { get; set; }

    public int? ParentId { get; set; }

    public bool? Status { get; set; }


}
