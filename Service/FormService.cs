using Microsoft.AspNetCore.Cors.Infrastructure;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;

        public FormService(IFormRepository formRepository) 
        {
            _formRepository = formRepository; 
        }

        public async Task<IEnumerable<FormDto>> GetFormList()
        {
            var forms = await _formRepository.GetForms();
            var result = forms.OrderBy(x => x.FormName).ToList();
            return result;
        }
    }
}
