﻿// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fireasy.Common.Ioc
{
    internal static class Helpers
    {
        internal static void CheckConstructable(Type serviceType)
        {
            string errorMessage;
            if (!IsConcreteConstructableType(serviceType, out errorMessage))
            {
                throw new Exception(errorMessage);
            }
        }

        internal static void CheckCollectionHasEmptyItem(IEnumerable collection, Type serviceType)
        {
            var hasNull = collection.Cast<object>().Any(c => c == null);

            if (hasNull)
            {
                throw new Exception(SR.GetString(SRKind.CollectionHasEmptyItem));
            }
        }

        internal static Action<T> CreateAction<T>(object action)
        {
            var actionArgumentType = action.GetType().GetGenericArguments()[0];

            var objParameter = Expression.Parameter(typeof(T), "s");

            var instanceInitializer = Expression.Lambda<Action<T>>(
                Expression.Invoke(
                    Expression.Constant(action),
                    new[] { Expression.Convert(objParameter, actionArgumentType) }),
                new ParameterExpression[] { objParameter });

            return instanceInitializer.Compile();
        }

        internal static bool IsConcreteConstructableType(Type serviceType)
        {
            string errorMesssage;

            return IsConcreteConstructableType(serviceType, out errorMesssage);
        }

        private static bool HasSinglePublicConstructor(Type serviceType)
        {
            return serviceType.GetConstructors().Length == 1;
        }

        private static bool HasConstructorWithOnlyValidParameters(Type serviceType)
        {
            return GetFirstInvalidConstructorParameter(serviceType) == null;
        }

        private static ParameterInfo GetFirstInvalidConstructorParameter(Type serviceType)
        {
            return (
                from constructor in serviceType.GetConstructors()
                from parameter in constructor.GetParameters()
                let type = parameter.ParameterType
                where type.IsValueType || type == typeof(string)
                select parameter).FirstOrDefault();
        }

        private static bool IsConcreteConstructableType(Type serviceType, out string errorMessage)
        {
            errorMessage = null;

            if (!serviceType.IsConcreteType())
            {
                errorMessage = SR.GetString(SRKind.NotConcreteType);
                return false;
            }

            if (!HasSinglePublicConstructor(serviceType))
            {
                errorMessage = SR.GetString(SRKind.NoDefaultConstructor);
                return false;
            }

            if (!HasConstructorWithOnlyValidParameters(serviceType))
            {
                var invalidParameter = GetFirstInvalidConstructorParameter(serviceType);
                errorMessage = SR.GetString(SRKind.ConstructorHasParameterOfValueType, invalidParameter.Name);
                return false;
            }

            return true;
        }
    }
}
