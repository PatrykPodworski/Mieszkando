using MarklogicDataLayer.DataStructs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using MarklogicDataLayer.XQuery.Functions;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer;
using MarklogicDataLayer.XQuery;
using System.Xml.Linq;

namespace OfferScraper.Repositories
{
    public class DatabaseHtmlDataRepository : IDataRepository<HtmlData>
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly RestConnector _restConnector;

        public DatabaseHtmlDataRepository(IDatabaseConnectionSettings databaseConnectionSettings)
        {
            _databaseConnectionSettings = databaseConnectionSettings;
            _restConnector = new RestConnector(_databaseConnectionSettings);
        }

        public void Delete(HtmlData entity)
        {
            var offerType = entity.OfferType == OfferType.Olx ? "Olx" : "OtoDom";
            var query = new XdmpDocumentDelete(new MlUri($"{offerType}_html_data_{entity.Id}", MlUriDocumentType.Xml)).Query;
            _restConnector.Submit(query);
        }

        public IQueryable<HtmlData> GetAll()
        {
            throw new NotImplementedException();
        }

        public HtmlData GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(HtmlData entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<HtmlData> SearchFor(Expression<Func<HtmlData, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        private static HtmlData ExtractHtmlDataInfo(XDocument xml, string htmlDataId)
        {
            return new HtmlData
            {

            };
        }
    }
}
