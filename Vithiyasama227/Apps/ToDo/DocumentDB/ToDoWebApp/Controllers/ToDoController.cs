﻿using Domain.Models;
using Newtonsoft.Json;
using BusinessLogic;
using System;
using System.Web.Mvc;
using ToDoWebApp.Models;
using System.Threading.Tasks;
namespace ToDoWebApp.Controllers
{
    public class ToDoController : Controller
    {
        readonly IToDoBL _toDoService;
        /// <summary>
        /// Constructor which accepts the service as a parameter which is a dependency.
        /// This dependency is configured in the UnityConfig file inside RegisterTypes function
        /// This is in Service layer project inside ServiceLocation folder
        /// This is a file called UnityMvcActivator which is responsible to Start and Shutdown the DI 
        /// Start will called when the application start
        /// Shutdown will called when the application stop
        /// </summary>
        /// <param name="service"></param>
        public ToDoController(IToDoBL service)
        {
            _toDoService = service;
        }
        /// <summary>
        /// This is the default page which will run when the application start
        /// In this we are loading all the todo's items and returning that todolist to view
        /// </summary>
        /// <returns>If this is success it will rediect to index page otherwise error page</returns>
        public async Task<ActionResult> Index()
        {
            //Below is how you can log the information
            Logger.Information("ToDoController Index Enter into method");
            var form = new ToDoList();
            try
            {
                form.Items = await _toDoService.GetAll();
            }
            catch (Exception ex)
            {
                Logger.Error("ToDoController Unable to consume Index:" + ex.Message + ex.StackTrace);
                return RedirectToAction("Index", "Error");
            }
            finally
            {
                //This is to dispose the object
                Dispose();
            }
            return View(form);
        }
        /// <summary>
        /// This is to create a new todo item 
        /// </summary>
        /// <param name="form"></param>
        /// <returns>If this is success it will rediect to index page otherwise error page</returns>
        [HttpPost]
        public async Task<ActionResult> Create(ToDoItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _toDoService.Add(item);
                }
                else
                {
                    Logger.Error("ToDoController Create: Invalid data" + JsonConvert.SerializeObject(item));
                    return RedirectToAction("Index", "Error");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ToDoController Unable to consume Create:" + ex.Message + ex.StackTrace);
                return RedirectToAction("Index", "Error");
            }
            finally
            {
                //This is to dispose the object
                Dispose();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// This is to update todo item 
        /// </summary>
        /// <param name="form"></param>
        /// <returns>If this is success it will rediect to index page otherwise error page</returns>
        [HttpPost]
        public async Task<ActionResult> Update(ToDoItem item)
        {
            try
            {
                if (ModelState.IsValid & !string.IsNullOrEmpty(item.Id))
                {
                    await _toDoService.Update(item);
                }
                else
                {
                    Logger.Error("ToDoController Create: Invalid data" + JsonConvert.SerializeObject(item));
                    return RedirectToAction("Index", "Error");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ToDoController Unable to consume Create:" + ex.Message + ex.StackTrace);
                return RedirectToAction("Index", "Error");
            }
            finally
            {
                //This is to dispose the object
                Dispose();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// This is to delete the todo item from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If this is success it will rediect to index page otherwise error page</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                bool status = await _toDoService.Delete(id);
                if (!status)
                {
                    Logger.Error("ToDoController : Delete : "+ id +" id is not present in the database");
                    return RedirectToAction("Index", "Error");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ToDoController Unable to consume Delete:" + ex.Message + ex.StackTrace);
                return RedirectToAction("Index", "Error");
            }
            finally
            {
                //This is to dispose the object
                Dispose();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// This is to dispose / release the object
        /// </summary>
        public new void Dispose()
        {
            GC.SuppressFinalize(this);
            _toDoService.Dispose();
        }
    }
}