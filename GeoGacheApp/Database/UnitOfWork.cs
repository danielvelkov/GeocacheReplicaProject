﻿using Geocache;
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
        
        public UnitOfWork(GeocachingContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Treasures = new TreasureRepository(_context);
            ChainedTreasures = new ChainedTreasureRepository(_context);
            FoundTreasures = new FoundTreasuresRepostitory(_context);
            TreasureComments = new TreasureCommentsRepository(_context);
            Markers = new MarkersRepository(_context);
        }

        public IUserRepository Users { get; private set; }
        public ITreasureRepository Treasures { get; private set; }
        public IMarkerRepository Markers { get; private set; }
        public IChainedTreasureRepository ChainedTreasures { get; private set; }
        public IFoundTreasuresRepository FoundTreasures { get; private set; }
        public ITreasureCommentsRepository TreasureComments { get; private set; }

        // basically save changes for the actions
        public int Complete()
        {
            try
            {
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
