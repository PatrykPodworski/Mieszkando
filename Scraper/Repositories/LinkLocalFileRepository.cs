using MarklogicDataLayer.DataStructs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using MarklogicDataLayer;
using MarklogicDataLayer.XQuery;

namespace OfferScraper.Repositories
{
    public class LinkLocalFileRepository : IDataRepository<Link>
    {
        private static string FileName => "links";
        private static string Extension => ".txt";

        public EnumerableQuery<Link> EntityCollection { get; set; }

        #region IDataRepository<Link> members

        public void Delete(Link entity)
        {
            if (!File.Exists($"{FileName}{Extension}"))
                return;

            string line = null;
            using (var reader = new StreamReader($"{FileName}{Extension}"))
            using (var writer = new StreamWriter($"{FileName}Temp{Extension}"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.Compare(line, entity.ToString()) == 0)
                        continue;

                    writer.WriteLine(line);
                }
            }

            MoveFromTempFile();
        }

        public IQueryable<Link> GetAll()
        {
            if (!File.Exists($"{FileName}{Extension}"))
                return new EnumerableQuery<Link>(new List<Link>());

            using (var reader = new StreamReader($"{FileName}{Extension}"))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    var linkElements = line.Split('|');
                    var link = new Link
                    {
                        Uri = linkElements[1],
                        LinkSourceKind = linkElements[1].Contains("otodom") ? OfferType.OtoDom : OfferType.Olx,
                        Status = Status.New,
                        LastUpdate = DateTime.Now,
                    };
                    EntityCollection.Append(link);
                }
            }

            return EntityCollection;
        }

        public Link GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Link entity)
        {
            using (var fileStream = new StreamWriter($"{FileName}{Extension}", true))
            {
                fileStream.WriteLine(entity.ToString());
            }
        }

        #endregion IDataRepository<Link> members

        public static int GetMaxId()
        {
            if (!File.Exists($"{FileName}{Extension}"))
                return 1;

            return int.Parse(File.ReadLines($"{FileName}{Extension}").Last().Split('|').First());
        }

        private void MoveFromTempFile()
        {
            File.SetAttributes($"{FileName}{Extension}", FileAttributes.Normal);
            File.Delete($"{FileName}{Extension}");
            File.Move($"{FileName}Temp{Extension}", $"{FileName}{Extension}");
            File.Delete($"{FileName}Temp{Extension}");
        }

        public void Insert(Link entity, ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<Link> entities, ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void Update(Link entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Link entity, ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<Link> entities, ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public ITransaction GetTransaction()
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<Link> entities)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Link> Get(MarklogicDataLayer.XQuery.Expression expression, long numberOfElements)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<Link> entities)
        {
            throw new NotImplementedException();
        }
    }
}