        namespace amorphie.consent.core.DTO.Contract;

        public class ProcessDto
        {
                public ProcessDto()
                {
                        client = "consent-application";
                        user = "34455667789";
                        name = "ekyc-process";
                        state = "load-document";
                        action = "web-mobil-document-load";
                }
                public string client { get; set; }
                public string user { get; set; }
                public string name { get; set; }
                public string state { get; set; }
                public string action { get; set; }
        }