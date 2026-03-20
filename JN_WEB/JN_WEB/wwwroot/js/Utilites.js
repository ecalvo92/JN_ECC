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