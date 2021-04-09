﻿// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Xml.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Geta.SEO.Sitemaps.Repositories;
using Geta.SEO.Sitemaps.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Geta.SEO.Sitemaps.XML
{
    public class MobileSitemapXmlGenerator : SitemapXmlGenerator, IMobileSitemapXmlGenerator
    {
        public MobileSitemapXmlGenerator(
            ISitemapRepository sitemapRepository,
            IContentRepository contentRepository,
            UrlResolver urlResolver,
            ISiteDefinitionRepository siteDefinitionRepository,
            ILanguageBranchRepository languageBranchRepository,
            IContentFilter contentFilter,
            IMemoryCache cache,
            ILogger<SitemapXmlGenerator> logger)
            : base(sitemapRepository, contentRepository, urlResolver, siteDefinitionRepository, languageBranchRepository, contentFilter, cache, logger)
        {
        }

        protected XNamespace MobileNamespace => @"http://www.google.com/schemas/sitemap-mobile/1.0";

        protected override XElement GenerateSiteElement(IContent contentData, string url)
        {
            var element = base.GenerateSiteElement(contentData, url);

            // add <mobile:mobile/> to standard sitemap url element
            element.Add(new XElement(MobileNamespace + "mobile"));

            return element;
        }

        protected override XElement GenerateRootElement()
        {
            var element = base.GenerateRootElement();

            element.Add(new XAttribute(XNamespace.Xmlns + "mobile", MobileNamespace));

            return element;
        }
    }
}
