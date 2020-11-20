using System.Linq;

namespace Npgg.MemberAssignerTests
{
    public class BaseFixture<T>
    {

        protected MemberAssigner GetAssigner(string name)
        {
            var memberType = typeof(T).GetMember(name).First();
            return new MemberAssigner(memberType);
        }
    }
}
