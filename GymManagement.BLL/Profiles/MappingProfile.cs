using AutoMapper;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Member, MemberViewModel>().
                ForMember(d => d.Address, p => p.MapFrom(src => $"{ src.Address.Street} - {src.Address.City} - {src.Address.Country}"));
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, SessionViewModel>().
                ForMember(d => d.TrainerName, p => p.MapFrom(src => src.Trainer.Name)).
                ForMember(d => d.CategoryName, p => p.MapFrom(src => src.Category.CategoryName));
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            CreateMap<Trainer, TrainerViewModel>().
                ForMember(d => d.Specialties, p => p.MapFrom(src => src.Specialty));

            CreateMap<CreateTrainerViewModel, Trainer>().
                ForPath(d => d.Address.BuildingNumber, p => p.MapFrom(src => src.BuildingNumber)).
                ForPath(d => d.Address.City, p => p.MapFrom(src => src.City)).
                ForPath(d => d.Address.Street, p => p.MapFrom(src => src.Street)).
                ForMember(d => d.Specialty, p => p.MapFrom(src => src.Specialties)).
                ReverseMap();
            
            CreateMap<Trainer, TrainerDetailsViewModel>().
                ForMember(d => d.Address, p => p.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}")).
                ForMember(d => d.Specialties, p => p.MapFrom(src => src.Specialty));
        }
    }
}
