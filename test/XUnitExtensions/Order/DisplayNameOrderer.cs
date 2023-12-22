namespace XUnitExtensions.Order;

/// <summary>
/// 按集合的字母顺序排序
/// </summary>
public class DisplayNameOrderer : ITestCollectionOrderer
{
    public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
    {
        return testCollections.OrderBy(collection => collection.DisplayName);
    }
}