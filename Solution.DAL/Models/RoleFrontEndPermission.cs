using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solution.DAL.Models;

public partial class RoleFrontEndPermission : BaseModel
{
    [Key]
    public int Id { get; set; }

    public string RoleId { get; set; } = null!;

    public int FrontendPermissionsId { get; set; }

    public bool? IsActive { get; set; }

    public bool? Index { get; set; }

    public bool? Add { get; set; }

    public bool? Edit { get; set; }

    public bool? Delete { get; set; }

    public bool Detail { get; set; }

    public bool? Import { get; set; }

    public bool? Export { get; set; }
    public bool? ChildPermission { get; set; }

    public virtual FrontendPermission FrontendPermission { get; set; } = null!;
}

