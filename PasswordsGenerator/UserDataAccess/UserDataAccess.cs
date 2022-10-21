using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDataAccess
    {
        private readonly PasswordsGeneratorDBContext _dbContext;

        public UserDataAccess(PasswordsGeneratorDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<UserPasswordGenerated> GetAllUsers()
        {
            return (from user in _dbContext.UserPasswordGenerated select user).ToList();
        }

        public UserPasswordGenerated? GetUserByUserID(string userID)
        {
            return (from user in _dbContext.UserPasswordGenerated where user.UserID == userID select user).FirstOrDefault();
        }

        public void SaveTheGeneratePasswordToDB(UserPasswordGenerated userPasswordGenerated) {

            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.UserPasswordGenerated.Add(userPasswordGenerated);
                    _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw new Exception("Error at insert into DB",ex.InnerException);
                }
            }
        }

        public void UpdateThePassowrdAndDatetime(UserPasswordGenerated userPasswordGenerated)
        {

            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var userFromDB = GetUserByUserID(userPasswordGenerated.UserID ?? "") ?? new UserPasswordGenerated();
                    userFromDB.GeneratedPassword = userPasswordGenerated.GeneratedPassword;
                    userFromDB.PasswordGenerationDatetime = userPasswordGenerated.PasswordGenerationDatetime;
                    _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw new Exception("Error at update info user.", ex.InnerException);
                }
            }
        }
    }
}
