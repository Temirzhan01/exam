    public interface IRepository<T> //Есть у меня такой интерфейс и наследуемые классы
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }

    public class PostgreEntityRepository : IRepository<Entity> //для работы с потгре
{ 
    private readonly PostgreDbContext _context;
    public PostgreEntityRepository(PostgreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Entity>> GetAllAsync() 
    {
        return await _context.Entities.ToListAsync();
    }
    public async Task<Entity> GetByIdAsync(int id) 
    {
        return await _context.Entities.FindAsync(id);
    }
    public async Task AddAsync(Entity entity) 
    {
        await _context.Entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Entity entity) 
    {
        _context.Entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) 
    {
        var entity = await _context.Entities.FindAsync(id);
        if (entity != null)
        {
            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

    public class OracleEntityRepository : IRepository<Entity> // для работы с оракл
    {
        private readonly OracleDbContext _context;
        public OracleEntityRepository(OracleDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Entity>> GetAllAsync()
        {
            return await _context.Entities.ToListAsync();
        }
        public async Task<Entity> GetByIdAsync(int id)
        {
            return await _context.Entities.FindAsync(id);
        }
        public async Task AddAsync(Entity entity)
        {
            await _context.Entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Entity entity)
        {
            _context.Entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Entities.FindAsync(id);
            if (entity != null)
            {
                _context.Entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

    Вот смотри, как мне их внедрить ? типа тут нет разницы для оракл или пострге, для каждой сущности свой репозиторий, типа OracleProductRepository, не суть, просто как внутри условно контроллера взять и использовать тот или иной репозиторий? пох на типы баз братан, или вовсе убрать обощение  с интерфейса? 
