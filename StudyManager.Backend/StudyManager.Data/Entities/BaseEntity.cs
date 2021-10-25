using System;

namespace StudyManager.Data.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

        public BaseEntity()
        {
            this.Id = Guid.NewGuid();
            DateCreated = DateTime.Now;
        }
    }
}