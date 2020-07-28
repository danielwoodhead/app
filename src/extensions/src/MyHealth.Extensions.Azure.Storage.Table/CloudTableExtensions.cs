using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace MyHealth.Extensions.Azure.Storage.Table
{
    public static class CloudTableExtensions
    {
        public static async Task DeleteAsync<TEntity>(this CloudTable table, TEntity entity)
            where TEntity : ITableEntity
        {
            var operation = TableOperation.Delete(entity);

            await table.ExecuteInternalAsync(operation);
        }

        public static async Task<TableResult> InsertAsync<TEntity>(this CloudTable table, TEntity entity)
            where TEntity : ITableEntity
        {
            var operation = TableOperation.Insert(entity);

            return await table.ExecuteInternalAsync(operation);
        }

        public static async Task<TableResult> InsertOrMergeAsync<TEntity>(this CloudTable table, TEntity entity)
            where TEntity : ITableEntity
        {
            var operation = TableOperation.InsertOrMerge(entity);

            return await table.ExecuteInternalAsync(operation);
        }

        public static async Task<TEntity> RetrieveAsync<TEntity>(this CloudTable table, string partitionKey, string rowKey)
            where TEntity : class, ITableEntity
        {
            var operation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(operation);

            return result.Result as TEntity;
        }

        public static async Task<IEnumerable<TEntity>> RetrievePartitionAsync<TEntity>(this CloudTable table, string partitionKey)
            where TEntity : class, ITableEntity, new()
        {
            var query = new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            return await table.QueryAsync(query);
        }

        public static async Task BatchInsertAsync(this CloudTable table, IEnumerable<ITableEntity> items)
        {
            await table.BatchExecuteAsync(items, (batch, item) => batch.Insert(item));
        }

        public static async Task BatchInsertAsync(this CloudTable table, params ITableEntity[] items)
        {
            await table.BatchExecuteAsync(items, (batch, item) => batch.Insert(item));
        }

        public static async Task BatchInsertOrReplaceAsync(this CloudTable table, IEnumerable<ITableEntity> items)
        {
            await table.BatchExecuteAsync(items, (batch, item) => batch.InsertOrReplace(item));
        }

        public static async Task BatchInsertOrReplaceAsync(this CloudTable table, params ITableEntity[] items)
        {
            await table.BatchExecuteAsync(items, (batch, item) => batch.InsertOrReplace(item));
        }

        public static async Task BatchDeleteAsync(this CloudTable table, IEnumerable<ITableEntity> items)
        {
            await table.BatchExecuteAsync(items, (batch, item) => batch.Delete(item));
        }

        public static async Task BatchDeleteAsync(this CloudTable table, params ITableEntity[] items)
        {
            await table.BatchExecuteAsync(items, (batch, item) => batch.Delete(item));
        }

        private static async Task BatchExecuteAsync(this CloudTable table, IEnumerable<ITableEntity> items, Action<TableBatchOperation, ITableEntity> batchAction)
        {
            var itemsGroupedByPartition = items.GroupBy(x => x.PartitionKey);
            var tasks = new List<Task>();

            foreach (var itemPartitionGroup in itemsGroupedByPartition)
            {
                int itemPartitionGroupCount = itemPartitionGroup.Count();
                for (int i = 0; i < itemPartitionGroupCount; i += Constants.TableServiceBatchMaximumOperations)
                {
                    var batch = new TableBatchOperation();
                    var batchItems = itemPartitionGroup.Skip(i).Take(Constants.TableServiceBatchMaximumOperations).ToList();

                    foreach (var item in batchItems)
                        batchAction(batch, item);

                    var task = table.ExecuteInternalAsync(batch);
                    tasks.Add(task);

                    if (tasks.Count >= Constants.DefaultMaxConcurrentBatchOperations)
                    {
                        await Task.WhenAll(tasks);
                        tasks.Clear();
                    }
                }
            }

            await Task.WhenAll(tasks);
        }

        public static async Task<IEnumerable<TResult>> QueryAsync<TElement, TResult>(
            this CloudTable table,
            TableQuery<TElement> query,
            EntityResolver<TResult> entityResolver,
            CancellationToken cancellationToken = default(CancellationToken),
            Action<IList<TResult>> onProgress = null)
            where TElement : class, ITableEntity, new()
            where TResult : class, ITableEntity, new()
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var items = new List<TResult>();
            TableContinuationToken continuationToken = null;

            do
            {
                TableQuerySegment<TResult> segment = await table.ExecuteInternalAsync(
                    async t => await t.ExecuteQuerySegmentedAsync(query, entityResolver, continuationToken, cancellationToken));
                continuationToken = segment.ContinuationToken;
                items.AddRange(segment);
                onProgress?.Invoke(items);

            } while (continuationToken != null && !cancellationToken.IsCancellationRequested);

            return items;
        }

        public static async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(
            this CloudTable table,
            TableQuery<TEntity> query,
            CancellationToken cancellationToken = default(CancellationToken),
            Action<IList<TEntity>> onProgress = null)
            where TEntity : class, ITableEntity, new()
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var items = new List<TEntity>();
            TableContinuationToken continuationToken = null;

            do
            {
                TableQuerySegment<TEntity> segment = await table.ExecuteInternalAsync(
                    async t => await t.ExecuteQuerySegmentedAsync(query, continuationToken, cancellationToken));
                continuationToken = segment.ContinuationToken;
                items.AddRange(segment);
                onProgress?.Invoke(items);

            } while (continuationToken != null && !cancellationToken.IsCancellationRequested);

            return items;
        }

        private static async Task<TableResult> ExecuteInternalAsync(this CloudTable table, TableOperation operation, bool isRetry = false)
        {
            return await table.ExecuteInternalAsync(async t => await t.ExecuteAsync(operation), isRetry);
        }

        private static async Task<TableBatchResult> ExecuteInternalAsync(this CloudTable table, TableBatchOperation operation, bool isRetry = false)
        {
            return await table.ExecuteInternalAsync(async t => await t.ExecuteBatchAsync(operation), isRetry);
        }

        private static async Task<T> ExecuteInternalAsync<T>(this CloudTable table, Func<CloudTable, Task<T>> action, bool isRetry = false)
        {
            try
            {
                return await action(table);
            }
            catch (StorageException ex) when (ex.RequestInformation?.ExtendedErrorInformation?.ErrorCode == ErrorCodes.TableNotFound)
            {
                if (isRetry)
                    throw;

                await table.CreateIfNotExistsAsync();
                return await table.ExecuteInternalAsync(action, isRetry: true);
            }
        }
    }
}
