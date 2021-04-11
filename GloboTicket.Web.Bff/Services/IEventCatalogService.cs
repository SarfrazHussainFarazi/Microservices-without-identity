using GloboTicket.Web.Bff.Models.Api;
using GloboTicket.Web.Models.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.Web.Services
{
    public interface IEventCatalogService
    {
        Task<IEnumerable<Event>> GetAll();
        Task<IEnumerable<Event>> GetByCategoryId(Guid categoryid);
        Task<Event> GetEvent(Guid id, string token);
        Task<IEnumerable<Category>> GetCategories();
        Task<CatalogBrowse> GetCatalogBrowse(Guid basketId, Guid categoryId, string token);
    }
}
