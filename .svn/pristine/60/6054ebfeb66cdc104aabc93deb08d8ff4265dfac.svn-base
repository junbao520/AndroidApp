//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using DCommon.Data;
//using TwoPole.Chameleon3.Domain;

//namespace TwoPole.Chameleon3.Infrastructure
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
//            Mapper.CreateMap<IDataReader, DeductionRule>()
//                .ForCreatedOnEntity()
//                .ForMember(dest => dest.DeductedScores, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("DeductedScores"))))
//                .ForMember(dest => dest.ExamItemId, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("ExamItemId"))))
//                ;
//            Mapper.CreateMap<IDataReader, ExamItem>()
//                .ForCreatedOnEntity()
//                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("SequenceNumber"))))
//                .ForMember(dest => dest.MapPointType, opt => opt.MapFrom(src => (MapPointType)src.GetInt32(src.GetOrdinal("MapPointType"))));
//            Mapper.CreateMap<IDataReader, LightExamItem>()
//                .ForCreatedOnEntity();
//            Mapper.CreateMap<IDataReader, LightRule>()
//                //.ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("Order"))))
//                .ForCreatedOnEntity();
//            Mapper.CreateMap<IDataReader, MapLine>()
//                .ForCreatedOnEntity();
//            Mapper.CreateMap<IDataReader, MapLinePoint>()
//                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("SequenceNumber"))))
//                .ForModifiedOnEntity();
//            Mapper.CreateMap<IDataReader, Setting>()
//                .ForModifiedOnEntity();
//            Mapper.CreateMap<IDataReader, VehicleModel>()
//                .ForCreatedOnEntity();
//            Mapper.CreateMap<IDataReader, VehicleModelGearSetting>()
//                .ForMember(dest => dest.Gear, opt => opt.MapFrom(src => (MapPointType)src.GetInt32(src.GetOrdinal("Gear"))))
//                .ForMember(dest => dest.MinRatio, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("MinRatio"))))
//                .ForMember(dest => dest.MaxRatio, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("MaxRatio"))))
//                .ForMember(dest => dest.VehicleModelId, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("VehicleModelId"))))
//                .ForCreatedOnEntity();

//            Mapper.AssertConfigurationIsValid();
//        }

//        private static IMappingExpression<IDataReader, TDestination> ForCreatedOnEntity<TDestination>(
//            this IMappingExpression<IDataReader, TDestination> config)
//            where TDestination : CreatedOnEntity<int>
//        {
//            config.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("Id"))));
//            config.ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.GetDateTime(src.GetOrdinal("CreatedOn"))));
//            return config;
//        }

//        private static IMappingExpression<IDataReader, TDestination> ForModifiedOnEntity<TDestination>(this IMappingExpression<IDataReader, TDestination> config)
//           where TDestination : ModifiedOnEntity<int>
//        {
//            config.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("Id"))));
//            config.ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.GetDateTime(src.GetOrdinal("CreatedOn"))));
//            config.ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => src.GetDateTime(src.GetOrdinal("ModifiedOn"))));
//            return config;
//        }

//        private static IMappingExpression<TSource, TDestination>
//           IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
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

//        private static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config, Expression<Func<TDestination, object>> ignoreMemeber)
//        {
//            if (ignoreMemeber != null)
//            {
//                config.ForMember(ignoreMemeber, mo => mo.Ignore());
//            }
//            return config;
//        }

//        private static IMappingExpression<TSource, TDestination> Ignores<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config, params Expression<Func<TDestination, object>>[] ignoreMemebers)
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
//        #endregion

//        public static IEnumerable<T> ToEntities<T>(this IDataReader reader)
//        {
//            var entities = Mapper.Map<IDataReader, IEnumerable<T>>(reader);
//            return entities;
//        }

//        public static T ToEntity<T>(this IDataReader reader)
//        {
//            var entity = Mapper.Map<IDataReader, T>(reader);
//            return entity;
//        }

//        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination target)
//        {
//            return Mapper.Map(source, target);
//        }

//        public static TDestination MapTo<TDestination>(this object source)
//        {
//            return Mapper.Map<TDestination>(source);
//        }
//    }
//}
