
using Stock_CMS.Models;

namespace Stock_CMS.Common
{
    public class NormalizeModel
    {
        public string? NormalizeString(string? str)
        {
            return str?.ToLower() switch
            {
                "undefined" or "null" or "" => null,
                _ => str?.Trim()
            };
        }
        public int? NormalizeInt(int? number)
        {
            return number == null ? 0 : number;
        }

        public DocDto FilterDoc(DocDto doc) 
        {
            doc.DeathCertiUrl = NormalizeString(doc.DeathCertiUrl);
            doc.NameAsPerDeathCerti = NormalizeString(doc.NameAsPerDeathCerti);
            doc.PlaceOfDeath = NormalizeString(doc.PlaceOfDeath);
            doc.VoterIdUrl = NormalizeString(doc.VoterIdUrl);
            doc.NameAsPerVoterId = NormalizeString(doc.NameAsPerVoterId);
            doc.AddressAsPerVoterId = NormalizeString(doc.AddressAsPerVoterId);

            return doc;
        }

    }
}
