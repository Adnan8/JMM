using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.DAL.Models;
using Solution.Repository.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services
{
    public class DDLDtlsService : IDDLDtlsService
    {
        private readonly ICommonService _common;
        private readonly IUnitofWork _unitofWork;
        HashIdToIntConverter obj = new HashIdToIntConverter();
        public DDLDtlsService(ICommonService common, IUnitofWork unitofWork)
        {
            _common = common;
            _unitofWork = unitofWork;
        }


        public async Task<bool> CreateAsync(DDLDtlsVM dDLDtlsDto)
        {
            try
            {
                var dDLDtls = _common.Map<DDLDtls>(dDLDtlsDto);
                await _unitofWork.DDLDtlsRepository.Insert(dDLDtls);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _unitofWork.DDLDtlsRepository.GetById(id);
            if (model == null)
            {
                return false;
            }
            model.IsDeleted = true;
            await _unitofWork.DDLDtlsRepository.Update(model);
            return true;

        }

        public async Task<List<DDLDtlsVM>> GetAll()
        {

            var alldDLDtls = await _unitofWork.DDLDtlsRepository.All.ToListAsync();
            var filtereddDLDtls = alldDLDtls.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
            if (filtereddDLDtls == null || !filtereddDLDtls.Any())
                throw new Exception("No Ddlhdrs found");
            var DDLDtls = filtereddDLDtls.Select(d => _common.Map<DDLDtlsVM>(d)).ToList();
            return DDLDtls;

        }
        public async Task<DDLDtlsVM> EditAsync(int id)
        {
            var existingDtls = await _unitofWork.DDLDtlsRepository.GetById(id);
            if (existingDtls == null)
                throw new Exception("DDLDtls not found");

            var DDLDtls = _common.Map<DDLDtlsVM>(existingDtls);

            return DDLDtls;

        }


        public async Task<bool> UpdateAsync(DDLDtlsVM updatedDtlsDto)
        {
            try
            {
                var Id = obj.Convert(updatedDtlsDto.Id, null);
                var model = await _unitofWork.DDLDtlsRepository.GetById(Id);
                if (model == null)
                {
                    return false;
                }

                var updatedDtls = _common.Map<DDLDtls>(model);
                updatedDtls.Ddltxt = updatedDtlsDto.Ddltxt;
                updatedDtls.Ddlorder = updatedDtlsDto.Ddlorder;
                updatedDtls.Data1 = updatedDtlsDto.Data1;
                updatedDtls.Data2 = updatedDtlsDto.Data2;

                await _unitofWork.DDLDtlsRepository.Update(updatedDtls);
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
