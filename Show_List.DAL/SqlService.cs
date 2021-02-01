using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using System.Text;
using System.Globalization;
using System.Collections.Specialized;

namespace Show_List.DAL
{
    public class SqlService
    {
        #region Protected Member Variables
        protected string _connectionString = string.Empty;
        protected SqlParameterCollection _parameterCollection;
        protected ArrayList _parameters = new ArrayList();
        protected bool _isSingleRow = false;
        protected bool _convertEmptyValuesToDbNull = true;
        protected bool _convertMinValuesToDbNull = true;
        protected bool _convertMaxValuesToDbNull = false;
        protected bool _autoCloseConnection = true;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected int _commandTimeout = 5000;
        #endregion Protected Member Variables

        #region Contructors
        public SqlService()
        {
            _connectionString = ConfigurationSettings.AppSettings["Show_List_DB"];
        }

        public SqlService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlService(string server, string database, string user, string password)
        {
            this.ConnectionString = "Server=" + server + ";Database=" + database + ";User ID=" + user + ";Password=" + password + ";";
        }

        public SqlService(string server, string database)
        {
            this.ConnectionString = "Server=" + server + ";Database=" + database + ";Integrated Security=true;";
        }

        public SqlService(SqlConnection connection)
        {
            this.Connection = connection;
            this.AutoCloseConnection = false;
        }
        #endregion Contructors

        #region Properties
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

        public bool IsSingleRow
        {
            get
            {
                return _isSingleRow;
            }
            set
            {
                _isSingleRow = value;
            }
        }

        public bool AutoCloseConnection
        {
            get
            {
                return _autoCloseConnection;
            }
            set
            {
                _autoCloseConnection = value;
            }
        }

