using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites
{
    public interface IEntity
    {

    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }

  
    public abstract class BaseEntity<TKey>:IEntity<TKey>
    {
        
        public BaseEntity()
        {
            IsActive = true;
        }
        public TKey Id { get; set; }

        [Required]
        public string InsertUser { get; set; }
        [Required]
        public DateTime InsertDate { get; set; }

        public string? UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string? DeleteUser { get; set; }

        public DateTime? DeleteDate { get; set; }

        public bool IsActive { get; set; }


    }
    public abstract class BaseEntity : BaseEntity<int>
    {

    }

}
