using Project.DAL.Entities.Identity;

namespace Project.BLL.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<UserRefreshToken>
    {
    }
}
