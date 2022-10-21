using AutoMapper;
using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using PasswordsGenerator.Dtos;
using PasswordsGenerator.Models;
using PasswordsGenerator.ViewModels.Home;
using System.Diagnostics;

namespace PasswordsGenerator.Controllers
{
    public class HomeController : Controller
    {
        private UserBusinessLogic _userBusinessLogic;
        private IMapper _mapper;

        public HomeController(UserBusinessLogic userBusinessLogic, IMapper mapper)
        {
            _userBusinessLogic = userBusinessLogic;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeViewModel homeViewModelcs = new HomeViewModel();
            return View(homeViewModelcs);
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel homeViewModel)
        {
            if (ModelState.IsValid)
            {
                var userFromDB = _userBusinessLogic.GetUserByUserID(homeViewModel.userID ?? "");
                if (userFromDB == null)
                {
                    try
                    {
                        var generateRandomPassword = _userBusinessLogic.GeneratePassword();
                        UserPasswordGeneratedDto userPasswordGeneratedDto = _mapper.Map<UserPasswordGeneratedDto>(homeViewModel);
                        userPasswordGeneratedDto.generatedPassword = generateRandomPassword;
                        _userBusinessLogic.SaveTheGeneratePasswordToDB(userPasswordGeneratedDto);
                        return RedirectToAction("GeneratedPassword", new { userID = userPasswordGeneratedDto.userID, newUser = 1 });
                    }
                    catch
                    {
                        return Error();
                    }
                }
                else
                {
                    return RedirectToAction("GeneratedPassword", new { userID = userFromDB.userID, newUser = 0 });
                }
            }
            else
            {
                return View(homeViewModel);
            }
        }

        [HttpGet]
        public IActionResult GeneratedPassword(string userID, int newUser)
        {
            var userFromDB = _userBusinessLogic.GetUserByUserID(userID);
            if (newUser == 1)
            {
                GeneratedPasswordViewModel generatedPasswordViewModel = _mapper.Map<GeneratedPasswordViewModel>(userFromDB);
                generatedPasswordViewModel.messageAlert = "The password validity is for 30 seconds. Then you will be redirected to home page.";
                return View(generatedPasswordViewModel);
            }
            else
            {
                var diffWithThirtySeconds = _userBusinessLogic.DifferenceBetweenGeneratedPasswordsDateTimes(userFromDB.passwordGenerationDatetime);
                if (diffWithThirtySeconds < 30 && diffWithThirtySeconds >= 0)
                {
                    GeneratedPasswordViewModel generatedPasswordViewModel = _mapper.Map<GeneratedPasswordViewModel>(userFromDB);
                    generatedPasswordViewModel.messageAlert = "The user already exists in DB and the password is valid for "+ diffWithThirtySeconds + " seconds.";
                    return View(generatedPasswordViewModel);
                }
                else
                {
                    try
                    {
                        var newGenerateRandomPassword = _userBusinessLogic.GeneratePassword();
                        UserPasswordGeneratedDto userPasswordGeneratedDto = _mapper.Map<UserPasswordGeneratedDto>(userFromDB);
                        userPasswordGeneratedDto.generatedPassword = newGenerateRandomPassword;
                        userPasswordGeneratedDto.passwordGenerationDatetime = DateTime.Now;
                        _userBusinessLogic.UpdateThePasswordAndTheDatetime(userPasswordGeneratedDto);
                        GeneratedPasswordViewModel generatedPasswordViewModel = _mapper.Map<GeneratedPasswordViewModel>(userPasswordGeneratedDto);
                        generatedPasswordViewModel.messageAlert = "The password validity is for 30 seconds. Then you will be redirected to home page.";
                        return View(generatedPasswordViewModel);
                    }
                    catch
                    {
                        return Error();
                    }
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}