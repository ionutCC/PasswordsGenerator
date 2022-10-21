using AutoMapper;
using BusinessLayer;
using Castle.Components.DictionaryAdapter.Xml;
using DataAccess;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using PasswordsGenerator.Dtos;
using PasswordsGenerator.Mapper;

namespace UnitTests
{
    [TestFixture]
    public class UserBusinessLogicTests
    {
        UserBusinessLogic? userBusinessLogic;
        UserDataAccess? userDataAccess;
        PasswordsGeneratorDBContext? passwordsGeneratorDBContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<PasswordsGeneratorDBContext>()
                .UseInMemoryDatabase(databaseName: "PasswordsGeneratorDB")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            using (var context = new PasswordsGeneratorDBContext(options))
            {
                context.UserPasswordGenerated.Add(new DataLayer.Entities.UserPasswordGenerated
                {
                    UserCountor = 1,
                    UserID = "MaPa123456",
                    GeneratedPassword = "fhj7ssd$",
                    PasswordGenerationDatetime = Convert.ToDateTime("2022-10-12 00:45:52.457")
                });
                context.UserPasswordGenerated.Add(new DataLayer.Entities.UserPasswordGenerated
                {
                    UserCountor = 2,
                    UserID = "CeIo12f34",
                    GeneratedPassword = "UVLWAG3Q",
                    PasswordGenerationDatetime = Convert.ToDateTime("2022-10-13 02:44:04.280")
                });
                context.SaveChanges();
            }
            passwordsGeneratorDBContext = new PasswordsGeneratorDBContext(options);
            userDataAccess = new UserDataAccess(passwordsGeneratorDBContext);
            var mapperMock = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mapperMock.CreateMapper();
            userBusinessLogic = new UserBusinessLogic(userDataAccess, mapper);
        }

        [Test, Order(1)]
        public void GatAllUsers_Works()
        {
            // Arrange
            var numberOfUser = 2;

            // Act
            IList<UserPasswordGeneratedDto> listOfUsers = userBusinessLogic.GetAllUsers();

            // Assert
            Assert.That(listOfUsers.Count, Is.EqualTo(numberOfUser));
        }

        [Test, Order(2)]
        public void GetUserByUserID_Works()
        {
            // Arrange
            var userPassword = "fhj7ssd$";

            // Act
            UserPasswordGeneratedDto userFromDB = userBusinessLogic.GetUserByUserID("MaPa123456");

            // Assert
            Assert.That(userFromDB.generatedPassword, Is.EqualTo(userPassword));
        }

        [Test, Order(3)]
        public void SaveTheGeneratePasswordToDB_Works()
        {
            // Arrange
            UserPasswordGeneratedDto newUser = new UserPasswordGeneratedDto() {
                userID = "LuAn789345",
                generatedPassword = "kdaoijngr",
                passwordGenerationDatetime = Convert.ToDateTime("2022-10-20 02:44:04.280")
            };

            // Act
            userBusinessLogic.SaveTheGeneratePasswordToDB(newUser);
            var newUsersList = userBusinessLogic.GetAllUsers();

            // Assert
            Assert.That(newUsersList.Count, Is.EqualTo(3));
            Assert.That(newUsersList.ElementAt(2).userID, Is.EqualTo("LuAn789345"));
        }

        [Test, Order(4)]
        public void UpdateThePasswordAndTheDatetime_Works()
        {
            // Arrange
            UserPasswordGeneratedDto updateUser = new UserPasswordGeneratedDto()
            {
                userID = "LuAn789345",
                generatedPassword = "00000000",
                passwordGenerationDatetime = Convert.ToDateTime("2022-10-21 00:44:04.280")
            };

            // Act
            userBusinessLogic.UpdateThePasswordAndTheDatetime(updateUser);
            UserPasswordGeneratedDto userFromDB = userBusinessLogic.GetUserByUserID("LuAn789345");

            // Assert
            Assert.That(userFromDB.generatedPassword, Is.EqualTo("00000000"));
            Assert.That(userFromDB.passwordGenerationDatetime, Is.EqualTo(Convert.ToDateTime("2022-10-21 00:44:04.280")));
        }

        [Test, Order(5)]
        public void DifferenceBetweenGeneratedPasswordsDateTimes_Works()
        {
            // Arrange
            UserPasswordGeneratedDto userPasswordGeneratedDto = new UserPasswordGeneratedDto()
            {
                userID = "LuAn789345",
                generatedPassword = "00000000",
                passwordGenerationDatetime = DateTime.Now
            };
            var timeLimitOfPassword = 30.00m;
            //Thread.Sleep(25000);
            Thread.Sleep(1000);

            // Act
            double differenceBetWeenDateTimes = userBusinessLogic.DifferenceBetweenGeneratedPasswordsDateTimes(userPasswordGeneratedDto.passwordGenerationDatetime);

            // Assert
            Assert.Greater(timeLimitOfPassword, differenceBetWeenDateTimes);
            Assert.Less(0, differenceBetWeenDateTimes);
        }

        [Test, Order(6)]
        public void SaveTheGeneratePasswordToDB_ThrowException()
        {
            // Arrange
            UserPasswordGeneratedDto? newUser = null;

            // Act
            Exception ex = Assert.Throws<Exception>(() => userBusinessLogic.SaveTheGeneratePasswordToDB(newUser));

            // Assert
            Assert.AreEqual("Error at insert into DB", ex.Message);
            //Assert.That(newUsersList.ElementAt(2).userID, Is.EqualTo("LuAn789345"));
        }

        [Test, Order(7)]
        public void UpdateThePasswordAndTheDatetime_ThrowException()
        {
            // Arrange
            UserPasswordGeneratedDto? updateUser = null;

            // Act
            Exception ex = Assert.Throws<Exception>(() => userBusinessLogic.UpdateThePasswordAndTheDatetime(updateUser));

            // Assert
            Assert.AreEqual("Error at update info user.", ex.Message);
            //Assert.That(newUsersList.ElementAt(2).userID, Is.EqualTo("LuAn789345"));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}