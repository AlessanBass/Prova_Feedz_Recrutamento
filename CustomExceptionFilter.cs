using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Prova
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Verifique se a exceção é do tipo que você quer capturar
            if (context.Exception is Exception ex)
            {
                // Retorna a mensagem da exceção sem o stack trace
                context.Result = new BadRequestObjectResult(new { message = ex.Message });

                // Marca a exceção como tratada
                context.ExceptionHandled = true;
            }
        }
    }
}
