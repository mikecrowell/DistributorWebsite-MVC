//-- ROOT HCE JAVASCRIPT OBJECTS --
if (typeof HCE == 'undefined') {
    var HCE = {};
}

if (typeof HCE.Resources == 'undefined') {
    HCE.Resources = {};
}

HCE.Resources.Error = 'Error';
HCE.Resources.OrderBy = 'Pedido por';
HCE.Resources.ThenBy = 'Después por';
HCE.Resources.NoRecordsFound = 'No se encontró ningún registro';
HCE.Resources.NoRecordsFoundText = 'No matching [ENTITY] could be found.<br/><br/>Please try again or check back later. (es)';
HCE.Resources.Page = 'Página';
HCE.Resources.Of = 'de';
HCE.Resources.RecordsPerPage = 'Registros por página';
HCE.Resources.TotalRecords = 'Número total de registros';
HCE.Resources.From = 'Desde';
HCE.Resources.To = 'Hasta';
HCE.Resources.Value = 'Valor';
HCE.Resources.UnexpectedError = 'Error Imprevisto';
HCE.Resources.UnexpectedErrorDesc = 'Encontramos un error imprevisto mientras procesábamos su solicitud.';
HCE.Resources.RequestProcessedSuccessfully = '¡Su solicitud ha sido enviado exitosamente!';
HCE.Resources.IsRequired = 'es obligatorio';

HCE.Resources.Alerts = {};
HCE.Resources.Alerts.CDS = '¡Usted tiene [TOTAL] orden(es) de Consumidor esperando por mas de 10 días en la estación de trabajo de Espera de Ficha de Depósito, por favor revise estas ordenes!';
HCE.Resources.Alerts.DDS = '¡Usted tiene [TOTAL] orden(es) de Distribuidor esperando por mas de 10 días en la estación de trabajo de Espera de Ficha de Depósito, por favor revise estas ordenes!';
HCE.Resources.Alerts.CWF = '¡Usted tiene [TOTAL] contrato(s) en el flujo de trabajo del cliente con comentarios no leídos!';
HCE.Resources.Alerts.DWF = '¡Usted tiene [TOTAL] orden(es) en cash en el flujo de trabajo del distribuidor con comentarios no leídos!';
HCE.Resources.Alerts.LWF = '¡Usted tiene [TOTAL] contrato(s) en el flujo de trabajo en el Sistema de Apartados con comentarios no leídos!';
HCE.Resources.Alerts.COL = '¡Usted tiene [TOTAL] mensajes del colector con comentarios no leídos!';
HCE.Resources.Alerts.DFE = '¡Usted tiene [TOTAL] Cuenta(s) Financiada(s) por el Distribuidor elegibles para el financiamiento regular!';
HCE.Resources.Alerts.LWE = '¡Usted tiene [TOTAL] Cuenta(s) de Sistema Apartado elegibles para el financiamiento regular!';
HCE.Resources.Alerts.PRV = '¡Usted tiene [TOTAL] nuevos mensajes privados!';
HCE.Resources.Alerts.ORD = 'Tiene [TOTAL] elementos en su carrito de compras en el cliente [CLIENT].';
HCE.Resources.Alerts.SPP = 'El perfil del vendedor está incompleto. Complete la información faltante del perfil lo antes posible para evitar las demoras al momento de procesar sus pedidos o pagos.';
HCE.Resources.Alerts.APP = '¡Usted tiena [TOTAL] pedidos de vendedores que requieren su aprobación!';
HCE.Resources.Alerts.ZTH = '¡Usted tiene [TOTAL] clientes con un atraso de 0 a 30 días!';
HCE.Resources.Alerts.RLT = '¡Usted tiene [TOTAL] clientes con carta de recompra!';
