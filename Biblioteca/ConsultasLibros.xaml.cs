using Biblioteca.Models;

namespace Biblioteca;

public partial class ConsultasLibros : ContentPage
{
    private string tipoConsultaActual = "Autor";
    private List<Libro> librosFiltrados = new();

    public ConsultasLibros()
    {
        InitializeComponent();
        MostrarConsulta(tipoConsultaActual);
    }

    // MAnejamos que tipo de consulta se ha seleccionado segun el radobutton 
    private void OnConsulta(object sender, CheckedChangedEventArgs e)
    {
        if (autorRadio.IsChecked)
        {
            tipoConsultaActual = "Autor";
        }
        else
        {
            tipoConsultaActual = "Editorial";
        }

        MostrarConsulta(tipoConsultaActual);
    }

    private void MostrarConsulta(string tipo)
    {
        // Obtenemos la lista de libros guardados
        var libros = DatosBiblio.LibrosGuardados;

        // Si no hay libros, mostramos mensaje y limpiamos vistas
        if (libros.Count == 0)
        {
            autoresView.ItemsSource = new List<string>();
            titulosView.ItemsSource = new List<string> { "No hay libros guardados." };
            portadaImage.Source = null;
            return;
        }

        // Obtenemos los valores únicos según el tipo de consulta
        List<string> valores = new List<string>();

        // Si elegimos autor recorremos con un foreach todos los libros y añadimos a la lista de valores los autores únicos
        if (tipo == "Autor")
        {
            foreach (Libro libro in libros)
            {
                if (!valores.Contains(libro.Autor))
                {
                    valores.Add(libro.Autor);
                }
            }
        }
        else if (tipo == "Editorial")
        {
            foreach (Libro libro in libros)
            {
                if (!valores.Contains(libro.Editorial))
                {
                    valores.Add(libro.Editorial);
                }
            }
        }
        else
        {
            
        }

        // Asignamos los valores a la vista correspondiente y limpiamos las otras vistas
        autoresView.ItemsSource = valores;
        titulosView.ItemsSource = null;
        portadaImage.Source = null;
    }

    // Manejador del evento ItemSelected del ListView de autores o editoriales
    private void OnAutorEditorialSeleccionado(object sender, SelectedItemChangedEventArgs e)
    {
        // Obtenemos el autor o editorial seleccionado
        var seleccionado = e.SelectedItem as string;
        if (string.IsNullOrEmpty(seleccionado)) return;

        // Filtramos los libros según el autor o editorial seleccionado
        librosFiltrados = new List<Libro>();

        // Recorremos todos los libros y añadimos a la lista de libros filtrados aquellos que coincidan con el autor o editorial seleccionado
        if (tipoConsultaActual == "Autor")
        {
            foreach (Libro libro in DatosBiblio.LibrosGuardados)
            {
                if (libro.Autor == seleccionado)
                {
                    librosFiltrados.Add(libro);
                }
            }
        }
        else if (tipoConsultaActual == "Editorial")
        {
            foreach (Libro libro in DatosBiblio.LibrosGuardados)
            {
                if (libro.Editorial == seleccionado)
                {
                    librosFiltrados.Add(libro);
                }
            }
        }

        // Extraemos los títulos de los libros filtrados
        List<string> titulos = new List<string>();
        foreach (Libro libro in librosFiltrados)
        {
            titulos.Add(libro.Titulo);
        }

        titulosView.ItemsSource = titulos;
        portadaImage.Source = null;
    }

    /* 
     * Con Lambda
     * 
     * private void OnAutorEditorialSeleccionado(object sender, SelectedItemChangedEventArgs e)
     {
         var seleccionado = e.SelectedItem as string;
         if (string.IsNullOrEmpty(seleccionado)) return;

         librosFiltrados = tipoConsultaActual == "Autor"
             ? DatosBiblio.LibrosGuardados.Where(l => l.Autor == seleccionado).ToList()
             : DatosBiblio.LibrosGuardados.Where(l => l.Editorial == seleccionado).ToList();

         titulosView.ItemsSource = librosFiltrados.Select(l => l.Titulo).ToList();
         portadaImage.Source = null;
     }
    */

    // Manejador del evento ItemTapped del ListView de títulos
    private void OnTituloDobleClick(object sender, ItemTappedEventArgs e)
    {
        // Obtenemos el título seleccionado
        var tituloSeleccionado = e.Item as string;
        if (string.IsNullOrEmpty(tituloSeleccionado)) return;

        Libro libroEncontrado = null;

        // Buscamos el libro correspondiente al título seleccionado en la lista de libros filtrados
        foreach (Libro libro in librosFiltrados)
        {
            if (libro.Titulo == tituloSeleccionado)
            {
                libroEncontrado = libro;
                break;
            }
        }

        // Si encontramos el libro y tiene una ruta de imagen válida, la mostramos
        if (libroEncontrado != null && !string.IsNullOrEmpty(libroEncontrado.ImagenPath))
        {
            portadaImage.Source = ImageSource.FromFile(libroEncontrado.ImagenPath);
        }
    }
}
