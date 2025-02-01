using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
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
            //_db.VillaNumbers.Include(u => u.villa).ToList();
            this.dbSet = _db.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

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

            /*
            includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries): 
            Splits the comma-separated string into an array of property names, ignoring empty entries
            query = query.Include(includeProp): Includes the specified property in the query
            */
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

            /*
            perché non include la proprietà se non uso include?
            La proprietà non viene inclusa automaticamente perché Entity Framework utilizza il caricamento lento ("lazy loading")
            di default.
            Questo significa che le proprietà di navigazione, come le collezioni di entità correlate, non vengono caricate finché
            non vengono esplicitamente richieste.
            Quando utilizzi il metodo Include(), stai indicando a Entity Framework di eseguire un "caricamento ansioso"
            ("eager loading"), che pre-carica le entità correlate insieme all'entità principale in una sola query.
            Senza Include(), Entity Framework eseguirà una query separata per ogni proprietà di navigazione quando viene
            acceduta per la prima volta, il che può risultare in molteplici round-trip al database e quindi in un'inefficienza.
            */

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
