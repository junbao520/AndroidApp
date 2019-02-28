//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using System.Data.Common;

//namespace TwoPole.Chameleon3.Foundation
//{
//    public static class MappingExtensions
//    {
//        #region Initialize
//        static MappingExtensions()
//        {
//            Initialize();
//        }

//        private static void Initialize()
//        {
//            Mapper.AssertConfigurationIsValid();
//        }
//        #endregion

//        internal static IEnumerable<T> ToEntities<T>(this IDataReader reader)
//        {
//            var entities = Mapper.Map<IDataReader, IEnumerable<T>>(reader);
//            return entities;
//        }

//        internal static T ToEntity<T>(this DbDataReader reader)
//        {
//            var entity = Mapper.Map<IDataReader, T>(reader);
//            return entity;
//        }

//        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
//        {
//            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
//            var sourceType = typeof(TSource);
//            var destinationProperties = typeof(TDestination).GetProperties(flags);

//            foreach (var property in destinationProperties)
//            {
//                if (sourceType.GetProperty(property.Name, flags) == null)
//                {
//                    expression.ForMember(property.Name, opt => opt.Ignore());
//                }
//            }
//            return expression;
//        }

//        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config, Expression<Func<TDestination, object>> ignoreMemeber)
//        {
//            if (ignoreMemeber != null)
//            {
//                config.ForMember(ignoreMemeber, mo => mo.Ignore());
//            }
//            return config;
//        }

//        public static IMappingExpression<TSource, TDestination> Ignores<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config, params Expression<Func<TDestination, object>>[] ignoreMemebers)
//        {
//            if (ignoreMemebers != null)
//            {
//                foreach (var ignoreMemeber in ignoreMemebers)
//                {
//                    config.ForMember(ignoreMemeber, mo => mo.Ignore());
//                }
//            }
//            return config;
//        }
//    }
//}
