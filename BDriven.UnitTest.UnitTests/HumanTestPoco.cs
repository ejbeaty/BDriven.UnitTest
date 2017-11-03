using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDriven.UnitTest.UnitTests
{
    public class HumanTestPoco
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Pet Pet { get; set; }
        public List<Pet> Pets { get; set; }
    }

    public class Pet
    {
        public string AnimalType { get; set; }
        public string Name { get; set; }

        public List<Pet> Friends { get; set; }

    }
}
