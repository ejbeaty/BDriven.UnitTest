using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BDriven.UnitTest
{
    public class ObjectInstance
    {
        private object _object { get; set; }
        private string _propertyPath { get; set; }
        private object _propertyInstanceAtWork { get; set; }


        public dynamic Instance { get { return _object; } }
        public Type InstanceType { get { return _object.GetType(); } }
        public ObjectInstance(object theObject)
        {
            if(theObject == null)
            {
                throw new ArgumentNullException("theObject");
            }
            _object = theObject;

        }

        #region Setting
        public ObjectInstance WithA(string propertyName)
        {
            _propertyPath = propertyName;
            return this;
        }
        public ObjectInstance WithACollection(string propertyName)
        {
            _propertyPath = propertyName;
            return this;
        }
        public ObjectInstance AndA(string propertyName)
        {
            _propertyPath = propertyName;
            return this;
        }
        public ObjectInstance WithAChildCollection(string propertyName)
        {
            _propertyPath = _propertyPath + "."+propertyName;
            return this;
        }
        public ObjectInstance WithAnItemThatHasA<T>(string propertyName, T propertyValue )
        {
            SetCollectionItemPropertyValue(propertyName, propertyValue);
            return this;
        }
        public ObjectInstance WithAnotherItemThatHasA<T>(string propertyName, T propertyValue)
        {
            SetCollectionItemPropertyValue(propertyName, propertyValue);
            return this;
        }
        private void SetCollectionItemPropertyValue<T>(string propertyName, T value)
        {
            var collection = GetPropertyValue(_propertyPath) as IEnumerable<object>;
            var type = collection.GetType().GetGenericArguments()[0];
           
            var itemInstance = GetInstance(type);
            var itemProperty = type.GetProperty(propertyName);
            if (itemProperty == null)
            {
                throw new InvalidOperationException(String.Format("{0} is not a valid property on the {1} class!", propertyName, _object.GetType().Name));
            }
            itemProperty.SetValue(itemInstance, value);
            var method = collection.GetType().GetMethod("Add");

            dynamic list = method.Invoke(collection, new object[] { itemInstance });
            SetPropertyValue(_propertyPath, collection);
        }
       
        public ObjectInstance AndAnotherItemThatHasA<T>(string propertyName, T propertyValue)
        {
            SetCollectionItemPropertyValue(propertyName, propertyValue);
            return this;
        }
        public ObjectInstance AndA<T>(string propertyName, T propertyValue)
        {
            var parentObject = GetPropertyValue(_propertyPath);
            if (parentObject is IEnumerable)
            {
                parentObject = (parentObject as IEnumerable<object>).Last();
            }
            var type = parentObject.GetType();
            var itemProperty = type.GetProperty(propertyName);
            if (itemProperty == null)
            {
                throw new InvalidOperationException(String.Format("{0} is not a valid property on the {1} class!", propertyName, _object.GetType().Name));
            }
            //SetCollectionItemPropertyValue(_propertyPath, propertyValue);

            itemProperty.SetValue(parentObject, propertyValue);

            return this;
        }
        public ObjectInstance Of<T>(T value)
        {
            SetPropertyValue(_propertyPath, value);
            return this;
        }

       
        #endregion

        #region Validating
        public ObjectInstance Where(string propertyName)
        {
            _propertyPath = propertyName;
            return this;
        }
        public ObjectInstance And(string propertyName)
        {
            _propertyPath = propertyName;
            return this;
        }
        public ObjectInstance Is<T>(T expectedValue)
        {
            var actualValue = GetPropertyValue(_propertyPath);
            Assert.Equal(expectedValue, actualValue);
            return this;
        }
        public ObjectInstance IsAnEmptyCollection()
        {
            var actualValue = GetPropertyValue(_propertyPath);
            Assert.Empty((IEnumerable)actualValue);
            return this;
        }
        public ObjectInstance IsNot<T>(T value)
        {
            var actualValue = GetPropertyValue(_propertyPath);
            Assert.NotEqual(value, (T)actualValue);
            return this;
        }
       
        public ObjectInstance WhereACollection(string propertyName)
        {
            _propertyPath = propertyName;
            //var prop = _object.GetType().GetProperty(propertyName);
            //if (prop == null)
            //{
            //    throw new InvalidOperationException(String.Format("{0} is not a valid property on the {1} class!", prop, _object.GetType().Name));
            //}
            //_collectionPropertiesToBeValidated.Enqueue(prop);
            return this;
        }
        
        public ObjectInstance ThatHasAnItemWith<T>(string propertyName, T expectedValue)
        {
            ValidateCollectionContainsItemWithValue(propertyName, expectedValue);
            return this;
        }
        public ObjectInstance ThatHasAnotherItemWith<T>(string propertyName, T expectedValue)
        {
            ValidateCollectionContainsItemWithValue(propertyName, expectedValue);
            return this;
        }
        public ObjectInstance ThatHasNoItemWith<T>(string propertyName, T expectedValue)
        {
            ValidateCollectionDoesNotContainItemWithValue(propertyName, expectedValue);
            return this;
        }
        public ObjectInstance HasAnItemWithACollection(string propertyName)
        {
            _propertyPath = _propertyPath+"."+propertyName;
            return this;
        }

        
        public ObjectInstance AndHasAnotherItemWith<T>(string propertyName, T expectedValue)
        {
            ValidateCollectionContainsItemWithValue(propertyName, expectedValue);
            return this;
        }
        public void ValidateCollectionContainsItemWithValue<T>(string propertyName, T expectedValue)
        {
            var collection = GetPropertyValue(_propertyPath) as IEnumerable<object>;
            if (collection == null)
            {
                throw new InvalidOperationException(String.Format("{0} is not a valid property on the {1} class!", propertyName, _object.GetType().Name));
            }
            var collectionType = collection.GetType().GetGenericArguments()[0];
            Assert.Contains(collection, c =>
            {
                var prop = c.GetType().GetProperty(propertyName);
                var propValue = prop.GetValue(c);
                if (expectedValue.Equals((T)propValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }
        public void ValidateCollectionDoesNotContainItemWithValue<T>(string propertyName, T expectedValue)
        {
            var collection = GetPropertyValue(_propertyPath) as IEnumerable<object>;
            if (collection == null)
            {
                throw new InvalidOperationException(String.Format("{0} is not a valid property on the {1} class!", propertyName, _object.GetType().Name));
            }
            var collectionType = collection.GetType().GetGenericArguments()[0];
            Assert.DoesNotContain(collection, c =>
            {
                var prop = c.GetType().GetProperty(propertyName);
                var propValue = prop.GetValue(c);
                if (expectedValue.Equals((T)propValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }
        public ObjectInstance And<T>(string propertyName, T expectedValue)
        {
            ValidateCollectionContainsItemWithValue(propertyName, expectedValue);
            return this;
        }
        #endregion
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
            if (type == typeof(byte))
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

        public object GetPropertyValue(string propertyPath)
        {
            var properties = propertyPath.Split('.');
            var parentObject = _object;
            for (var i = 0; i < properties.Length; i++)
            {
                var propertyName = properties[i];
                if(parentObject is IEnumerable)
                {
                    parentObject = (parentObject as IEnumerable<object>).Last();
                }
                var prop = parentObject.GetType().GetProperty(propertyName);
                if (prop == null)
                {
                    throw new InvalidOperationException(String.Format("{0} is not a valid property on the {1} class!", propertyName, parentObject.GetType().Name));
                }

                var propValue = prop.GetValue(parentObject);
                if (propValue == null)
                {
                    parentObject = GetInstance(prop.PropertyType);
                    //throw new InvalidOperationException(String.Format("Cannot get {0} on {1} because {1} is null!", propertyName, parentObject.GetType().Name));
                }
                else
                {
                    parentObject = propValue;
                }
            }
            return parentObject;

        }
        public ObjectInstance SetPropertyValue<T>(string propertyPath, T value)
        {
            var properties = propertyPath.Split('.');
            var parentObject = _object;
            for (var i = 0; i < properties.Length; i++)
            {
                var propertyName = properties[i];
                if (parentObject is IEnumerable)
                {
                    parentObject = (parentObject as IEnumerable<object>).Last();
                }
                var prop = parentObject.GetType().GetProperty(propertyName);
                if (prop == null)
                {
                    throw new InvalidOperationException(String.Format("{0} is not a valid property on the {1} class!", propertyName, parentObject.GetType().Name));
                }
                
                object obj = null;
                // If it's the last property in the chain, set the value provided 
                if (i == properties.Length - 1)
                {
                    obj = value;
                }
                else
                {
                    var propValue = prop.GetValue(parentObject);
                    if (propValue == null)
                    {
                        // Just get an empty instance of the object because we are trying to set a nested property
                        obj = GetInstance(prop.PropertyType);
                    }
                    else
                    {
                        obj = propValue;
                    }
                    
                }
               prop.SetValue(parentObject, obj);

               parentObject = obj;
            }
             
            return this;
        }
    }
}
