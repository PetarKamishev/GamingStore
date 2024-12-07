using Dapper;
using GamingStore.GamingStore.Models;
using GamingStore.GamingStore.Models.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Data.SqlClient;

namespace GamingStore.GamingStore.DL.Repositories
{
    public class IdentityUserStore : IUserPasswordStore<Models.Models.Users.IdentityUser>, IUserRoleStore<Models.Models.Users.IdentityUser>
    {

        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<Models.Models.Users.IdentityUser> _passwordHasher;
        private SQLConfiguration _sqlConfiguration = new SQLConfiguration();
        public IdentityUserStore(IConfiguration configuration,
            IPasswordHasher<Models.Models.Users.IdentityUser> passwordHasher)
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public Task AddToRoleAsync(Models.Models.Users.IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CreateAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();
                if (user != null)
                {
                    string query = @"
                    INSERT INTO Users
                        ([UserName],
                        [Password],
                        [Email],
                        [CreatedDate])
                        VALUES(
                        @UserName, @Password, @Email, @CreatedDate)";

                    user.Password = _passwordHasher.HashPassword(user, user.Password);

                    var result = await connect.ExecuteAsync(query, user);
                    connect.Close();
                    return IdentityResult.Success;
                }
                else
                {
                    connect.Close();
                    return IdentityResult.Failed();
                }
            }
        }

        public async Task<IdentityResult> DeleteAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString )))
            {
                await connect.OpenAsync();

                var result = await connect.QueryAsync<Models.Models.Users.IdentityUser>("DELETE FROM Users WHERE UserId = @userId", new { UserId = user.UserId });
                connect.Close();
                return IdentityResult.Success;
            }
        }

        public void Dispose()
        {
        }

        public async Task<Models.Models.Users.IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                var result = await connect.QueryAsync<Models.Models.Users.IdentityUser>("SELECT * FROM Users WHERE UserId = @userId", new { UserId = userId });
                connect.Close();
                return result.FirstOrDefault();
            }
        }

        public async Task<Models.Models.Users.IdentityUser?> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();
                var result = await connect.QueryAsync<Models.Models.Users.IdentityUser>("SELECT * FROM Users WHERE UserName = @userName", new { UserName = userName });
                connect.Close();
                return result.FirstOrDefault();
            }
        }

        public Task<string?> GetNormalizedUserNameAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> GetPasswordHashAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                var passwordHash = await connect.QueryFirstOrDefaultAsync<string>("SELECT Password FROM Users WHERE UserId = @UserId", new { UserId = user.UserId });
                connect.Close();
                return passwordHash;              
            }
        }

        public async Task<IList<string>> GetRolesAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();
                var result = await connect.QueryAsync<string>("SELECT RoleName FROM UserRoles WHERE UserId = @userId", new { UserId = user.UserId });
                connect.Close();
                return result.ToList();
            }
        }

        public async Task<string?> GetUserIdAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();
                var result = await connect.QuerySingleOrDefaultAsync<Models.Models.Users.IdentityUser>("SELECT * FROM Users WHERE UserId = @UserId", new { UserId = user.UserId });
                connect.Close();
                return result?.UserId.ToString();
            }
        }

        public Task<string?> GetUserNameAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<IList<Models.Models.Users.IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HasPasswordAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            var passwordHash = await GetPasswordHashAsync(user, cancellationToken);
            if (String.IsNullOrEmpty(passwordHash))
                return false;
            else
                return true;
        }

        public Task<bool> IsInRoleAsync(Models.Models.Users.IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(Models.Models.Users.IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(Models.Models.Users.IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task SetPasswordHashAsync(Models.Models.Users.IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                await connect.ExecuteAsync("UPDATE Users SET Password = @passwordHash WHERE UserId = @UserId", new { UserId = user.UserId, PasswordHash = passwordHash });
                connect.Close();
            }
        }

        public async Task SetUserNameAsync(Models.Models.Users.IdentityUser user, string? userName, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();

                await connect.ExecuteAsync("UPDATE Users SET UserName = @userName WHERE UserId = @UserId", new { UserId = user.UserId, UserName = userName });
                connect.Close();
            }

        }

        public async Task<IdentityResult> UpdateAsync(Models.Models.Users.IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connect = new SqlConnection(_configuration.GetConnectionString(_sqlConfiguration.ConnectionString)))
            {
                await connect.OpenAsync();
                if (user != null)
                {
                    var query = @"
                                UPDATE Users
                                SET  
                                     UserName = @UserName, Password = @Password, Email = @Email
                                Where UserId = @userId
                            ";

                    await connect.ExecuteAsync(query, new { UserId = user.UserId, UserName = user.UserName, Password = user.Password, Email = user.Email });
                    connect.Close();
                    return IdentityResult.Success;
                }
                else
                {
                    connect.Close();
                    return IdentityResult.Failed();
                }
            }
        }
    }
}
