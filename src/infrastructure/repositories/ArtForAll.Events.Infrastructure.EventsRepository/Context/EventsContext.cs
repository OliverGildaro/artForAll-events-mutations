namespace OCP.PortalEvents.Repositories.Context
{
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.Extensions.Logging;

    public class EventsContext : DbContext
    {
        private readonly string connectionString;
        private readonly bool useConsoleLogger; //IHostingEnvironment.IsDevelopment()
        private readonly IDomainEventVisitor eventVisitor;

        public EventsContext(string connectionString, bool useConsoleLogger, IDomainEventVisitor eventVisitor)
        {
            this.connectionString = connectionString;
            this.useConsoleLogger = useConsoleLogger;
            this.eventVisitor = eventVisitor;
        }

        //for testing porpouses
        public EventsContext(DbContextOptions<EventsContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events => Set<Event>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });

            optionsBuilder
                .UseSqlServer(this.connectionString)
                .UseLazyLoadingProxies();//To enable lazy loading (Only writes, never reads)

            if (useConsoleLogger)
            {
                optionsBuilder
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();//Now we can see the sql query on the console for performance purposes
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(x =>
            {
                x.ToTable("Events")
                    .HasKey(p => p.Id);

                x.Property(p => p.Id)
                    .HasMaxLength(36)
                    .IsRequired();

                x.Property(p => p.Name)
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();

                x.Property(p => p.Description)
                    .IsUnicode()
                    .IsRequired(false)
                    .HasMaxLength(1000);

                x.Property(p => p.StartDate)
                    .HasColumnType("dateTime")
                    .IsRequired();


                x.Property(p => p.EndDate)
                    .HasColumnType("dateTime")
                    .IsRequired();

                x.Property(p => p.CreatedAt)
                    .HasColumnType("dateTime")
                    .IsRequired();


                x.Property(p => p.UpdatedAt)
                    .HasColumnType("dateTime")
                    .IsRequired();

                x.Property(p => p.Type)
                    .HasMaxLength(30)
                    .IsUnicode()
                    .IsRequired()
                    .HasConversion(p => p.Value, p => TypeEvent.CreateNew(p).Value);

                x.Property(p => p.State)
                    .HasMaxLength(10)
                    .IsUnicode()
                    .IsRequired()
                    .HasConversion(p => p.Value, p => StateEvent.CreateNew(p).Value);

                x.Property(p => p.Capacity)
                    .IsRequired();

                x.OwnsOne(p => p.Address, p =>
                {
                    p.Property(pp => pp.Country)
                        .HasMaxLength(30)
                        .IsRequired();

                    p.Property(pp => pp.City)
                        .HasMaxLength(30)
                        .IsRequired();

                    p.Property(pp => pp.Street)
                        .HasMaxLength(100)
                        .IsRequired();

                    p.Property(pp => pp.Number)
                        .HasMaxLength(10)
                        .IsRequired();

                    p.Property(pp => pp.ZipCode)
                        .HasMaxLength(10)
                        .IsRequired();
                });

                x.OwnsOne(p => p.Price, p =>
                {
                    p.Property(pp => pp.CurrencyExchange)
                        .HasMaxLength(5)
                        .IsRequired();

                    p.Property(pp => pp.MonetaryValue)
                        .HasMaxLength(10)
                        .IsRequired();
                });

                // Define the one-to-one relationship with Image
                x.HasOne(p => p.Image)
                    .WithOne()
                    .HasForeignKey<Image>(p => p.EventId);
            });

            modelBuilder.Entity<Image>(x =>
            {
                x.ToTable("Images")
                    .HasKey(p => p.Id);

                x.Property(p => p.Id)
                    .HasMaxLength(36)
                    .IsRequired();

                x.Property(p => p.EventId) // Foreign key to Event
                     .HasMaxLength(36)
                     .IsRequired();

                x.Property(p => p.ContentType)
                    .IsUnicode()
                    .IsRequired(false)
                    .HasMaxLength(1000);

                x.Property(p => p.FileName)
                    .IsUnicode()
                    .IsRequired(false)
                    .HasMaxLength(1000);
            });
        }

        public async Task<Result> SaveChangesAsync()
        {
            List<Entity> entities = ChangeTracker
                .Entries()
                .Where(x => x.Entity is AggregateRoot)
                .Select(x => (Entity)x.Entity)
                .ToList();

            int result = await base.SaveChangesAsync();
            if (result < 1)
            {
                return Result.Failure("");
            }

            foreach (AggregateRoot entity in entities)
            {
                if (entity.DomainEvents != null && entity.DomainEvents.Count() != 0)
                {
                    await this.PublishDomainEvents(entity.DomainEvents);
                }
                entity.RemoveDomainEvent();
            }

            return Result.Success();
        }

        private async Task<Result> PublishDomainEvents(IReadOnlyList<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await domainEvent.Accept(eventVisitor);
            }
            return Result.Success();
        }
    }
}
