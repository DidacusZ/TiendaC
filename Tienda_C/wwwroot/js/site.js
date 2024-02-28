function revisarContraseña() {
    const clave = document.getElementById('clave').value;
    const clave2 = document.getElementById('clave2').value;
    const mensajeClave = document.getElementById('mensajeClave');
    //mensajeClave.style.fontSize = '30px';

    if (clave === "" && clave2 === "") {
        mensajeClave.innerHTML = '<div class="alert alert-danger" role="alert">La contraseña no puede estar vacía</div>';
        //mensajeClave.style.color = 'black';
        document.getElementById("boton").disabled = true;
        boton.style.backgroundColor = "#959595";

    } else if (clave === clave2) {
        mensajeClave.innerHTML = '<div class="alert alert-success" role="alert">Contraseña correcta</div>';
        //mensajeClave.style.color = 'black';
        document.getElementById("boton").disabled = false;
        boton.style.backgroundColor = "#007bff";
    } else {
        mensajeClave.innerHTML = '<div class="alert alert-danger" role="alert">Las contraseñas no coinciden</div>';
        //mensajeClave.style.color = 'black';
        document.getElementById("boton").disabled = true;
        boton.style.backgroundColor = "#959595";
    }
}

function cerrarSesion() {
    if (window.confirm("Seguro que quieres cerrar sesión"))
        document.getElementById('cerrarSesion').submit();
}


function eliminarUsuario() {
    if (window.confirm("Seguro que quieres eliminar este Usuario"))
        document.getElementById('eliminarUsario').submit();
}