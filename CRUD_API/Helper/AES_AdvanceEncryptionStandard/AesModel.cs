using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Helper.AES_AdvanceEncryptionStandard
{
    public class AesModel
    {
        private readonly IHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public AesModel(IHostEnvironment env, IConfiguration config)
        {
            _env = env;
            Configuration = config;
        }

        public async Task<dynamic> Encription(string data)
        {

            //var audienceConfig = Configuration.GetSection("Audience");
            //string key = audienceConfig["sec"].Trim();
            //string iv = audienceConfig["sec"].Trim();

            if (_env.IsDevelopment())
            {
                var datas = await Task.FromResult(AesOperation.EncryptionStringWithOutKey(data));
                return datas;
            }
            else
            {
                return await Task.FromResult(data);
            }


        }


    }
}
