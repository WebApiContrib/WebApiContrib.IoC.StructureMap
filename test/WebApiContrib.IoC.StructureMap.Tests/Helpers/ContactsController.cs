using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiContrib.IoC.StructureMap.Tests.Helpers
{
    public class ContactsController : ApiController
    {
        private IContactRepository _repository;

        public ContactsController(IContactRepository repository)
        {
            _repository = repository;
        }

        public HttpResponseMessage Post(List<Contact> contacts)
        {
            Debug.WriteLine(String.Format("POSTed Contacts: {0}", contacts.Count));

            var response = new HttpResponseMessage
                               {
                                   StatusCode = HttpStatusCode.Created
                               };

            return response;
        }
    }
}
