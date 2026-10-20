$(document).ready(function () {
    // Intercepta CUALQUIER formulario que tenga la clase 'swal-delete-form'
    $(document).on('submit', 'form.swal-delete-form', function (e) {
        e.preventDefault(); // Detiene el envío inmediato
        var form = this; // Guarda la referencia al formulario

        Swal.fire({
            title: '¿Estás seguro?',
            text: 'Esta acción no se puede deshacer.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit(); // Si confirma, envía el formulario
            }
        });
    });
});