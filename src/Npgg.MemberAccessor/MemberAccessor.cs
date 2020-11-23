//source : https://www.codeproject.com/Articles/993798/FieldInfo-PropertyInfo-GetValue-SetValue-Alternati
//lisence : The Code Project Open License(CPOL)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Npgg.Reflection
{
    public partial class MemberAccessor
    {

        public static Dictionary<string, MemberAccessor> GetAssigners(Type type)
            => GetVariables(type).ToDictionary(
                        memberInfo => memberInfo.Name,
                        memberInfo => new MemberAccessor(memberInfo));

        public static Dictionary<string, MemberAccessor> GetAssigners<T>() => GetAssigners(typeof(T));

        public static MemberAccessor GetAccessor<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new Exception("Get Accessor Expression must be Member Expression. (ex: member=>member.Id");
            }

            return new MemberAccessor(memberExpression.Member);
        }

        public static List<MemberInfo> GetVariables(Type type)
        {
            List<MemberInfo> result = new List<MemberInfo>();
            foreach (var memberInfo in type.GetMembers())
            {
                if (memberInfo.MemberType != MemberTypes.Field && memberInfo.MemberType != MemberTypes.Property)
                    continue;

                result.Add(memberInfo);
            }

            return result;
        }

    }

    public partial class MemberAccessor
    {


        private readonly static MethodInfo sm_valueAssignerMethod
            = typeof(MemberAccessor).GetMethod("ValueAssigner", BindingFlags.Static | BindingFlags.NonPublic);

        private static void ValueAssigner<T>(out T dest, T src) => dest = src;

        private readonly Func<object, object> getter;

        private readonly Action<object, object> setter;


        public object GetValue(object targetObject) => getter(targetObject);

        public T GetValue<T>(object targetObject) => (T)getter(targetObject);
        public void SetValue(object targetObject, object memberValue) => setter(targetObject, memberValue);


        public bool CheckType(Type type)
        {
            return type.IsAssignableFrom(this.ValueType);
        }

        public readonly Type DeclaringType;
        public readonly Type ValueType;

        public readonly bool IsReadonly;
        public readonly string Name;

        public MemberAccessor(MemberInfo memberInfo)
        {
            this.Name = memberInfo.Name;
            this.DeclaringType = memberInfo.DeclaringType;
            MemberExpression exMember = null;
            Func<Expression, MemberExpression> getMemberExpression;
            Func<Expression, Expression, MethodCallExpression> makeCallExpression;

            if (memberInfo is FieldInfo fi)
            {
                this.ValueType = fi.FieldType;
                var assignmentMethod = sm_valueAssignerMethod.MakeGenericMethod(fi.FieldType);

                getMemberExpression = _ex => exMember = Expression.Field(_ex, fi);
                makeCallExpression = (_, _val) => Expression.Call(assignmentMethod, exMember, _val);
            }
            else if (memberInfo is PropertyInfo pi)
            {
                this.ValueType = pi.PropertyType;
                var assignmentMethod = pi.GetSetMethod(true);

                getMemberExpression = _ex => exMember = Expression.Property(_ex, pi);

                makeCallExpression = null;

                if(assignmentMethod !=null)
                    makeCallExpression = (_obj, _val) => Expression.Call(_obj, assignmentMethod, _val);
                else
                    makeCallExpression = null;
            }
            else
            {
                throw new ArgumentException
                ("The member must be either a Field or a Property, not " + memberInfo.MemberType);
            }

            Init(getMemberExpression
                    , makeCallExpression
                    , out this.getter
                    , ref this.setter
                );

            this.IsReadonly = setter == null;
        }



        private void Init(
            Func<Expression, MemberExpression> getMember,
            Func<Expression, Expression, MethodCallExpression> makeCallExpression,
            out Func<object, object> getter,
            ref Action<object, object> setter)
        {
            var exObjParam = Expression.Parameter(typeof(object), "theObject");
            var exValParam = Expression.Parameter(typeof(object), "theProperty");

            var exObjConverted = Expression.Convert(exObjParam, this.DeclaringType);
            var exValConverted = Expression.Convert(exValParam, this.ValueType);

            Expression exMember = getMember(exObjConverted);

            Expression getterMember = ValueType.IsValueType ? Expression.Convert(exMember, typeof(object)) : exMember;
            getter = Expression.Lambda<Func<object, object>>(getterMember, exObjParam).Compile();

            if (makeCallExpression != null)
            {
                Expression exAssignment = makeCallExpression(exObjConverted, exValConverted);
                setter = Expression.Lambda<Action<object, object>>(exAssignment, exObjParam, exValParam).Compile();
            }
        }
    }



}
