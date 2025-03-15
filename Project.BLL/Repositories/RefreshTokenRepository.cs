using Microsoft.EntityFrameworkCore;
using Project.BLL.Interfaces;
using Project.DAL.Data;
using Project.DAL.Entities.Identity;

namespace Project.BLL.Repositories
{
    public class RefreshTokenRepository : GenericRepository<UserRefreshToken>, IRefreshTokenRepository
    {
        private DbSet<UserRefreshToken> _userRefreshTokens;
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _userRefreshTokens = context.Set<UserRefreshToken>();
        }


    }

}
