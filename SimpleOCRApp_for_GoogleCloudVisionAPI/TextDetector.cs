using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using Newtonsoft.Json;

namespace GoogleCloudVisionAPI
{
    class GCPVisonAPI
    {
        private VisionService CreateAuthorizedClient()
        {
            GoogleCredential credential =
                GoogleCredential.GetApplicationDefaultAsync().Result;
            // Inject the Cloud Vision scopes
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    VisionService.Scope.CloudPlatform
                });
            }
            return new VisionService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                GZipEnabled = false
            });
        }

        private int DetectTextWord(
            VisionService vision, byte[] getImage, ref string FullText)
        {
            int result = 1;
            Console.WriteLine("Detecting image to texts...");
            // Convert image to Base64 encoded for JSON ASCII text based request
            string imageContent = Convert.ToBase64String(getImage);

            try
            {
                // Post label detection request to the Vision API
                var responses = vision.Images.Annotate(
                    new BatchAnnotateImagesRequest()
                    {
                        Requests = new[] {
                    new AnnotateImageRequest() {
                        Features = new [] { new Feature() { Type =
                          "TEXT_DETECTION"}},
                        Image = new Image() { Content = imageContent }
                    }
                   }
                    }).Execute();

                if (responses.Responses != null)
                {
                    FullText = responses.Responses[0].TextAnnotations[0].Description;

                    Console.WriteLine("SUCCESS：Cloud Vision API Access.");
                    result = 0;
                }
                else
                {
                    FullText = "";
                    Console.WriteLine("ERROR : No text found.");
                    result = -1;
                }
            }
            catch
            {
                FullText = "";
                Console.WriteLine("ERROR : Not Access Cloud Vision API.");
                result = -1;
            }

            return result;
        }

        public int getTextAndRoi(byte[] getImage, ref string FullText)
        {
            int result = 1;
            GCPVisonAPI sample = new GCPVisonAPI();
            // Create a new Cloud Vision clieuthorint azed via Application
            // Default Credentials
            VisionService vision = sample.CreateAuthorizedClient();
            // Use the client to get label annotations for the given image
            string getFullText = "";
            result = sample.DetectTextWord(vision, getImage, ref getFullText);
            FullText = getFullText;
            return result;
        }
    }
}
