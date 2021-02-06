using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithoutIdentity.Models;
using WithoutIdentity.Models.ManageViewModels;

namespace WithoutIdentity.Controllers
{
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ManageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); //User: disponibilizado pelo controllerBase

            if (user == null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User); //User: disponibilizado pelo controllerBase

            if (user == null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            var email = user.Email;

            if (email != model.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Erro inesperado ao aribuir um email para o usuário com o ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;

            if (phoneNumber != model.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Erro inesperado ao aribuir um telefone para o usuário com o ID '{user.Id}'.");
                }
            }

            StatusMessage = "Seu perfil foi atualizado";

            return (RedirectToAction(nameof(Index)));
        }
    }
}
