using SohoWeb.Entity;
using Soho.Utility.DataAccess;

namespace SohoWeb.DataAccess
{
    public class DataAccessUtil
    {
        public static PagingInfoEntity ToPagingInfo(FilterBase filter)
        {
            PagingInfoEntity page = new PagingInfoEntity();
            page.MaximumRows = filter.PageSize;
            page.StartRowIndex = filter.ServicePageIndex * filter.PageSize;
            return page;
        }
    }
}
