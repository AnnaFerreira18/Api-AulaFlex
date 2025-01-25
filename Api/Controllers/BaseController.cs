using Microsoft.AspNetCore.Mvc;
using Domain.Repositories;
using System.Net;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Infrastructure.Transaction;

namespace Api.Controllers
{
    public class BaseController : ApiController
    {
        private readonly IUnitOfWork _uow;
        public new HttpResponseMessage ResponseMessage;

        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        //[ResponseType(typeof(Task<HttpResponseMessage>))]
        //public Task<HttpResponseMessage> ReturnResponse(ServerCommand service, object success, object error)
        //{
        //    if (service.HasNotifications())
        //        ResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest,
        //            new { data = error, errors = service.GetNotifications() });
        //    else
        //    {
        //        _uow.Commit();
        //        ResponseMessage = Request.CreateResponse(HttpStatusCode.OK, success);
        //    }

        //    return Task.FromResult(ResponseMessage);
        //}
    }
}
