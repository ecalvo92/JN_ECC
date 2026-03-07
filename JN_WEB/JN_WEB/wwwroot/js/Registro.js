$(function () {

  $("#formRegistro").validate({
    rules: {
      Identificacion: {
        required: true
      },
      Nombre: {
        required: true
      },
      CorreoElectronico: {
        required: true,
        email: true
      },
      Contrasenna: {
        required: true
      }
    },
    messages: {
      Identificacion: {
        required: "Campo obligatorio",
      },
      Nombre: {
        required: "Campo obligatorio",
      },
      CorreoElectronico: {
        required: "Campo obligatorio",
        email: "Formato incorrecto"
      },
      Contrasenna: {
        required: "Campo obligatorio",
      }
    },
     errorClass: "text-white",
      errorElement: "span",
      highlight: function (element) {
        $(element).addClass("is-invalid");
      },
      unhighlight: function (element) {
        $(element).removeClass("is-invalid");
      }
  });

});

function ConsultarNombre() {

  $("#Nombre").val("");
  var cedula = $("#Identificacion").val().trim();

  if (cedula.length >= 9) {
    $.ajax({
      url: "https://apis.gometa.org/cedulas/" + cedula,
      type: "GET",
      success: function (data) {
        if (data.results.length > 0) {
          $("#Nombre").val(data.results[0].fullname);
        }
      }
    });
  }

}