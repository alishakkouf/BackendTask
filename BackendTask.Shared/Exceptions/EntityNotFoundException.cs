using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Shared.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public string EntityId { get; }

        public string EntityName { get; }

        /// <summary>
        /// Exception for entity (name) not found. 
        /// </summary>
        public EntityNotFoundException(string name) : this(name, "")
        {
        }

        /// <summary>
        /// Exception for entity not found, name (entityName) and optional id (entityId) . 
        /// </summary>
        public EntityNotFoundException(string name, string id)
            : base($"Entity {name}{(!string.IsNullOrEmpty(id) ? " (" + id + ")" : "")} was not found")
        {
            EntityName = name;

            EntityId = id;
        }

        public EntityNotFoundException(string localizedMessage, string name, long id) : base(string.Format(localizedMessage, name, id))
        {
            EntityName = name;
            EntityId = id.ToString();
        }
    }
}
