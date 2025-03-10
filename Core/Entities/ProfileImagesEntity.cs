using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProfileImagesEntity
    {
        public required Guid Id {get; set; }
        public string ProfileImageUrl { get; set; }
        public string ImagePublicId { get; set; }
    }
}
