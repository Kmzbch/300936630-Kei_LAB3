using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DroppingBox.Models
{
    public class S3Manager
    {
        private AmazonS3Client s3Client;

        public S3Manager(String accessId, String secretKey, RegionEndpoint regionAZ)
        {
            var credentials = new BasicAWSCredentials(accessId, secretKey);
            this.s3Client = new AmazonS3Client(credentials, regionAZ);
        }

        public async Task<bool> CreateBucket(String bucketName)
        {
            bool isSuccess = false;

            try
            {
                PutBucketRequest request = new PutBucketRequest();
                request.BucketName = bucketName;

                PutBucketResponse response = await s3Client.PutBucketAsync(request);

                isSuccess = true;
            }
            catch (AmazonS3Exception exc)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public async Task<bool> AddObject(String bucketName, String filePath)
        {
            bool isSuccess = false;

            try
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    FilePath = filePath
                };

                PutObjectResponse response = await s3Client.PutObjectAsync(request);

                isSuccess = true;
            }
            catch (AmazonS3Exception exc)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public async Task<List<S3Object>> ListObjects(string bucketName)
        {
            List<S3Object> objects = new List<S3Object>();

            try
            {
                ListObjectsResponse response = await s3Client.ListObjectsAsync(bucketName);
                objects = response.S3Objects;
            }
            catch (AmazonS3Exception exc)
            {
                Console.WriteLine("===> Error: " + exc.Message);
            }

            return objects;

        }

        public async Task<List<S3Bucket>> ListBuckets()
        {
            List<S3Bucket> buckets = new List<S3Bucket>();

            try
            {
                ListBucketsResponse response = await s3Client.ListBucketsAsync();
                buckets = response.Buckets;
            }
            catch (AmazonS3Exception exc)
            {
                Console.WriteLine("===> Error: " + exc.Message);
            }

            return buckets;
        }




        public async Task<string> UploadFileAsync(IFormFile formFile)
        {
            try
            {
                byte[] fileBytes;
                using (var fileStream = formFile.OpenReadStream())
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    fileBytes = ms.ToArray();

                    PutObjectRequest request = new PutObjectRequest()
                    {
                        InputStream = ms,
                        Key = formFile.FileName,
                        BucketName = "comp306003lab3bucket",
                        CannedACL = S3CannedACL.PublicRead
                    };
                    PutObjectResponse response = await s3Client.PutObjectAsync(request);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //                        https://comp306003lab3bucket.s3.amazonaws.com/pigeon.jpg

                        string url = "https://" + "comp306003lab3bucket" + ".s3.amazonaws.com/" + formFile.FileName;
                    return url;
                    }

                    else {
                        return "";
                    }
                }

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return "";

        }

    }
}
