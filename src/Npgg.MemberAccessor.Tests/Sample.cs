namespace Npgg.MemberAccessorTests
{
    public class Sample
    {
        public string Name { get; set; }
        public int Age { get; private set; }
        public string Description { get; set; }
        public Sample()
        {

        }

        public Sample(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
