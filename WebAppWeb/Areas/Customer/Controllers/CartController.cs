using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models.ViewModels;
using System.Security.Claims;

namespace WebAppWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM itemList = new CartVM()
            {
                ListOfCart = _unitOfWork.CartReposotory.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties : "Product" )
            };
            return View(itemList);
        }
    }
}
