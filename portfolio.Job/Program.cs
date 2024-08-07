using System.Text.Json.Serialization;
using MailKit.Net.Smtp;
using MimeKit;
using MongoDB.Bson;
using MongoDB.Driver;
using RestSharp;


var expirationDaysRange = -1;
var now = DateTime.Now;
now = new DateTime(now.Year, now.Month, now.Day, hour: 23, minute: 59, second: 59).AddDays(expirationDaysRange);
var investmentsCollection = "Investments";

var mongoClient = new MongoClient("mongodb://db:27017");
var _investmentCollection = mongoClient.GetDatabase("XPInc").GetCollection<Entities.Investment>(investmentsCollection);

// var filter = Builders<Entities.Investment>.Filter.Lte(i => i.Epiration, now);
var investmentsAboutToExpire = "";
foreach (var i in await _investmentCollection.Find(i => i.Expiration <= now).ToListAsync())
{
    investmentsAboutToExpire += $"Investimento: {i.Name} - Expira em: {i.Expiration}\n";
}

// https://api.sendpulse.com/oauth/access_token
// id = 57aa5358d36177dc868ddd64553b11ad
// secret = 79efa48c9ebf4e3d4a3c0ddaa3266e25

var mailClient = new RestSharp.RestClient("https://api.sendpulse.com");
var tokenResponse = await mailClient.PostAsync<TokenResponse>(new RestRequest("oauth/access_token").
AddParameter("grant_type", "client_credentials").
AddParameter("client_id", "237b4af9c99d0f89bdbd876dcd5a0000").
AddParameter("client_secret", "a99e7d506d3701c5c04de3db1913eeee"));

// https://api.sendpulse.com/smtp/emails
var body = /*lang=json,strict*/ """
{
  "email": {
    "text": "",
    "subject": "Investimentos Que Expirarao",
    "from": {
      "email": "portfolio.management@xpinc.com"
    },
    "to": [
      {
        "email": "recipient1@example.com"
      }
    ]
  }
}
""";
await mailClient.PostAsync(new RestRequest("smtp/emails").AddJsonBody(body));

System.Console.WriteLine(investmentsAboutToExpire);

class TokenResponse
{
    [JsonPropertyName("access_token")]
    public String AccessToken { get; set; }
}