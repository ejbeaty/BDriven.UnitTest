using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDriven.UnitTest
{
    public class Whens
    {
        private Dictionary<string, Func<object>> MethodCalls = new Dictionary<string, Func<object>>();

        public Givens Given;
        public Exception Exception { get; private set; }
        internal object ResponseObject { get; set; }

        internal Whens(Givens givens)
        {
            this.Given = givens;
        }

        internal void Register(string actionName, Func<object> action)
        {
            this.MethodCalls.Add(actionName, action);
        }
        public void I(string methodName)
        {
            var action = MethodCalls[methodName];
            try
            {
                this.ResponseObject = action();
            }
            catch (Exception e)
            {
                this.Exception = e;
            }
        }
    }
}
