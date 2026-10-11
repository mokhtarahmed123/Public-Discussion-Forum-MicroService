using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PostsCommentsService.Data.Interfaces;
using PostsCommentsService.Infrastructure.DataBaseConfiguration;
using System.Linq.Expressions;

namespace PostsCommentsService.Infrastructure.InfrastructureBases
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        protected readonly ForumService forumService;
        protected readonly IMongoCollection<T> _collection;

        public GenericRepositoryAsync(ForumService forumService)
        {
            this.forumService = forumService;
            _collection = forumService.GetCollection<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(entity, cancellationToken);
            return entity;

        }

        public async Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken)
        {
            if (entities.Count == 0) return;

            await _collection.InsertManyAsync(entities, cancellationToken: cancellationToken);

        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            await _collection.DeleteOneAsync(IdFilter(entity), cancellationToken);


        }

        public async Task DeleteRangeAsync(ICollection<T> entities, CancellationToken cancellationToken)
        {
            if (entities.Count == 0) return;

            var ids = entities.Select(GetId).ToList();
            var filter = Builders<T>.Filter.In("_id", ids);
            await _collection.DeleteManyAsync(filter, cancellationToken);
        }

        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);


        }

        public IQueryable<T> GetTableAsTracking(CancellationToken cancellationToken)
        {
            return _collection.AsQueryable();
        }

        public IQueryable<T> GetTableNoTracking(CancellationToken cancellationToken)
        {
            return _collection.AsQueryable();
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            var result = await _collection.ReplaceOneAsync(IdFilter(entity), entity, cancellationToken: cancellationToken);
            if (result.MatchedCount == 0)
                throw new InvalidOperationException("Update matched no documents.");
        }

        public async Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken)
        {
            if (entities.Count == 0) return;

            var models = entities.Select(e => new ReplaceOneModel<T>(IdFilter(e), e));
            await _collection.BulkWriteAsync(models, cancellationToken: cancellationToken);

        }

        private static object GetId(T entity)
        {
            var idMember = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap
                ?? throw new InvalidOperationException($"{typeof(T).Name} has no Id member.");

            return idMember.Getter(entity);
        }

        private static FilterDefinition<T> IdFilter(T entity)
        {
            var idValue = entity.ToBsonDocument()["_id"];
            return new BsonDocumentFilterDefinition<T>(new BsonDocument("_id", idValue));
        }

        public async Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return await _collection
                    .Find(FilterDefinition<T>.Empty)
                    .ToListAsync(cancellationToken);
        }
    }
}
