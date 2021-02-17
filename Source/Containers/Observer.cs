using System;

// particularly useful when implemented CRTP-style

public interface IPublisher<T>
{
    event Action<T> Publish;
}

public interface ISubscriber<T>
{
    IPublisher<T> Source { get; }
    void OnUpdate(T source);
}

public abstract class Publisher<T> : IPublisher<T>
{
    public event Action<T> Publish;//TODO alternatively: replace with a proper Publish method + list of subscribers (might be better that way since you'd be able to know which objects they are; good for a generic publisher-subscriber design => could you get that from the delegate itself though? => unsubscribe in destructor; a more complete scheme should allow special event callbacks for destruction events)

    public void AddSubscriber(ISubscriber<T> target) {
        if (target.Source != null) 
            target.Source.Publish -= target.OnUpdate;
        Publish += target.OnUpdate;
    }

    public bool RemoveSubscriber(ISubscriber<T> target) {
        if (target.Source != this)
            return false;
        else {
            Publish -= target.OnUpdate;
            return true;
        }
    }
}

// can only subscribe to one thing; while publisher has "addSubscriber" and "removeSubscriber", subscriber only has "setPublisher" (possibly with null, i.e. subscribe and unsubscribe)
public abstract class Subscriber<T> : ISubscriber<T>
{
    public IPublisher<T> Source { get; protected set; }

    public abstract void OnUpdate(T source);

    public Subscriber() {
        Source = null;
    }

    // shortcut to create *and* subscribe
    public Subscriber(IPublisher<T> source) {
        Source = source;
        Source.Publish += OnUpdate;
    }

    ~Subscriber() {
        Source.Publish -= OnUpdate;
    }
}
