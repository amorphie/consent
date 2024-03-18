namespace amorphie.consent.core.DTO.MessagingGateway
{
    public class SendPushDto
    {
        public string Sender { get; set; }
        public string CitizenshipNo { get; set; }
        public string Template { get; set; }
        public string TemplateParams { get; set; }
        public string CustomParameters { get; set; }

        public bool SaveInbox { get; set; }
        public ProcessInfo Process { get; set; }
    }

    public class ProcessInfo
    {
        public string Name { get; set; }
        public string ItemId { get; set; }
        public string Action { get; set; }
        public string Identity { get; set; }
    }
}
