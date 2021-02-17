using System;

//* Note: a value captured in a BindingSource (Result) can have multiple representations (e.g. as tuple vs. decomposed fields), but a BindingTarget should not

public class BindingSource<T>
    : StandardResult<T>
{
    public static implicit operator T(BindingSource<T> i) => i.Val;

    public BindingSource() : base()
    {}

    public void Connect(BindingTarget<T> next) 
    {
        valueChangedHandlers += next.setValue;
        next.BindingUpdate(this);
    }
    public void Disconnect(BindingTarget<T> next)
    {
        valueChangedHandlers -= next.setValue;
        next.BindingUpdate(null);
    }
}

public class BindingTarget<T> // aka Dependency Property
    : StandardCachedUpdate<T>
{
    public static implicit operator T(BindingTarget<T> i) => i.Val;

    public BindingTarget(Action u) : base(u)
    {
        source = null;
    }

    // The intended usage is that whe connected, val set() of the SCU should not be invoked other than as part of BindingSource's update handlers
    BindingSource<T> source;
    public bool IsDisconnected() => (source == null);
    public bool Bind(BindingSource<T> source)
    // returns: boolean indicating whether object was previously connected
    {
        bool r = false;
        if (!IsDisconnected())
        {
            source.Disconnect(this);
            r = true;
        }
        this.source = source;
        source.Connect(this);
        return r;
    }
    public bool Unbind(BindingSource<T> source) 
    // returns: boolean indicating whether object was previously connected to the specified source (equivalently, whether a disconnection happened)
    {
        if (this.source == source)
        {
            source.Disconnect(this);
            return true;
        }
        return false;
    }
    public void BindingUpdate(BindingSource<T> sender)
    {
        source = sender;
        if (!IsDisconnected)
            setValue(sender.Val); // initial value change
    }
}


/* 
 * Type parameters:
 * T (, S, R, ...) - input type(s)
 * U (, V, W, ...) - output type(s)
 * Both the individual "in" parameters and the object as a whole support the CachedUpdate interface
 */

interface IConverter<A,B>
{
    B Convert(A input);
    void Materialize();
}

public 
abstract class BoundConverter<T,U> : 
    CachedUpdate<T>, IConverter<T,U>
{
    public BindingTarget<T> in_val;
    public BindingSource<U> out_val;

    public BoundConverter() : base()
	{
        in_val = new BindingTarget<T>(Materialize);
        out_val = new BindingSource<U>();
	}

    public override void Materialize()
    {
        out_val.Val = Convert(in_val.Val);
    }

    public abstract U Convert(T input);
}

// convenience class, similar to BoundConverter<ValueTuple<S,T>,U>; alternative is to derive from it
// in that case, consider changing in_val, out_val to abstract properties (at the cost of an extra indirection)
public 
abstract class BoundConverter<S,T,U> : 
    CachedUpdate<ValueTuple<S,T>>, 
    IConverter<ValueTuple<S,T>,U>
{
    public BindingTarget<S> in_val_1;
    public BindingTarget<T> in_val_2;
    public BindingSource<U> out_val;

    public BoundConverter() : base()
	{
        in_val_1 = new BindingTarget<S>(Materialize);
        in_val_2 = new BindingTarget<T>(Materialize);
        out_val = new BindingSource<U>();
	}

    public override void Materialize()
    {
        out_val.Val = Convert((in_val_1.Val, in_val_2.Val));
    }

    public U Convert(ValueTuple<S,T> input)
    {
        return Convert(input.Item1, input.Item2);
    }
    public abstract U Convert(S input_1, T input_2);
}
