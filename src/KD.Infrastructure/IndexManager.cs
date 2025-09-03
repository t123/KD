using Examine;
using KD.Infrastructure.k8s.ViewModels.Objects;
using KD.Infrastructure.k8s.ViewModels.Search;

namespace KD.Infrastructure;

public interface IIndexManager
{
    Task IndexItem<T>(string category, string itemType, T? item) where T : IObjectViewModel;
    Task IndexItems<T>(string category, string itemType, IEnumerable<T>? items) where T : IObjectViewModel;
    Task IndexItem<T>(string indexName, string category, string itemType, T? item) where T : IObjectViewModel;
    Task IndexItems<T>(string indexName, string category, string itemType, IEnumerable<T>? items) where T : IObjectViewModel;
    Task<IEnumerable<KDSearchResult>> Search(string text);
    Task<IEnumerable<KDSearchResult>> Search(string indexName, string text);
}

public class IndexManager : IIndexManager
{

    private readonly IExamineManager _examineManager;
    public const string IndexName = "kd-k8s-index";

    public IndexManager(IExamineManager examineManager)
    {
        _examineManager = examineManager;
    }

    public async Task IndexItem<T>(string category, string itemType, T? item) where T : IObjectViewModel
    {
        await IndexItem(IndexName, category, itemType, item);
    }

    public async Task IndexItems<T>(string category, string itemType, IEnumerable<T>? items) where T : IObjectViewModel
    {
        await IndexItems(IndexName, category, itemType, items);
    }

    public async Task IndexItem<T>(string indexName, string category, string itemType, T? item) where T : IObjectViewModel
    {
        if (item == null)
        {
            return;
        }

        if (_examineManager.TryGetIndex(indexName, out var index))
        {
            index.IndexItem(ValueSet.FromObject(item.Uid, category, itemType, item));
        }
    }

    public async Task IndexItems<T>(string indexName, string category, string itemType, IEnumerable<T>? items) where T : IObjectViewModel
    {
        if (items?.Count() == 0)
        {
            return;
        }

        if (_examineManager.TryGetIndex(indexName, out var index))
        {
            foreach (var item in items ?? [])
            {
                index.IndexItem(ValueSet.FromObject(item.Uid, category, itemType, item));
            }
        }
    }

    public void GetStats(string indexName)
    {
        if (_examineManager.TryGetIndex(indexName, out var index))
        {
            if (index is IIndexStats stats)
            {
                var count = stats.GetDocumentCount();
            }
        }
    }

    public async Task<IEnumerable<KDSearchResult>> Search(string text)
    {
        return await Search(IndexName, text);
    }

    public async Task<IEnumerable<KDSearchResult>> Search(string indexName, string text)
    {
        if (_examineManager.TryGetIndex(indexName, out var index))
        {
            var results = index.Searcher.Search(text);
            var model = results.Select(x => new KDSearchResult(x.Values["__IndexType"], x.Values["__NodeTypeAlias"], x.Score, x.Values)).ToArray();
            return model;
        }

        throw new Exception("Unknown index");
    }
}