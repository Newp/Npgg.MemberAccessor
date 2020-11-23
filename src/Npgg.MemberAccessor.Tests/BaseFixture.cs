using System.Linq;
using Npgg.Reflection;

namespace Npgg.MemberAccessorTests
{
    public class BaseFixture<T>
    {

        protected MemberAccessor GetAssigner(string name)
        {
            var memberType = typeof(T).GetMember(name).First();
            return new MemberAccessor(memberType);
        }
    }
}
