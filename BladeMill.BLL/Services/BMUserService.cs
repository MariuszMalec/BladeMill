using AutoMapper;
using BladeMill.BLL.DAL;
using BladeMill.BLL.DatatBaseAcess;
using BladeMill.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Dane dla Web app
    /// </summary>
    public class BMUserService : IBMUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public BMUserService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BMUserDto>> GetAll()
        {
            var users = new List<BMUserDto>();    
            if (_dbContext.Uzytkownicy.Any())
                //mapowanie reczne
                return await _dbContext.Uzytkownicy.Select(u=> new BMUserDto () 
                { 
                    Id = u.Id,
                    Created = u.Created,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Sso = u.Sso,
                    FullName = u.FullName
                }).ToListAsync();
            return users;
        }

        public async Task<BMUserDto> GetById(int id)
        {
            var bmuser = new BMUserDto();
            if (_dbContext.Uzytkownicy.Any())
            {
                var user = await _dbContext.Uzytkownicy.SingleOrDefaultAsync(s => s.Id == id);
                if (user == null)
                {
                    throw new Exception("Brak uzytkownika!");
                }
                bmuser = _mapper.Map<BMUserDto>(user);//mapowanie automatyczne wg konwencji , patrz profiles
                return bmuser;
            }
            return bmuser;
        }
    }
}
