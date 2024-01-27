using System;

namespace AgentsTest.Core
{
    public class Observable<T>
    {
        public event Action<T> OnValueChanged;
        public T Value { 
                        get { return _value; } 
                        set { _value = value; OnValueChanged?.Invoke(value); } 
                        }
        private T _value;

        public Observable(T initialValue = default)
        {
            _value = initialValue;
        }
    }
}