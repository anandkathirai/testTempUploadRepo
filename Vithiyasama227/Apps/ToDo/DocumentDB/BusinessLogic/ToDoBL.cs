﻿using AutoMapper;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
[assembly: CLSCompliant(true)]
namespace BusinessLogic
{
    public sealed class ToDoBL : IDisposable, IToDoBL
    {
        /// <summary>
        /// This is to create an object for the ToDoRepository
        /// </summary>
        readonly IToDoRepository _repository;
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        /// <summary>
        /// Constructor which accepts the repository as a parameter which is a dependency.
        /// This dependency is configured in the UnityConfig file inside RegisterTypes function
        /// This is inside ServiceLocation folder
        /// </summary>
        /// <param name="repository"></param>
        public ToDoBL(IToDoRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// This is to add a new todo item 
        /// Here we have used automapper to convert the domain model to DAL model
        /// </summary>
        /// <param name="item"></param>
        /// <returns>After adding todo item it will append the id in todo object and return that object</returns>
        public async Task<Domain.Models.ToDoItem> Add(Domain.Models.ToDoItem item)
        {
            if(null != item)
            {
                //This is to initilize the automapper
                Mapper.Initialize(cfg => cfg.CreateMap<Domain.Models.ToDoItem, DataAccessLayer.Models.ToDoItem>());
                //This is convert the Domain model to DAL model object, this will take domain model as parameter and return DAL model as response
                DataAccessLayer.Models.ToDoItem itemDAL = Mapper.Map<Domain.Models.ToDoItem, DataAccessLayer.Models.ToDoItem>(item);
                var record = await _repository.Create(itemDAL);
                if(null != record)
                {
                    item.Id = record.id;
                }
            }            
            return item;
        }
        /// <summary>
        /// This is to update todo item 
        /// Here we have used automapper to convert the domain model to DAL model
        /// </summary>
        /// <param name="item"></param>
        /// <returns>After updating todo item, it will retrun true if it updated successfully otherwise false</returns>
        public async Task<bool> Update(Domain.Models.ToDoItem item)
        {
            bool status = false;
            if (null != item)
            {
                //This is to initilize the automapper
                Mapper.Initialize(cfg => cfg.CreateMap<Domain.Models.ToDoItem, DataAccessLayer.Models.ToDoItem>());
                //This is convert the Domain model to DAL model object, this will take domain model as parameter and return DAL model as response
                DataAccessLayer.Models.ToDoItem itemDAL = Mapper.Map<Domain.Models.ToDoItem, DataAccessLayer.Models.ToDoItem>(item);
                var record = await _repository.Update(itemDAL.id, itemDAL);
                if (record != null)
                {
                    status = true;
                }
            }
            return status;
        }
        /// <summary>
        /// This is to delete an item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This will retrun bool. If the todo item is deleted successfully then it will return true otherwise false. </returns>
        public async Task<bool> Delete(string id)
        {
            bool status = false;
            var item = await _repository.GetById(id);
            if (null != item)
            {
                return await _repository.Delete(item);
            }
            return status;
        }
        /// <summary>
        /// This is to get all the todo's items
        /// Here we have used automapper to convert the DAL model list to domain model list
        /// </summary>
        /// <returns>This will retun a list of todo's items as list</returns>
        public async Task<List<Domain.Models.ToDoItem>> GetAll()
        {
            List<Domain.Models.ToDoItem> items = new List<Domain.Models.ToDoItem>();
            var itemsDAL = (await _repository.All()).ToList();
            if (null != itemsDAL & itemsDAL.Count > 0)
            {
                //This is to initilize the automapper
                Mapper.Initialize(cfg => cfg.CreateMap<DataAccessLayer.Models.ToDoItem, Domain.Models.ToDoItem>());
                //This is convert the DAL model list to Domain model list, this will take DAL model list as parameter and return Domain model list as response
                items = Mapper.Map<List<DataAccessLayer.Models.ToDoItem>, List<Domain.Models.ToDoItem>>(itemsDAL);
            }            
            return items;
        }
        /// <summary>
        /// This is to dispose / release the connection 
        /// </summary>
        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
