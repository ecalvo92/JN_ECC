$(function () {

  $("#formCambiarAcceso").validate({
    rules: {
      NuevaContrasenna: {
        required: true,
        minlength: 8
      },
      ConfirmarContrasenna: {
        required: true,
        equalTo: "#NuevaContrasenna"
      }
    },
    messages: {
      NuevaContrasenna: {
        required: "Campo obligatorio",
        minlength: "Tamaño incorrecto"
      },
      ConfirmarContrasenna: {
        required: "Campo obligatorio",
        equalTo: "Contraseñas distintas"
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