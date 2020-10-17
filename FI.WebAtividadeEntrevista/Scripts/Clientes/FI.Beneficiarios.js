$(document).ready(function () {

    //Mascara de CPF
    $("#formBeneficiario #CPF").mask("999.999.999-99");

    // Incluindo um beneficiário na table do modal
    $('#formBeneficiario').submit(function (e) {

        e.preventDefault();

        var id = $(this).find("#Id").val() != "" ? $(this).find("#Id").val() : 0;
        var nome = $(this).find("#Nome").val();
        var cpf = $(this).find("#CPF").val();

        preencherRowTable(id, cpf, nome)

        $("#formBeneficiario")[0].reset();
    });

});

// Obtendo os valores da table de beneficiários e atribuindo a listagem de beneficiários ao cliente
function getBeneficiarios(beneficiarios) {

    $('#tableBeneficiarios tbody tr').each(function () {

        var colunas = $(this).children();

        var beneficiario = {
            'Id': $(colunas[0]).text(),
            'CPF': $(colunas[1]).text(),
            'Nome': $(colunas[2]).text()
        };

        beneficiarios.push(beneficiario);
    });

    return beneficiarios;
}

// Remover Beneficiário da table do modal
function RemoverBeneficiario(item) {

    var tr = $(item).closest('tr');

    tr.fadeOut(400, function () {
        tr.remove();
    });
}

// Alterar Beneficiário da table do modal
function AlterarBeneficiario(item) {

    var id = $(item).parents("tr").find("td:nth-child(1)");
    var cpf = $(item).parents("tr").find("td:nth-child(2)");
    var nome = $(item).parents("tr").find("td:nth-child(3)");

    $("#formBeneficiario").find("#Id").val(id.text().trim());
    $("#formBeneficiario").find("#Nome").val(nome.text().trim());
    $("#formBeneficiario").find("#CPF").val(cpf.text().trim());

    RemoverBeneficiario(item);
}

// Preencher os dados do Beneficiários na table do modal
function preencherRowTable(Id, CPF, Nome) {

    $("<tr>" +
        "<td hidden> " + Id + "</td>" +
        "<td> " + CPF + "</td>" +
        "<td> " + Nome + "</td>" +
        "<td align='right'><button style='margin-right:10px' type='button' class='btn btn-sm btn-primary' id='alterarBeneficiario' onclick='AlterarBeneficiario(this,[1,2,3])'>Alterar</button>" +
        "<button type='button' class='btn btn-sm btn-primary' id='excluirBeneficiario' onclick='RemoverBeneficiario(this)'>Excluir</button></td>" +
      "</tr> ").appendTo("#tableBeneficiarios tbody");

}