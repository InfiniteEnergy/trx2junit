using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class Trx2JunitConverter : ITrx2JunitConverter
    {
        public async Task Convert(IEnumerable<Stream> trxInputs, TextWriter jUnitOutput){
            var tests = await Task.WhenAll(trxInputs.Select(Parse));

            var jUnitBuilder = new JUnitBuilder(tests);
            jUnitBuilder.Build();

            await jUnitBuilder.Result.SaveAsync(jUnitOutput, SaveOptions.None, CancellationToken.None);
        }

        private static async Task<Test> Parse(Stream trxInput){
            XElement trx = await XElement.LoadAsync(trxInput, LoadOptions.None, CancellationToken.None);
            var trxParser = new TrxParser(trx);
            trxParser.Parse();
            return trxParser.Result;
        }
    }
}
