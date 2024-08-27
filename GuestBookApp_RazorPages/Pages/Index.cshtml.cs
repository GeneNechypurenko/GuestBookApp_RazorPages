using GuestBookApp.Models.ViewModels;
using GuestBookApp.Models;
using GuestBookApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestBookApp_RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMessageRepository _messageRepository;

        public IEnumerable<Message> Messages { get; set; }

        [BindProperty]
        public MessageViewModel MessageVM { get; set; }

        public IndexModel(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task OnGetAsync()
        {
            Messages = await _messageRepository.GetMessagesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var username = HttpContext.Session.GetString("Username");
                if (!string.IsNullOrEmpty(username))
                {
                    var message = new Message
                    {
                        Username = username,
                        Text = MessageVM.Text,
                        PostedAt = DateTime.Now
                    };

                    await _messageRepository.AddMessageAsync(message);
                }
            }

            return RedirectToPage();
        }
    }
}
