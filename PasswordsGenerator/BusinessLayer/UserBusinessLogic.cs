using AutoMapper;
using DataAccess;
using DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using PasswordsGenerator.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserBusinessLogic
    {
        private UserDataAccess _userDataAccess;
        private IMapper _mapper; 

        public UserBusinessLogic(UserDataAccess userDataAccess, IMapper mapper)
        {
            _userDataAccess = userDataAccess;
            _mapper = mapper;
        }

        public virtual IList<UserPasswordGeneratedDto> GetAllUsers()
        {
            return _userDataAccess.GetAllUsers().Select(u => _mapper.Map<UserPasswordGeneratedDto>(u)).ToList();
        }

        public string GeneratePassword()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars,8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void SaveTheGeneratePasswordToDB(UserPasswordGeneratedDto userPasswordGeneratedDto)
        {
            try
            {
                UserPasswordGenerated userPasswordGenerated = _mapper.Map<UserPasswordGenerated>(userPasswordGeneratedDto);
                _userDataAccess.SaveTheGeneratePasswordToDB(userPasswordGenerated);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateThePasswordAndTheDatetime(UserPasswordGeneratedDto userPasswordGeneratedDto)
        {
            try
            {
                UserPasswordGenerated userPasswordGenerated = _mapper.Map<UserPasswordGenerated>(userPasswordGeneratedDto);
                _userDataAccess.UpdateThePassowrdAndDatetime(userPasswordGenerated);
            }
            catch
            {
                throw;
            }
        }

        public UserPasswordGeneratedDto GetUserByUserID(string userID)
        {
            return _mapper.Map<UserPasswordGeneratedDto>(_userDataAccess.GetUserByUserID(userID));
        }

        public double DifferenceBetweenGeneratedPasswordsDateTimes(DateTime? passwordGenerationDatetime)
        {
            var diffInSeconds = (DateTime.Now - (passwordGenerationDatetime ?? DateTime.Now)).TotalSeconds;
            var diffWithThirtySeconds = 30 - diffInSeconds;
            return diffWithThirtySeconds;
        }
    }
}
