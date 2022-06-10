using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace SalesAdvertisementApi.Services;

public class AwsS3BucketServices
{
    public readonly BasicAWSCredentials Credentials = new BasicAWSCredentials(
        accessKey: "Acess Key", secretKey: "Secret Key");
    
    public static async Task<bool> CreateBucketAsync(IAmazonS3 client, string bucketName)
    {
        try
        {
            var request = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            var response = await client.PutBucketAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (AmazonS3Exception exception)
        {
            Console.WriteLine($"Error creating bucket: {exception}");
            return false;
        }
    }

    public static async Task<bool> DeleteBucketAsync(IAmazonS3 client, string bucketName)
    {
        var request = new DeleteBucketRequest
        {
            BucketName = bucketName,
        };

        var response = await client.DeleteBucketAsync(request);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }
    
    public static async Task<bool> UploadFileAsync(
        IAmazonS3 client, string bucketName, string objectName, string filePath)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectName,
            FilePath = filePath
        };

        var response = await client.PutObjectAsync(request);

        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine($"Successfully uploaded {objectName} to {bucketName}");
            return true;
        }
        else
        {
            Console.WriteLine($"Could not upload {objectName} to {bucketName}");
            return false;
        }
    }

    public static async Task AddAclToExistingObjectAsync(IAmazonS3 client, string bucketName, string keyName)
    {
        GetACLResponse aclResponse = await client.GetACLAsync(new GetACLRequest
        {
            BucketName = bucketName,
            Key = keyName
        });

        S3AccessControlList acl = aclResponse.AccessControlList;

        Owner owner = acl.Owner;

        var grantPublicGroup = new S3Grant
        {
            Grantee = new S3Grantee{URI = "http://acs.amazonaws.com/groups/global/AllUsers"},
            Permission = S3Permission.READ
        };

        var newAcl = new S3AccessControlList
        {
            Grants = new List<S3Grant> { grantPublicGroup },
            Owner = owner
        };

        _ = await client.PutACLAsync(new PutACLRequest
        {
            BucketName = bucketName,
            Key = keyName,
            AccessControlList = newAcl
        });
    }

    public static async Task<bool> DeleteBucketContentsAsync(IAmazonS3 client, string bucketName)
    {
        var request = new ListObjectsV2Request { BucketName = bucketName };

        try
        {
            var response = await client.ListObjectsV2Async(request);

            do
            {
                response.S3Objects
                    .ForEach(async obj => await client.DeleteObjectAsync(bucketName, obj.Key));

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            return true;
        }
        catch (AmazonClientException exception)
        {
            Console.WriteLine($"Error deleting objects: {exception}");
            return false;
        }
    }

    public static async Task DeleteBucketContentAsync(IAmazonS3 client, string bucketName, string keyName)
    {
        await client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = bucketName, Key = keyName });
    }
}
