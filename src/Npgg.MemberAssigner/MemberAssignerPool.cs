//source : https://www.codeproject.com/Articles/993798/FieldInfo-PropertyInfo-GetValue-SetValue-Alternati
//lisence : The Code Project Open License(CPOL)

using System;
using System.Collections.Generic;

namespace Npgg
{
    public class MemberAssignerPool
    {
        public Dictionary<Type, Dictionary<string, MemberAssigner>> Cached = new Dictionary<Type, Dictionary<string, MemberAssigner>>();

        public Dictionary<string, MemberAssigner> GetAssigners(Type type)
        {
            if (Cached.TryGetValue(type, out var result) == false)
            {
                result = MemberAssigner.GetAssigners(type);

                Cached[type] = result;
            }
            return result;
        }

        public Dictionary<string, MemberAssigner> GetAssigners<T>() => GetAssigners(typeof(T));
    }



}
