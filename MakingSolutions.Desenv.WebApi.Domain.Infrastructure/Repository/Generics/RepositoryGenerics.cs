using MakingSolutions.Desenv.WebApi.Domain.Infrastructure.Configuration;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces.Generics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace MakingSolutions.Desenv.WebApi.Domain.Infrastructure.Repository.Generics
{
    public class RepositoryGenerics<T> : IGeneric<T>, IDisposable where T : class
    {
        private readonly DbContextOptions<MyDbContext> _OptionsBuilder;
        public RepositoryGenerics()
        {
            _OptionsBuilder = new DbContextOptions<MyDbContext>();
        }

        public async Task Add(T Objeto)
        {
            using (var context = new MyDbContext(_OptionsBuilder))
            {
                await context.Set<T>().AddAsync(Objeto);
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(T Objeto)
        {
            using (var context = new MyDbContext(_OptionsBuilder))
            {
                context.Set<T>().Remove(Objeto);
                await context.SaveChangesAsync();
            }
        }

        public async Task<T> GetEntityById(int Id)
        {
            using (var context = new MyDbContext(_OptionsBuilder))
            {
                return await context.Set<T>().FindAsync(Id);
            }
        }

        public async Task<List<T>> List()
        {
            using (var context = new MyDbContext(_OptionsBuilder))
            {
                return await context.Set<T>().ToListAsync();
            }
        }

        public async Task Update(T Objeto)
        {
            using (var context = new MyDbContext(_OptionsBuilder))
            {
                context.Set<T>().Update(Objeto);
                await context.SaveChangesAsync();
            }
        }

        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);



        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion
    }

}
