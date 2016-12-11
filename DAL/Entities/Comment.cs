using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Comment : IEntity<long>
    {
        public long ID { get; set; }
        public string Text { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

        [NotMapped]
        public bool HasParent => ParentComment != null;

        public virtual User Author { get; set; }
        public virtual Auction Auction { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> ChildComments { get; set; } = new HashSet<Comment>();

    }
}
