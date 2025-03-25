//-- ROOT HCE JAVASCRIPT OBJECTS --
if (typeof HCE == 'undefined') {
    var HCE = {};
}

if (typeof HCE.Resources == 'undefined') {
    HCE.Resources = {};
}

HCE.Resources.Error = 'Erro';
HCE.Resources.OrderBy = 'Ordenar por';
HCE.Resources.ThenBy = 'Então até';
HCE.Resources.NoRecordsFound = 'Nenhum registro localizado';
HCE.Resources.NoRecordsFoundText = 'No matching [ENTITY] could be found.<br/><br/>Please try again or check back later. (pt)';
HCE.Resources.Page = 'Página';
HCE.Resources.Of = 'de';
HCE.Resources.RecordsPerPage = 'Registros por página';
HCE.Resources.TotalRecords = 'Total de registros';
HCE.Resources.From = 'De';
HCE.Resources.To = 'Para';
HCE.Resources.Value = 'Valor';
HCE.Resources.UnexpectedError = 'Erro inesperado';
HCE.Resources.UnexpectedErrorDesc = 'Encontramos um erro inesperado ao processar seu pedido.';
HCE.Resources.RequestProcessedSuccessfully = 'Sua solicitação foi enviada com sucesso!';
HCE.Resources.IsRequired = 'é obrigatório';

HCE.Resources.Alerts = {};
HCE.Resources.Alerts.CDS = 'Você tem [TOTAL] ordens do Consumidor à espera de mais de 10 dias na estação de trabalho espera comprovante de depósito, leia estas ordens!';
HCE.Resources.Alerts.DDS = 'Você tem [TOTAL] ordens Distribuidor esperando por mais de 10 dias na estação de trabalho espera comprovante de depósito, leia estas ordens!';
HCE.Resources.Alerts.CWF = 'Você tem [TOTAL] contrato(s) no fluxo de trabalho de cliente com comentários não lidos!';
HCE.Resources.Alerts.DWF = 'Você tem [TOTAL] ordens de pagamento no fluxo de trabalho de distribuidor com comentários não lidos!';
HCE.Resources.Alerts.LWF = 'Você tem [TOTAL] contrato(s) no fluxo de trabalho com comentários não lidos!';
HCE.Resources.Alerts.COL = 'Você tem [TOTAL] mensagens coletor com comentários não lidos!';
HCE.Resources.Alerts.DFE = 'Você tem [TOTAL] Conta(s) financiada(s) pelo distribuidor aceitáveis para financiamento regular!';
HCE.Resources.Alerts.LWE = 'Você tem [TOTAL] Conta(s) Layaway aceitáveis para financiamento regular!';
HCE.Resources.Alerts.PRV = 'Você tem [TOTAL] novas mensagens privadas!';
HCE.Resources.Alerts.ORD = 'Você tem [TOTAL] itens no seu carrinho de compras no cliente [CLIENT]!';
HCE.Resources.Alerts.SPP = 'Seu perfil de vendedor está incompleto! Preencha as informações faltantes do perfil o mais rapidamente possível para evitar atrasos no processamento de seus pedidos ou pagamentos.';
HCE.Resources.Alerts.APP = 'Você tem [TOTAL] ordens de vendedor que exigem sua aprovação!';
HCE.Resources.Alerts.ZTH = 'Você tem [TOTAL] clientes com atraso de 0 a 30 dias!';
HCE.Resources.Alerts.RLT = 'Você tem [TOTAL] clientes com carta de recompra!';