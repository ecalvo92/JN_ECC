$.validator.addMethod("soloPng", function (value, element) {
  if (element.files.length === 0) return true;
  return element.files[0].name.toLowerCase().endsWith(".png");
}, "Solo se permiten imágenes en formato .png");

$.validator.addMethod("tamanoMaximo", function (value, element) {
  if (element.files.length === 0) return true;
  return element.files[0].size <= 524288;
}, "La imagen no puede superar los 0.5 MB");

$(function () {

  $("#formCambiarPerfil").validate({
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
      ImagenPerfil: {
        soloPng: true,
        tamanoMaximo: true
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
      ImagenPerfil: {
        soloPng: "Solo se permiten imágenes en formato .png",
        tamanoMaximo: "La imagen no puede superar los 0.5 MB"
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