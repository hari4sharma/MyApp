using System.Diagnostics;
using System.Security.Claims;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.DataAccessLayer.Infrastructure.Repository;
using MyApp.Models;

namespace WebAppWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "category");
            return View(productList);
        }
        [HttpGet]
        public IActionResult Details(int? ProductId)
        {
            if (ProductId == null || ProductId == 0)
            {
                return NotFound();
            }
            Cart cart = new Cart()
            {
                Product = _unitOfWork.Product.Get(x => x.Id == ProductId, includeProperties: "category"),
                Count = 1,
                ProductId = (int)ProductId
            };
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cart.ApplicationUserId = claims.Value;

                var cartItem = _unitOfWork.CartReposotory.Get(p => p.ProductId == cart.ProductId
                && p.ApplicationUserId == claims.Value);

                if (cartItem == null)
                {
                    _unitOfWork.CartReposotory.Add(cart);
                }
                else
                {
                    _unitOfWork.CartReposotory.IncrementCartItem(cartItem, cart.Count);
                }

                _unitOfWork.save();
            }
            return RedirectToAction("Index");
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
