using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solution.DAL.Models;

public partial class RoleMenu : BaseModel
{
	[Key]
	public int Id { get; set; }

	public string? RoleId { get; set; } 

	public int MenuId { get; set; }

	public virtual Menu Menu { get; set; } = null!;

}