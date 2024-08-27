using GuestBookApp.Models.ViewModels;
using GuestBookApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBookApp_RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        [BindProperty]
        public LoginViewModel LoginVM { get; set; }

        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByUsernameAsync(LoginVM.Username);
                if (user != null && BCrypt.Net.BCrypt.Verify(LoginVM.Password, user.PasswordHash))
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return Page();
        }
    }
}
