		using Solution.DAL.Models.Base;
		using System;
		using System.Collections.Generic;
		using System.ComponentModel.DataAnnotations;

		namespace Solution.DAL.Models;

		public partial class Menu : BaseModel
		{
			[Key]
			public int Id { get; set; }

			public string Title { get; set; } = null!;

			public string? Descr { get; set; }

			public int? ParentId { get; set; }

			public string Icon { get; set; } = null!;

			public string? Url { get; set; }

			public int? MenuOrder { get; set; }

			public string? Controller { get; set; }

			public string? Page { get; set; }
			public string? MenuLevel { get; set; }
			public bool? IsDefault { get; set; }
			public virtual ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();

			//public virtual ICollection<Menu> InverseParent { get; set; } = new List<Menu>();

			public virtual Menu? Parent { get; set; }
		}