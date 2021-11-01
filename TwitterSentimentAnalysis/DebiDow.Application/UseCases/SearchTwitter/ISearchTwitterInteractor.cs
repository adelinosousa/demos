using System.Threading.Tasks;

namespace DebiDow.Application.UseCases.SearchTwitter
{
    public interface ISearchTwitterInteractor
    {
        Task<SearchTwitterResponse> Handle(SearchTwitterRequest request);
    }
}
