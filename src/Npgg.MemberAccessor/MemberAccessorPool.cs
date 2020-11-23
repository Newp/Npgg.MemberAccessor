//source : https://www.codeproject.com/Articles/993798/FieldInfo-PropertyInfo-GetValue-SetValue-Alternati
//lisence : The Code Project Open License(CPOL)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Npgg.Reflection
{
    public class MemberAccessorPool
    {
        Dictionary<Type, Dictionary<string, MemberAccessor>> Cached = new Dictionary<Type, Dictionary<string, MemberAccessor>>();
        Dictionary<(string,string), MemberAccessor> CachedMemberAccessor = new Dictionary<(string, string), MemberAccessor>();

        public Dictionary<string, MemberAccessor> GetAccessors<T>() => GetAccessors(typeof(T));
        public Dictionary<string, MemberAccessor> GetAccessors(Type type)
        {
            if (Cached.TryGetValue(type, out var result) == false)
            {
                result = MemberAccessor.GetAssigners(type);
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
                CachedMemberAccessor.Add(key, result);
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
    }



}
