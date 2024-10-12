using Dapper;
using GamingStore.GamingStore.Models.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace GamingStore.GamingStore.DL.Repositories
{
    public class IdentityUserRolesStore : IRoleStore<IdentityUserRoles>
    {
        private readonly IConfiguration _configuration;

        public IdentityUserRolesStore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUserRoles role, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync(cancellationToken);
                var query = @"INSERT INTO UserRoles([Id], [RoleName], [UserId]) VALUES(@Id, @RoleName, @UserId)";

                var result = await connect.ExecuteAsync(query, role);
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUserRoles role, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();

                var query = "DELETE FROM UserRoles WHERE Id = roleId";
                var result = await connect.ExecuteAsync(query);
                return IdentityResult.Success;
            }
        }

        public void Dispose()
        {
        }

        public async Task<IdentityUserRoles?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                await connect.OpenAsync();

                var result = await connect.QueryAsync<Models.Models.Users.IdentityUser>("SELECT * FROM UserRoles WHERE Id = @roleId", new { Id = roleId });
                return result as IdentityUserRoles;
            }
        }

        public async Task<IdentityUserRoles?> FindByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                 await connect.OpenAsync();

                var result = await connect.QueryAsync<Models.Models.Users.IdentityUser>("SELECT * FROM UserRoles WHERE RoleName = @roleName", new { RoleName = roleName });

                return result as IdentityUserRoles;
            }
        }

        public Task<string?> GetNormalizedRoleNameAsync(IdentityUserRoles role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IdentityUserRoles role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetRoleNameAsync(IdentityUserRoles role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(IdentityUserRoles role, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(IdentityUserRoles role, string? roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityUserRoles role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
