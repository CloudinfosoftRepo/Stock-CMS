﻿using AutoMapper;
using System.Collections.Generic;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.RepositoryInterface;
using System.Linq.Expressions;
using Stock_CMS.Models;
using Stock_CMS.Data;

namespace Stock_CMS.Repository
{
    public class UserRepository : EfRepository<TblUser, UserDto>, IUserRepository
    {
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Login(string userName, string passWord)
        {
            return (await GetMany(x => x.IsActive == true && (x.Name.Equals(userName) || x.Email.Equals(userName)) && x.Password.Equals(passWord)));

        }
        public async Task<IEnumerable<UserDto>> ForgotPassword(string user)
        {
            var result = await GetMany(x => x.IsActive == true && x.Email.Equals(user));
            return result;
        }
        public async Task<IEnumerable<UserDto>> LoginDetails()
        {
            return (await GetMany(x => x.IsActive == true));

        }

        public async Task<IEnumerable<UserDto>> AddUser(IEnumerable<UserDto> user)
        {
            return await AddEntities(user);
        }
        public async Task<IEnumerable<UserDto>> UpdateUser(IEnumerable<UserDto> user)
        {
            return await UpdateEntities(user);
        }
        public async Task<UserDto> GetUserById(int id)
        {
            return await GetOne(x => x.Id == id);
        }
        public async Task<IEnumerable<UserDto>> GetUserByIds(int[] ids)
        {
            return await GetMany(x => x.IsActive == true && ids.Contains(x.Id));
        }
        public async Task<UserDto> GetUserByEmail(string email)
        {
            return await GetOne(x => x.IsActive == true && x.Email == email);
        }
        public async Task<IEnumerable<UserDto>> GetUsers(Expression<Func<TblUser, bool>> criteria)
        {
            return await GetMany(criteria);
        }
        public async Task<IEnumerable<UserDto>> GetUsersByIds(int[] ids)
        {
            return await GetMany(x => ids.Contains(x.Id));
        }
    }
}