using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.DAL.Data;
using Solution.DAL.Models;
using Solution.Repository.Repo;
using Solution.Repository.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services
{
    public class DdlhdrService : IDdlhdrService
    {
        private readonly ICommonService _common;
        private readonly IUnitofWork _unitofWork;
        HashIdToIntConverter obj = new HashIdToIntConverter();

        public DdlhdrService(ICommonService common, IUnitofWork unitofWork)
        {
            _common = common;
            _unitofWork = unitofWork;
        }

        public async Task<bool> CreateAsync(DdlhdrVM dDlhdrDto)
        {
            try
            {
                var ddlhdr = _common.Map<Ddlhdr>(dDlhdrDto);
                await _unitofWork.DdlhdrRepository.Insert(ddlhdr);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var Id = obj.Convert(id, null);
                await _unitofWork.DdlhdrRepository.Delete(Id);
                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<DdlhdrVM> EditAsync(string id)
        {
            try
            {
                var Id = obj.Convert(id, null);
                var existingDlhr = await _unitofWork.DdlhdrRepository.GetById(Id);
                if (existingDlhr == null)
                    throw new Exception("Ddlhdr not found");
                var dDlhdr = _common.Map<DdlhdrVM>(existingDlhr);
                return dDlhdr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DdlhdrVM>> GetAll()
        {
            try
            {
                var allDdlhdrs = await _unitofWork.DdlhdrRepository.All.ToListAsync();

                if (allDdlhdrs == null || !allDdlhdrs.Any())
                    throw new Exception("No Ddlhdrs found");

                var dDlhdrs = allDdlhdrs.Select(d => _common.Map<DdlhdrVM>(d)).ToList();
                return dDlhdrs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DDLDtlsVM>> GetDdldtlsByDdlhdrIdAsync(string ddlhdrId)
        {
            var Id = obj.Convert(ddlhdrId, null);
            var ddldtls = await _unitofWork.DDLDtlsRepository.All
                                .Where(dd => dd.DdlhdrId == Id && (dd.IsDeleted
                                == false || dd.IsDeleted == null))
                                .ToListAsync();
            var ddldtl = ddldtls.Select(dd => _common.Map<DDLDtlsVM>(dd)).ToList();
            return ddldtl;
        }



        public async Task<bool> UpdateAsync(DdlhdrVM dDlhdrDto)
        {
            try
            {

                var Id = obj.Convert(dDlhdrDto.Id, null);
                var model = await _unitofWork.DdlhdrRepository.GetById(Id);
                if (model == null)
                {
                    return false;
                }

                var updatedDtls = _common.Map<Ddlhdr>(model);
                updatedDtls.Ddlname = dDlhdrDto.Ddlname;
                updatedDtls.Ddldesciption = dDlhdrDto.Ddldesciption;

                await _unitofWork.DdlhdrRepository.Update(updatedDtls);
                return true;


            }
            catch (Exception)
            {

                return false;
            }

        }



    }
}

