using System.Collections.Generic;

public class ObjectPool<T> where T : new()
{
    public delegate T0 CreateObject<T0>();
    public delegate void DestroyObject<T1>(T1 arg);

    private readonly int mCapacity;
    private readonly Stack<T> m_Stack = new Stack<T>();
    private readonly CreateObject<T> mCreateFun;
    private readonly DestroyObject<T> mDestroyFun;

    public int countAll { get; private set; }
    public int countActive { get { return countAll - countInactive; } }
    public int countInactive { get { return m_Stack.Count; } }

    public ObjectPool(int capacity, CreateObject<T> createFun, DestroyObject<T> destroyFun)
    {
        mCapacity = capacity;
        mCreateFun = createFun;
        mDestroyFun = destroyFun;
    }

    public T Get()
    {
        T element;
        if (m_Stack.Count == 0)
        {
            element = mCreateFun();
            countAll++;
        }
        else
        {
            element = m_Stack.Pop();
        }
        return element;
    }

    public void Recycle(T element)
    {
        if (mCapacity > 0 && m_Stack.Count >= mCapacity)
        {
            mDestroyFun(element);
            --countAll;
            return;
        }

        m_Stack.Push(element);
    }

    public void Clear()
    {
        m_Stack.Clear();
        countAll = 0;
    }
}