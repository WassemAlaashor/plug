using System;
using System.Collections.Generic;
using UnityEngine;

/*
Property Event System:
Custom event system that makes it possible to subscribe to a variable/class without needing to manage callbacks.
*/
public class Property<T>
{
    class Act<TT>
    {
        public bool m_CallEvenIfDisabled = false;
        public MonoBehaviour m_Mb;
        public bool m_HasMb;

        public Action<TT> m_Changed = null;
        public Action<TT, TT> m_ChangedWithPrev = null;
    }

    List<Act<T>> Callbacks = new List<Act<T>>();

    T currentValue;

    public Property() { }

    public Property(T defaultValue)
    {
        currentValue = defaultValue;
    }

    public void AddEvent(Action<T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        Callbacks.Add(new Act<T>()
        {
            m_Mb = mb,
            m_HasMb = mb != null,
            m_Changed = onChanged,
            m_CallEvenIfDisabled = callEvenIfDisabled,
        });
    }

    public void AddEvent(Action<T, T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        Callbacks.Add(new Act<T>()
        {
            m_Mb = mb,
            m_HasMb = mb != null,
            m_ChangedWithPrev = onChanged,
            m_CallEvenIfDisabled = callEvenIfDisabled,
        });
    }

    public void AddEventAndFire(Action<T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        AddEvent(onChanged, mb, callEvenIfDisabled);
        onChanged(currentValue);
    }

    public void AddEventAndFire(Action<T, T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        AddEvent(onChanged, mb, callEvenIfDisabled);
        onChanged(currentValue, currentValue);
    }

    public void RemoveEvent(Action<T> onChanged)
    {
        Callbacks.RemoveAll(el => el.m_Changed == onChanged);
    }

    public void RemoveEvent(Action<T, T> onChanged)
    {
        Callbacks.RemoveAll(el => el.m_ChangedWithPrev == onChanged);
    }

    public void RemoveEvent(MonoBehaviour mb)
    {
        Callbacks.RemoveAll(el => el.m_Mb == mb);
    }

    public void RemoveAllEvents()
    {
        Callbacks.Clear();
    }

    public void Fire() { ChangeValue(currentValue); }
    public void Fire(MonoBehaviour mb) { ChangeValue(currentValue, mb); }
    public void Fire(T newValue) { Value = newValue; }

    public virtual T Value
    {
        get { return currentValue; }
        set { ChangeValue(value); }
    }

    void ChangeValue(T value, MonoBehaviour mb = null)
    {
        var oldValue = currentValue;
        currentValue = value;

        Callbacks.RemoveAll(el =>
        {
            try
            {
                // If monoBehaviour has been already removed we'll have null here
                if (el.m_HasMb && el.m_Mb == null)
                    return true;

                if (!el.m_HasMb || (el.m_Mb.gameObject.activeInHierarchy && el.m_Mb.enabled) || el.m_CallEvenIfDisabled)
                    if (mb == null || el.m_Mb == mb)
                    {
                        if (el.m_Changed != null)
                            el.m_Changed(currentValue);
                        if (el.m_ChangedWithPrev != null)
                            el.m_ChangedWithPrev(currentValue, oldValue);
                    }
                return false;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
                return false;
            }
        });
    }
}
