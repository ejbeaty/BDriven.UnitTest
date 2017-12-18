using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BDriven.UnitTest.UnitTests
{
    public class GivensTests: BehaviorDriven
    {

        public static IEnumerable<object[]> StorableGivenTypes()
        {
            yield return new object[] { typeof(HumanTestPoco) };
        }
        public static IEnumerable<object[]> StorableSimpleTypesWithValues()
        {
            yield return new object[] { typeof(byte), 1 };
            yield return new object[] { typeof(sbyte), 2 };
            yield return new object[] { typeof(int), 3 };
            yield return new object[] { typeof(uint), 4 };
            yield return new object[] { typeof(short), 5 };
            yield return new object[] { typeof(ushort), 6 };
            yield return new object[] { typeof(long), 7 };
            yield return new object[] { typeof(ulong), 8 };
            yield return new object[] { typeof(float), 9 };
            yield return new object[] { typeof(double), 10 };
            yield return new object[] { typeof(char), 'A' };
            yield return new object[] { typeof(bool), true };
            yield return new object[] { typeof(string), "someString" };
            yield return new object[] { typeof(decimal), 1.65M };
        }

        [Theory]
        [MemberData(nameof(StorableGivenTypes))]
        public void IHaveA_Accepts_All_Types(Type type)
        {
            var abstractClass = new GivensTests();

            var item = abstractClass.given.IHaveA<HumanTestPoco>();

            Assert.IsType(type, item.Instance);
        }

        [Theory]
        [MemberData(nameof(StorableGivenTypes))]
        public void IHaveAnother_Accepts_All_Types(Type type)
        {
            var abstractClass = new GivensTests();
            abstractClass.given.IHaveA<HumanTestPoco>();
            var item = abstractClass.given.IHaveA<HumanTestPoco>();

            Assert.IsType(type, item.Instance);
        }

      
        [Theory]
        [MemberData(nameof(StorableGivenTypes))]
        public void TypeCollection_Can_Be_Saved_And_Retrieved(Type type)
        {
            var abstractClass = new GivensTests();
            abstractClass.given.IHaveA<HumanTestPoco>();
            abstractClass.given.IHaveA<HumanTestPoco>();

            var collection = typeof(BehaviorDriven)
                .GetMethod("MyGivenItems")
                .MakeGenericMethod(type)
                .Invoke(abstractClass, null);

            Assert.True(collection is IEnumerable);
            
            var enumerable = ((IEnumerable)collection).Cast<object>();
            var array = enumerable.ToArray();
            Assert.IsType(type, array[0]);
            Assert.NotNull(array[1]);
        }
    }
}
