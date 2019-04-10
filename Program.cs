using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace WebRequestApp
{
  public class Result
  {
    public string resource_uri { get; set; }
    public int id { get; set; }
    public string absolute_url { get; set; }
    public List<object> panel { get; set; }
    public List<object> non_participating_judges { get; set; }
    public string docket { get; set; }
    public List<string> sub_opinions { get; set; }
    public List<object> citations { get; set; }
    public string judges { get; set; }
    public DateTime date_created { get; set; }
    public DateTime date_modified { get; set; }
    public string date_filed { get; set; }
    public bool date_filed_is_approximate { get; set; }
    public string slug { get; set; }
    public string case_name_short { get; set; }
    public string case_name { get; set; }
    public string case_name_full { get; set; }
    public string federal_cite_one { get; set; }
    public string federal_cite_two { get; set; }
    public string federal_cite_three { get; set; }
    public string state_cite_one { get; set; }
    public string state_cite_two { get; set; }
    public string state_cite_three { get; set; }
    public string state_cite_regional { get; set; }
    public string specialty_cite_one { get; set; }
    public string scotus_early_cite { get; set; }
    public string lexis_cite { get; set; }
    public string westlaw_cite { get; set; }
    public string neutral_cite { get; set; }
    public string scdb_id { get; set; }
    public object scdb_decision_direction { get; set; }
    public object scdb_votes_majority { get; set; }
    public object scdb_votes_minority { get; set; }
    public string source { get; set; }
    public string procedural_history { get; set; }
    public string attorneys { get; set; }
    public string nature_of_suit { get; set; }
    public string posture { get; set; }
    public string syllabus { get; set; }
    public int citation_count { get; set; }
    public string precedential_status { get; set; }
    public object date_blocked { get; set; }
    public bool blocked { get; set; }
  }

  public class DocketResponse
  {
    public int count { get; set; }
    public string next { get; set; }
    public object previous { get; set; }
    public List<Result> results { get; set; }
  }
  class Program
  {
    static void Main(string[] args)
    {
      //curl  --header 'Authorization: Token 196d3ced7796a93a8100edf12f2fbea9df491b35'

      WebRequest request = WebRequest.Create("https://www.courtlistener.com/api/rest/v3/dockets?page=2");
      request.Headers.Add(HttpRequestHeader.Authorization, "Token 196d3ced7796a93a8100edf12f2fbea9df491b35");
      // If required by the server, set the credentials.
      request.Credentials = CredentialCache.DefaultCredentials;
      // Get the response.
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      // Display the status.
      Console.WriteLine(response.StatusDescription);
      // Get the stream containing content returned by the server.
      Stream dataStream = response.GetResponseStream();
      // Open the stream using a StreamReader for easy access.
      StreamReader reader = new StreamReader(dataStream);
      // Read the content.
      string responseFromServer = reader.ReadToEnd();
      // Display the content.
      // Cleanup the streams and the response.
      reader.Close();
      dataStream.Close();
      response.Close();

      Console.WriteLine(responseFromServer);
      var result = JsonConvert.DeserializeObject<DocketResponse>(responseFromServer);
      Console.WriteLine(result.count);
      foreach (var dockets in result.results)
      {
        Console.WriteLine(dockets.case_name);
      }
      Console.WriteLine(response);

      //   POSTing Data Pulled from Court Listener
      //  I need to change the response to take in the response from server of the previous webrequest
      WebRequest requestOut = WebRequest.Create("https://localhost:5001/api/Dockets");
      //   requestOut.Headers.Add(HttpRequestHeader.Authorization, "Token 196d3ced7796a93a8100edf12f2fbea9df491b35");
      // If required by the server, set the credentials.
      requestOut.Credentials = CredentialCache.DefaultCredentials;
      // Change Method
      requestOut.Method = "POST";

      // Set request length
      // requestOut.ContentLength = result.Length; 

      // Specify Content Type
      requestOut.ContentType = "application/x-www-form-urlencoded";

      // Get the response.
      HttpWebResponse responseOut = (HttpWebResponse)requestOut.GetResponse();
      // Display the status.
      Console.WriteLine(responseOut.StatusDescription);
      // Get the stream containing content returned by the server.
      Stream dataStreamOut = requestOut.GetRequestStream();
      //   Stream dataStreamOut = responseOut.GetResponseStream();
      // Open the stream using a StreamReader for easy access.
      StreamReader readerOut = new StreamReader(dataStreamOut);
      // Read the content.
      //   string responseFromServerOut = readerOut.ReadToEnd();

      dataStreamOut.Write(result, 0, result.Length);
      // Display the content.
      // Cleanup the streams and the response.
      readerOut.Close();
      dataStreamOut.Close();
      responseOut.Close();
    }
  }
}
