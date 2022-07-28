using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace SalesAdvertisementApi.Services;

public class AwsS3Services
{
    public readonly BasicAWSCredentials Credentials = new (
        accessKey: Environment.GetEnvironmentVariable("ACCESS_KEY"), 
        secretKey: Environment.GetEnvironmentVariable("SECRET_KEY")
        );

    // Sets the bucket name based on the environment.
    public readonly string BucketName = 
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? 
            "sales-advertisement-api-dev" : "sales-advertisement-api";
    
    public static async Task<bool> UploadFileAsync(
        IAmazonS3 client, string bucketName, int userId, string objectName, string filePath)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = $"advertisement-images/{userId}/{objectName}",
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

    public static async Task AddAclToExistingObjectAsync(IAmazonS3 client, string bucketName, int userId, string keyName)
    {
        GetACLResponse aclResponse = await client.GetACLAsync(new GetACLRequest
        {
            BucketName = bucketName,
            Key = $"advertisement-images/{userId}/{keyName}"
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
            Key = $"advertisement-images/{userId}/{keyName}",
            AccessControlList = newAcl
        });
    }

    public static async Task<ListObjectsV2Response> ListObjectsAsync(IAmazonS3 client, string bucketName, int userId)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = $"advertisement-images/{userId}/"
        };

        var response = await client.ListObjectsV2Async(request);
        
        return response;
    }
    
    public static async Task DeleteObjectAsync(IAmazonS3 client, string bucketName, int userId, string keyName)
    {
        await client.DeleteObjectAsync(new DeleteObjectRequest()
        {
            BucketName = bucketName, 
            Key = $"advertisement-images/{userId}/{keyName}"
        });
    }

    public static async Task DeleteUserFoldAsync(IAmazonS3 client, string bucketName, int userId)
    {
        var objects = await ListObjectsAsync(client, bucketName, userId);

        foreach (var obj in objects.S3Objects)
        {
            var objectKey = obj.Key;
            
            var fileDeleteRequest = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = objectKey
            };
            
            await client.DeleteObjectAsync(fileDeleteRequest);
        }

        var folderDeleteRequest = new DeleteObjectRequest()
        {
            BucketName = bucketName,
            Key = $"advertisement-images/{userId}/"
        };
        await client.DeleteObjectAsync(folderDeleteRequest);
    }
}
