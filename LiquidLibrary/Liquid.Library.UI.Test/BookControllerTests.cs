using Liquid.Actions;
using Liquid.Library.Domain.Inventory;
using Liquid.Library.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Liquid.Library.UI.Test
{
    [TestClass]
    public class BookControllerTests
    {
        private Guid? _bookIdInstance;

        private Guid BookId
        {
            get
            {
                if (!_bookIdInstance.HasValue)
                    CreateNew();
                return _bookIdInstance.Value;
            }
            set
            {
                _bookIdInstance = value;
            }
        }

        [TestMethod]
        public void CreateNew()
        {
            var requestContext = new RequestContext();
            var controllerFactory = new LiquidControllerFactory();
            var controller = controllerFactory.CreateController(requestContext, typeof(Book)) as EntityController<Book>;
            var result = controller.CreateNew() as ViewResult;

            var book = new Book();
            book.Name = "UnitTest - CreateNew - Name - " + DateTimeOffset.Now;
            book.Author = "UnitTest - CreateNew - Author - " + DateTimeOffset.Now;
            book.ISBN = "UnitTest - CreateNew - ISBN - " + DateTimeOffset.Now;
            controller.CreateNew(book);

            BookId = book.Id;
        }

        [TestMethod]
        public void Update()
        {
            var requestContext = new RequestContext();
            var controllerFactory = new LiquidControllerFactory();
            var controller = controllerFactory.CreateController(requestContext, typeof(Book)) as EntityController<Book>;
            var result = controller.Update(BookId) as ViewResult;

            var actionContext = result.Model as IFormActionContext;
            var book = actionContext.GetEntity() as Book;
            book.Name = "UnitTest - Update - Name - " + DateTimeOffset.Now;
            book.Author = "UnitTest - Update - Author - " + DateTimeOffset.Now;
            book.ISBN = "UnitTest - Update - ISBN - " + DateTimeOffset.Now;
            controller.Update(book, BookId);
        }
    }
}
