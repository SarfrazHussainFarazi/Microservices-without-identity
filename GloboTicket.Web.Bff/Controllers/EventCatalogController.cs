using System;
using System.Threading.Tasks;
using GloboTicket.Web.Extensions;
using GloboTicket.Web.Models;
using GloboTicket.Web.Models.Api;
using GloboTicket.Web.Models.View;
using GloboTicket.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.Web.Controllers
{
    public class EventCatalogController : Controller
    {
        private readonly IEventCatalogService eventCatalogService;
        private readonly IShoppingBasketService shoppingBasketService;
        private readonly Settings settings;
        private readonly IHttpContextAccessor _GetCurrentConext;

        public EventCatalogController(IEventCatalogService eventCatalogService, IShoppingBasketService shoppingBasketService, Settings settings, IHttpContextAccessor IHttpContextAccessor)
        {
            this.eventCatalogService = eventCatalogService;
            this.shoppingBasketService = shoppingBasketService;
            this.settings = settings;
            this._GetCurrentConext = IHttpContextAccessor;
        }

        //public async Task<IActionResult> Index(Guid categoryId)
        //{
        //    var currentBasketId = Request.Cookies.GetCurrentBasketId(settings);

        //    var getBasket = currentBasketId == Guid.Empty ? Task.FromResult<Basket>(null) :
        //        shoppingBasketService.GetBasket(currentBasketId);

        //    var getCategories = eventCatalogService.GetCategories();

        //    var getEvents = categoryId == Guid.Empty ? eventCatalogService.GetAll() :
        //        eventCatalogService.GetByCategoryId(categoryId);

        //    await Task.WhenAll(new Task[] { getBasket, getCategories, getEvents });

        //    var numberOfItems = getBasket.Result?.NumberOfItems ?? 0;

        //    return View(
        //        new EventListModel
        //        {
        //            Events = getEvents.Result,
        //            Categories = getCategories.Result,
        //            NumberOfItems = numberOfItems,
        //            SelectedCategory = categoryId
        //        }
        //    );
        //}

        public async Task<IActionResult> Index(Guid categoryId)
        {
            try
            {
                


            var currentBasketId = Request.Cookies.GetCurrentBasketId(settings);
            var result = await eventCatalogService.GetCatalogBrowse(currentBasketId, categoryId, this._GetCurrentConext.HttpContext.Session.GetString("token"));

            return View(
                new EventListModel
                {
                    Events = result.Events,
                    Categories = result.Categories,
                    NumberOfItems = result.NumberOfItems,
                    SelectedCategory = categoryId
                }
            );
            }
            catch (Exception e)
            {

               return RedirectToAction( "Unathorized", "Account",new { msg=e.Message.ToString()});
            }
        }

        [HttpPost]
        public IActionResult SelectCategory([FromForm]Guid selectedCategory)
        {
            return RedirectToAction("Index", new { categoryId = selectedCategory });
        }

        public async Task<IActionResult> Detail(Guid eventId)
        {
            try
            {

           
            var ev = await eventCatalogService.GetEvent(eventId, this._GetCurrentConext.HttpContext.Session.GetString("token"));
            return View(ev);
            }
            catch (Exception e)
            {

                return RedirectToAction("Unathorized", "Account", new { msg = e.Message.ToString() });
            }
        }
    }
}
