using Xunit;
namespace BDriven.UnitTest.UnitTests
{
    public class ObjectInstanceTests: BehaviorDriven
    {
        public ObjectInstanceTests()
        {
            RegisterBehavior("CallAPassThroughMethod", () => {
                var response = MyGivenItem<HumanTestPoco>();
                return response;
            });
        }
        [Fact]
        public void Can_Set_Property_Value()
        {
            var classInstance = new HumanTestPoco();
            var obj = new ObjectInstance(classInstance);
            obj.WithA("Name").Of("Elliott").AndA("Age").Of(100);

            Assert.Equal(obj.Instance.Name, "Elliott");
            Assert.Equal(obj.Instance.Age, 100);
        }

        [Fact]
        public void Can_Set_Property_Value_UsingWithA()
        {
            var testClass = new ObjectInstanceTests();
            testClass.given.IHaveA<HumanTestPoco>().WithA("Name").Of("Elliott").AndA("Age").Of(100);

            testClass.when.I("CallAPassThroughMethod");

            testClass.then.IExpectAResponse<HumanTestPoco>()
                .Where("Name").Is("Elliott").And("Age").Is(100)
                .And("Name").IsNot("Charlie");
        }
        

        [Fact]
        public void Can_Validate_CollectionProperty_Value()
        {
           
            var testClass = new ObjectInstanceTests();
            testClass.given.IHaveA<HumanTestPoco>()
                .WithACollection("Pets")
                    .WithAnItemThatHasA("AnimalType", "Dog")
                        .AndA("Name", "Bud");

            testClass.when.I("CallAPassThroughMethod");

            testClass.then.IExpectAResponse<HumanTestPoco>()
                .WhereACollection("Pets")
                .ThatHasAnItemWith("AnimalType", "Dog")
                    .And("Name", "Bud");
                
        }
        [Fact]
        public void Can_Validate_2ndCollectionProperty_Value()
        {

            var testClass = new ObjectInstanceTests();
            testClass.given.IHaveA<HumanTestPoco>()
                .WithACollection("Pets")
                    .WithAnItemThatHasA("AnimalType", "Dog")
                        .AndA("Name", "Bud")
                    .AndAnotherItemThatHasA("AnimalType", "Frog")
                        .AndA("Name", "Kermit");

            testClass.when.I("CallAPassThroughMethod");

            testClass.then.IExpectAResponse<HumanTestPoco>()
                .WhereACollection("Pets")
                .ThatHasAnItemWith("AnimalType", "Dog")
                    .And("Name", "Bud")
                .AndHasAnotherItemWith("AnimalType", "Frog")
                    .And("Name", "Kermit");

        }
        [Fact]
        public void Can_Validate_NestedCollectionProperty_Value()
        {
                
            var testClass = new ObjectInstanceTests();
            testClass.given.IHaveA<HumanTestPoco>()
                .WithACollection("Pets")
                   .WithAnItemThatHasA("Name", "Frank The Tank")
                    .WithAChildCollection("Friends")
                        .WithAnItemThatHasA("AnimalType", "Fish")
                            .AndA("Name", "Bert")
                        .AndAnotherItemThatHasA("AnimalType", "Bird")
                            .AndA("Name", "Sally");

            testClass.when.I("CallAPassThroughMethod");

            testClass.then.IExpectAResponse<HumanTestPoco>()
                .WhereACollection("Pets")
                .HasAnItemWithACollection("Friends")
                    .ThatHasAnItemWith("AnimalType", "Fish")
                        .And("Name", "Bert")
                    .AndHasAnotherItemWith("AnimalType", "Bird")
                        .And("Name", "Sally");
        }
        [Fact]
        public void Can_Set_NameOn_PetProperty()
        {
                      
            var classInstance = new HumanTestPoco();
            var obj = new ObjectInstance(classInstance);
            obj.WithA("Pet.Name").Of("Elliott")
                .AndA("Pet.AnimalType").Of("Turtle")
                .AndA("Age").Of(100);

            Assert.Equal(obj.Instance.Pet.Name, "Elliott");
            Assert.Equal(obj.Instance.Age, 100);
            Assert.Equal(obj.Instance.Pet.AnimalType, "Turtle");
        }
        [Fact]
        public void Can_Create_Item_In_CollectionProperty()
        {

            var classInstance = new HumanTestPoco();
            var obj = new ObjectInstance(classInstance);
            obj.WithACollection("Pets")
                .WithAnItemThatHasA("Name", "Frank The Tank")
                    .AndA("AnimalType", "Cow")
                .WithA("Age").Of(100)
                .AndA("Name").Of("Elliott");

            Assert.Equal(obj.Instance.Pets[0].Name, "Frank The Tank");
            Assert.Equal(obj.Instance.Pets[0].AnimalType, "Cow");
            Assert.Equal(obj.Instance.Age, 100);
            Assert.Equal(obj.Instance.Name, "Elliott");
        }

        [Fact]
        public void Can_Create_2ndItem_In_CollectionProperty()
        {

            var classInstance = new HumanTestPoco();
            var obj = new ObjectInstance(classInstance);
            obj.WithACollection("Pets")
                .WithAnItemThatHasA("Name", "Frank The Tank")
                    .AndA("AnimalType", "Cow")
                .WithAnotherItemThatHasA("Name", "Hank Williams")
                    .AndA("AnimalType", "Horse")
                .WithA("Age").Of(100)
                .AndA("Name").Of("Elliott");

            Assert.Equal(obj.Instance.Pets[0].Name, "Frank The Tank");
            Assert.Equal(obj.Instance.Pets[0].AnimalType, "Cow");
            Assert.Equal(obj.Instance.Age, 100);
            Assert.Equal(obj.Instance.Name, "Elliott");
            Assert.Equal(obj.Instance.Pets[1].Name, "Hank Williams");
            Assert.Equal(obj.Instance.Pets[1].AnimalType, "Horse");
        }
        [Fact]
        public void Can_Create_Item_In_NestedCollectionProperty()
        {

            var classInstance = new HumanTestPoco();
            var obj = new ObjectInstance(classInstance);
            obj.WithACollection("Pets")
                .WithAnItemThatHasA("Name", "Frank The Tank")
                    .WithAChildCollection("Friends")
                        .WithAnItemThatHasA("AnimalType", "Fish");
               

            Assert.Equal(obj.Instance.Pets[0].Name, "Frank The Tank");
            Assert.Equal(obj.Instance.Pets[0].Friends[0].AnimalType, "Fish");
        }
        [Fact]
        public void Can_UpdateMultiplePropsOn_Item_In_NestedCollectionProperty()
        {

            var classInstance = new HumanTestPoco();
            var obj = new ObjectInstance(classInstance);
            obj.WithACollection("Pets")
                .WithAnItemThatHasA("Name", "Frank The Tank")
                    .WithAChildCollection("Friends")
                        .WithAnItemThatHasA("AnimalType", "Fish")
                            .AndA("Name", "Bert")
                        .AndAnotherItemThatHasA("AnimalType", "Bird")
                            .AndA("Name", "Sally");


            Assert.Equal(obj.Instance.Pets[0].Name, "Frank The Tank");
            Assert.Equal(obj.Instance.Pets[0].Friends[0].AnimalType, "Fish");
            Assert.Equal(obj.Instance.Pets[0].Friends[0].Name, "Bert");
            Assert.Equal(obj.Instance.Pets[0].Friends[1].AnimalType, "Bird");
            Assert.Equal(obj.Instance.Pets[0].Friends[1].Name, "Sally");
        }
    }
}
