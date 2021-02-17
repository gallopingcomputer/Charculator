using System;
using System.Collections.Generic;

//* The values should be immutable, because unless we're invoking the assignment operator the events won't be triggered
//! too bad we don't have C++-style recursive constness in C#

//* The "standard" versions have field-backed implementations of the properties
//TODO: performance implications
//TODO: consider making the strategy pattern (via the materialize delegate) as the default and only option instead (since inheritance is equivalent to that)

public abstract class Result<T>
{
    public Result()
    {
        val = default(T);
    }

    protected abstract T val { get; set; }
    public T Val {
        get => val;
        set {
            if (EqualityComparer<T>.Default.Equals(val, value)) 
                return;
            val = value;
            ValueChangedEvent(val);
        }
    }

    public abstract void ValueChangedEvent(T value);
}

public class StandardResult<T> : Result<T>
{
    public StandardResult() : base()
    {}

    protected sealed override T val { get; set; } // field-backed concrete property implementation

    public event Action<T> valueChangedHandlers;
    public sealed override void ValueChangedEvent(T value) => valueChangedHandlers(value);
}

public abstract class CachedUpdate
{
    public CachedUpdate()
    {
        IsDirty = true;
        IsUpdateSuppressed = false;
    }

    public bool IsDirty { get; protected set; }

    public bool IsUpdateSuppressed { get; protected set; }
    public void SuppressUpdate(bool s = true) { IsUpdateSuppressed = s; }
    public void WhileSuppressingUpdate(Action a)
    // if a changes the value (see below), it will not immediately trigger an update
    {
        IsUpdateSuppressed = true;
        a();
        IsUpdateSuppressed = false;
    }

    protected abstract void Materialize(); // should not access IsDirty, IsUpdateSuppressed, previous value, etc. -> should itself be stateless, prefereably pure ("pure functional")
    public void Refresh() {
        if (IsDirty && !IsUpdateSuppressed) {
            Materialize();
            IsDirty = false;
        }
    }
}

public abstract class CachedUpdate<T> : CachedUpdate
{
    public CachedUpdate() : base()
    {
        val = default(T);
    }

    protected abstract T val { get; set; }
    public T Val
    {
        get => val;
        set => setValue(value);
    }
    public void setValue(T value)
    {
        if (!IsDirty && 
            EqualityComparer<T>.Default.Equals(val, value))
            /* IsDirty = false */;
        else {
            // either it was previously dirty, or it wasn't but will become dirty
            val = value;
            if (!IsUpdateSuppressed) Materialize();
            IsDirty = IsUpdateSuppressed;
        }
    }
}

public class StandardCachedUpdate<T> : CachedUpdate<T>
{
    public StandardCachedUpdate(Action u) : base()
    {
        materialize = u;
    }

    protected sealed override T val { get; set; }

    Action materialize { get; set; }
    protected sealed override void Materialize() => materialize();
}

