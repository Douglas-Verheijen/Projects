using System;

namespace Liquid.Library.Domain.Philosophy
{
    public class PhilosophyNote //: Entity
    {
        public virtual bool IsQuote
        {
            get { return !string.IsNullOrEmpty(QuoteAuthor); }
        }

        public virtual string Note { get; set; }

        public virtual DateTimeOffset? OriginallyDated { get; set; }

        public virtual string QuoteAuthor { get; set; }
    }
}
