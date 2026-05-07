using NPetrovich;
using Stubble.Core.Builders;
using TemplateEngine.Models;

namespace TemplateEngine.Services
{
    public class ExplicitTemplateProcessor
    {
        public string ProcessTemplate(User user, string template)
        {
            var stubble = new StubbleBuilder().Build();
     
            var model = new
            {
                name = new
                {
                    nom = Petrovich().InflectFirstNameTo(Case.Nominative),
                    gen = Petrovich().InflectFirstNameTo(Case.Genitive),
                    dat = Petrovich().InflectFirstNameTo(Case.Dative),
                    acc = Petrovich().InflectFirstNameTo(Case.Accusative),
                    ins = Petrovich().InflectFirstNameTo(Case.Instrumental),
                    pre = Petrovich().InflectFirstNameTo(Case.Prepositional)
                },
                lastname = new
                {
                    nom = Petrovich().InflectLastNameTo(Case.Nominative),
                    gen = Petrovich().InflectLastNameTo(Case.Genitive),
                    dat = Petrovich().InflectLastNameTo(Case.Dative),
                    acc = Petrovich().InflectLastNameTo(Case.Accusative),
                    ins = Petrovich().InflectLastNameTo(Case.Instrumental),
                    pre = Petrovich().InflectLastNameTo(Case.Prepositional)
                },
                middlename = new
                {
                    nom = Petrovich().InflectMiddleNameTo(Case.Nominative),
                    gen = Petrovich().InflectMiddleNameTo(Case.Genitive),
                    dat = Petrovich().InflectMiddleNameTo(Case.Dative),
                    acc = Petrovich().InflectMiddleNameTo(Case.Accusative),
                    ins = Petrovich().InflectMiddleNameTo(Case.Instrumental),
                    pre = Petrovich().InflectMiddleNameTo(Case.Prepositional)
                }
            };
        
            return stubble.Render(template, model);
            
            Petrovich Petrovich()
            {
                var p = new Petrovich
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    AutoDetectGender = user.AutoDetectGender
                };
                if (!user.AutoDetectGender)
                    p.Gender = user.Gender;
                return p;
            }
        }
        
        
    }
}
