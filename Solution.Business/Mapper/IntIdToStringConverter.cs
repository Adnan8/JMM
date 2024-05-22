using AutoMapper;
using HashidsNet;
using Microsoft.Extensions.Configuration;
using Solution.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Mapper
{
    public class IntIdToStringConverter : IValueConverter<int, string>
    {
        private readonly Hashids hashids;

        public IntIdToStringConverter()
        {
            hashids = new Hashids(ConstantUnique.HashidsName, ConstantUnique.HashidsLength);
        }

        public string Convert(int sourceMember, ResolutionContext context)
        {
            return hashids.Encode(sourceMember);
        }
    }
    public class HashIdToIntConverter : IValueConverter<string, int>
    {
        private readonly Hashids hashids;

        public HashIdToIntConverter()
        {
            hashids = new Hashids(ConstantUnique.HashidsName, ConstantUnique.HashidsLength);
        }

        public int Convert(string sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(sourceMember))
            {
                return 0;
            }

            var numbers = hashids.Decode(sourceMember);
            return numbers.Length > 0 ? numbers[0] : 0;
            //var numbers = hashids.Decode(sourceMember);
            //return numbers.Length > 0 ? numbers[0] : 0; 
        }
    }

    //var configuration = new MapperConfiguration(cfg =>
    //{
    //    cfg.CreateMap<SourceModel, DestinationModel>()
    //       .ForMember(dest => dest.StringId, opt => opt.ConvertUsing(new IntToHashIdConverter("your_salt"), src => src.IntId));
    //    cfg.CreateMap<DestinationModel, SourceModel>()
    //       .ForMember(dest => dest.IntId, opt => opt.ConvertUsing(new HashIdToIntConverter("your_salt"), src => src.StringId));
    //});

    //var mapper = configuration.CreateMapper();


}
