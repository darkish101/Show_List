using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Show_List.Base
{
    public interface IRepository<T>
    {
        T GetById(object id);
        IEnumerable<T> Get(NHibernate.Criterion.DetachedCriteria criteria);
        T GetObjectByCritaria(NHibernate.Criterion.DetachedCriteria criteria);
        IEnumerable<T> Get(NHibernate.Criterion.DetachedCriteria criteria, int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> GetByCrit(NHibernate.Criterion.DetachedCriteria criteria, int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> Get(NHibernate.Criterion.DetachedCriteria criteria, int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> Get(string fieldName, object fieldValue);
        IEnumerable<T> In(string fieldName, object[] values);
        IEnumerable<T> Get(string fieldName, object fieldValue, int pageIndex, int pageSize, out int count);
        IEnumerable<T> Get(string fieldName, object fieldValue, int baseIndex, short pageSize, out int count);
        IEnumerable<T> Get(string fieldName, object fieldValue, int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> Get(string fieldName, object fieldValue, int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> Get(NHibernate.IQuery query);
        IEnumerable<T> Get(NHibernate.IQuery query, int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> Get(NHibernate.IQuery query, int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(int pageIndex, int pageSize, out int count);
        IEnumerable<T> GetAll(int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> GetAll(int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection);
        IEnumerable<T> GetAll(string sortFieldName, SortDirection sortDirection);//Rizwan:28-Feb-2012   To allow sorting even paging is not applied

        int GetCount();
        void SaveOrUpdate(T obj);
        void Save(T obj);
        void Update(T obj);
        void SaveOrUpdate(T obj, bool autoCommit);
        void SaveOrUpdate(IEnumerable<T> objList);
        void SaveOrUpdate(IEnumerable<T> objList, bool autoCommit);
        void Delete(T obj);
        void Delete(T obj, bool autoCommit);
        void Delete(IEnumerable<T> objList);
        void Delete(IEnumerable<T> objList, bool autoCommit);
        string Get_Loc_Access_Rights(string empId, string Connection); //Added by Saud on Wednesday, October 29, 2014
    }
}