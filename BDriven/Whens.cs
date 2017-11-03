using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDriven.UnitTest
{
    public class Whens
    {
        private Dictionary<string, Func<object[], object>> MethodCalls = new Dictionary<string, Func<object[], object>>();

        public Givens Given;
        public Exception Exception { get; private set; }
        internal object ResponseObject { get; set; }

        internal Whens(Givens givens)
        {
            this.Given = givens;
        }

        internal void Register(string actionName, Func<object> action)
        {
            Func<object[], object> actionWithEmptyArgs = (args) => { return action(); };
            this.MethodCalls.Add(actionName, actionWithEmptyArgs);
        }
        internal void Register(string actionName, Func<object[],object> action)
        {
            this.MethodCalls.Add(actionName, action);
        }
        public void I(string methodName)
        {
            var action = MethodCalls[methodName];
            try
            {
                var emptyArgs = new object[0];
                this.ResponseObject = action(emptyArgs);
            }
            catch (Exception e)
            {
                this.Exception = e;
            }
        }

        public void I(string methodName, params object[] withArgs)
        {
            var action = MethodCalls[methodName];
            try
            {
                this.ResponseObject = action(withArgs);
            }
            catch (Exception e)
            {
                this.Exception = e;
            }
        }
    }
}
