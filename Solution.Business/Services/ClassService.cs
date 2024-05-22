using Microsoft.EntityFrameworkCore;
using Solution.Business.Enums;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.DAL.Models;
using Solution.Repository.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Solution.Business.Services
{
    public class ClassService : IClassService
    {
        private readonly ICommonService _common;
        private readonly IUnitofWork _unitofWork;
        HashIdToIntConverter obj = new HashIdToIntConverter();

        public ClassService(ICommonService common, IUnitofWork unitofWork)
        {
            _common = common;
            _unitofWork = unitofWork;
        }

        public async Task<bool> CreateAsync(ClassVM Dto)
        {
            try
            {
                var result = _common.Map<Class>(Dto);
                await _unitofWork.ClassRepository.Insert(result);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var Id = obj.Convert(id, null);
            var model = await _unitofWork.ClassRepository.GetById(Id);
            if (model == null)
            {
                return false;
            }
            model.IsDeleted = true;
            await _unitofWork.ClassRepository.Update(model);
            return true;
        }

        public async Task<bool> UpdateAsync(ClassVM Dto)
        {
            var Id = obj.Convert(Dto.ClassId, null);
            var model = await _unitofWork.ClassRepository.GetById(Id);
            if (obj == null)
            {
                return false;
            }
            var result = _common.Map<Class>(model);
            result.ClassTime=Dto.ClassTime;
            result.GradeLevel = Dto.GradeLevel;
            result.ProgramDetails = Dto.ProgramDetails;
            result.MaxSize = Dto.MaxSize.Value;
            await _unitofWork.ClassRepository.Update(result);

            return true;
        }
        public async Task<List<ClassVM>> GetAll()
        {
            var all = await _unitofWork.ClassRepository.All.ToListAsync();
            var filtered = all.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
            if (filtered == null || !filtered.Any())
                return new List<ClassVM>();
            var permission = filtered.Select(d => _common.Map<ClassVM>(d)).ToList();
            return permission;
        }
        public async Task<ClassVM> EditAsync(string id)
        {
            try
            {
                var Id = obj.Convert(id, null);
                var existing = await _unitofWork.ClassRepository.GetById(Id);
                if (existing == null)
                    return new ClassVM();

                var result = _common.Map<ClassVM>(existing);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
