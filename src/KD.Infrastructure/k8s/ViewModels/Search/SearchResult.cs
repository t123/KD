namespace KD.Infrastructure.k8s.ViewModels.Search;

public record KDSearchResult(string IndexType, string NodeTypeAlias, float Score, IReadOnlyDictionary<string, string> Values);