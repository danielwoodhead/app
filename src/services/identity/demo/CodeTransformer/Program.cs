using System.Text;
using IdentityModel;
using IdentityServer4.Models;

namespace CodeTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var codeVerifier = "c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646";
            var codeVerifierBytes = Encoding.ASCII.GetBytes(codeVerifier);
            var hashedBytes = codeVerifierBytes.Sha256();
            var transformedCodeVerifier = Base64Url.Encode(hashedBytes);
        }
    }
}