        public SqlConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
                this.ConnectionString = _connection.ConnectionString;
            }
        }

        public SqlTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
            }
        }

        public bool ConvertEmptyValuesToDbNull
        {
            get
            {
                return _convertEmptyValuesToDbNull;
            }
            set
            {
                _convertEmptyValuesToDbNull = value;
            }
        }

        public bool ConvertMinValuesToDbNull
        {
            get
            {
                return _convertMinValuesToDbNull;
            }
            set
            {
                _convertMinValuesToDbNull = value;
            }
        }

        public bool ConvertMaxValuesToDbNull
        {
            get
            {
                return _convertMaxValuesToDbNull;
            }
            set
            {
                _convertMaxValuesToDbNull = value;
            }
        }

        public SqlParameterCollection Parameters
        {
            get
            {
                return _parameterCollection;
            }
        }

        public int ReturnValue
        {
            get
            {
                if (_parameterCollection.Contains("@ReturnValue"))
                {
                    return (int)_parameterCollection["@ReturnValue"].Value;
                }
                else
                {
                    throw new Exception("You must call the AddReturnValueParameter method before executing your request.");
                }
            }
        }
        #endregion Properties

        #region Execute Methods
        public void ExecuteSql(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = sql;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.Text;
            this.CopyParameters(cmd);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();
        }

        public SqlDataReader ExecuteSqlReader(string sql)
        {
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = sql;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.Text;
            this.CopyParameters(cmd);

            CommandBehavior behavior = CommandBehavior.Default;

            if (this.AutoCloseConnection) behavior = behavior | CommandBehavior.CloseConnection;
            if (_isSingleRow) behavior = behavior | CommandBehavior.SingleRow;

            reader = cmd.ExecuteReader(behavior);
            cmd.Dispose();

            return reader;
        }

        public XmlReader ExecuteSqlXmlReader(string sql)
        {
            XmlReader reader;
            SqlCommand cmd = new SqlCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = sql;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.Text;

            reader = cmd.ExecuteXmlReader();
            cmd.Dispose();

            return reader;
        }

        public DataSet ExecuteSqlDataSet(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            this.Connect();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            this.CopyParameters(cmd);
            da.SelectCommand = cmd;

            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public DataSet ExecuteSqlDataSet(SqlCommand cmd)
        {
            this.Connect();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            //cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            this.CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public DataSet ExecuteSqlDataSet(string sql, string tableName)
        {
            SqlCommand cmd = new SqlCommand();
            this.Connect();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            da.SelectCommand = cmd;

            da.Fill(ds, tableName);
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public void ExecuteSqlDataSet(ref DataSet dataSet, string sql, string tableName)
        {
            SqlCommand cmd = new SqlCommand();
            this.Connect();
            SqlDataAdapter da = new SqlDataAdapter();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            da.SelectCommand = cmd;

            da.Fill(dataSet, tableName);
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();
        }
        public DataSet ExecuteSPDataSet(SqlCommand cmd)
        {
            // SqlCommand cmd = new SqlCommand();
            this.Connect();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            // cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(ds);

            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public DataSet ExecuteSPDataSet(string procedureName)
        {
            SqlCommand cmd = new SqlCommand();
            this.Connect();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(ds);

            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public DataSet ExecuteSPDataSet(string procedureName, string tableName)
        {
            SqlCommand cmd = new SqlCommand();
            this.Connect();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(ds, tableName);

            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public void ExecuteSPDataSet(ref DataSet dataSet, string procedureName, string tableName)
        {
            SqlCommand cmd = new SqlCommand();
            this.Connect();
            SqlDataAdapter da = new SqlDataAdapter();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(dataSet, tableName);

            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();
        }

        public void ExecuteSP(string procedureName)
        {
            SqlCommand cmd = new SqlCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            cmd.ExecuteNonQuery();

            _parameterCollection = cmd.Parameters;
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();
        }

        public SqlDataReader ExecuteSPReader(string procedureName)
        {
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            CommandBehavior behavior = CommandBehavior.Default;

            if (this.AutoCloseConnection) behavior = behavior | CommandBehavior.CloseConnection;
            if (_isSingleRow) behavior = behavior | CommandBehavior.SingleRow;

            reader = cmd.ExecuteReader(behavior);

            _parameterCollection = cmd.Parameters;
            cmd.Dispose();

            return reader;
        }

        public XmlReader ExecuteSPXmlReader(string procedureName)
        {
            XmlReader reader;
            SqlCommand cmd = new SqlCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            reader = cmd.ExecuteXmlReader();

            _parameterCollection = cmd.Parameters;
            cmd.Dispose();

            return reader;
        }
        #endregion Execute Methods

        #region AddParameter
        public SqlParameter AddParameter(string name, SqlDbType type, object value)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, bool convertZeroToDBNull)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Value = this.PrepareSqlValue(value, convertZeroToDBNull);

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, DbType type, object value, bool convertZeroToDBNull)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.DbType = type;
            prm.Value = this.PrepareSqlValue(value, convertZeroToDBNull);

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, int size)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Size = size;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, ParameterDirection direction)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = direction;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddParameter(string name, SqlDbType type, object value, int size, ParameterDirection direction)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = direction;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Size = size;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }

        public void AddParameter(SqlParameter parameter)
        {
            _parameters.Add(parameter);
        }
        #endregion AddParameter

        #region Specialized AddParameter Methods
        public SqlParameter AddOutputParameter(string name, SqlDbType type)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Output;
            prm.ParameterName = name;
            prm.SqlDbType = type;

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddOutputParameter(string name, SqlDbType type, int size)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Output;
            prm.ParameterName = name;
            prm.SqlDbType = type;
            prm.Size = size;

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddReturnValueParameter()
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.ReturnValue;
            prm.ParameterName = "@ReturnValue";
            prm.SqlDbType = SqlDbType.Int;

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddStreamParameter(string name, Stream value)
        {
            return this.AddStreamParameter(name, value, SqlDbType.Image);
        }

        public SqlParameter AddStreamParameter(string name, Stream value, SqlDbType type)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = type;

            value.Position = 0;
            byte[] data = new byte[value.Length];
            value.Read(data, 0, (int)value.Length);
            prm.Value = data;

            _parameters.Add(prm);

            return prm;
        }

        public SqlParameter AddTextParameter(string name, string value)
        {
            SqlParameter prm = new SqlParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.SqlDbType = SqlDbType.Text;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }
        #endregion Specialized AddParameter Methods

        #region Private Methods
        private void CopyParameters(SqlCommand command)
        {
            for (int i = 0; i < _parameters.Count; i++)
            {
                command.Parameters.Add(_parameters[i]);
            }
        }

        private object PrepareSqlValue(object value)
        {
            return this.PrepareSqlValue(value, false);
        }

        private object PrepareSqlValue(object value, bool convertZeroToDBNull)
        {
            if (value is string)
            {
                if (this.ConvertEmptyValuesToDbNull && (string)value == string.Empty)
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Guid)
            {
                if (this.ConvertEmptyValuesToDbNull && (Guid)value == Guid.Empty)
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is DateTime)
            {
                if ((this.ConvertMinValuesToDbNull && (DateTime)value == DateTime.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (DateTime)value == DateTime.MaxValue))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is short)
            {
                if ((this.ConvertMinValuesToDbNull && (short)value == short.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (short)value == short.MaxValue)
                    || (convertZeroToDBNull && (short)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is int)
            {
                if ((this.ConvertMinValuesToDbNull && (int)value == int.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (int)value == int.MaxValue)
                    || (convertZeroToDBNull && (int)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is long)
            {
                if ((this.ConvertMinValuesToDbNull && (long)value == long.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (long)value == long.MaxValue)
                    || (convertZeroToDBNull && (long)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is float)
            {
                if ((this.ConvertMinValuesToDbNull && (float)value == float.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (float)value == float.MaxValue)
                    || (convertZeroToDBNull && (float)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is double)
            {
                if ((this.ConvertMinValuesToDbNull && (double)value == double.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (double)value == double.MaxValue)
                    || (convertZeroToDBNull && (double)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is decimal)
            {
                if ((this.ConvertMinValuesToDbNull && (decimal)value == decimal.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (decimal)value == decimal.MaxValue)
                    || (convertZeroToDBNull && (decimal)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return value;
            }
        }

        private Hashtable ParseConfigString(string config)
        {
            Hashtable attributes = new Hashtable(10, new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
            string[] keyValuePairs = config.Split(';');
            for (int i = 0; i < keyValuePairs.Length; i++)
            {
                string[] keyValuePair = keyValuePairs[i].Split('=');
                if (keyValuePair.Length == 2)
                {
                    attributes.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim());
                }
                else
                {
                    attributes.Add(keyValuePairs[i].Trim(), null);
                }
            }

            return attributes;
        }
        #endregion Private Methods

        #region Public Methods
        public void Connect()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
            }
            else
            {
                if (_connectionString != string.Empty)
                {
                    StringCollection initKeys = new StringCollection();
                    initKeys.AddRange(new string[] { "ARITHABORT", "ANSI_NULLS", "ANSI_WARNINGS", "ARITHIGNORE", "ANSI_DEFAULTS", "ANSI_NULL_DFLT_OFF", "ANSI_NULL_DFLT_ON", "ANSI_PADDING", "ANSI_WARNINGS" });

                    StringBuilder initStatements = new StringBuilder();
                    StringBuilder connectionString = new StringBuilder();

                    Hashtable attribs = this.ParseConfigString(_connectionString);
                    foreach (string key in attribs.Keys)
                    {
                        if (initKeys.Contains(key.Trim().ToUpper()))
                        {
                            initStatements.AppendFormat("SET {0} {1};", key, attribs[key]);
                        }
                        else if (key.Trim().Length > 0)
                        {
                            connectionString.AppendFormat("{0}={1};", key, attribs[key]);
                        }
                    }

                    _connection = new SqlConnection(connectionString.ToString());
                    _connection.Open();

                    if (initStatements.Length > 0)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandTimeout = this.CommandTimeout;
                        cmd.CommandText = initStatements.ToString();
                        cmd.Connection = _connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                else
                {
                    throw new InvalidOperationException("You must set a connection object or specify a connection string before calling Connect.");
                }
            }
        }

        public void Disconnect()
        {
            if ((_connection != null) && (_connection.State != ConnectionState.Closed))
            {
                _connection.Close();
            }

            if (_connection != null) _connection.Dispose();
            if (_transaction != null) _transaction.Dispose();

            _transaction = null;
            _connection = null;
        }

        public void BeginTransaction()
        {
            if (_connection != null)
            {
                _transaction = _connection.BeginTransaction();
            }
            else
            {
                throw new InvalidOperationException("You must have a valid connection object before calling BeginTransaction.");
            }
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Commit();
                }
                catch (Exception)
                {
                    // TODO: We need to handle this situation.  Maybe just write a log entry or something.
                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException("You must call BeginTransaction before calling CommitTransaction.");
            }
        }

        public void RollbackTransaction()
        {

            if (_transaction != null)
            {
                try
                {
                    _transaction.Rollback();
                }
                catch (Exception)
                {
                    // TODO: We need to handle this situation.  Maybe just write a log entry or something.
                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException("You must call BeginTransaction before calling RollbackTransaction.");
            }
        }

        public void Reset()
        {
            if (_parameters != null)
            {
                _parameters.Clear();
            }

            if (_parameterCollection != null)
            {
                _parameterCollection = null;
            }
        }
        #endregion
    }
}
