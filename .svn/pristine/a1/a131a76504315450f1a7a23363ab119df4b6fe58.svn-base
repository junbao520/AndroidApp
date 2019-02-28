//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TwoPole.Chameleon3.Infrastructure;
//using DCommon;
//using DCommon.Collections.Pagination;
//using DCommon.Data;
//using DCommon.Events;
//using DCommon.Search;

//namespace TwoPole.Chameleon3.Infrastructure.Services
//{
//    public class BasicDataService<T> : IBasicDataService<T>
//        where T : CreatedOnEntity<int>
//    {
//        private readonly IRepository<T> _settingRepository;
//        private readonly IEventPublisher _eventPublisher;
//        public BasicDataService(IRepository<T> settingRepository, IEventPublisher eventPublisher)
//        {
//            _settingRepository = settingRepository;
//            _eventPublisher = eventPublisher;
//        }

//        protected IRepository<T> SettingRepository
//        {
//            get { return _settingRepository; }
//        }

//        public virtual IPagination<T> Search(SearchCriteria<T> criteria)
//        {
//            var entities = _settingRepository.Table.ToPagination(criteria);
//            return entities;
//        }

//        public virtual IResult Delete(int id)
//        {
//            var entity = _settingRepository.Get(id);
//            if (entity == null)
//                return ResultFactory.Failed(MessageCodes.EntityDoesNotExists, ErrorCodes.EntityDoesNotExists);
//            try
//            {
//                _settingRepository.Delete(entity);
//                _settingRepository.Flush();
//                _eventPublisher.EntityDeleted(entity);
//            }
//            catch (Exception)
//            {
//                return ResultFactory.Failed(MessageCodes.DataIsBeingUsed, ErrorCodes.Failed);
//            }

//            return ResultFactory.Success(MessageCodes.DeleteSuccess, ErrorCodes.Success);
//        }

//        public virtual T Get(int id)
//        {
//            return _settingRepository.Get(id);
//        }

//        public virtual IResult Create(T entity)
//        {
//            var result = CheckEntity(entity);
//            if (!result.Success)
//                return result;

//            _settingRepository.Create(entity);
//            _settingRepository.Flush();
//            _eventPublisher.EntityInserted(entity);
//            result.Message = MessageCodes.CreateSuccess;
//            return result;
//        }

//        public virtual IResult Update(T entity)
//        {
//            var result = CheckEntity(entity);
//            if (!result.Success)
//                return result;

//            var modifiedEntity = entity as ModifiedOnEntity<int>;
//            if (modifiedEntity != null)
//                modifiedEntity.ModifiedOn = DateTime.Now;    
            
//            _settingRepository.Update(entity);
//            _settingRepository.Flush();
//            _eventPublisher.EntityUpdated(entity);
//            result.Message = MessageCodes.UpdateSuccess;
//            return result;
//        }

//        public virtual IList<T> GetAll()
//        {
//            return _settingRepository.Table.ToList();
//        }

//        #region Protected virtual methods
//        protected virtual IResult CheckEntity(T entity)
//        {
//            //if (_settingRepository.Table.Any(d => d.Id != entity.Id && d.Name == entity.Name))
//            //    return ResultFactory.Failed("名称不能重复", ErrorCodes.Failed);

//            return ResultFactory.Create(true);
//        }

//        protected virtual T Update(int id, Action<T> action, bool notify = true)
//        {
//            var entity = _settingRepository.Get(id);
//            if (entity != null)
//            {
//                action(entity);

//                var modifiedEntity = entity as ModifiedOnEntity<int>;
//                if (modifiedEntity != null)
//                    modifiedEntity.ModifiedOn = DateTime.Now;  

//                _settingRepository.Update(entity);
//                _settingRepository.Flush();
//                if (notify)
//                    _eventPublisher.EntityUpdated(entity);
//            }
//            return entity;
//        }
//        #endregion

//        #region ILookupService
//        //public IList<KeyValueDto> GetKeyValues()
//        //{
//        //    var query = from a in _settingRepository.Table
//        //                select new KeyValueDto
//        //                {
//        //                    Key = a.Id,
//        //                    Value = a.Name
//        //                };
//        //    var items = query.ToList();
//        //    return items;
//        //}
//        #endregion
//    }
//}
