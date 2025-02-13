using System.Net.Mail;
using System.Net;

namespace Api.Helper
{
    public static class Email
    {

        /// Envia um email, utilizando o serviço de email transacional
        /// <param name="emailDestino">Email de destino</param>
        /// <param name="assunto">Assunto</param>
        /// <param name="corpo">Mensagem</param>
        public static bool Enviar(IEnumerable<string> destinatarios, string assunto, string corpo)
        {
            try
            {
                // Configuração do servidor SMTP 
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("testesombria@gmail.com", "tdrh tber eitc qsts"), // e-mail e senha
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
