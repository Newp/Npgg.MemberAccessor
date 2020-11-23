//source : https://www.codeproject.com/Articles/993798/FieldInfo-PropertyInfo-GetValue-SetValue-Alternati
//lisence : The Code Project Open License(CPOL)

using System;
using System.Collections.Generic;

namespace Npgg.Reflection
{
    public class MemberAccessorPool
    {
        public Dictionary<Type, Dictionary<string, MemberAccessor>> Cached = new Dictionary<Type, Dictionary<string, MemberAccessor>>();

        public Dictionary<string, MemberAccessor> GetAccessors(Type type)
        {
            if (Cached.TryGetValue(type, out var result) == false)
            {
                result = MemberAccessor.GetAssigners(type);

                Cached[type] = result;
            }
            return result;
        }

        public Dictionary<string, MemberAccessor> GetAccessors<T>() => GetAccessors(typeof(T));

        
    }



}
