﻿using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReportDesignerServerSide
{
    public class CustomReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        readonly string ReportDirectory;
        const string FileExtension = ".repx";
        public CustomReportStorageWebExtension(IWebHostEnvironment env)
        {
            ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
        }

        public override bool CanSetData(string url) { return true; }
        public override bool IsValidUrl(string url) { return true; }
        public override byte[] GetData(string url)
        {
            try
            {
                if (Directory.EnumerateFiles(ReportDirectory).
                    Select(Path.GetFileNameWithoutExtension).Contains(url))
                {
                    return File.ReadAllBytes(Path.Combine(ReportDirectory, url + FileExtension));
                }
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
                    string.Format("Could not find report '{0}'.", url));
            }
            catch (Exception)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
                    string.Format("Could not find report '{0}'.", url));
            }
        }

        public override Dictionary<string, string> GetUrls()
        {
            return Directory.GetFiles(ReportDirectory, "*" + FileExtension)
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToDictionary<string, string>(x => x);
        }

        public override void SetData(XtraReport report, string url)
        {
            report.SaveLayoutToXml(Path.Combine(ReportDirectory, url + FileExtension));
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            SetData(report, defaultUrl);
            return defaultUrl;
        }
    }
}