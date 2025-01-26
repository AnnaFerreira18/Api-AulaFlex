using System.Net.Mail;
using System.Net;

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
        public static bool Enviar(IEnumerable<string> destinatarios, string assunto, string corpo)
        {
            try
            {
                // Configuração do servidor SMTP (Aqui você pode usar o servidor da sua escolha)
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("testesombria@gmail.com", "tdrh tber eitc qsts"), // Use o seu e-mail e senha
                    EnableSsl = true
                };

                // Montagem do e-mail
                var mensagem = new MailMessage
                {
                    From = new MailAddress("nao-responder@dominio.com"), // E-mail do remetente
                    Subject = assunto,
                    Body = corpo,
                    IsBodyHtml = true // Se o corpo do e-mail for em HTML
                };

                // Adicionar destinatários
                foreach (var email in destinatarios)
                {
                    mensagem.To.Add(email);
                }

                try
                {
                    smtpClient.Send(mensagem);
                }
                catch (SmtpException smtpEx)
                {
                    Console.WriteLine($"Erro SMTP: {smtpEx.Message}");
                    Console.WriteLine($"Detalhes: {smtpEx.InnerException?.Message}");
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                // Log do erro (ex.Message) pode ser útil para diagnóstico
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
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
