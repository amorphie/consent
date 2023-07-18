using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.template.core.Model;
using AutoMapper;

namespace amorphie.template.Mapper
{
    public sealed class ResourceMapper : Profile
    {
        public ResourceMapper()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
        }
    }
}
