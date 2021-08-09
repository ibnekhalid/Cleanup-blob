using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobCleanUp
{
    class Program
    {
        public static string again = "y";
        // storage key
        private static readonly string storageConnectionString = "Place Your blob account connection string here";

        static async System.Threading.Tasks.Task Main(string[ ] args)
        {
            do
            {
                var containerName = Console.ReadLine( );
                // Check if invalid container name
                if (IsInvalidContainer(containerName))
                    return;
                //confirm before deletion of blob items
                if (Confirm(containerName))
                {
                    var client = new BlobServiceClient(storageConnectionString);
                    var containerClient = client.GetBlobContainerClient(containerName);

                    var blobs = containerClient.GetBlobs( );
                    foreach (var blobItem in blobs)
                    {
                        await containerClient.DeleteBlobIfExistsAsync(blobItem.Name);
                    }
                    Console.WriteLine($"Do you want to run again? Press (y/n)");
                    again = Console.ReadLine( );
                }

            } while (again?.ToLower( ) == "y");

        }
        private static bool IsInvalidContainer(string containerName)
        {
            var invalid = string.IsNullOrWhiteSpace(containerName);
            if (invalid)
                Console.WriteLine($"Invalid Container Name: '{containerName}'");
            return invalid;
        }
        private static bool Confirm(string containerName)
        {
            Console.WriteLine($"Do you want to delete all records in container '{containerName}'? It would not be reverted. Press (y/n)");
            string confirm = Console.ReadLine( );
            return confirm?.ToLower( ) == "y";
        }
    }
}
