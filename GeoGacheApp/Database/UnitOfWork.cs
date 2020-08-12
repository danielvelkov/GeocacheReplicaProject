using Geocache;
using Geocache.Database.Repositories;
using Geocache.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Database
{
    // Unit of work is used to commit changes to multiple tables in one transaction
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GeocachingContext _context;

        // TODO: could be made for specific repositories so its not that bulky
        // for example most of the cases i use are entities add then X type model with that entity ID 
        public UnitOfWork(GeocachingContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Treasures = new TreasureRepository(_context);
            ChainedTreasures = new ChainedTreasureRepository(_context);
        }

        public IUserRepository Users { get; private set; }
        public ITreasureRepository Treasures { get; private set; }
        public IChainedTreasuresRepository ChainedTreasures { get; private set; }
        // basically save changes for the actions
        public int Complete()
        {
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                return _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
