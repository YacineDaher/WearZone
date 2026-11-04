using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WearZone.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public ContactInputModel Input { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // In a real application, you would send an email here.
            // For now, we'll just simulate a successful submission.
            
            TempData["SuccessMessage"] = "Thank you for contacting us! We will get back to you shortly.";
            return RedirectToPage();
        }

        public class ContactInputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Subject { get; set; }

            [Required]
            public string Message { get; set; }
        }
    }
}
