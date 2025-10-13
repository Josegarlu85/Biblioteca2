using Biblioteca.Models;
using Microsoft.Maui.Storage;

namespace Biblioteca;

public partial class AltasLibros : ContentPage
{
    private string rutaImagenSeleccionada;

    public AltasLibros()
    {
        InitializeComponent();
    }

    private async void GuardarClicked(object sender, EventArgs e)
    {
        // Exigimos que esten rellenos todos los campos
        if (string.IsNullOrWhiteSpace(tituloEntry.Text) ||
            string.IsNullOrWhiteSpace(autorEntry.Text) ||
            string.IsNullOrWhiteSpace(editorialEntry.Text) ||
            string.IsNullOrWhiteSpace(rutaImagenSeleccionada))
        {
            await DisplayAlert("Error", "Todos los campos deben estar completos y debe seleccionarse una imagen.", "OK");
            return;
        }

        // Crear y guardar el libro
        var nuevoLibro = new Libro
        {
            Titulo = tituloEntry.Text,
            Autor = autorEntry.Text,
            Editorial = editorialEntry.Text,
            ImagenPath = rutaImagenSeleccionada
        };

        DatosBiblio.LibrosGuardados.Add(nuevoLibro);

        await DisplayAlert("Éxito", "Libro guardado correctamente.", "OK");
        LimpiarFormulario();
    }
    // Evento del boton limpiar
    private void LimpiarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();
    }
    // Limpiamos los campos del formulario
    private void LimpiarFormulario()
    {
        tituloEntry.Text = string.Empty;
        autorEntry.Text = string.Empty;
        editorialEntry.Text = string.Empty;
        portadaImage.Source = null;
        rutaImagenSeleccionada = null;
    }
    // MEtodo al seleccionar imagen 
    private async void SeleccionarImagenClicked(object sender, EventArgs e)
    {
        // Usamos un try catch por si no se selecciona ninguna imagen
        try
        {
            // Abrimos el selector de archivos para elegir una imagen
            var resultado = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una imagen de portada",
                FileTypes = FilePickerFileType.Images
            });
            // Si se selecciona una imagen, la mostramos en la interfaz
            if (resultado != null)
            {
                rutaImagenSeleccionada = resultado.FullPath;
                portadaImage.Source = ImageSource.FromFile(rutaImagenSeleccionada);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo seleccionar la imagen: {ex.Message}", "OK");
        }
    }
}
