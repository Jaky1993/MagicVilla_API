using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly ApplicationDbContext _db;
        //DbSet<T> is a class in Entity Framework that represents a collection of entities of a specific type.
        //It is typically used to perform CRUD (Create, Read, Update, Delete) operations against the database.
        //Each DbSet<T> corresponds to a table in the database, and each entity in the DbSet<T> represents a row in that table.
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            //Create DbSet Entity can be used to query and save instance of T
            this.dbSet = _db.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        //The Func delegate represent methods that return a value in this case bool and take from zero to sixteen parameters -> Func<T, bool>
        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            //IQueryable<T> is a powerful interface in C# that provides functionality for querying data from a variety of data sources.
            //It allows you to write queries using LINQ (Language-Integrated Query) that can be translated into the native
            //query language of the data source, like SQL for a database.

            IQueryable<T> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking(); //not persist the change to the database
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //RemoveEmptyEntries -> Remove array element that contain empty string from the result
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.FirstOrDefaultAsync();
        }        

        //l'output del filtro sarà un boolean
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            /*
            IQueryable<T> is an interface in C# that represents a queryable data source,
            which is used primarily for querying data from a database using LINQ (Language-Integrated Query).
            It is part of the System.Linq namespace and is typically used with Entity Framework or other 
            ORM (Object-Relational Mapping) tools.  
            */

            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
