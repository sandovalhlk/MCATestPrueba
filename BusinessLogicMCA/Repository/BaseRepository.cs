using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using System.Data.Entity.Validation;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using DataLayerMCA;

namespace Repository
{
    /// <summary>
    /// Implementacion de un repositorio generico
    /// </summary>
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private ContabilidadMCA context = null;
        private Collection _parameters;

        /// <summary>
        /// Unidad de trabajo expuesta a la aplicacion
        /// </summary>
        /// <param name="unitofwork">The current Unit of Work</param>
        public ContabilidadMCA ContabilidadMCA
        {
            get { return this.context; }
        }

        public Collection Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseRepository()
        {
            this.context = new ContabilidadMCA();
            this.Parameters = new Collection();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitofwork">Unidad de trabajo</param>
        public BaseRepository(ContabilidadMCA context)
        {
            this.context = context;
            this.Parameters = new Collection();
        }

        /// <summary>
        /// Metodo generico para añadir una entidad al contexto de trabajo
        /// </summary>
        /// <param name="entity">La entidad para añadir</param>
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        /// <summary>
        ///  Borra objetos de la base de datos en base a una expresión de filtrado
        /// </summary>
        /// <param name="predicate">Expresión de filtrado</param>
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var entities = context.Set<T>().Where(predicate).ToList();
            entities.ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();
        }

        /// <summary>
        /// Metodo generico para eliminar una entidad del contexto de trabajo pasandole la entidad
        /// </summary>
        /// <param name="entityToDelete">Entidad a eliminar</param>
        public void Delete(T entityToDelete)
        {
            context.Entry(entityToDelete).State = EntityState.Deleted;
            context.SaveChanges();
        }

        /// <summary>
        /// Metodo generico para eliminar una entidad del contexto de trabajo
        /// </summary>
        /// <param name="id">La identidad de la entidad</param>
        public void Delete(object id)
        {
            T entityToDelete = context.Set<T>().Find(id);
            Delete(entityToDelete);
            context.SaveChanges();
        }

        /// <summary>
        /// Ejecutar un command en la base de datos 
        /// </summary>
        /// <param name="sqlCommand">La query</param>
        /// <param name="parameters">Los parametros</param>
        /// <returns>El sql code que devuelve la query</returns>
        public int ExecuteInDatabaseByQuery(string sqlCommand, params object[] parameters)
        {
            //return this.context.ExecuteCommand(sqlCommand, parameters);
            return this.context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        /// <summary>
        /// Ejecutar un command en la base de datos 
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public int ExecuteInDatabaseByQuery(string sqlCommand)
        {
            var parameters = new List<SqlParameter>();
            foreach (StoredProcedureParameter item in this.Parameters)
            {
                parameters.Add(new SqlParameter(item.Parameter, item.Value));
            }
            // sqlCommand = GenerateSqlProcedureWithParameter(sqlCommand);
            return this.context.Database.ExecuteSqlCommand(sqlCommand, parameters.ToArray());
        }

        /// <summary>
        /// Devuelve un conjunto de registros filtrado por la expresión indicada en el parámetro
        /// </summary>
        /// <param name="predicate">Expresión de filtrado</param>
        /// <returns></returns>
        public List<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return (List<T>)context.Set<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// Devuelve el primer registro que satisfaga la expresión de búsqueda pasada como parámetro
        /// </summary>
        /// <param name="predicate">Expresión de búsqueda</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<T> Filter(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            DbQuery<T> query = context.Set<T>();
            includelist.ForEach(x => query = query.Include(x));

            return (List<T>)query.Where(predicate).ToList();
        }

