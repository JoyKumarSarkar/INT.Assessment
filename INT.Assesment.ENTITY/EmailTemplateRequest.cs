using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Assessment.ENTITY
{
    internal class EmailTemplateRequest
    {
            public string Purpose { get; set; }
            public string RecipientName { get; set; }
            public string Tone { get; set; } // formal, friendly, professional
    }
}
