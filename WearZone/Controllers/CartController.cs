using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WearZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetCartSessionId()
        {
            if (Request.Cookies["CartSessionId"] == null)
            {
                var sessionId = Guid.NewGuid().ToString();
                Response.Cookies.Append("CartSessionId", sessionId, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                return sessionId;
            }
            return Request.Cookies["CartSessionId"];
        }

        private async Task<Panier> GetOrCreateCartAsync()
        {
            string userId;

            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.Name;

                // CHECK FOR MERGE: If there is a session cart, merge it.
                var sessionId = Request.Cookies["CartSessionId"];
                if (!string.IsNullOrEmpty(sessionId))
                {
                    var sessionCart = await _context.Paniers
                        .Include(p => p.Elements)
                        .FirstOrDefaultAsync(p => p.UserId == sessionId);

                    if (sessionCart != null && sessionCart.Elements.Any())
                    {
                        var userCart = await _context.Paniers
                            .Include(p => p.Elements)
                            .FirstOrDefaultAsync(p => p.UserId == userId);

                        if (userCart == null)
                        {
                            userCart = new Panier { UserId = userId };
                            _context.Paniers.Add(userCart);
                            await _context.SaveChangesAsync(); // Save to get ID
                        }

                        foreach (var item in sessionCart.Elements)
                        {
                            var existingItem = userCart.Elements.FirstOrDefault(e => e.ProductId == item.ProductId);
                            if (existingItem != null)
                            {
                                existingItem.Quantite += item.Quantite;
                            }
                            else
                            {
                                // Must create new instance to attach to new parent
                                userCart.Elements.Add(new ElementPanier 
                                { 
                                    ProductId = item.ProductId, 
                                    Quantite = item.Quantite,
                                    PanierId = userCart.Id
                                });
                            }
                        }

                        _context.Paniers.Remove(sessionCart); // Remove old session cart
                        Response.Cookies.Delete("CartSessionId"); // Clear cookie
                        await _context.SaveChangesAsync();
                        return userCart; // Return merged cart immediately
                    }
                }
            }
            else
            {
                userId = GetCartSessionId();
            }
            
            var cart = await _context.Paniers
                .Include(p => p.Elements)
                .ThenInclude(e => e.Product)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (cart == null)
            {
                cart = new Panier { UserId = userId };
                _context.Paniers.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await GetOrCreateCartAsync();
            return Ok(new { 
                items = cart.Elements.Select(e => new {
                    productId = e.ProductId,
                    productName = e.Product.Name,
                    price = e.Product.Price,
                    imageUrl = e.Product.ImageUrl,
                    quantity = e.Quantite
                }),
                total = cart.Elements.Sum(e => e.Product.Price * e.Quantite),
                count = cart.Elements.Sum(e => e.Quantite)
            });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] int productId)
        {
            var cart = await GetOrCreateCartAsync();
            var element = cart.Elements.FirstOrDefault(e => e.ProductId == productId);

            if (element != null)
            {
                element.Quantite++;
            }
            else
            {
                element = new ElementPanier { ProductId = productId, Quantite = 1, PanierId = cart.Id };
                _context.ElementsPanier.Add(element);
            }

            await _context.SaveChangesAsync();
            return await GetCart(); // Return updated cart
        }

        [HttpPost("decrease")]
        public async Task<IActionResult> DecreaseFromCart([FromBody] int productId)
        {
            var cart = await GetOrCreateCartAsync();
            var element = cart.Elements.FirstOrDefault(e => e.ProductId == productId);

            if (element != null)
            {
                if (element.Quantite > 1)
                {
                    element.Quantite--;
                }
                else
                {
                    _context.ElementsPanier.Remove(element);
                }
                await _context.SaveChangesAsync();
            }

            return await GetCart(); // Return updated cart
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] int productId)
        {
            var cart = await GetOrCreateCartAsync();
            var element = cart.Elements.FirstOrDefault(e => e.ProductId == productId);

            if (element != null)
            {
                _context.ElementsPanier.Remove(element);
                await _context.SaveChangesAsync();
            }

            return await GetCart();
        }
    }
}
