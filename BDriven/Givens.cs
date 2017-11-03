﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDriven.UnitTest
{
    public class Givens
    {
        private GenericCollectionDictionary _collectionDict = new GenericCollectionDictionary();
        private GenericItemDictionary _singleItemDict = new GenericItemDictionary();
        
        internal IEnumerable<T> Items<T>()
        {
            IEnumerable<T> classInstances = null;
            if (_collectionDict.KeyExists<T>())
            {
                classInstances = _collectionDict.Get<T>();
            }

            return classInstances;
        }
        internal T Item<T>()
        {
            T item = default(T);
            if (_singleItemDict.KeyExists<T>())
            {
                item = _singleItemDict.Get<T>();
            }

            return item;
        }
        public ObjectInstance IHaveA<T>()
        {
            var obj = new ObjectInstance(GetInstance(typeof(T)));

            // Add it to the dict for when we only need one of this type for the test
            _singleItemDict.Add<T>(obj);

            // Add it to the collection dictionary as well, so if they use given.IHaveAnother(), it's already there in the dictionary
            _collectionDict.Add<T>(obj);

            return obj;
        }
        public ObjectInstance IHaveA<T>(T value)
        {
            var obj = new ObjectInstance(value);
            // Add it to the dict for when we only need one of this type for the test
            _singleItemDict.Add<T>(obj);

            // Add it to the collection dictionary as well, so if they use given.IHaveAnother(), it's already there in the dictionary
            _collectionDict.Add<T>(obj);

            return obj;
        }
        public ObjectInstance IHaveAnother<T>()
        {
            var obj = new ObjectInstance(GetInstance(typeof(T)));

            // Add it to the collection dictionary as well, so if they use given.IHaveAnother(), it's already there in the dictionary
            _collectionDict.Add<T>(obj);

            return obj;
        }
        public IEnumerable<T> IHaveThisMany<T>(int count) where T : class, new()
        {
            for (var i = 0; i < count; i++)
            {
                var classInstance = new T();
                
                _collectionDict.Add<T>(new ObjectInstance(classInstance));
            }
            return this.Items<T>();
        }

        public object GetInstance(Type instanceType)
        {
            if (instanceType.GetConstructor(Type.EmptyTypes) != null)
            {
                return Activator.CreateInstance(instanceType);
            }
            else
            {
                return GetDefaultValue(instanceType);
            }
           
        }
        public object GetDefaultValue(Type type)
        {
            if(type == typeof(byte))
            {
                byte result = 0;
                return result;
            }
            if (type == typeof(sbyte))
            {
                sbyte result = 0;
                return result;
            }
            if (type == typeof(int))
            {
                int result = 0;
                return result;
            }
            if (type == typeof(uint))
            {
                uint result = 0;
                return result;
            }
            if (type == typeof(short))
            {
                short result = 0;
                return result;
            }
            if (type == typeof(ushort))
            {
                ushort result = 0;
                return result;
            }
            if (type == typeof(long))
            {
                long result = 0;
                return result;
            }
            if (type == typeof(ulong))
            {
                ulong result = 0;
                return result;
            }
            if (type == typeof(float))
            {
                float result = 0;
                return result;
            }
            if (type == typeof(double))
            {
                double result = 0;
                return result;
            }
            if (type == typeof(char))
            {
                char result = '\0'; // Unicode 'null'
                return result;
            }
            if (type == typeof(bool))
            {
                return false;
            }
            if (type == typeof(object))
            {
                object result = null;
                return result;
            }
            if (type == typeof(string))
            {
                string result = String.Empty;
                return result;
            }
            if (type == typeof(decimal))
            {
                decimal result = 0.0M;
                return result;
            }
            throw new InvalidCastException("Unknown object type encountered");
        }
    }

    internal class GenericCollectionDictionary
    {
        private Dictionary<Type, List<ObjectInstance>> _dict = new Dictionary<Type, List<ObjectInstance>>();

        public void Add<T>(ObjectInstance objectInstance)
        {
            if (!this.KeyExists<T>())
            {
                var newObjectCollection = new List<ObjectInstance>();
                _dict.Add(typeof(T), newObjectCollection);
            }
            _dict[typeof(T)].Add(objectInstance);
        }

        public bool KeyExists<T>()
        {
            List<ObjectInstance> objectCollection = null;
            if (_dict.TryGetValue(typeof(T), out objectCollection))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<T> Get<T>()
        {
            List<ObjectInstance> objectCollection = null;
            if (_dict.TryGetValue(typeof(T), out objectCollection))
            {
                foreach (var item in objectCollection)
                {
                    yield return (T)item.Instance;
                }

            }
            else
            {
                throw new KeyNotFoundException(String.Format("No collection of type {0} was found!", typeof(T).Name));
            }
        }
    }
    internal class GenericItemDictionary
    {
        private Dictionary<Type, ObjectInstance>  _dict = new Dictionary<Type, ObjectInstance>();

        public void Add<T>(ObjectInstance classInstance)
        {
            var type = typeof(T);
            if (!this.KeyExists<T>())
            {
                _dict.Add(type, classInstance);
            }
            else
            {
                throw new InvalidOperationException(String.Format("You already have a given {0}. Did you mean to use given.IHaveAnother()?", type.Name));
            }
        }
        public bool KeyExists<T>()
        {
            ObjectInstance item = null;
            if (_dict.TryGetValue(typeof(T), out item))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public T Get<T>()
        {
            ObjectInstance item = null;
            if (_dict.TryGetValue(typeof(T), out item))
            {
                return (T)item.Instance;

            }
            else
            {
                throw new KeyNotFoundException(String.Format("No given object of type {0} was found!", typeof(T).Name));
            }
        }
    }
}
