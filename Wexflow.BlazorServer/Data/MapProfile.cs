using AutoMapper;

namespace Wexflow.BlazorServer.Data
{
    public class MapProfile: Profile
    {

        public MapProfile()
        {
            this.CreateMap<Core.Db.User, User>().ReverseMap();
        }


    }
}
