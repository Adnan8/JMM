using AutoMapper;
using HashidsNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Business.Services;
using Solution.Common;
using Solution.Repository.Repo.IRepo;
using Solution.Repository.Repo;
using Solution.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Microsoft.AspNetCore.Identity;
public class IdentityResultExtend
{
    public string UserId { get; set; }
    public IdentityResult Result { get; set; }
}




