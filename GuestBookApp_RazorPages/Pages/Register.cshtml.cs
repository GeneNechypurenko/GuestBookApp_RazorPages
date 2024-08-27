using GuestBookApp.Models.ViewModels;
using GuestBookApp.Models;
using GuestBookApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBookApp_RazorPages.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        [BindProperty]
        public RegisterViewModel RegisterVM { get; set; }

        public RegisterModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetUserByUsernameAsync(RegisterVM.Username);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        Username = RegisterVM.Username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(RegisterVM.Password)
                    };

                    await _userRepository.AddUserAsync(user);
                    return RedirectToPage("/Login");
                }

                ModelState.AddModelError("", "Username already exists.");
            }

            return Page();
        }
    }
}
