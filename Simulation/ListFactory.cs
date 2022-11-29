namespace SahptSimulation;

public static class ListFactory
{
    public static List<T> Create<T>(params T[] objs)
    {
        return objs.ToList();
    }
    
    public static List<T> Create<T>(T objs)
    {
        return new List<T>()
        {
            objs
        };
    }
    
}