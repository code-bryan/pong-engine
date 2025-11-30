namespace Engine.Core.ECS;

public class EntityManager
{
    private readonly Dictionary<int, Dictionary<Type, object>> _entityComponents = [];
    private int _nextEntityId = 1;
    
    public int CreateEntity()
    {
        int id = _nextEntityId++;
        _entityComponents.Add(id, new Dictionary<Type, object>());
        return id;
    }

    public void AddComponent<T>(int id, T component) where T : class
    {
        _entityComponents[id].Add(typeof(T), component);
    }

    public T GetComponent<T>(int id) where T : class
    {
        if (_entityComponents[id].TryGetValue(typeof(T), out object component))
        {
            return (T)component;
        }

        return null;
    }

    public IEnumerable<int> GetEntitiesWith<T1, T2>() where T1 : class where T2 : class
    {
        foreach (var pair in _entityComponents)
        {
            if (pair.Value.ContainsKey(typeof(T1)) && pair.Value.ContainsKey(typeof(T2)))
            {
                yield return pair.Key;
            }
        }
    }
}