using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solution.DAL.Models;

public partial class RolePermission : BaseModel
{
    [Key]
    public int Id { get; set; }

    public string RoleId { get; set; } = null!;

    public int PermissionId { get; set; }

    public virtual Permission Permission { get; set; } = null!;
}
