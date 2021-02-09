using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace Show_List.Base
{
    public enum SortDirection { Ascending = 0, Descending = 1 };
    public class Repository<T> : IRepository<T>
    {
        protected ISession _session = null;

    public Repository(ISession session)
    {
        this._session = session;
    }

    public T GetById(object id)
    {
        T obj = default(T);
        if (id != null)
            obj = (T)_session.Get<T>(id);

        return obj;
    }
    public T GetObjectByCritaria(DetachedCriteria query)
    {
        T obj = default(T);
        obj = query.GetExecutableCriteria(this._session).List<T>().FirstOrDefault();
        return obj;
    }
    public IEnumerable<T> Get(DetachedCriteria query)
    {
        return query.GetExecutableCriteria(this._session).List<T>();
    }

    public IEnumerable<T> Get(DetachedCriteria query, int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            query.SetFirstResult(baseIndex)
                .SetMaxResults(pageSize)//Rizwan:24-Jun-2011  Commented this line ignore as next line contains same thing as well
                .SetMaxResults(pageSize);
        query.AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false));
        IList<T> result = query.GetExecutableCriteria(this._session).List<T>();
        //count = result.Count;//Rizwan:24-JUN-2011
        var rowCount = query.SetFirstResult(0).GetExecutableCriteria(this._session).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        count = rowCount.Value;
        return result;
        //}
    }
    //public IEnumerable<T> Get(IQuery query, int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    //{
    //    //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
    //    if (pageSize > 0)
    //        query.SetFirstResult(baseIndex)
    //            .SetMaxResults(pageSize)//Rizwan:24-Jun-2011  Commented this line ignore as next line contains same thing as well
    //            .SetMaxResults(pageSize);
    //   // query.AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false));
    //    IList<T> result = query.List<T>();
    //    //count = result.Count;//Rizwan:24-JUN-2011
    //    var rowCount = query.SetFirstResult(0).List().Count;
    //    count = rowCount;
    //    return result;
    //    //}
    //}
    public IEnumerable<T> Get(DetachedCriteria query, int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        /*
        if ((!string.IsNullOrEmpty(sortFieldName))&& sortFieldName != "LeaveTypeId")
        {
            query.SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)//Rizwan:24-Jun-2011  Commented this line ignore as next line contains same thing as well
                .SetMaxResults(pageSize)
                .AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending));

            IList<T> result = query.GetExecutableCriteria(this._session).List<T>();
            //count = result.Count;//Rizwan:24-JUN-2011
            var rowCount = query.SetFirstResult(0).GetExecutableCriteria(this._session).SetProjection(Projections.RowCount()).FutureValue<Int32>();
            count = rowCount.Value;
             return result;

        }
        else
        {
        */

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            query.SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)//Rizwan:24-Jun-2011  Commented this line ignore as next line contains same thing as well
                .SetMaxResults(pageSize);
        //query.AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false));
        IList<T> result = query.GetExecutableCriteria(this._session).List<T>();
        //count = result.Count;//Rizwan:24-JUN-2011
        var rowCount = query.SetFirstResult(0).GetExecutableCriteria(this._session).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        count = rowCount.Value;
        return result;
        //}
    }
    public IEnumerable<T> GetByCrit(DetachedCriteria query, int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        if (pageSize > 0)
            query.SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)//Rizwan:24-Jun-2011  Commented this line ignore as next line contains same thing as well
                .SetMaxResults(pageSize);
        query.AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false));
        IList<T> result = query.GetExecutableCriteria(this._session).List<T>();
        //count = result.Count;//Rizwan:24-JUN-2011
        var rowCount = query.SetFirstResult(0).GetExecutableCriteria(this._session).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        count = rowCount.Value;
        return result;
        //}
    }
    public IEnumerable<T> Get(string fieldName, object fieldValue)
    {
        IEnumerable<T> list = null;

        if (fieldValue == null || fieldValue == DBNull.Value)
            list = (IEnumerable<T>)_session.CreateCriteria(typeof(T))
                        .Add(Expression.IsNull(fieldName))
                        .List<T>();
        else
            list = (IEnumerable<T>)_session.CreateCriteria(typeof(T))
                    .Add(Expression.Eq(fieldName, fieldValue))
                    .List<T>();

        return list;
    }

    public IEnumerable<T> In(string fieldName, object[] values)
    {
        IEnumerable<T> list = null;

        //DetachedCriteria crit = DetachedCriteria.For<T>();
        //list = crit.Add(Expression.In(fieldName, list.ToArray())).GetExecutableCriteria(this._session).List<T>();

        list = (IEnumerable<T>)_session.CreateCriteria(typeof(T))
                .Add(Expression.In(fieldName, values))
                .List<T>();

        return list;
    }

    public IEnumerable<T> Get(string fieldName, object fieldValue, int pageIndex, int pageSize, out int count)
    {
        /*
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        IEnumerable<T> results = null;

        if (fieldValue == null || fieldValue == DBNull.Value)
        {
            results = _session.CreateCriteria(typeof(T))
                 .Add(Expression.IsNull(fieldName))
                 .SetFirstResult(pageIndex * pageSize)
                 .SetMaxResults(pageSize)
                 .List<T>();
        }
        else
        {
            results = _session.CreateCriteria(typeof(T))
                 .Add(Expression.Eq(fieldName, fieldValue))
                 .SetFirstResult(pageIndex * pageSize)
                 .SetMaxResults(pageSize)
                 .List<T>();
        }

        count = rowCount.Value;

        return results;
        */

        //Rizwan:27-Jan-2011    Following line using the code of another overloaded method to take advantage of OOP. That's why commented the above older code that was the original code of this method
        return Get(fieldName, fieldValue, pageIndex, pageSize, out count, null, SortDirection.Ascending);
    }
    public IEnumerable<T> Get(string fieldName, object fieldValue, int baseIndex, short pageSize, out int count)
    {
        return Get(fieldName, fieldValue, baseIndex, pageSize, out count, null, SortDirection.Ascending);
    }

    public IEnumerable<T> Get(string fieldName, object fieldValue, int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        /*
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        IEnumerable<T> results = null;

        if (fieldValue == null || fieldValue == DBNull.Value)
        {
            results = _session.CreateCriteria(typeof(T))
                 .Add(Expression.IsNull(fieldName))
                 .SetFirstResult(pageIndex * pageSize)
                 .SetMaxResults(pageSize)
                 .AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false))
                 .List<T>();
        }
        else
        {
            results = _session.CreateCriteria(typeof(T))
                .Add(Expression.Eq(fieldName, fieldValue))
                .SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)
                .AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false))
                .List<T>();
        }

        */

        //Rizwan:27-Jan-2012    Commented the above old code and re-wrote the code as rowCount was giving count of all rows in the table instead of those which satisfy in the criteria
        IEnumerable<T> results = null;
        NHibernate.Criterion.DetachedCriteria crit = NHibernate.Criterion.DetachedCriteria.For<T>();

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            crit.SetFirstResult((int)pageIndex * pageSize).SetMaxResults(pageSize);

        if (fieldValue == null || fieldValue == DBNull.Value)
            crit.Add(Expression.IsNull(fieldName));
        else
            crit.Add(Expression.Eq(fieldName, fieldValue));

        if (!(string.IsNullOrEmpty(sortFieldName) || string.IsNullOrWhiteSpace(sortFieldName)))
            crit.AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false));

        results = crit.GetExecutableCriteria(this._session).List<T>();

        var rowCount = crit.SetFirstResult(0).GetExecutableCriteria(this._session).SetProjection(Projections.RowCount()).FutureValue<Int32>();

        count = rowCount.Value;

        return results;
    }
    public IEnumerable<T> Get(string fieldName, object fieldValue, int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        IEnumerable<T> results = null;
        NHibernate.Criterion.DetachedCriteria crit = NHibernate.Criterion.DetachedCriteria.For<T>();

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            crit.SetFirstResult(baseIndex).SetMaxResults(pageSize);

        if (fieldValue == null || fieldValue == DBNull.Value)
            crit.Add(Expression.IsNull(fieldName));
        else
            crit.Add(Expression.Eq(fieldName, fieldValue));

        if (!(string.IsNullOrEmpty(sortFieldName) || string.IsNullOrWhiteSpace(sortFieldName)))
            crit.AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false));

        results = crit.GetExecutableCriteria(this._session).List<T>();

        var rowCount = crit.SetFirstResult(0).GetExecutableCriteria(this._session).SetProjection(Projections.RowCount()).FutureValue<Int32>();

        count = rowCount.Value;

        return results;
    }

    public IEnumerable<T> Get(NHibernate.IQuery query)
    {
        return query.List<T>();
    }

    public IEnumerable<T> Get(NHibernate.IQuery query, int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        count = query.List<T>().Count;

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            query.SetFirstResult((int)pageIndex * pageSize)
              .SetMaxResults(pageSize)
              .SetMaxResults(pageSize);

        IList<T> result = query.List<T>();
        //var rowCount = query.SetFirstResult(0).FutureValue<Int32>();
        //count = rowCount.Value;
        return result;
    }
    public IEnumerable<T> Get(NHibernate.IQuery query, int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        count = query.List<T>().Count;

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            query.SetFirstResult(baseIndex)
              .SetMaxResults(pageSize)
              .SetMaxResults(pageSize);

        IList<T> result = query.List<T>();
        return result;
    }

    public IEnumerable<T> GetAll()
    {
        IEnumerable<T> list = null;
        list = (IEnumerable<T>)_session.CreateCriteria(typeof(T)).List<T>();

        return list;
    }

    public IEnumerable<T> GetAll(int pageIndex, int pageSize, out int count)
    {
        /*
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        IEnumerable<T> results = _session.CreateCriteria(typeof(T))
            .SetFirstResult(pageIndex * pageSize)
            .SetMaxResults(pageSize)
            .List<T>();

        count = rowCount.Value;

        return results;
        */

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        ICriteria crit = _session.CreateCriteria(typeof(T));

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            crit.SetFirstResult((int)pageIndex * pageSize).SetMaxResults(pageSize);

        IEnumerable<T> results = crit.List<T>();

        count = rowCount.Value;

        return results;
    }
    public IEnumerable<T> GetAll(int baseIndex, short pageSize, out int count)
    {
        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        ICriteria crit = _session.CreateCriteria(typeof(T));

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            crit.SetFirstResult(baseIndex).SetMaxResults(pageSize);

        IEnumerable<T> results = crit.List<T>();

        count = rowCount.Value;

        return results;
    }
    public IEnumerable<T> GetAll(int pageIndex, int pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        /*
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        IEnumerable<T> results = _session.CreateCriteria(typeof(T))
            .SetFirstResult(pageIndex * pageSize)
            .SetMaxResults(pageSize)
            .AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false))
            .List<T>();

        count = rowCount.Value;

        return results;
        */

        //Rizwan:28-Feb-2012    Commented above code because when pageSize=0 then no pagination.

        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        ICriteria crit = _session.CreateCriteria(typeof(T));

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            crit.SetFirstResult((int)pageIndex * pageSize).SetMaxResults(pageSize);

        crit.AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false));
        IEnumerable<T> results = crit.List<T>();

        count = rowCount.Value;

        return results;
    }
    public IEnumerable<T> GetAll(int baseIndex, short pageSize, out int count, string sortFieldName, SortDirection sortDirection)
    {
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        ICriteria crit = _session.CreateCriteria(typeof(T));

        //if pageSize>0 then do paging, otherwise no paging and all records will be displayed
        if (pageSize > 0)
            crit.SetFirstResult(baseIndex).SetMaxResults(pageSize);

        crit.AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false));
        IEnumerable<T> results = crit.List<T>();

        count = rowCount.Value;

        return results;
    }

    //Rizwan:28-Feb-2012   To allow sorting even paging is not applied
    public IEnumerable<T> GetAll(string sortFieldName, SortDirection sortDirection)
    {
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        IEnumerable<T> results = _session.CreateCriteria(typeof(T))
            .AddOrder(new Order(sortFieldName, sortDirection == SortDirection.Ascending ? true : false))
            .List<T>();

        return results;
    }


    public void SaveOrUpdate(T obj)
    {
        this.SaveOrUpdate(obj, true);
    }

    public void SaveOrUpdate(T obj, bool commit)
    {
        if (commit)
            using (ITransaction transaction = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(obj);
                transaction.Commit();
            }
        else
            _session.SaveOrUpdate(obj);
    }
    public void Save(T obj)
    {
        using (ITransaction transaction = _session.BeginTransaction())
        {
            _session.Save(obj);
            transaction.Commit();
        }
    }
    public void Update(T obj)
    {
        using (ITransaction transaction = _session.BeginTransaction())
        {
            _session.Update(obj);
            transaction.Commit();
        }
    }
    public void SaveOrUpdate(IEnumerable<T> objList)
    {
        this.SaveOrUpdate(objList, true);
    }
    public void SaveOrUpdate(IEnumerable<T> objList, bool commit)
    {
        if (commit)
            using (ITransaction transaction = _session.BeginTransaction())
            {
                foreach (T obj in objList)
                    _session.SaveOrUpdate(obj);
                transaction.Commit();
            }
        else
            foreach (T obj in objList)
                _session.SaveOrUpdate(obj);
    }
    public void Delete(T obj)
    {
        using (ITransaction transaction = _session.BeginTransaction())
        {
            _session.Delete(obj);
            transaction.Commit();
        }
    }
    public void Delete(T obj, bool commit)
    {
        if (commit)
            using (ITransaction transaction = _session.BeginTransaction())
            {
                _session.Delete(obj);
                transaction.Commit();
            }
        else
            _session.Delete(obj);
    }
    public void Delete(IEnumerable<T> objList)
    {
        using (ITransaction transaction = _session.BeginTransaction())
        {
            foreach (T obj in objList)
                _session.Delete(obj);
            transaction.Commit();
        }
    }

    public void Delete(IEnumerable<T> objList, bool commit)
    {
        if (commit)
            using (ITransaction transaction = _session.BeginTransaction())
            {
                foreach (T obj in objList)
                    _session.Delete(obj);
                transaction.Commit();
            }
        else
            foreach (T obj in objList)
                _session.Delete(obj);
    }

    public int GetCount()
    {
        var rowCount = _session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).FutureValue<Int32>();
        return rowCount.Value;
    }


    public bool IsNumber(string Text)
    {
        Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");

        return regex.IsMatch(Text);
    }

    //Added by Saud on Wednesday, October 29, 2014
    // function ll return A , D or S for All Default and Selected rights respectively
    public string Get_Loc_Access_Rights(string EmployeeId, string Connection)
    {
        //    //ISession session = _session.GetSession(EntityMode.Poco);
        //    //using (ITransaction transaction = session.BeginTransaction())
        //    //{

        //    //    // Pdate = da;
        //    //    IDbCommand coom = new OracleCommand();
        //    //    coom.Connection = _session.Connection;
        //    //    transaction.Enlist(coom);
        //    //    coom.CommandType = CommandType.Text;
        //    //    coom.CommandText = string.Format("SELECT F.LOC_ACCESS FROM SEC_USER_PROFILE F WHERE F.EMPLOYEE_ID = '{0}' ", EmployeeId);

        //    //    var result = new OracleParameter("@return_val", OracleDbType.Char);
        //    //    result.Direction = ParameterDirection.ReturnValue;
        //    //    coom.Parameters.Add(result);
        //    //    object Access_Rights;
        //    //    Access_Rights = coom.ExecuteScalar();
        //    //    if (Access_Rights.ToString() != null)
        //    //        return Access_Rights.ToString();
        //    //    else
        //    //        return "A";


        //    //}
        //    OracleConnection con = new OracleConnection(Connection);
        //    con.Open();
        //    OracleCommand cmd = con.CreateCommand();
        //    cmd.CommandType = CommandType.Text;
        //    // cmd.CommandText =string.Format("select encrypt_text('{0}','{1}') from dual", MonthName);
        //    cmd.CommandText = string.Format("SELECT F.LOC_ACCESS FROM SEC_USER_PROFILE F WHERE F.EMPLOYEE_ID = '{0}' ", EmployeeId);
        //    var result = new OracleParameter("@return_val", OracleDbType.Varchar2);
        //    result.Direction = ParameterDirection.ReturnValue;

        //    string Access_Rights;
        //    Access_Rights = (string)cmd.ExecuteScalar();
        //    //sMonthName;

        //    con.Close();
        //    con.Dispose();
        //    if (Access_Rights!= null)
        //        return Access_Rights.ToString();
        //    else
        return "A";
    }
    //End by Saud
}
}