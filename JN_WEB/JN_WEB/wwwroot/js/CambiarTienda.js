$.validator.addMethod("telefono", function (value, element) {
  return this.optional(element) || /^\d{8}$/.test(value);
}, "Debe contener exactamente 8 dígitos numéricos");

$(function () {

  $("#formCambiarTienda").validate({
    rules: {
      Nombre: {
        required: true,
        maxlength: 50
      },
      Descripcion: {
        required: true,
        maxlength: 500
      },
      Contacto: {
        required: true,
        telefono: true,
        maxlength: 50
      },
      Ubicacion: {
        required: true,
        maxlength: 50
      }
    },
    messages: {
      Nombre: {
        required: "Campo obligatorio",
        maxlength: "Máximo 50 caracteres"
      },
      Descripcion: {
        required: "Campo obligatorio",
        maxlength: "Máximo 500 caracteres"
      },
      Contacto: {
        required: "Campo obligatorio",
        telefono: "Formato de número telefónico inválido",
        maxlength: "Máximo 50 caracteres"
      },
      Ubicacion: {
        required: "Campo obligatorio",
        maxlength: "Máximo 50 caracteres"
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