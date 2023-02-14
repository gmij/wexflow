using AutoMapper;

namespace Wexflow.BlazorServer.Data
{
    public class MapProfile: Profile
    {

        public MapProfile()
        {
            this.CreateMap<Core.Db.User, User>().ReverseMap();
            this.CreateMap<Core.Db.Entry, Entry>().ForMember(entry => entry.Id, opt => opt.MapFrom(o => o.JobId)).ReverseMap();
        }


    }
}
