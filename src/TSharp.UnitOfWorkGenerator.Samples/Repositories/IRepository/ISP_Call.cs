using Dapper;
using System.Data;

namespace TSharp.UnitOfWorkGenerator.Samples.Repositories.IRepository
{
    public partial interface ISP_Call : IDisposable
    {
        void Dispose();

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        void Execute(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
       
        /// <summary>
        /// Execute a command asynchronously using Task.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        Task ExecuteAsync(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>IEnumerable of <typeparamref name="T"/></returns>
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>IEnumerable of <typeparamref name="T"/></returns>
        Task<IEnumerable<T>> ListAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>Tuple IEnumerable <typeparamref name="T1"/> and IEnumerable <typeparamref name="T2"/></returns>
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Execute a command asynchronously using Task that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>Tuple IEnumerable <typeparamref name="T1"/> and IEnumerable <typeparamref name="T2"/></returns>
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> ListAsync<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>One record of type <typeparamref name="T"/></returns>
        T OneRecord<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Executes a query asynchronously using Task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>One record of type <typeparamref name="T"/></returns>
        Task<T> OneRecordAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>A single record of type <typeparamref name="T"/></returns>
        T Single<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Execute parameterized SQL asynchronously using Task that selects a single value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>A single record of type <typeparamref name="T"/></returns>
        Task<T> SingleAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
    }
}