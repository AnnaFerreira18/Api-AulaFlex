namespace Api.Helper
{
    public static class Email
    {
        /// <summary>
        /// Envia um email, utilizando o serviço de email transacional
        /// </summary>
        /// <param name="emailDestino">Email de destino</param>
        /// <param name="assunto">Assunto</param>
        /// <param name="corpo">Mensagem</param>
        /// <param name="tema">Sest Senat, CNT, ITL</param>
        /// <param name="emailDestinoComCopia">Destinatários que receberão a mensagem como cópia</param>
        /// <param name="emailDestinoComCopiaOculta">Destinatários que receberão a mensagem como cópia oculta</param>
        /// <returns></returns>
        public static bool Enviar(IEnumerable<string> emailDestino, string assunto, string corpo, IEnumerable<string> emailDestinoComCopia = null, IEnumerable<string> emailDestinoComCopiaOculta = null)
        {
            try
            {
                var objRequest = new EmailModel()
                {
                    tema = 0,
                    enderecoRemetente = "nao-responder@mkt.sestsenat.org.br",
                    assunto = assunto,
                    corpo = corpo,
                    enderecosDestino = new List<string>(),
                    enderecosDestinoComCopia = new List<string>(),
                    enderecosDestinoComCopiaOculta = new List<string>()
                };


                foreach (var email in emailDestino)
                {
                    objRequest.enderecosDestino.Add(email);
                }

                // Para destinatários que irão receber o email como cópia
                if (emailDestinoComCopia != null)
                {
                    foreach (var emailCc in emailDestinoComCopia)
                    {
                        objRequest.enderecosDestinoComCopia.Add(emailCc);
                    }
                }

                // Para destinatários que irão receber o email como cópia oculta
                if (emailDestinoComCopiaOculta != null)
                {
                    foreach (var emailBcc in emailDestinoComCopiaOculta)
                    {
                        objRequest.enderecosDestinoComCopiaOculta.Add(emailBcc);
                    }
                }

                //string urlBase = ConfigurationManager.AppSettings["UrlSestSenatService"];
                //var response = Task.Run(async () => await RequestApiSestSenat.PostData(urlBase, "envioMensagens/envioEmail", objRequest)).GetAwaiter().GetResult();
                //string content = Task.Run(async () => await response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    internal class EmailModel
    {
        public string assunto { get; set; }
        public string corpo { get; set; }
        public string enderecoRemetente { get; set; }
        public int tema { get; set; }
        public List<string> enderecosDestino { get; set; }
        public List<string> enderecosDestinoComCopia { get; set; }
        public List<string> enderecosDestinoComCopiaOculta { get; set; }

    }
}
