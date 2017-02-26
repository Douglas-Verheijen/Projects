using Liquid.Actions;
using Liquid.Domain;
using Liquid.IoC;
using System;
using System.Web.Mvc;

namespace Liquid.Library.UI.MVC.Controllers
{
    public class EntityController<TEntity> : Controller
        where TEntity : Entity
    {
        private readonly IInstanceProvider<IListActionContext<TEntity>> _listActionContextInstanceProvider;
        private readonly IInstanceProvider<IViewActionContext<TEntity>> _viewActionContextInstanceProvider;
        private readonly IInstanceProvider<ICreateNewActionContext<TEntity>> _createNewActionContextInstanceProvider;
        private readonly IInstanceProvider<IUpdateActionContext<TEntity>> _updateActionContextInstanceProvider;
        private readonly IInstanceProvider<IDeleteActionContext<TEntity>> _deleteActionContextInstanceProvider;

        public EntityController(
            IInstanceProvider<IListActionContext<TEntity>> listActionContextInstanceProvider,
            IInstanceProvider<IViewActionContext<TEntity>> viewActionContextInstanceProvider,
            IInstanceProvider<ICreateNewActionContext<TEntity>> createNewActionContextInstanceProvider,
            IInstanceProvider<IUpdateActionContext<TEntity>> updateActionContextInstanceProvider,
            IInstanceProvider<IDeleteActionContext<TEntity>> deleteActionContextInstanceProvider)
        {
            _listActionContextInstanceProvider = listActionContextInstanceProvider;
            _viewActionContextInstanceProvider = viewActionContextInstanceProvider;
            _createNewActionContextInstanceProvider = createNewActionContextInstanceProvider;
            _updateActionContextInstanceProvider = updateActionContextInstanceProvider;
            _deleteActionContextInstanceProvider = deleteActionContextInstanceProvider;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var actionContext = _listActionContextInstanceProvider.GetInstance();
            actionContext.Page = page.HasValue ? page.Value : 0;
            actionContext.PageSize = pageSize.HasValue ? pageSize.Value : 10;
            actionContext.InitiateProcess();
            return View(actionContext);
        }

        [HttpGet]
        public ActionResult Overview(Guid? id)
        {
            if (id.HasValue)
            {
                var actionContext = _viewActionContextInstanceProvider.GetInstance();
                actionContext.EntityId = id.Value;
                actionContext.InitiateProcess();
                return View(actionContext);
            }

            return null;
        }

        [HttpGet]
        public ActionResult CreateNew()
        {
            var actionContext = _createNewActionContextInstanceProvider.GetInstance();
            actionContext.InitiateProcess();
            return View(actionContext);
        }

        [HttpPost]
        public ActionResult CreateNew(TEntity entity)
        {
            var actionContext = _createNewActionContextInstanceProvider.GetInstance();
            actionContext.Entity = entity;
            actionContext.Process();

            if (!actionContext.HasErrors())
                return RedirectToAction("Overview", new { id = actionContext.Entity.Id.ToString() });

            throw new Exception("Failed to save.");
        }

        [HttpGet]
        public ActionResult Update(Guid id)
        {
            var actionContext = _updateActionContextInstanceProvider.GetInstance();
            actionContext.EntityId = id;
            actionContext.InitiateProcess();
            return View(actionContext);
        }

        [HttpPost]
        public ActionResult Update(TEntity model, Guid id)
        {
            var actionContext = _updateActionContextInstanceProvider.GetInstance();
            actionContext.EntityId = id;
            actionContext.InitiateProcess();

            if (actionContext.Entity != null)
            {
                foreach (var property in typeof(TEntity).GetProperties())
                {
                    var value = property.GetValue(model);
                    property.SetValue(actionContext.Entity, value);
                }
                
                actionContext.Process();

                if (!actionContext.HasErrors())
                    return RedirectToAction("Overview", new { id = actionContext.Entity.Id.ToString() });
            }

            return View();
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var actionContext = _deleteActionContextInstanceProvider.GetInstance();
            actionContext.EntityId = id;

            actionContext.InitiateProcess();
            if (!actionContext.HasErrors())
            {
                actionContext.Process();
                if (!actionContext.HasErrors())
                    return RedirectToAction("Index");
            }

            return View();
        }
    }
}