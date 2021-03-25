using System;
using Realms;

namespace TakeMyAdvice.Models
{
    public class AdviceModel : RealmObject
    {
        public AdviceModel()
        {
        }

        public string AdviceType { get; set; }
        [PrimaryKey]
        public long AdviceNumber { get; set; }
        public string AdviceMessage { get; set; }
    }
}
