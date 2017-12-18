using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BDriven.UnitTest
{
    public class Thens
    {
        private Whens When;

        internal Thens(Whens when)
        {
            if (when == null)
            {
                throw new ArgumentNullException("when");
            }
            this.When = when;
        }

        public void IExpectAnExceptionOfType(Type type)
        {
            Assert.Equal(type, When.Exception.GetType());
        }

        public void IExpectNoException()
        {
            Assert.Null(When.Exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of object you expect to be returned from the "when.ICall()" method</typeparam>
        /// <param name="expectation"></param>
        public void IExpect<T>(Action<T> expectation)
        {
            expectation((T)this.When.ResponseObject);
        }

        public ObjectValidator IExpectAResponse<T>()
        {
            Assert.Null(When.Exception);
            var responseObject = When.ResponseObject;
            Assert.NotNull(responseObject);
            Assert.Equal(typeof(T), responseObject.GetType());
            return new ObjectValidator(responseObject);
        }
        public ObjectValidator IExpectAResponse<T>(T expectedValue)
        {
            Assert.Null(When.Exception);
            var responseObject = When.ResponseObject;
            Assert.NotNull(responseObject);
            Assert.Equal(typeof(T), responseObject.GetType());
            Assert.Equal(expectedValue, responseObject);

            return new ObjectValidator(responseObject);
        }

        public object IExpectAResponse(Type responseType)
        {
            var responseObject = When.ResponseObject;
            Assert.NotNull(responseObject);
            Assert.Equal(responseType, responseObject.GetType());
            return responseObject;
        }
    }
}
