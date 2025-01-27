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
    }
}
