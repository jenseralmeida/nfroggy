using Froggy.Validation.Object;

namespace Froggy.Test.Validation.Object
{
    class Stub
    {
        [SystemTypeValidator(IsNullable = false)]
        public string Name { get; set; }

        //[Validator(CustomTestValidators={new StubNameTestValidator()})]
        public string Vogal { get; set; }
    }
}
