using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Show_List.Base
{
    public class LanguageTranslationRepository : Repository<LanguageTranslation>
    {
        public LanguageTranslationRepository(ISession session)
            : base(session)
        {
        }
    }
}
