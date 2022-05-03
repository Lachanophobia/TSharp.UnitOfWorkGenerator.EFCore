using System.Text;
using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Templates
{
    internal static partial class BuildTemplate
    {
        public static string BuildISP_CallTemplate(this Template templateISP_Call)
        {
            var template = $@"// Auto-generated code
using Dapper;
using System.Data;

namespace {templateISP_Call.Namespace}
{{
    public partial interface ISP_Call : IDisposable
    {{
        void Dispose();

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        void Execute(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
       
        /// <summary>
        /// Execute a command asynchronously using Task.
        /// </summary>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        Task ExecuteAsync(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <typeparam name=""T""></typeparam>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        /// <returns>IEnumerable of <typeparamref name=""T""/></returns>
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name=""T""></typeparam>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        /// <returns>IEnumerable of <typeparamref name=""T""/></returns>
        Task<IEnumerable<T>> ListAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <typeparam name=""T1""></typeparam>
        /// <typeparam name=""T2""></typeparam>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        /// <returns>Tuple IEnumerable <typeparamref name=""T1""/> and IEnumerable <typeparamref name=""T2""/></returns>
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Execute a command asynchronously using Task that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <typeparam name=""T1""></typeparam>
        /// <typeparam name=""T2""></typeparam>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        /// <returns>Tuple IEnumerable <typeparamref name=""T1""/> and IEnumerable <typeparamref name=""T2""/></returns>
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> ListAsync<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name=""T""></typeparam>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        /// <returns>One record of type <typeparamref name=""T""/></returns>
        T OneRecord<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Executes a query asynchronously using Task
        /// </summary>
        /// <typeparam name=""T""></typeparam>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        /// <returns>One record of type <typeparamref name=""T""/></returns>
        Task<T> OneRecordAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <typeparam name=""T""></typeparam>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        /// <returns>A single record of type <typeparamref name=""T""/></returns>
        T Single<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Execute parameterized SQL asynchronously using Task that selects a single value.
        /// </summary>
        /// <typeparam name=""T""></typeparam>
        /// <param name=""procedureName""></param>
        /// <param name=""param""></param>
        /// <param name=""connection""></param>
        /// <param name=""transaction""></param>
        /// <param name=""commandTimeout""></param>
        /// <returns>A single record of type <typeparamref name=""T""/></returns>
        Task<T> SingleAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
    }}
}}
";

            return template;
        }

        public static string BuildSP_CallTemplate(this Template templateSP_Call)
        {
            var template = $@"// Auto-generated code
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
{templateSP_Call.UsingStatements}

namespace {templateSP_Call.Namespace}
{{
    public partial class SP_Call : ISP_Call
    {{

        private readonly {templateSP_Call.DBContextName} _db;
        private static string connectionString = """";

        public SP_Call({templateSP_Call.DBContextName} db)
        {{
            _db = db;
            connectionString = db.Database.GetDbConnection().ConnectionString;
        }}

        public void Dispose()
        {{
            _db.Dispose();
        }}

        #region asynchronous methods

        /// <inheritdoc/>
        public async Task ExecuteAsync(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            await dbConnection.ExecuteAsync(new CommandDefinition(procedureName, param, transaction, commandTimeout, CommandType.StoredProcedure, CommandFlags.Buffered, cancellationToken));

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}
        }}

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ListAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = await dbConnection
                .QueryAsync<T>(new CommandDefinition(procedureName, param, transaction, commandTimeout, CommandType.StoredProcedure, CommandFlags.Buffered, cancellationToken));


            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}

            return result;
        }}

        /// <inheritdoc/>
        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> ListAsync<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = await connection.QueryMultipleAsync(new CommandDefinition(procedureName, param, transaction, commandTimeout, CommandType.StoredProcedure, CommandFlags.Buffered, cancellationToken));
            var item1 = (await result.ReadAsync<T1>()).ToList();
            var item2 = (await result.ReadAsync<T2>()).ToList();

            if (item1 != null && item2 != null)
            {{
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }}

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }}

        /// <inheritdoc/>
        public async Task<T> OneRecordAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var value = dbConnection.Query<T>(new CommandDefinition(procedureName, param, transaction, commandTimeout, CommandType.StoredProcedure, CommandFlags.Buffered, cancellationToken));
            var result = value.FirstOrDefault();

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}

            return result;
        }}

        /// <inheritdoc/>
        public async Task<T> SingleAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = await connection.ExecuteScalarAsync<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}

            return result;
        }}
        #endregion

        #region synchronous methods
        /// <inheritdoc/>
        public void Execute(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            dbConnection.Execute(procedureName, param, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}
        }}

        /// <inheritdoc/>
        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = dbConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}

            return result;
        }}

        /// <inheritdoc/>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = connection.QueryMultiple(procedureName, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);
            var item1 = result.Read<T1>().ToList();
            var item2 = result.Read<T2>().ToList();

            if (item1 != null && item2 != null)
            {{
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }}

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }}

        /// <inheritdoc/>
        public T OneRecord<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var value = dbConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);
            var result = value.FirstOrDefault();

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}

            return result;
        }}

        /// <inheritdoc/>
        public T Single<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {{
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = connection.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);

            if (connection == null)
            {{
                dbConnection.Close();
                dbConnection.Dispose();
            }}

            return result;
        }}
        #endregion


    }}
}}

";

            
            return template;
        }
    }
}
