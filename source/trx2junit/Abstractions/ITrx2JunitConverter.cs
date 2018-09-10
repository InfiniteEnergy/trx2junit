using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace trx2junit
{
    public interface ITrx2JunitConverter
    {
        /// <summary>
        /// Convert many TRX files to one junit XML file
        /// </summary>
        Task Convert(IEnumerable<Stream> trxInputs, TextWriter jUnitOutput);
    }
}
