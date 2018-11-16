using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO.Compression;
using System.Runtime.Remoting.Messaging;
using Common;


namespace NugetHttpModule
{
    public class CheckPlatformHttpModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(BeginRequest);
        }

        public void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            var request = context.Request;

            if (request.HttpMethod.ToLower().Equals("put") && request.Path.ToLower().Equals("/nuget/"))
            {
                var temporaryFile = Path.GetTempFileName();
                var tempDir = Path.GetTempPath() + Path.GetRandomFileName();

                request.Files[0].SaveAs(temporaryFile);
                ZipFile.ExtractToDirectory(temporaryFile, tempDir);

                var files = new List<string>();
                FileOperation.GetFiles(tempDir, "*.dll", ref files);

                var result = FileOperation.CheckFiles(files);

                File.Delete(temporaryFile);
                DirectoryInfo di = new DirectoryInfo(tempDir);
                di.Delete(true);

                if(!result) EndRequest(context);
            }
        }

        void EndRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            response.StatusDescription = "不允许上传x86版本的包，请把项目>生成>平台目标设置为Any CPU";
            response.StatusCode = (int) HttpStatusCode.UnsupportedMediaType;
            response.Write("不允许上传x86版本的包，请把项目>生成>平台目标设置为Any CPU");
            response.End();
        }

        public void Dispose()
        {

        }
    }
}
