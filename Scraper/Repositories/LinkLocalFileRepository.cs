﻿using OfferScrapper.DataStructs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace OfferScrapper.Repositories
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
                    var link = new Link(linkElements[0], linkElements[1], linkElements[1].Contains("otodom") ? LinkKind.OtoDom : LinkKind.Olx);
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

        public IQueryable<Link> SearchFor(Expression<Func<Link, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

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
    }
}
