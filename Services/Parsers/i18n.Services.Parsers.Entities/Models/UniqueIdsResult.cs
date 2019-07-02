using Karambolo.Common.Collections;
using System.Collections.Generic;

namespace i18n.Services.Parsers.Entities.Models
{
    public class UniqueIdsResult
    {
        public IOrderedDictionary<string, string> HTMLOriginalValues { get; set; }
        public IList<string> HTMLTextKeys { get; set; }
        public string UpdatedHTML { get; set; }
    }
}
