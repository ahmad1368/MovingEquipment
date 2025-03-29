using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Data.Repositories;
using Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Filters;

namespace WebFramework.Api
{
   
    public class CrudController<TDto, TSelectDto, TEntity, TKey> : BaseController
        where TDto : BaseDto<TDto, TEntity, TKey>, new()
        where TSelectDto : BaseDto<TSelectDto, TEntity, TKey>, new()
        where TEntity : BaseEntity<TKey>, new()
    {
        private IRepository<TEntity> repository;
        private UserManager<User> userManager;
        private readonly IMapper Mapper;


        public CrudController(IRepository<TEntity> repository, UserManager<User> userManager, IMapper mapper)
        {
            this.repository = repository;
            this.userManager = userManager;

            this.Mapper = mapper;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ApiResult<List<TSelectDto>>> Get()
        {

            var Entitys = await repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider)
                .ToListAsync();

            return Entitys;

        }

        [HttpGet("{Id}")]

        public virtual async Task<ApiResult<TSelectDto>> Get(TKey Id, CancellationToken cancellationToken)
        {
            var Entity = await repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(Id), cancellationToken);
            if (Entity == null)
            {
                throw new NotFoundException("entity Not Found");
            }


            return Entity;
        }

        [HttpPost]
        public virtual async Task<ApiResult<TEntity>> Create(TDto dto, CancellationToken cancellationToken)
        {
            var NewEntity = dto.ToEntity();
            await repository.AddAsync(NewEntity, cancellationToken);
            return NewEntity;

        }

        [HttpPut]
        public virtual async Task<ApiResult<TEntity>> Update(TDto dto, CancellationToken cancellationToken)
        {

            var updateEntity = await repository.GetByIdAsync(cancellationToken, dto.Id);

            updateEntity = dto.ToEntity(updateEntity);

            await repository.UpdateAsync(updateEntity, CancellationToken.None);
            return Ok(updateEntity);
        }
        [HttpDelete]
        public virtual async Task<ApiResult> Delete(TKey id, CancellationToken cancellationToken)
        {

            var Entity = await repository.GetByIdAsync(cancellationToken, id);
            await repository.DeleteAsync(Entity, CancellationToken.None);
            return Ok();
        }

    }

   

    public class CrudController<TDto, TSelectDto, TEntity> : CrudController<TDto, TSelectDto, TEntity, Guid>
    where TDto : BaseDto<TDto, TEntity, Guid>, new()
    where TSelectDto : BaseDto<TSelectDto, TEntity, Guid>, new()
    where TEntity : BaseEntity<Guid>, new()
    {
        public CrudController(IRepository<TEntity> repository, UserManager<User> userManager, IMapper mapper) : base(repository, userManager, mapper)
        {
        }
    }

    public class CrudController<TDto, TEntity> : CrudController<TDto, TDto, TEntity, Guid>
        where TDto : BaseDto<TDto, TEntity, Guid>, new()
        where TEntity : BaseEntity<Guid>, new()
    {
        public CrudController(IRepository<TEntity> repository, UserManager<User> userManager, IMapper mapper) : base(repository, userManager, mapper)
        {
        }
    }

}
