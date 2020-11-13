//using Amazon;
//using Amazon.Runtime;
//using Amazon.S3;
//using Amazon.S3.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace DroppingBox.Models
//{
//    public class S3Manager
//    {
//        private AmazonS3Client s3Client;

//        public S3Manager(String accessId, String secretKey, RegionEndpoint regionAZ)
//        {
//            var credentials = new BasicAWSCredentials(accessId, secretKey);
//            this.s3Client = new AmazonS3Client(credentials, regionAZ);
//        }

//        public bool CreateBucket(String bucketName)
//        {
//            bool isSuccess = false;

//            try
//            {
//                PutBucketRequest request = new PutBucketRequest();
//                request.BucketName = bucketName;

//                PutBucketResponse response = s3Client.PutBucket(request);

//                isSuccess = true;
//            }
//            catch (AmazonS3Exception exc)
//            {
//                isSuccess = false;
//            }

//            return isSuccess;
//        }

//        public bool AddObject(String bucketName, String filePath)
//        {
//            bool isSuccess = false;

//            try
//            {
//                PutObjectRequest request = new PutObjectRequest
//                {
//                    BucketName = bucketName,
//                    FilePath = filePath
//                };

//                PutObjectResponse response = s3Client.PutObject(request);

//                isSuccess = true;
//            }
//            catch (AmazonS3Exception exc)
//            {
//                isSuccess = false;
//            }

//            return isSuccess;
//        }

//        public List<S3Object> ListObjects(string bucketName)
//        {
//            List<S3Object> objects = new List<S3Object>();

//            try
//            {
//                ListObjectsResponse response = s3Client.ListObjects(bucketName);
//                objects = response.S3Objects;
//            }
//            catch (AmazonS3Exception exc)
//            {
//                Console.WriteLine("===> Error: " + exc.Message);
//            }

//            return objects;

//        }

//        public List<S3Bucket> ListBuckets()
//        {
//            List<S3Bucket> buckets = new List<S3Bucket>();

//            try
//            {
//                ListBucketsResponse response = s3Client.ListBuckets();
//                buckets = response.Buckets;
//            }
//            catch (AmazonS3Exception exc)
//            {
//                Console.WriteLine("===> Error: " + exc.Message);
//            }

//            return buckets;
//        }

//    }
//}
