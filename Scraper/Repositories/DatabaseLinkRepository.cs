﻿using MarklogicDataLayer.DataStructs;
using System;
using System.Linq;
using System.Linq.Expressions;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer;
using MarklogicDataLayer.XQuery.Functions;
using MarklogicDataLayer.XQuery;
using System.Net.Http;
using System.Xml.Linq;
using System.Collections.Generic;

namespace OfferScraper.Repositories
{
    public class DatabaseLinkRepository : IDataRepository<Link>
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly RestConnector _restConnector;

        public DatabaseLinkRepository(IDatabaseConnectionSettings databaseConnectionSettings)
        {
            _databaseConnectionSettings = databaseConnectionSettings;
            _restConnector = new RestConnector(_databaseConnectionSettings);
        }

        public void Delete(Link entity)
        {
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? "Olx" : "OtoDom";
            var query = new XdmpDocumentDelete(new MlUri($"{linkKind}_link_{entity.Id}", MlUriDocumentType.Xml)).Query;
            _restConnector.Submit(query);
        }

        public IQueryable<Link> GetAll()
        {
            var query = new CtsSearch("/", new CtsCollectionQuery(new[] { "Olx", "OtoDom" })).Query;
            var response = _restConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return new List<Link>().AsQueryable();

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            var result = new List<Link>();
            foreach (var data in content)
            {
                var text = ReadAsString(data); 
                var xml = XDocument.Parse(text);
                var linkId = xml.Descendants().Where(x => x.Name == "link_id").First().Value;
                var linkUri = xml.Descendants().Where(x => x.Name == "uri").First().Value;
                var linkKind = xml.Descendants().Where(x => x.Name == "link_kind").First().Value == "Olx" ? OfferType.Olx : OfferType.OtoDom;
                result.Add(new Link(linkId, linkUri, linkKind));
            }
            return result.AsQueryable();
        }

        public Link GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery("link_id", id.ToString())).Query;
            var response = _restConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                var xml = XDocument.Parse(text);
                var linkId = xml.Descendants().Where(x => x.Name == "link_id").First().Value;
                if (linkId == id.ToString())
                {
                    var linkUri = xml.Descendants().Where(x => x.Name == "uri").First().Value;
                    var linkKind = xml.Descendants().Where(x => x.Name == "link_kind").First().Value == "Olx" ? OfferType.Olx : OfferType.OtoDom;
                    return new Link(linkId, linkUri, linkKind);
                }
            }

            return null;
        }

        public void Insert(Link entity)
        {
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? "Olx" : "OtoDom";
            var content = MarklogicContent.Xml($"{linkKind}_link_{entity.Id}", $"<link><link_id>{entity.Id}</link_id><uri>{entity.Uri}</uri><link_kind>{linkKind}</link_kind></link>", new[] { linkKind });
            _restConnector.Insert(content);
        }

        public IQueryable<Link> SearchFor(Expression<Func<Link, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        private static string ReadAsString(HttpContent content)
        {
            return content.ReadAsStringAsync().Result;
        }
    }
}