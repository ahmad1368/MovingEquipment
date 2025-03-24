using AutoMapper;
using Common.Utilities;
using Entites;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using WebFramework.CustomMapping;

namespace WebFramework.Api
{

    public abstract class BaseDto<TDto, TEntity, TKey> : IHaveCustomMapping
        where TDto : class, new()
        where TEntity : BaseEntity<TKey>, new()
    {
        
        private IMapper? _mapper;

        private IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    var serviceProvider = ServiceProviderHelper.Instance;
                    _mapper = serviceProvider.GetRequiredService<IMapper>();
                }
                return _mapper;


            }
        }

        [Display(Name = "ردیف")]
        public TKey Id { get; set; }

        public TEntity ToEntity()
        {
            TEntity entity = Mapper.Map<TEntity>(CastToDerivedClass(this));
            return SetBaseEntityFields(entity);     
        }

        public TEntity ToEntity(TEntity entity)
        {
            var newEntity = Mapper.Map(CastToDerivedClass(this), entity);
            return SetBaseEntityFields(newEntity);
        }

      
        public TDto FromEntity(TEntity model)
        {
            return Mapper.Map<TDto>(model);
        }

        protected TDto CastToDerivedClass(BaseDto<TDto, TEntity, TKey> baseInstance)
        {

            return Mapper.Map<TDto>(baseInstance);
        }

        public void CreateMappings(Profile profile)
        {
            var mappingExpression = profile.CreateMap<TDto, TEntity>();

            var dtoType = typeof(TDto);
            var entityType = typeof(TEntity);
            //Ignore any property of source (like Post.Author) that dose not contains in destination 
            foreach (var property in entityType.GetProperties())
            {
                if (dtoType.GetProperty(property.Name) == null)
                    mappingExpression.ForMember(property.Name, opt => opt.Ignore());
            }

            CustomMappings(mappingExpression.ReverseMap());
        }

        public virtual void CustomMappings(IMappingExpression<TEntity, TDto> mapping)
        {
        }

        private TEntity SetBaseEntityFields(TEntity entity)
        {
            bool isDerived = typeof(TEntity).IsSubclassOf(typeof(BaseEntity<TKey>));

            if (!isDerived)
                return entity;


            var UpdateEntity = entity as BaseEntity<TKey>;

            if ((typeof(TKey) == typeof(Guid) && Guid.Parse(Id.ToString()) == Guid.Empty)
                       ||
                       (typeof(TKey) == typeof(int) && int.Parse(Id.ToString()) == 0)
                       )
            {
                UpdateEntity.InsertUser = IdentityHelper.GetUsername();
            }

            if ((typeof(TKey) == typeof(Guid) && Guid.Parse(Id.ToString()) == Guid.Empty)
                   ||
                   (typeof(TKey) == typeof(int) && int.Parse(Id.ToString()) == 0)
                   )
            {
                UpdateEntity.InsertDate = DateTime.Now;
            }

            if ((typeof(TKey) == typeof(Guid) && Guid.Parse(Id.ToString()) != Guid.Empty)
                      ||
                      (typeof(TKey) == typeof(int) && int.Parse(Id.ToString()) != 0)
                      )
            {
                UpdateEntity.UpdateUser = IdentityHelper.GetUsername();
            }

            if ((typeof(TKey) == typeof(Guid) && Guid.Parse(Id.ToString()) != Guid.Empty)
                   ||
                   (typeof(TKey) == typeof(int) && int.Parse(Id.ToString()) != 0)
                   )
            {
                UpdateEntity.UpdateDate = DateTime.Now;
            }

            if ((typeof(TKey) == typeof(Guid) && Guid.Parse(Id.ToString()) == Guid.Empty)
                   ||
                   (typeof(TKey) == typeof(int) && int.Parse(Id.ToString()) == 0)
                   )
            {
                UpdateEntity.IsActive = true;
            }

            return UpdateEntity as TEntity;

        }

    }

    public abstract class BaseDto<TDto, TEntity> : BaseDto<TDto, TEntity, Guid>
        where TDto : class, new()
        where TEntity : BaseEntity<Guid>, new()
    {


    }
}
