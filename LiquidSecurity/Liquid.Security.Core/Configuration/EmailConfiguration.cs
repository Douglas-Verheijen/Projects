using System;

namespace Liquid.Security.Configuration
{
    public interface IEmailConfiguration
    {
        string Host { get; }

        string Username { get; }

        string Password { get; }

        string SendFrom { get; }

        string SendFromName { get; }

        string SendTo { get; }

        string SendToName { get; }
    }

    class EmailConfiguration : IEmailConfiguration
    {
        public EmailConfiguration(string host,
            string username,
            string password,
            string sendTo,
            string sendToName,
            string sendFrom,
            string sendFromName)
        {
            Host = host;
            Username = username;
            Password = password;
            SendTo = sendTo;
            SendToName = sendToName;
            SendFrom = sendFrom;
            SendFromName = sendFromName;
        }

        public string Host { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public string SendTo { get; private set; }

        public string SendToName { get; private set; }

        public string SendFrom { get; private set; }

        public string SendFromName { get; private set; }
    }
}