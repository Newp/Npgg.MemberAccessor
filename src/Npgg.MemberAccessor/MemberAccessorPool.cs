//source : https://www.codeproject.com/Articles/993798/FieldInfo-PropertyInfo-GetValue-SetValue-Alternati
//lisence : The Code Project Open License(CPOL)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/*
 * 
 * 캐싱된 데이터는 일부러 Type별/Member별로 나눈것은 별도로 캐싱한다.
 * 점유하는 메모리가 크지 않으며, 한 타입에 대해 항상 같은 결과를 갖는만큼 atomic 한 캐싱은 필요하지 않다.
*/
namespace Npgg.Reflection
{
    public class MemberAccessorPool
    {
        readonly Dictionary<Type, Dictionary<string, MemberAccessor>> Cached = new Dictionary<Type, Dictionary<string, MemberAccessor>>();
        readonly Dictionary<(string,string), MemberAccessor> CachedMemberAccessor = new Dictionary<(string, string), MemberAccessor>();

        public Dictionary<string, MemberAccessor> GetAccessors<T>() => GetAccessors(typeof(T));
        public Dictionary<string, MemberAccessor> GetAccessors(Type type)
        {
            if (Cached.TryGetValue(type, out var result) == false)
            {
                result = MemberAccessor.GetAccessors(type);
                Cached[type] = result;
            }
            return result;
        }

        public MemberAccessor GetAccessor(Type type, string memberName) => this.GetAccessor(type, type.GetMember(memberName).First());
        public MemberAccessor GetAccessor(Type type, MemberInfo memberInfo)
        {
            var key = (memberInfo.DeclaringType.FullName, memberInfo.Name);

            if (CachedMemberAccessor.TryGetValue(key, out var result) == false)
            {
                result = new MemberAccessor(memberInfo);
                CachedMemberAccessor[key]= result;
            }

            return result;
        }

        public MemberAccessor GetAccessor<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new Exception("Get Accessor Expression must be Member Expression. (ex: member=>member.Id");
            }

            return GetAccessor(memberExpression.Expression.Type, memberExpression.Member);
        }

        public void Overwrite<T>(T source, T target)
        {
            var accessors = this.GetAccessors<T>();

            foreach (var accessor in accessors.Values)
            {
                var value = accessor.GetValue(source);
                accessor.SetValue(target, value);
            }
        }

        public T CreateClone<T>(T source) where T : new()
        {
            T result = new T();
            this.Overwrite(source, result);

            return result;
        }
    }



}
