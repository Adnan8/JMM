using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solution.DAL.Models;

public partial class Permission : BaseModel
{
    [Key]
    public int Id { get; set; }

    public string? PermissionName { get; set; }

    public string? Controller { get; set; }

    public string? Action { get; set; }

    public int? OperationType { get; set; }
    public bool? IsDefault { get; set; }
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

