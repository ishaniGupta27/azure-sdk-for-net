﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Azure.WebJobs.Extensions.Storage.Common.Tests;
using Microsoft.Azure.WebJobs.Extensions.Storage.Common;
using Xunit;

namespace Microsoft.Azure.WebJobs.Host.FunctionalTests
{
    public class BinderTests : IClassFixture<AzuriteFixture>
    {
        private const string QueueName = "input";
        private readonly AzuriteFixture azuriteFixture;

        public BinderTests(AzuriteFixture azuriteFixture)
        {
            this.azuriteFixture = azuriteFixture;
        }

        [AzuriteFact]
        public async Task Trigger_ViaIBinder_CannotBind()
        {
            // Arrange
            const string expectedContents = "abc";
            var account = CreateFakeStorageAccount();
            QueueClient queue = CreateQueue(account, QueueName);
            await queue.SendMessageAsync(expectedContents);

            // Act
            Exception exception = await RunTriggerFailureAsync<string>(account, typeof(BindToQueueTriggerViaIBinderProgram),
                (s) => BindToQueueTriggerViaIBinderProgram.TaskSource = s);

            // Assert
            Assert.Equal("No binding found for attribute 'Microsoft.Azure.WebJobs.QueueTriggerAttribute'.", exception.Message);
        }

        private StorageAccount CreateFakeStorageAccount()
        {
            return StorageAccount.NewFromConnectionString(azuriteFixture.GetAccount().ConnectionString);
        }

        private static QueueClient CreateQueue(StorageAccount account, string queueName)
        {
            var client = account.CreateQueueServiceClient();
            var queue = client.GetQueueClient(queueName);
            queue.CreateIfNotExists();
            return queue;
        }

        private static async Task<Exception> RunTriggerFailureAsync<TResult>(StorageAccount account, Type programType,
            Action<TaskCompletionSource<TResult>> setTaskSource)
        {
            return await FunctionalTest.RunTriggerFailureAsync<TResult>(account, programType, setTaskSource);
        }

        private class BindToQueueTriggerViaIBinderProgram
        {
            public static TaskCompletionSource<string> TaskSource { get; set; }

            public static void Run([QueueTrigger(QueueName)] QueueMessage message, IBinder binder)
            {
                TaskSource.TrySetResult(binder.Bind<string>(new QueueTriggerAttribute(QueueName)));
            }
        }
    }
}
