﻿namespace TweyesBackend.Core.DTO.Request
{
    public class WelcomeEmailTemplateRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
