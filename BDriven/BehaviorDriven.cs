using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDriven.UnitTest
{
    public abstract class BehaviorDriven
    {
        public Givens given;
        public Whens when;
        public Thens then;

        public BehaviorDriven()
        {
            this.given = new Givens();
            this.when = new Whens(given);
            this.then = new Thens(when);
        }

        public void RegisterBehavior(string actionName, Func<object> action)
        {
            this.when.Register(actionName, action);
        }
        public void RegisterBehavior(string actionName, Func<object[],object> action)
        {
            this.when.Register(actionName, action);
        }
        public IEnumerable<T> MyGivenItems<T>()
        {
            return this.given.Items<T>();
        }
        public T MyGivenItem<T>() 
        {
            return this.given.Item<T>();
        }

    }
}
