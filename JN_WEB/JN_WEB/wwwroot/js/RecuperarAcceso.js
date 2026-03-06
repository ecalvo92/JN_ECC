$(function () {

  $("#formRecuperarAcceso").validate({
    rules: {
      CorreoElectronico: {
        required: true,
        email: true
      }
    },
    messages: {
      CorreoElectronico: {
        required: "Campo obligatorio",
        email: "Formato incorrecto"
      }
    },
    errorElement: "span",
    errorClass: "text-white",
    highlight: function (element) {
      $(element).addClass("is-invalid");
    },
    unhighlight: function (element) {
      $(element).removeClass("is-invalid");
    }
  });

});