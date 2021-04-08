﻿// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.ServiceLocation;
using Geta.SEO.Sitemaps.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Geta.SEO.Sitemaps.Controllers
{
    [Route("sitemapindex.xml")]
    public class GetaSitemapIndexController : Controller
    {
        private readonly ISitemapRepository _sitemapRepository;

        protected XNamespace SitemapXmlNamespace
        {
            get { return @"http://www.sitemaps.org/schemas/sitemap/0.9"; }
        }

        public GetaSitemapIndexController() : this(ServiceLocator.Current.GetInstance<ISitemapRepository>())
        {
        }

        public GetaSitemapIndexController(ISitemapRepository sitemapRepository)
        {
            _sitemapRepository = sitemapRepository;
        }

        [Route("", Name = "Sitemap index")]
        public ActionResult Index()
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", null));

            var indexElement = new XElement(SitemapXmlNamespace + "sitemapindex");

            foreach (var sitemapData in _sitemapRepository.GetAllSitemapData())
            {
                var sitemapElement = new XElement(
                    SitemapXmlNamespace + "sitemap",
                    new XElement(SitemapXmlNamespace + "loc", _sitemapRepository.GetSitemapUrl(sitemapData))
                );

                indexElement.Add(sitemapElement);
            }

            doc.Add(indexElement);

            byte[] sitemapIndexData;

            using (var ms = new MemoryStream())
            {
                var xtw = new XmlTextWriter(ms, Encoding.UTF8);
                doc.Save(xtw);
                xtw.Flush();
                sitemapIndexData = ms.ToArray();
            }

            return new FileContentResult(sitemapIndexData, "text/xml");
        }
    }
}