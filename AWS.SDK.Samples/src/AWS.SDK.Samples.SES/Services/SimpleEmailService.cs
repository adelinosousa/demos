using Amazon.S3;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using AWS.SDK.Samples.Application.Extensions;
using AWS.SDK.Samples.SES.Models;

namespace AWS.SDK.Samples.SES.Services
{
    public class SimpleEmailService : IEmailService
    {
        private readonly IAmazonSimpleEmailService amazonSimpleEmail;
        private readonly IAmazonS3 s3Client;

        public SimpleEmailService(
            IAmazonSimpleEmailService amazonSimpleEmail,
            IAmazonS3 s3Client)
        {
            this.amazonSimpleEmail = amazonSimpleEmail;
            this.s3Client = s3Client;
        }

        public async Task<bool> Send(Email email)
        {
            var response = await amazonSimpleEmail.SendEmailAsync(new SendEmailRequest
            {
                Source = email.From,
                Destination = new Destination
                {
                    ToAddresses = email.To.ToList()
                },
                Message = new Message
                {
                    Subject = new Content(email.Subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = email.HtmlMessage
                        },
                    }
                },
            });

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> CreateRule(string ruleSetName, string ruleName, string forEmail, string s3BucketName)
        {
            var response = await amazonSimpleEmail.CreateReceiptRuleAsync(new CreateReceiptRuleRequest
            {
                RuleSetName = ruleSetName,
                Rule = new ReceiptRule
                {
                    Name = ruleName,
                    Enabled = true,
                    ScanEnabled = true,
                    Recipients = { forEmail },
                    Actions = {
                        new ReceiptAction
                        {
                            S3Action = new S3Action
                            {
                                BucketName = s3BucketName,
                                ObjectKeyPrefix = ruleName
                            }
                        },
                        new ReceiptAction
                        {
                            LambdaAction = new LambdaAction
                            {
                                FunctionArn = "arn:aws:lambda:eu-west-1:377856458995:function:FYMI-Function-EmailReceived"
                            }
                        },
                        new ReceiptAction
                        {
                            StopAction = new StopAction
                            {
                                Scope = StopScope.RuleSet
                            }
                        }
                    }
                }
            });

            return response.HttpStatusCode != System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> DeleteRule(string ruleSetName, string ruleName, string s3BucketName)
        {
            var deleteReceiptResponse = await amazonSimpleEmail.DeleteReceiptRuleAsync(new DeleteReceiptRuleRequest
            {
                RuleName = ruleName,
                RuleSetName = ruleSetName
            });

            if (deleteReceiptResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return await s3Client.DeleteDirectoryAsync(s3BucketName, $"{ruleName}/");
            }

            return false;
        }
    }
}