        /// <summary>
        /// Metodo generico para recuperar una coleccion de entidades
        /// </summary>
        /// <param name="filter">Expresion para filtrar las entidades</param>
        /// <param name="orderBy">Orden en el que se quiere recuperar las entidades</param>
        /// <param name="includeProperties">Propiedades de Navegacion a incluir</param>
        /// <returns>Un listado de objetos de la entidadgenerica</returns>
        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Metodo generico para recuperar una lista de entidades
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            return (List<T>)context.Set<T>().ToList();
        }

        /// <summary>
        /// Metodo generico para recuperar una lista de entidades incluyendo algunas expresiones
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<T> GetAll(List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            DbQuery<T> query = context.Set<T>();
            includelist.ForEach(x => query = query.Include(x));

            return (List<T>)query.ToList();
        }

        /// <summary>
        /// Metodo generico para recuperar una entidad a partir de su identidad
        /// </summary>
        /// <param name="id">La identidad de la entidad</param>
        /// <returns>La entidad</returns>
        public T GetByID(object id)
        {
            return context.Set<T>().Find(id);
        }

        /// <summary>
        /// Ejecutar una query en la base de datos
        /// </summary>
        /// <param name="sqlQuery">La Query</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>Listado de entidades que recupera la query</returns>
        public IEnumerable<T> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters)
        {
            //return this.context.ExecuteQuery<T>(sqlQuery, parameters);
            return this.context.Database.SqlQuery<T>(sqlQuery, parameters);
        }

        /// <summary>
        ///  Ejecutar una query en la base de datos con parametros agregados a la propiedad
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public IEnumerable<T> GetFromDatabaseWithQuery(string sqlQuery)
        {
            var parameters = new List<SqlParameter>();
            foreach (StoredProcedureParameter item in this.Parameters)
            {
                parameters.Add(new SqlParameter(item.Parameter, item.Value));
            }
            sqlQuery = GenerateSqlProcedureWithParameter(sqlQuery);
            return this.context.Database.SqlQuery<T>(sqlQuery, parameters.ToArray());
        }

        /// <summary>
        /// Ejecutar una query en la base de datos con parametros agregados a la propiedad
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public List<string> GetSelectDatabaseWithQuery(string sqlQuery)
        {
            var parameters = new List<SqlParameter>();
            foreach (StoredProcedureParameter item in this.Parameters)
            {
                parameters.Add(new SqlParameter(item.Parameter, item.Value));
            }
            //sqlQuery = GenerateSqlProcedureWithParameter(sqlQuery);
            return context.Database.SqlQuery<string>(sqlQuery, parameters.ToArray()).ToList();
        }

        /// <summary>
        /// Implementacion generica de un metodo para paginar
        /// </summary>
        /// <typeparam name="TKey">Clave para el orden</typeparam>
        /// <param name="pageIndex">Indice de la pagina a recuperar</param>/// 
        /// <param name="pageCount">Numero de entidades a recuperar</param>
        /// <param name="orderByExpression">Order expression</param>
        /// <param name="ascending">Orden ascendente o descendente</param>
        /// <returns>Listado de todas las entidades qeu cumplan los requisitos</returns>
        public IEnumerable<T> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderByExpression, bool ascending = true)
        {
            if (pageIndex < 1) { pageIndex = 1; }

            if (orderByExpression == (Expression<Func<T, TKey>>)null)
                throw new ArgumentNullException();

            return (ascending)
                            ?
                        context.Set<T>().OrderBy(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList()
                            :
                        context.Set<T>().OrderByDescending(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList();
        }

        /// <summary>
        /// Implementacion generica de un metodo para paginar
        /// </summary>
        /// <typeparam name="TKey">Clave para el orden</typeparam>
        /// <param name="pageIndex">Indice de la pagina a recuperar</param>/// 
        /// <param name="pageCount">Numero de entidades a recuperar</param>
        /// <param name="orderByExpression">La expresion para establecer el orden</param>
        /// <param name="ascending">Si el orden es ascendente o descendente</param>
        /// <param name="includeProperties">Includes</param>
        /// <returns>Listado con todas las entidades que cumplan los criterios</returns>        
        public IEnumerable<T> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderByExpression, bool ascending = true, string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (pageIndex < 1) { pageIndex = 1; }

            if (orderByExpression == (Expression<Func<T, TKey>>)null)
                throw new ArgumentNullException();

            return (ascending)
                            ?
                        query.OrderBy(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList()
                            :
                        query.OrderByDescending(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList();
        }

        /// <summary>
        /// Devuelve un conjunto de registros filtrado por la expresión indicada en el parámetro
        /// </summary>
        /// <param name="predicate">Expresión de filtrado</param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Devuelve un conjunto de registros filtrado por la expresión indicada en el parámetro y Propiedades de Navegacion a incluir
        /// </summary>
        /// <param name="predicate">Expresión de filtrado</param>
        /// <param name="includes">Propiedades de Navegacion a incluir</param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            DbQuery<T> query = context.Set<T>();

            includelist.ForEach(x => query = query.Include(x));

            return query.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Metodo generico para modificar una entidad en el contexto de trabajo
        /// </summary>
        /// <param name="entityToUpdate">La entidad a modificar</param>
        public void Update(T entityToUpdate)
        {
            context.Entry(entityToUpdate).State = EntityState.Modified;
            context.SaveChanges();
        }

        /// <summary>
        /// Agregar Parametros para ejecutar un procedimiento Almacenado
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public void AddParameter(string parameter, object value)
        {
            this.Parameters.Add(new StoredProcedureParameter((parameter[0].ToString().Trim() == "@" ? parameter : "@" + parameter), value));
        }

        /// <summary>
        /// Guardar Cambios a la Base de Datos
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        /// <summary>
        /// Concatenar parametros segun lista
        /// </summary>
        /// <param name="sqlQuery">Nombre del procedimiento almacenado</param>
        /// <returns></returns>
        private string GenerateSqlProcedureWithParameter(string sqlQuery)
        {
            sqlQuery = sqlQuery + " ";
            foreach (StoredProcedureParameter par in this.Parameters)
            {
                if (par.Value == null || Information.IsDBNull(par.Value))
                {
                    sqlQuery = sqlQuery + Constants.vbNewLine + Strings.Format("{0} \\b", par.Parameter) + ",";
                }
                else if (par.Value is System.DateTime)
                {
                    sqlQuery = sqlQuery + Constants.vbNewLine + "" + Strings.Format("{0} \\b", par.Parameter) + "" + ",";
                }
                else if (par.Value is TimeSpan)
                {
                    sqlQuery = sqlQuery + Constants.vbNewLine + "" + Strings.Format("{0} \\b", par.Parameter) + "" + ",";
                }
                else if (par.Value is double | par.Value is decimal | par.Value is float)
                {
                    sqlQuery = sqlQuery + Constants.vbNewLine + "" + Strings.Replace(Strings.Format("{0} \\b", par.Parameter), ",", ".") + "" + ",";
                }
                else if (par.Value is int | par.Value is uint | par.Value is long | par.Value is ulong)
                {
                    sqlQuery = sqlQuery + Constants.vbNewLine + Strings.Format("{0}", Strings.Replace(Strings.Format("{0} \\b", par.Parameter), ",", ".")) + ",";
                }
                else {
                    sqlQuery = sqlQuery + Constants.vbNewLine + "" + Strings.Format("{0} \\b", par.Parameter) + "" + ",";
                }
            }
            if (sqlQuery != null)
            {
                if (sqlQuery[sqlQuery.Length - 1].ToString().Trim() == ",")
                    sqlQuery = sqlQuery.Remove(sqlQuery.Trim().Length - 1);
            }

            return sqlQuery + Constants.vbNewLine;
        }

        /// <summary>
        /// Proxy Creacion 
        /// </summary>
        public void ProxyCreationEnabled(bool value = false)
        {
            context.Configuration.ProxyCreationEnabled = value;
        }

        /// <summary>
        /// Geerar SQL Query con parametros, estableciendo los datos segun parametro
        /// </summary>
        /// <param name="sqlQuery">Query</param>
        /// <returns></returns>
        private string GenerateSqlQuery(string sqlQuery)
        {
            sqlQuery = sqlQuery.Replace("SET", "SET" + Constants.vbNewLine);
            sqlQuery = sqlQuery.Replace("WHERE", Constants.vbNewLine + "WHERE");
            sqlQuery = sqlQuery.Replace("GROUP BY", Constants.vbNewLine + "GROUP BY");
            sqlQuery = sqlQuery.Replace("ORDER BY", Constants.vbNewLine + "ORDER BY");
            sqlQuery = sqlQuery.Replace("INNER JOIN", Constants.vbNewLine + "INNER JOIN");
            sqlQuery = sqlQuery.Replace("LEFT JOIN", Constants.vbNewLine + "LEFT JOIN");
            sqlQuery = sqlQuery.Replace("RIGHT JOIN", Constants.vbNewLine + "RIGHT JOIN");
            if (sqlQuery.Contains("UNION ALL"))
            {
                sqlQuery = sqlQuery.Replace("UNION ALL", Constants.vbNewLine + "UNION ALL" + Constants.vbNewLine);
            }
            else if (sqlQuery.Contains("UNION DISTINCT"))
            {
                sqlQuery = sqlQuery.Replace("UNION DISTINCT", Constants.vbNewLine + "UNION DISTINCT" + Constants.vbNewLine);
            }
            else {
                sqlQuery = sqlQuery.Replace("UNION", Constants.vbNewLine + "UNION" + Constants.vbNewLine);
            }

            foreach (StoredProcedureParameter par in this.Parameters)
            {
                if (par.Value == null || Information.IsDBNull(par.Value))
                {
                    sqlQuery = System.Text.RegularExpressions.Regex.Replace(sqlQuery, par.Parameter + "\\b", "NULL");
                }
                else if (par.Value is System.DateTime)
                {
                    sqlQuery = System.Text.RegularExpressions.Regex.Replace(sqlQuery, par.Parameter + "\\b", "'" + Strings.Format(par.Value, "yyyy-MM-dd HH:mm:ss") + "'");
                }
                else if (par.Value is TimeSpan)
                {
                    sqlQuery = System.Text.RegularExpressions.Regex.Replace(sqlQuery, par.Parameter + "\\b", "'" + par.Value + "'");
                }
                else if (par.Value is double | par.Value is decimal | par.Value is float)
                {
                    sqlQuery = System.Text.RegularExpressions.Regex.Replace(sqlQuery, par.Parameter + "\\b", Strings.Replace(par.Value.ToString(), ",", "."));
                }
                else if (par.Value is int | par.Value is uint | par.Value is long | par.Value is ulong)
                {
                    sqlQuery = System.Text.RegularExpressions.Regex.Replace(sqlQuery, par.Parameter + "\\b", par.Value.ToString());
                }
                else {
                    sqlQuery = System.Text.RegularExpressions.Regex.Replace(sqlQuery, par.Parameter + "\\b", "'" + Convert.ToString(par.Value) + "'");
                }
            }
            return sqlQuery + Constants.vbNewLine + "GO";
        }

        /// <summary>
        /// Metodo generico para recuperar una entidad a partir de su identidad
        /// </summary>
        /// <param name="id">La identidad de la entidad</param>
        /// <returns>La entidad</returns>
        public Task<T> GetByIDAsync(object id)
        {
            return context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Metodo generico para recuperar una lista de entidades
        /// </summary>
        /// <returns></returns>
        public Task<List<T>> GetAllAsync()
        {
            return context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Metodo generico para recuperar una lista de entidades incluyendo algunas expresiones
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<List<T>> GetAllAsync(List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            DbQuery<T> query = context.Set<T>();
            includelist.ForEach(x => query = query.Include(x));

            return query.ToListAsync();
        }

        /// <summary>
        /// Devuelve un conjunto de registros filtrado por la expresión indicada en el parámetro
        /// </summary>
        /// <param name="predicate">Expresión de filtrado</param>
        /// <returns></returns>
        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Devuelve un conjunto de registros filtrado por la expresión indicada en el parámetro y Propiedades de Navegacion a incluir
        /// </summary>
        /// <param name="predicate">Expresión de filtrado</param>
        /// <param name="includes">Propiedades de Navegacion a incluir</param>
        /// <returns></returns>
        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            DbQuery<T> query = context.Set<T>();

            includelist.ForEach(x => query = query.Include(x));

            return query.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Devuelve un conjunto de registros filtrado por la expresión indicada en el parámetro
        /// </summary>
        /// <param name="predicate">Expresión de filtrado</param>
        /// <returns></returns>
        public Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Devuelve el primer registro que satisfaga la expresión de búsqueda pasada como parámetro
        /// </summary>
        /// <param name="predicate">Expresión de búsqueda</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            DbQuery<T> query = context.Set<T>();
            includelist.ForEach(x => query = query.Include(x));

            return query.Where(predicate).ToListAsync();
        }


        /// <summary>
        /// Ejecutar un command en la base de datos 
        /// </summary>
        /// <param name="sqlCommand">La query</param>
        /// <param name="parameters">Los parametros</param>
        /// <returns>El sql code que devuelve la query</returns>
        public Task<int> ExecuteInDatabaseByQueryAsync(string sqlCommand, params object[] parameters)
        {
            //return this.context.ExecuteCommand(sqlCommand, parameters);
            return this.context.Database.ExecuteSqlCommandAsync(sqlCommand, parameters);
        }

        /// <summary>
        /// Ejecutar un command en la base de datos 
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public Task<int> ExecuteInDatabaseByQueryAsync(string sqlCommand)
        {
            var parameters = new List<SqlParameter>();
            foreach (StoredProcedureParameter item in this.Parameters)
            {
                parameters.Add(new SqlParameter(item.Parameter, item.Value));
            }
            // sqlCommand = GenerateSqlProcedureWithParameter(sqlCommand);
            return this.context.Database.ExecuteSqlCommandAsync(sqlCommand, parameters.ToArray());
        }


        /// <summary>
        /// Guardar Cambios a la Base de Datos
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        /// <summary>
        /// Ejecutar una consulta y obtener el resultado en un dataTable
        /// </summary>
        /// <param name="sqlCommand">Consultar</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sqlCommand, params object[] parameters)
        {
            SqlConnection conn = context.Database.Connection as SqlConnection;
            if (conn == null)
            {
                throw new InvalidCastException("SqlConnection is invalid for this database");
            }

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand, conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            foreach (StoredProcedureParameter item in this.Parameters)
            {
                da.SelectCommand.Parameters.Add(new SqlParameter(item.Parameter, item.Value));
            }
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Ejecutar una consulta y obtener el resultado en un dataTable
        /// </summary>
        /// <param name="sqlCommand">Consulta query</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sqlCommand)
        {
            var parameters = new List<SqlParameter>();
            foreach (StoredProcedureParameter item in this.Parameters)
            {
                parameters.Add(new SqlParameter(item.Parameter, item.Value));
            }
            return this.GetDataTable(sqlCommand, parameters.ToArray());
        }
    }
}
